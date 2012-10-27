using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Configuration
{
    public class FieldConfiguration : AbstractPropertyConfiguration
    {
        public string FieldName { get; set; }

        public bool ReadOnly { get; set; }
    }
}
