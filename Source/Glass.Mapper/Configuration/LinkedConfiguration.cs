using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Configuration
{
    public class LinkedConfiguration : AbstractPropertyConfiguration
    {
        public bool IsLazy { get; set; }

        public bool InferType { get; set; }
    }
}
