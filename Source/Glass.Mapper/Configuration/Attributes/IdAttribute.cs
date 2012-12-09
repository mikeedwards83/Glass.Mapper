using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Configuration.Attributes
{
    public abstract class IdAttribute : AbstractPropertyAttribute
    {
        public IdAttribute(IEnumerable<Type> acceptedTypes)
        {
            this.AcceptedTypes= acceptedTypes;
        }

        protected IEnumerable<Type> AcceptedTypes { get; private set; }

        public Type Type { get; set; }

        public virtual void Configure(System.Reflection.PropertyInfo propertyInfo, IdConfiguration config)
        {
            if(!AcceptedTypes.Any(x=>propertyInfo.PropertyType == x))
                throw new ConfigurationException("Property type {0} does not match required type {1} on {2}"
                    .Formatted(propertyInfo.PropertyType.FullName, Type.FullName, propertyInfo.DeclaringType.FullName));

            config.Type = this.Type;
            base.Configure(propertyInfo, config);
        
        }
    }
}
