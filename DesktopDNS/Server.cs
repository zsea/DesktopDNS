using DNS.Protocol;
using DNS.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopDNS
{
    internal static class Server
    {
        public static Configure.Server configure=new Configure.Server();
        public static bool LoadConfigure()
        {
            Configure.Server server = Configure.Server.Load();
            configure = server;
            return true;
        }
        static Server() {
            LoadConfigure();
        }
        public static long StartupTime { get;private set; }

        private static DnsServer? DnsServer { get; set; }
        public static bool IsRuning { get; private set; }
        public static int ListenPort { get;private set; } 
        public static long RequestedTimes { get; private set; }
        public static long LocalResolveTimes { get;  set; }
        public static string DefaultServer { get; private set; } = "8.8.8.8";
        public static Action<DnsServer.RequestedEventArgs>? OnRequested { get; set; } = null;
        public static Action<DNS.Server.DnsServer.RespondedEventArgs>? OnResponded { get; set; } = null;
        public static bool Start() {
            if (DnsServer != null) return false;
            DefaultServer = configure.DefaultServer ?? "8.8.8.8";
            DnsServer server = new DnsServer(new LocalRequestResolver(configure), DefaultServer);
            // Log every request
            server.Requested += (sender, e) => {
                RequestedTimes++;
                
                if (OnRequested != null) OnRequested(e);

            };
            // On every successful request log the request and the response
            server.Responded += (sender, e) => {
                foreach (var item in e.Response.AnswerRecords)
                {
                    if (item == null) continue;
                    Logger.Debug(item.ToString() ?? "");
                }
                if (OnResponded!=null) OnResponded(e);
                
            };
            // Log errors
            server.Errored += (sender, e) => {
                Logger.Error(e.Exception.Message+"\r\n"+e.Exception.StackTrace);
                throw e.Exception;
            };
            ListenPort = configure.Port == 0 ? 53 : configure.Port;
            if (!server.Listen(ListenPort))
            {
                server.Dispose();
                //server = null;
                return false;
            }
            //server.Listen
            DnsServer = server;
            StartupTime = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            IsRuning = true;
            Logger.Info("Service startup.");
            return true;
        }



        public static void Shutodwn()
        {
            if(Server.DnsServer == null) return;
            DnsServer server=Server.DnsServer;
            server.Shutdown();
            //server.Dispose();
            StartupTime = 0;
            LocalResolveTimes = 0;
            RequestedTimes = 0;
            DnsServer = null;
            IsRuning= false;
            Logger.Info("Service stop.");
        }
    }
}
