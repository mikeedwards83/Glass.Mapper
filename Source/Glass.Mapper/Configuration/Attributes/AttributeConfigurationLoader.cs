/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Glass.Mapper.Configuration.Attributes
{

    /// <summary>
    /// Class AttributeConfigurationLoader
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
   public class AttributeConfigurationLoader<T, K> : IConfigurationLoader 
        where T : AbstractTypeConfiguration, new() 
        where K: AbstractPropertyConfiguration, new ()
    {
        private readonly string[] _assemblies;


        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeConfigurationLoader{T, K}"/> class.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        public AttributeConfigurationLoader(params string [] assemblies)
        {
            _assemblies = assemblies;
        }

        /// <summary>
        /// Finds the assembly.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns>Assembly.</returns>
        /// <exception cref="Glass.Mapper.Configuration.ConfigurationException">Could not find assembly called {0}.Formatted(assemblyName)</exception>
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
                    var path = "./";

                    try
                    {
                        if (HttpContext.Current != null)
                        {
                            path=   HttpContext.Current.Server.MapPath("/bin");
                            path += "/";
                        }
                    }
                    catch
                    {
                    }

                    return Assembly.LoadFrom(path+assemblyName + ".dll") ?? Assembly.LoadFrom(path+assemblyName + ".exe");
                }
            }catch(FileNotFoundException ex)
            {
                throw new ConfigurationException("Could not find assembly called {0}".Formatted(assemblyName), ex);
            }
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        /// <returns>IEnumerable{AbstractTypeConfiguration}.</returns>
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
        /// <param name="assembly">The assembly.</param>
        /// <returns>IEnumerable{`0}.</returns>
        /// <exception cref="Glass.Mapper.Configuration.ConfigurationException">Failed to load types {0}.Formatted(ex.LoaderExceptions.First().Message)</exception>
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
        /// <param name="config">The config.</param>
       protected virtual void ConfigCreated(AbstractTypeConfiguration config)
       {
       }

       /// <summary>
       /// Loads the type of the properties from.
       /// </summary>
       /// <param name="type">The type.</param>
       /// <returns>IEnumerable{AbstractPropertyConfiguration}.</returns>
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

       /// <summary>
       /// Processes the property.
       /// </summary>
       /// <param name="property">The property.</param>
       /// <returns>AbstractPropertyConfiguration.</returns>
        protected AbstractPropertyConfiguration ProcessProperty(PropertyInfo property)
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




