using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Configuration.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public abstract class AbstractTypeAttribute : Attribute , ITypeSetting
    {
        public AbstractTypeAttribute()
        { }

        public virtual void Configure(Type type, AbstractTypeConfiguration config)
        {
            config.Type = type;
        }
    }
}
