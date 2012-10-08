using System;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Configuration
{
    /// <summary>
    /// Represents the configuration for a .Net type
    /// </summary>
    public abstract class AbstractTypeConfiguration
    {
        public Type Type { get; private set; }

        public virtual void Configure(AbstractTypeAttribute attr, Type type)
        {
            Type = type;
        }
    }
}

