using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Configuration
{
    public class ParentConfiguration : AbstractPropertyConfiguration
    {
        public bool IsLazy { get; set; }

        public bool InferType { get; set; }
    }
}
