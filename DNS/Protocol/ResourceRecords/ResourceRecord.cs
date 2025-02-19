using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DNS.Protocol.Utils;

namespace DNS.Protocol.ResourceRecords {
    public class ResourceRecord : IResourceRecord {
        private Domain domain;
        private RecordType type;
        private RecordClass klass;
        private TimeSpan ttl;
        private byte[] data;

        public static IList<ResourceRecord> GetAllFromArray(byte[] message, int offset, int count) {
            return GetAllFromArray(message, offset, count, out offset);
        }

        public static IList<ResourceRecord> GetAllFromArray(byte[] message, int offset, int count, out int endOffset) {
            IList<ResourceRecord> records = new List<ResourceRecord>(count);

            for (int i = 0; i < count; i++) {
                records.Add(FromArray(message, offset, out offset));
            }

            endOffset = offset;
            return records;
        }

        public static ResourceRecord FromArray(byte[] message, int offset) {
            return FromArray(message, offset, out offset);
        }

        public static ResourceRecord FromArray(byte[] message, int offset, out int endOffset) {
            Domain domain = Domain.FromArray(message, offset, out offset);
            Tail tail = Tail.FromArray(message,offset,Tail.SIZE);// Marshalling.Struct.GetStruct<Tail>(message, offset, Tail.SIZE);

            byte[] data = new byte[tail.DataLength];

            offset += Tail.SIZE;
            Array.Copy(message, offset, data, 0, data.Length);

            endOffset = offset + data.Length;

            return new ResourceRecord(domain, data, tail.Type, tail.Class, tail.TimeToLive);
        }

        public static ResourceRecord FromQuestion(Question question, byte[] data, TimeSpan ttl = default(TimeSpan)) {
            return new ResourceRecord(question.Name, data, question.Type, question.Class, ttl);
        }

        public ResourceRecord(Domain domain, byte[] data, RecordType type,
                RecordClass klass = RecordClass.IN, TimeSpan ttl = default(TimeSpan)) {
            this.domain = domain;
            this.type = type;
            this.klass = klass;
            this.ttl = ttl;
            this.data = data;
        }

        public Domain Name {
            get { return domain; }
        }

        public RecordType Type {
            get { return type; }
        }

        public RecordClass Class {
            get { return klass; }
        }

        public TimeSpan TimeToLive {
            get { return ttl; }
        }

        public int DataLength {
            get { return data.Length; }
        }

        public byte[] Data {
            get { return data; }
        }

        public int Size {
            get { return domain.Size + Tail.SIZE + data.Length; }
        }

        public byte[] ToArray() {
            ByteStream result = new ByteStream(Size);

            result
                .Append(domain.ToArray())
                //.Append(Marshalling.Struct.GetBytes<Tail>(new Tail() {
                //    Type = Type,
                //    Class = Class,
                //    TimeToLive = ttl,
                //    DataLength = data.Length
                //}))
                .Append(new Tail()
                {
                    Type = Type,
                    Class = Class,
                    TimeToLive = ttl,
                    DataLength = data.Length
                }.GetBytes())
                .Append(data);

            return result.ToArray();
        }

        public override string ToString() {
            //return ObjectStringifier.New(this)
            //    .Add(nameof(Name), nameof(Type), nameof(Class), nameof(TimeToLive), nameof(DataLength))
            //    .ToString();
            return ObjectStringify.New()
                .Add(nameof(Name),this.Name)
                .Add(nameof(Type), this.Type)
                .Add(nameof(Class), this.Class)
                .Add(nameof(TimeToLive), this.TimeToLive)
                .Add(nameof(DataLength), this.DataLength)
                .ToString();
        }

        private struct Tail {
            public const int SIZE = 10;

            private ushort type;
            private ushort klass;
            private uint ttl;
            private ushort dataLength;
            public static Tail FromArray(byte[] messages,int dataOffset,int size)
            {
                Tail tail = new Tail();
                tail.type = (ushort)(messages[dataOffset++] << 8 | messages[dataOffset++]);
                tail.klass = (ushort)(messages[dataOffset++] << 8 | messages[dataOffset++]);
                tail.ttl = (uint)((messages[dataOffset++] << 24) | (messages[dataOffset++] << 16) | (messages[dataOffset++] << 8) | messages[dataOffset++]);
                tail.dataLength = (ushort)(messages[dataOffset++] << 8 | messages[dataOffset++]);
                return tail;
            }
            public byte[] GetBytes()
            {
                byte[] bytes = new byte[SIZE]; // type(2) + klass(2) + ttl(4) + dataLength(2) = 10字节

                int offset = 0;

                // 写入 type（大端序）
                bytes[offset++] = (byte)(type >> 8);    // 高8位
                bytes[offset++] = (byte)(type & 0xFF);  // 低8位

                // 写入 klass（大端序）
                bytes[offset++] = (byte)(klass >> 8);   // 高8位
                bytes[offset++] = (byte)(klass & 0xFF); // 低8位

                // 写入 ttl（大端序）
                bytes[offset++] = (byte)(ttl >> 24);     // 最高8位
                bytes[offset++] = (byte)(ttl >> 16);     // 次高8位
                bytes[offset++] = (byte)(ttl >> 8);      // 次低8位
                bytes[offset++] = (byte)(ttl & 0xFF);    // 最低8位

                // 写入 dataLength（大端序）
                bytes[offset++] = (byte)(dataLength >> 8);   // 高8位
                bytes[offset] = (byte)(dataLength & 0xFF);   // 低8位

                return bytes;
            }
            public RecordType Type {
                get { return (RecordType) type; }
                set { type = (ushort) value; }
            }

            public RecordClass Class {
                get { return (RecordClass) klass; }
                set { klass = (ushort) value; }
            }

            public TimeSpan TimeToLive {
                get { return TimeSpan.FromSeconds(ttl); }
                set { ttl = (uint) value.TotalSeconds; }
            }

            public int DataLength {
                get { return dataLength; }
                set { dataLength = (ushort) value; }
            }
        }
    }
}
