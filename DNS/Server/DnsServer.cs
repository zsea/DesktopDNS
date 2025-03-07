﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using DNS.Protocol;
using DNS.Protocol.Utils;
using DNS.Client;
using DNS.Client.RequestResolver;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DNS.Server
{
    public class DnsServer : IDisposable
    {
        private const int SIO_UDP_CONNRESET = unchecked((int)0x9800000C);
        private const int DEFAULT_PORT = 53;
        private const int UDP_TIMEOUT = 2000;

        public event EventHandler<RequestedEventArgs> Requested;
        public event EventHandler<RespondedEventArgs> Responded;
        public event EventHandler<EventArgs> Listening;
        public event EventHandler<EventArgs> Stoped;
        public event EventHandler<ErroredEventArgs> Errored;

        private bool run = true;
        private bool disposed = false;
        private UdpClient udp;
        private IRequestResolver resolver;
        private CancellationTokenSource cancellationTokenSource = null;

        public DnsServer(IRequestResolver resolver, IPEndPoint endServer) :
            this(new FallbackRequestResolver(resolver, new UdpRequestResolver(endServer)))
        { }

        public DnsServer(IRequestResolver resolver, IPAddress endServer, int port = DEFAULT_PORT) :
            this(resolver, new IPEndPoint(endServer, port))
        { }

        public DnsServer(IRequestResolver resolver, string endServer, int port = DEFAULT_PORT) :
            this(resolver, IPAddress.Parse(endServer), port)
        { }

        public DnsServer(IPEndPoint endServer) :
            this(new UdpRequestResolver(endServer))
        { }

        public DnsServer(IPAddress endServer, int port = DEFAULT_PORT) :
            this(new IPEndPoint(endServer, port))
        { }

        public DnsServer(string endServer, int port = DEFAULT_PORT) :
            this(IPAddress.Parse(endServer), port)
        { }

        public DnsServer(IRequestResolver resolver)
        {
            this.resolver = resolver;
        }

        private async Task ReceiveHandle(CancellationToken cancellationToken)
        {
            OnEvent(Listening, EventArgs.Empty);

            while (run)
            {
                try
                {
                    UdpReceiveResult result = await udp.ReceiveAsync(cancellationToken);
                    _ = HandleRequest(result.Buffer, result.RemoteEndPoint);
                }
                catch (OperationCanceledException)
                {
                    break;
                }

            }

            OnEvent(Stoped, EventArgs.Empty);
        }
        public bool Listen(int port = DEFAULT_PORT, IPAddress ip = null)
        {
            return Listen(new IPEndPoint(ip ?? IPAddress.Any, port));
        }
        public bool Listen(IPEndPoint endpoint)
        {
            if (run)
            {
                try
                {
                    udp = new UdpClient(endpoint);

                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        udp.Client.IOControl(SIO_UDP_CONNRESET, new byte[4], new byte[4]);
                    }
                }
                catch (SocketException e)
                {
                    OnError(e);
                    return false;
                }
                cancellationTokenSource = new CancellationTokenSource();
                _ = ReceiveHandle(cancellationTokenSource.Token);
                return true;
            }
            return false;
        }
        public bool Shutdown()
        {
            if (cancellationTokenSource == null) return false;
            cancellationTokenSource.Cancel();
            Dispose(true);
            return true;
        }
        /*
        public async Task Listen(IPEndPoint endpoint) {
            await Task.Yield();
            //return await ListenPoint(endpoint);
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

            if (run) {
                try {
                    udp = new UdpClient(endpoint);

                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                        udp.Client.IOControl(SIO_UDP_CONNRESET, new byte[4], new byte[4]);
                    }
                } catch (SocketException e) {
                    OnError(e);
                    return;
                }
            }

            void ReceiveCallback(IAsyncResult result) {
                byte[] data;

                try {
                    IPEndPoint remote = new IPEndPoint(0, 0);
                    data = udp.EndReceive(result, ref remote);
                    File.AppendAllText(@"E:\1.txt", $"R:::{string.Join(" ",data.Select(x=>x.ToString("X")))}\n");
                    HandleRequest(data, remote);
                }
                catch (ObjectDisposedException) {
                    // run should already be false
                    run = false;
                }
                catch (SocketException e) {
                    OnError(e);
                }

                if (run) udp.BeginReceive(ReceiveCallback, null);
                else tcs.SetResult(null);
            }
            //udp.ReceiveAsync()
            udp.BeginReceive(ReceiveCallback, null);
            OnEvent(Listening, EventArgs.Empty);
            await tcs.Task.ConfigureAwait(false);
        }
        */
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void OnEvent<T>(EventHandler<T> handler, T args)
        {
            if (handler != null) handler(this, args);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;

                if (disposing)
                {
                    run = false;
                    udp?.Dispose();
                }
            }
        }

        private void OnError(Exception e)
        {
            OnEvent(Errored, new ErroredEventArgs(e));
        }

        private async Task HandleRequest(byte[] data, IPEndPoint remote)
        {
            Request request = null;
            try
            {
                Console.WriteLine("接收到数据:{0}", string.Join(" ", data.Select(x => x.ToString("X2"))));
                request = Request.FromArray(data);
                OnEvent(Requested, new RequestedEventArgs(request, data, remote));

                IResponse response = await resolver.Resolve(request).ConfigureAwait(false);
                try
                {
                    OnEvent(Responded, new RespondedEventArgs(request, response, data, remote));
                }
                catch (Exception ex)
                {
                    OnError(ex);
                }
                finally
                {
                    Console.WriteLine("发送响应数据:{0}====>{1}", remote.ToString(), string.Join(" ", response.ToArray().Select(x => x.ToString("X2"))));
                    await udp
                        .SendAsync(response.ToArray(), response.Size, remote)
                        .WithCancellationTimeout(TimeSpan.FromMilliseconds(UDP_TIMEOUT)).ConfigureAwait(false);
                }
            }
            catch (SocketException e) { OnError(e); }
            catch (ArgumentException e) { OnError(e); }
            catch (IndexOutOfRangeException e) { OnError(e); }
            catch (OperationCanceledException e) { OnError(e); }
            catch (IOException e) { OnError(e); }
            catch (ObjectDisposedException e) { OnError(e); }
            catch (ResponseException e)
            {
                IResponse response = e.Response;

                if (response == null)
                {
                    response = Response.FromRequest(request);
                }

                try
                {
                    await udp
                        .SendAsync(response.ToArray(), response.Size, remote)
                        .WithCancellationTimeout(TimeSpan.FromMilliseconds(UDP_TIMEOUT)).ConfigureAwait(false);
                }
                catch (SocketException) { }
                catch (OperationCanceledException) { }
                finally { OnError(e); }
            }
        }

        public class RequestedEventArgs : EventArgs
        {
            public RequestedEventArgs(IRequest request, byte[] data, IPEndPoint remote)
            {
                Request = request;
                Data = data;
                Remote = remote;
            }

            public IRequest Request { get; }
            public byte[] Data { get; }
            public IPEndPoint Remote { get; }
        }

        public class RespondedEventArgs : EventArgs
        {
            public RespondedEventArgs(IRequest request, IResponse response, byte[] data, IPEndPoint remote)
            {
                Request = request;
                Response = response;
                Data = data;
                Remote = remote;
            }

            public IRequest Request { get; }
            public IResponse Response { get; }
            public byte[] Data { get; }
            public IPEndPoint Remote { get; }
        }

        public class ErroredEventArgs : EventArgs
        {
            public ErroredEventArgs(Exception e)
            {
                Exception = e;
            }

            public Exception Exception { get; }
        }

        private class FallbackRequestResolver : IRequestResolver
        {
            private IRequestResolver[] resolvers;

            public FallbackRequestResolver(params IRequestResolver[] resolvers)
            {
                this.resolvers = resolvers;
            }

            public async Task<IResponse> Resolve(IRequest request, CancellationToken cancellationToken = default(CancellationToken))
            {
                IResponse response = null;

                foreach (IRequestResolver resolver in resolvers)
                {
                    response = await resolver.Resolve(request, cancellationToken).ConfigureAwait(false);
                    if (response.AnswerRecords.Count > 0) break;
                }

                return response;
            }
        }
    }
}
