using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Glass.Mapper.Configuration.Attributes
{
    public class AttributeConfigurationLoader<T, K> : IConfigurationLoader 
        where T : AbstractTypeConfiguration, new() 
        where K: AbstractPropertyConfiguration, new ()
    {



        public IEnumerable<AbstractTypeConfiguration> Load()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Processes a specific assembly 
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        protected IEnumerable<T> LoadFromAssembly(Assembly assembly)
        {
            var configs = new List<T>();

            if (assembly != null)
            {
                try
                {
                    var types = assembly.GetTypes();

                    foreach (var type in types)
                    {
                        IEnumerable<object> attrs = type.GetCustomAttributes(true);
                        var attr = attrs.FirstOrDefault(y => y is AbstractTypeAttribute) as AbstractTypeAttribute;

                        if (attr != null)
                        {
                            var config = new T();
                            attr.Configure(type, config);
                            configs.Add(config);

                            //load the properties on the type
                            foreach(var property in LoadFromType(type))
                            {
                                if(property != null)
                                    config.AddProperty(property);
                            }
                        }
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    throw new ConfigurationException(
                        "Failed to load types {0}".Formatted(ex.LoaderExceptions.First().Message), ex);
                }
            }

            return configs;
        }

        protected IEnumerable<AbstractPropertyConfiguration> LoadFromType(Type type)
        {
            //we have to get the property definition fromt he declaring type so that 
            //we can set private setters.
            var finalInfo = Utilities.GetProperty(type.DeclaringType, type.Name);


            if (finalInfo != null)
            {
                var attr = GetPropertyAttribute(finalInfo);
                
                //if we can't get a Sitecore attribute from current property we search down the 
                // inheritence chain to find the first declared attribute.
                if (attr == null)
                {
                    var interfaces = finalInfo.DeclaringType.GetInterfaces();
                    string propertyName = finalInfo.Name;

                    foreach (var inter in interfaces)
                    {
                        finalInfo = inter.GetProperty(propertyName);
                        if (finalInfo != null)
                            attr = GetPropertyAttribute(finalInfo);

                        if (attr != null) break;
                    }

                    if (attr != null)
                    {
                        var config = new K();
                        attr.Configure(finalInfo, config);
                        yield return config;
                    }
                }

                yield return null;

            }

           
        }

        public static AbstractPropertyAttribute GetPropertyAttribute(PropertyInfo info)
        {
            var attrs = info.GetCustomAttributes(true);
            var attr = attrs.FirstOrDefault(y => y is AbstractPropertyAttribute) as AbstractPropertyAttribute;
            return attr;
        }
    }
}
