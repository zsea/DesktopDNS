using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNS.Protocol.Utils
{
    public class ObjectStringify
    {
        private List<KeyValuePair<string, object>> items=new List<KeyValuePair<string, object>>();
        public ObjectStringify() { }
        public static ObjectStringify New()
        {
            return new ObjectStringify();
        }
        public ObjectStringify Add(string name,object value)
        {
            items.Add(new KeyValuePair<string, object>(name,value));
            return this;
        }
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.Append("{");

            foreach (KeyValuePair<string, object> item in items)
            {
                result
                    .Append(item.Key)
                    .Append("=")
                    .Append(item.Value?.ToString())
                    .Append(", ");
            }

            if (result.Length > 1)
            {
                result.Remove(result.Length - 2, 2);
            }

            return result.Append("}").ToString();
        }
    }
}
