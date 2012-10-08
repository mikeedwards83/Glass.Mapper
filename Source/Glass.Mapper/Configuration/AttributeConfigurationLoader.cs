using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Configuration
{
    public class AttributeConfigurationLoader<T> : IConfigurationLoader where T : AbstractTypeConfiguration, new()
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
        protected IEnumerable<AbstractTypeConfiguration> LoadFromAssembly(Assembly assembly)
        {
            var configs = new List<AbstractTypeConfiguration>();

            if (assembly != null)
            {
                try
                {
                    var types = assembly.GetTypes();

                    foreach (var type in types)
                    {
                        IEnumerable<object> attrs = type.GetCustomAttributes(true);
                        var attr = attrs.FirstOrDefault(y => y is AbstractClassAttribute) as AbstractClassAttribute;

                        if (attr != null)
                        {
                            AbstractTypeConfiguration config = new T();
                            config.Configure(attr, type);
                            configs.Add(config);
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
    }
}
