using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Configuration
{
    public class NodeConfiguration : AbstractPropertyConfiguration
    {
        public string Id { get; set; }

        public bool IsLazy { get; set; }

        public string Path { get; set; }

        public bool InferType { get; set; }
    }
}
