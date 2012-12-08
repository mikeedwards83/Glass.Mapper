using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Glass.Mapper.Configuration.Attributes
{

   public class AttributeConfigurationLoader<T, K> : IConfigurationLoader 
        where T : AbstractTypeConfiguration, new() 
        where K: AbstractPropertyConfiguration, new ()
    {
        private readonly string[] _assemblies;


        public AttributeConfigurationLoader(params string [] assemblies)
        {
            _assemblies = assemblies;
        }

        public static Assembly FindAssembly(string assemblyName)
        {
            try
            {
                assemblyName = assemblyName.ToLowerInvariant();
                if (assemblyName.EndsWith(".dll") || assemblyName.EndsWith(".exe"))
                {
                    return Assembly.LoadFrom(assemblyName);
                }
                else
                {
                    //try to find a dll or exe
                    //TODO: can we move this to config
                    return Assembly.LoadFrom(assemblyName + ".dll") ?? Assembly.LoadFrom(assemblyName + ".exe");
                }
            }catch(FileNotFoundException ex)
            {
                throw new ConfigurationException("Could not find assembly called {0}".Formatted(assemblyName), ex);
            }
        }

        public IEnumerable<AbstractTypeConfiguration> Load()
        {
            //this should mean that things evaluate lazily
            return _assemblies
                .Select(assemblyName =>
                            {
                                var assembly = FindAssembly(assemblyName);
                                return LoadFromAssembly(assembly);
                            })
                .Aggregate((x, y) => x.Union(y));
         
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
                            foreach(var property in LoadPropertiesFromType(type))
                            {
                                if(property != null)
                                    config.AddProperty(property);
                            }

                            ConfigCreated(config);
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

       /// <summary>
       /// This method is called after a configuration has been loaded. Use this method for any specific
       /// post configuration processing
       /// </summary>
       /// <param name="config"></param>
       protected virtual void ConfigCreated(AbstractTypeConfiguration config)
       {
       }

       protected IEnumerable<AbstractPropertyConfiguration> LoadPropertiesFromType(Type type)
        {
            //we have to get the property definition from the declaring type so that 
            //we can set private setters.
            var properties = Utilities.GetAllProperties(type);

            foreach(var property in properties)
            {
                var config = ProcessProperty(property);
                if (config == null) continue;
                yield return config;
            }
           
        }
   
        protected AbstractPropertyConfiguration ProcessProperty(PropertyInfo property)
        {
            if (property != null)
            {
                var attr = GetPropertyAttribute(property);

                //if we can't get a Sitecore attribute from current property we search down the 
                // inheritence chain to find the first declared attribute.
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

        public static AbstractPropertyAttribute GetPropertyAttribute(PropertyInfo info)
        {
            var attrs = info.GetCustomAttributes(true);
            var attr = attrs.FirstOrDefault(y => y is AbstractPropertyAttribute) as AbstractPropertyAttribute;
            return attr;
        }
    }
}
