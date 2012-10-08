using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Configuration
{
    /// <summary>
    /// Represents the configuration for a property on a .Net type
    /// </summary>
    public abstract class AbstractPropertyConfiguration
    {
        public PropertyInfo PropertyInfo { get;  set; }
    }
}
