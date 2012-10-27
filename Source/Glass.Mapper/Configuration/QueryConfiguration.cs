using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Configuration
{
    public class QueryConfiguration : AbstractPropertyConfiguration
    {
        public string Query { get; set; }

        public bool IsLazy { get; set; }

        public bool IsRelative { get; set; }

        public bool InferType { get; set; }
    }
}
