using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.Sc
{
    public struct Query
    {
        public string Value { get; }

        public Query(string value)
        {
            Value = value;
        }

        public static Query New(string value)
        {
            return new Query(value);
        }

        public bool IsNullOrEmpty()
        {
            return Value.IsNullOrEmpty();
        }
    }
}
