using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Glass.Mapper.Configuration.Attributes
{
    public class AttributeTypeLoader : IConfigurationLoader
    {
        private readonly Type _type;

        public AttributeTypeLoader(Type type)
        {
            _type = type;
        }

        public IEnumerable<AbstractTypeConfiguration> Load()
        {
            return new []{LoadType(_type)};
        }

        public AbstractTypeConfiguration LoadType(Type type)
        {
            AbstractTypeConfiguration config = null;

            IEnumerable<object> attrs = type.GetCustomAttributes(true);
            var attr = attrs.FirstOrDefault(y => y is AbstractTypeAttribute) as AbstractTypeAttribute;

            if (attr != null)
            {
                config = attr.Configure(type);
                if (config != null)
                {
                    //load the properties on the type
                    foreach (var property in LoadPropertiesFromType(type))
                    {
                        if (property != null)
                            config.AddProperty(property);
                    }
                }
            }

            return config;
        }


        /// <summary>
        /// Loads the type of the properties from.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>IEnumerable{AbstractPropertyConfiguration}.</returns>
        public IEnumerable<AbstractPropertyConfiguration> LoadPropertiesFromType(Type type)
        {
            //we have to get the property definition from the declaring type so that 
            //we can set private setters.
            var properties = Utilities.GetAllProperties(type);

            foreach (var property in properties)
            {
                var config = ProcessProperty(property);
                if (config == null) continue;
                yield return config;
            }

        }

        /// <summary>
        /// Processes the property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>AbstractPropertyConfiguration.</returns>
        public static AbstractPropertyConfiguration ProcessProperty(PropertyInfo property)
        {
            if (property != null)
            {
                var attr = GetPropertyAttribute(property);

                //if we can't get a umbraco attribute from current property we search down the 
                // inheritance chain to find the first declared attribute.
                if (attr == null)
                {
                    var interfaces = property.DeclaringType.GetInterfaces();

                    //TODO: put a check in here to check that two interface don't implement an attribute
                    foreach (var inter in interfaces)
                    {
                        var interProperty = inter.GetProperty(property.Name);
                        if (interProperty != null)
                            attr = GetPropertyAttribute(interProperty);

                        if (attr != null) break;
                    }
                }

                if (attr != null)
                {

                    var config = attr.Configure(property);
                    return config;
                }

            }
            return null;

        }

        /// <summary>
        /// Gets the property attribute.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <returns>AbstractPropertyAttribute.</returns>
        public static AbstractPropertyAttribute GetPropertyAttribute(PropertyInfo info)
        {
            var attrs = info.GetCustomAttributes(true);
            var attr = attrs.FirstOrDefault(y => y is AbstractPropertyAttribute) as AbstractPropertyAttribute;
            return attr;
        }
    }
}
