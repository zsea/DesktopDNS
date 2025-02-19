using System;
using System.Collections.Generic;
using System.IO;
using DNS.Protocol.Utils;

namespace DNS.Protocol.ResourceRecords {
    public abstract class BaseResourceRecord : IResourceRecord {
        private IResourceRecord record;

        public BaseResourceRecord(IResourceRecord record) {
            this.record = record;
        }

        public Domain Name {
            get { return record.Name; }
        }

        public RecordType Type {
            get { return record.Type; }
        }

        public RecordClass Class {
            get { return record.Class; }
        }

        public TimeSpan TimeToLive {
            get { return record.TimeToLive; }
        }

        public int DataLength {
            get { return record.DataLength; }
        }

        public byte[] Data {
            get { return record.Data; }
        }

        public int Size {
            get { return record.Size; }
        }

        public byte[] ToArray() {
            return record.ToArray();
        }
        protected ObjectStringify Stringify()
        {
            return new ObjectStringify()
                .Add(nameof(Name), this.Name)
                .Add(nameof(Type), this.Type)
                .Add(nameof(Class), this.Class)
                .Add(nameof(TimeToLive), this.TimeToLive)
                .Add(nameof(DataLength), this.DataLength)
                ;
        } 
        //internal ObjectStringifier Stringify() {
        //    return ObjectStringifier.New(this)
        //        .Add(nameof(Name), nameof(Type), nameof(Class), nameof(TimeToLive), nameof(DataLength));
        //}
    }
}
