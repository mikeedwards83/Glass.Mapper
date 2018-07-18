using System;
using System.Collections.Generic;
using System.Linq;

namespace Glass.Mapper.Configuration.Attributes
{
    /// <summary>
    /// Class IdAttribute
    /// </summary>
    public abstract class IdAttribute : AbstractPropertyAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdAttribute"/> class.
        /// </summary>
        /// <param name="acceptedTypes">The accepted types.</param>
        public IdAttribute(IEnumerable<Type> acceptedTypes)
        {
            AcceptedTypes= acceptedTypes;
        }

        /// <summary>
        /// Gets the accepted types.
        /// </summary>
        /// <value>The accepted types.</value>
        protected IEnumerable<Type> AcceptedTypes { get; private set; }


        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="config">The config.</param>
        /// <exception cref="Glass.Mapper.Configuration.ConfigurationException">Property type {0} not supported as an ID on {1}
        ///                     .Formatted(propertyInfo.PropertyType.FullName, propertyInfo.DeclaringType.FullName)</exception>
        public virtual void Configure(System.Reflection.PropertyInfo propertyInfo, IdConfiguration config)
        {
            if(!AcceptedTypes.Any(x=>propertyInfo.PropertyType == x))
                throw new ConfigurationException("Property type {0} not supported as an ID on {1}"
                    .Formatted(propertyInfo.PropertyType.FullName, propertyInfo.DeclaringType.FullName));

            config.Type = propertyInfo.PropertyType;
            base.Configure(propertyInfo, config);
        
        }
    }
}




