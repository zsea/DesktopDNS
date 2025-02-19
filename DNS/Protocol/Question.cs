using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using DNS.Protocol.Utils;

namespace DNS.Protocol {
    public class Question : IMessageEntry {
        public static IList<Question> GetAllFromArray(byte[] message, int offset, int questionCount) {
            return GetAllFromArray(message, offset, questionCount, out offset);
        }

        public static IList<Question> GetAllFromArray(byte[] message, int offset, int questionCount, out int endOffset) {
            IList<Question> questions = new List<Question>(questionCount);
            for (int i = 0; i < questionCount; i++) {

                questions.Add(FromArray(message, offset, out offset));
            }
            endOffset = offset;
            return questions;
        }

        public static Question FromArray(byte[] message, int offset) {
            return FromArray(message, offset, out offset);
        }

        public static Question FromArray(byte[] message, int offset, out int endOffset) {
            Domain domain = Domain.FromArray(message, offset, out offset);
            Tail tail = Tail.FromArray(message,offset,Tail.SIZE);// Marshalling.Struct.GetStruct<Tail>(message, offset, Tail.SIZE);
            endOffset = offset + Tail.SIZE;

            return new Question(domain, tail.Type, tail.Class);
        }

        private Domain domain;
        private RecordType type;
        private RecordClass klass;

        public Question(Domain domain, RecordType type = RecordType.A, RecordClass klass = RecordClass.IN) {
            this.domain = domain;
            this.type = type;
            this.klass = klass;
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

        public int Size {
            get { return domain.Size + Tail.SIZE; }
        }

        public byte[] ToArray() {
            ByteStream result = new ByteStream(Size);

            result
                .Append(domain.ToArray())
                //.Append(Marshalling.Struct.GetBytes(new Tail { Type = Type, Class = Class }));
                .Append(new Tail { Type = Type, Class = Class }.GetBytes());

            return result.ToArray();
        }

        public override string ToString() {
            
            return new ObjectStringify()
                .Add(nameof(Name),this.Name)
                .Add(nameof(Type), this.Type)
                .Add(nameof(Class), this.Class)
                .ToString();
        }

        private struct Tail {
            public const int SIZE = 4;

            private ushort type;
            private ushort klass;
            public static Tail FromArray(byte[] messages,int dataOffset,int size)
            {
                Tail tail = new Tail();
                tail.type = (ushort)(messages[dataOffset++] << 8 | messages[dataOffset++]);
                tail.klass = (ushort)(messages[dataOffset++] << 8 | messages[dataOffset++]);
                return tail;
            }
            public byte[] GetBytes()
            {
                byte[] bytes = new byte[4]; // type(2字节) + klass(2字节) = 4字节

                // 将 type 转换为大端序字节（高位在前）
                bytes[0] = (byte)(type >> 8);  // 取高8位
                bytes[1] = (byte)(type & 0xFF); // 取低8位

                // 将 klass 转换为大端序字节（高位在前）
                bytes[2] = (byte)(klass >> 8);  // 取高8位
                bytes[3] = (byte)(klass & 0xFF); // 取低8位

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
        }
    }
}
