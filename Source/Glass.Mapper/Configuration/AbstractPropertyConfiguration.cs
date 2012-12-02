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

        public AbstractDataMapper Mapper { get; internal set; }

        public override string ToString()
        {
            if (PropertyInfo == null)
                return "AbstractPropertyConfiguration: Property: Null";

            return "AbstractPropertyConfiguration Property: {0} Type: {1} Assembly: {2}".Formatted(PropertyInfo.Name,
                                                                     PropertyInfo.ReflectedType.FullName,
                                                                     PropertyInfo.ReflectedType.Assembly.FullName);
        }
    }
}
