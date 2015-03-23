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
using System.Security.Cryptography;
using System.Web;

namespace Glass.Mapper.Configuration.Attributes
{

    /// <summary>
    /// Class AttributeConfigurationLoader
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    public class AttributeConfigurationLoader : IConfigurationLoader
    {
        private readonly string[] _assemblies;

        public virtual IEnumerable<string> AllowedNamespaces { get; set; } 

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeConfigurationLoader{T, K}"/> class.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        public AttributeConfigurationLoader(params string[] assemblies)
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
                            path = HttpContext.Current.Server.MapPath("/bin");
                            path += "/";
                        }
                    }
                    catch
                    {
                    }

                    return Assembly.LoadFrom(path + assemblyName + ".dll") ??
                           Assembly.LoadFrom(path + assemblyName + ".exe");
                }
            }
            catch (FileNotFoundException ex)
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
                        if(AllowedNamespaces == null || AllowedNamespaces.Any(x=> type.FullName.StartsWith(x)))
                        {
                            if (type.IsGenericTypeDefinition)
                            {
                                continue;
                            }

                            var loader = new AttributeTypeLoader(type);
                            var config = loader.Load().FirstOrDefault();

                            if (config != null)
                            {
                                configs.Add(config);
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



    }
}




