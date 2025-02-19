using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using DNS.Protocol.Utils;

namespace DNS.Protocol.ResourceRecords
{
    public class StartOfAuthorityResourceRecord : BaseResourceRecord
    {
        private static IResourceRecord Create(Domain domain, Domain master, Domain responsible, long serial,
                TimeSpan refresh, TimeSpan retry, TimeSpan expire, TimeSpan minTtl, TimeSpan ttl)
        {
            ByteStream data = new ByteStream(Options.SIZE + master.Size + responsible.Size);
            Options tail = new Options()
            {
                SerialNumber = serial,
                RefreshInterval = refresh,
                RetryInterval = retry,
                ExpireInterval = expire,
                MinimumTimeToLive = minTtl
            };

            data
                .Append(master.ToArray())
                .Append(responsible.ToArray())
                //.Append(Marshalling.Struct.GetBytes(tail));
                .Append(tail.GetBytes());

            return new ResourceRecord(domain, data.ToArray(), RecordType.SOA, RecordClass.IN, ttl);
        }

        public StartOfAuthorityResourceRecord(IResourceRecord record, byte[] message, int dataOffset)
            : base(record)
        {
            MasterDomainName = Domain.FromArray(message, dataOffset, out dataOffset);
            ResponsibleDomainName = Domain.FromArray(message, dataOffset, out dataOffset);

            Options tail = Options.FromArray(message,dataOffset,Options.SIZE) ;// Marshalling.Struct.GetStruct<Options>(message, dataOffset, Options.SIZE);

            SerialNumber = tail.SerialNumber;
            RefreshInterval = tail.RefreshInterval;
            RetryInterval = tail.RetryInterval;
            ExpireInterval = tail.ExpireInterval;
            MinimumTimeToLive = tail.MinimumTimeToLive;
        }

        public StartOfAuthorityResourceRecord(Domain domain, Domain master, Domain responsible, long serial,
                TimeSpan refresh, TimeSpan retry, TimeSpan expire, TimeSpan minTtl, TimeSpan ttl = default(TimeSpan)) :
            base(Create(domain, master, responsible, serial, refresh, retry, expire, minTtl, ttl))
        {
            MasterDomainName = master;
            ResponsibleDomainName = responsible;

            SerialNumber = serial;
            RefreshInterval = refresh;
            RetryInterval = retry;
            ExpireInterval = expire;
            MinimumTimeToLive = minTtl;
        }

        public StartOfAuthorityResourceRecord(Domain domain, Domain master, Domain responsible,
                Options options = default(Options), TimeSpan ttl = default(TimeSpan)) :
            this(domain, master, responsible, options.SerialNumber, options.RefreshInterval, options.RetryInterval,
                    options.ExpireInterval, options.MinimumTimeToLive, ttl)
        { }

        public Domain MasterDomainName { get; }
        public Domain ResponsibleDomainName { get; }
        public long SerialNumber { get; }
        public TimeSpan RefreshInterval { get; }
        public TimeSpan RetryInterval { get; }
        public TimeSpan ExpireInterval { get; }
        public TimeSpan MinimumTimeToLive { get; }

        public override string ToString()
        {
            //return Stringify().Add("MasterDomainName", "ResponsibleDomainName", "SerialNumber").ToString();
            return Stringify()
                .Add(nameof(MasterDomainName), this.MasterDomainName)
                .Add(nameof(ResponsibleDomainName), this.ResponsibleDomainName)
                .Add(nameof(SerialNumber), this.SerialNumber)
                .ToString();
        }


        public struct Options
        {
            public const int SIZE = 20;

            private uint serialNumber;
            private uint refreshInterval;
            private uint retryInterval;
            private uint expireInterval;
            private uint ttl;

            public static Options FromArray(byte[] messages, int dataOffset, int size)
            {
                Options options = new Options();
                options.serialNumber = (uint)(    (messages[dataOffset++] << 24) |     (messages[dataOffset++] << 16) |    (messages[dataOffset++] << 8) |    messages[dataOffset++]);
                options.refreshInterval = (uint)((messages[dataOffset++] << 24) | (messages[dataOffset++] << 16) | (messages[dataOffset++] << 8) | messages[dataOffset++]);
                options.retryInterval = (uint)((messages[dataOffset++] << 24) | (messages[dataOffset++] << 16) | (messages[dataOffset++] << 8) | messages[dataOffset++]);
                options.expireInterval = (uint)((messages[dataOffset++] << 24) | (messages[dataOffset++] << 16) | (messages[dataOffset++] << 8) | messages[dataOffset++]);
                options.ttl = (uint)((messages[dataOffset++] << 24) | (messages[dataOffset++] << 16) | (messages[dataOffset++] << 8) | messages[dataOffset++]);
                return options;
            }
            public byte[] GetBytes()
            {
                byte[] bytes = new byte[SIZE]; // 5个uint字段 × 4字节 = 20字节
                int offset = 0;

                // 写入 serialNumber（大端序）
                bytes[offset++] = (byte)(serialNumber >> 24);  // 最高8位
                bytes[offset++] = (byte)(serialNumber >> 16);  // 次高8位
                bytes[offset++] = (byte)(serialNumber >> 8);   // 次低8位
                bytes[offset++] = (byte)(serialNumber & 0xFF); // 最低8位

                // 写入 refreshInterval（大端序）
                bytes[offset++] = (byte)(refreshInterval >> 24);
                bytes[offset++] = (byte)(refreshInterval >> 16);
                bytes[offset++] = (byte)(refreshInterval >> 8);
                bytes[offset++] = (byte)(refreshInterval & 0xFF);

                // 写入 retryInterval（大端序）
                bytes[offset++] = (byte)(retryInterval >> 24);
                bytes[offset++] = (byte)(retryInterval >> 16);
                bytes[offset++] = (byte)(retryInterval >> 8);
                bytes[offset++] = (byte)(retryInterval & 0xFF);

                // 写入 expireInterval（大端序）
                bytes[offset++] = (byte)(expireInterval >> 24);
                bytes[offset++] = (byte)(expireInterval >> 16);
                bytes[offset++] = (byte)(expireInterval >> 8);
                bytes[offset++] = (byte)(expireInterval & 0xFF);

                // 写入 ttl（大端序）
                bytes[offset++] = (byte)(ttl >> 24);
                bytes[offset++] = (byte)(ttl >> 16);
                bytes[offset++] = (byte)(ttl >> 8);
                bytes[offset] = (byte)(ttl & 0xFF); // 最后一个字节无需递增offset

                return bytes;
            }
            public long SerialNumber
            {
                get { return serialNumber; }
                set { serialNumber = (uint)value; }
            }

            public TimeSpan RefreshInterval
            {
                get { return TimeSpan.FromSeconds(refreshInterval); }
                set { refreshInterval = (uint)value.TotalSeconds; }
            }

            public TimeSpan RetryInterval
            {
                get { return TimeSpan.FromSeconds(retryInterval); }
                set { retryInterval = (uint)value.TotalSeconds; }
            }

            public TimeSpan ExpireInterval
            {
                get { return TimeSpan.FromSeconds(expireInterval); }
                set { expireInterval = (uint)value.TotalSeconds; }
            }

            public TimeSpan MinimumTimeToLive
            {
                get { return TimeSpan.FromSeconds(ttl); }
                set { ttl = (uint)value.TotalSeconds; }
            }
        }
    }
}
