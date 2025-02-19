using System;
using System.Runtime.InteropServices;

namespace DNS.Protocol.ResourceRecords {
    public class ServiceResourceRecord : BaseResourceRecord {
        private static IResourceRecord Create(Domain domain, ushort priority, ushort weight, ushort port, Domain target, TimeSpan ttl) {
            byte[] trg = target.ToArray();
            byte[] data = new byte[Head.SIZE + trg.Length];

            Head head = new Head() {
                Priority = priority,
                Weight = weight,
                Port = port
            };

            //Marshalling.Struct.GetBytes(head).CopyTo(data, 0);
            head.GetBytes().CopyTo(data, 0);
            trg.CopyTo(data, Head.SIZE);

            return new ResourceRecord(domain, data, RecordType.SRV, RecordClass.IN, ttl);
        }

        public ServiceResourceRecord(IResourceRecord record, byte[] message, int dataOffset) : base(record) {
            Head head = Head.FromArray(message,dataOffset,Head.SIZE) ;// Marshalling.Struct.GetStruct<Head>(message, dataOffset, Head.SIZE);

            Priority = head.Priority;
            Weight = head.Weight;
            Port = head.Port;
            Target = Domain.FromArray(message, dataOffset + Head.SIZE);
        }

        public ServiceResourceRecord(Domain domain, ushort priority, ushort weight, ushort port, Domain target, TimeSpan ttl = default(TimeSpan)) :
                base(Create(domain, priority, weight, port, target, ttl)) {
            Priority = priority;
            Weight = weight;
            Port = port;
            Target = target;
        }

        public ushort Priority { get; }
        public ushort Weight { get; }
        public ushort Port { get; }
        public Domain Target { get; }

        public override string ToString() {
            //return Stringify().Add("Priority", "Weight", "Port", "Target").ToString();
            return Stringify()
                .Add(nameof(Priority), this.Priority)
                .Add(nameof(Weight), this.Weight)
                .Add(nameof(Port), this.Port)
                .Add(nameof(Target), this.Target)
                .ToString();
        }

        private struct Head {
            public const int SIZE = 6;

            private ushort priority;
            private ushort weight;
            private ushort port;

            public ushort Priority {
                get { return priority; }
                set { priority = value; }
            }

            public ushort Weight {
                get { return weight; }
                set { weight = value; }
            }

            public ushort Port {
                get { return port; }
                set { port = value; }
            }
            public static Head FromArray(byte[] message, int dataOffset,int size)
            {
                Head head = new Head();
                head.priority=(ushort)(message[dataOffset++]<<8|message[dataOffset++]);
                head.weight = (ushort)(message[dataOffset++] << 8 | message[dataOffset++]);
                head.port = (ushort)(message[dataOffset++] << 8 | message[dataOffset++]);
                return head;
            }
            public byte[] GetBytes() {
                byte[] bytes = new byte[SIZE]; // priority(2) + weight(2) + port(2) = 6字节

                // 写入 priority（大端序）
                bytes[0] = (byte)(priority >> 8);    // 高8位
                bytes[1] = (byte)(priority & 0xFF);  // 低8位

                // 写入 weight（大端序）
                bytes[2] = (byte)(weight >> 8);      // 高8位
                bytes[3] = (byte)(weight & 0xFF);    // 低8位

                // 写入 port（大端序）
                bytes[4] = (byte)(port >> 8);        // 高8位
                bytes[5] = (byte)(port & 0xFF);      // 低8位

                return bytes;
            }
        }
    }
}
