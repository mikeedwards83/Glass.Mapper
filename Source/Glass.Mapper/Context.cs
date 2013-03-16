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
using System.Linq;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.DataMapperResolver;

namespace Glass.Mapper
{
    /// <summary>
    /// The context contains the configuration of Glass.Mapper
    /// </summary>
    public class Context
    {
        /// <summary>
        /// The default context name
        /// </summary>
        public const string DefaultContextName = "Default";

        #region STATICS

        /// <summary>
        /// The default Context. Used by services if no Context is specified.
        /// </summary>
        /// <value>The default.</value>
        public static Context Default { get; private set; }

        /// <summary>
        /// Contains the list of Contexts currently loaded.
        /// </summary>
        /// <value>The contexts.</value>
        public static IDictionary<string, Context> Contexts { get; private set; }

        /// <summary>
        /// Initializes static members of the <see cref="Context"/> class.
        /// </summary>
        static Context()
        {
            Contexts = new Dictionary<string, Context>();
        }
        /// <summary>
        /// Creates a Context and creates it as the default Context. This is assigned to the Default static property.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        /// <returns>Context.</returns>
        public static Context Create(IDependencyResolver resolver)
        {
            return Context.Create(resolver, DefaultContextName, true);
        }

        /// <summary>
        /// Creates a new context and adds it to the Contexts dictionary.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        /// <param name="contextName">The context name, used as the key in the Contexts dictionary.</param>
        /// <param name="isDefault">Indicates if this is the default context. If it is the context is assigned to the Default static property.</param>
        /// <returns>Context.</returns>
        /// <exception cref="System.NullReferenceException">No dependency resolver set.</exception>
        public static Context Create(IDependencyResolver resolver, string contextName, bool isDefault = false)
        {

            if (resolver == null)
                throw new NullReferenceException("No dependency resolver set.");

            var context = new Context();
            context.DependencyResolver = resolver;
            Contexts[contextName] = context;

            if (isDefault)
                Default = context;

            return context;
        }

        /// <summary>
        /// Clears all static and default contexts
        /// </summary>
        public static void Clear()
        {
            Default = null;
            Contexts = new Dictionary<string, Context>();

        }

        #endregion

        /// <summary>
        /// List of the type configurations loaded by this context
        /// </summary>
        /// <value>The type configurations.</value>
        public IDictionary<Type, AbstractTypeConfiguration> TypeConfigurations { get; private set; }

        /// <summary>
        /// The dependency resolver used by services using the context
        /// </summary>
        /// <value>The dependency resolver.</value>
        public IDependencyResolver DependencyResolver { get; set; }

        /// <summary>
        /// Prevents a default instance of the <see cref="Context"/> class from being created.
        /// </summary>
        private Context()
        {
            TypeConfigurations = new Dictionary<Type, AbstractTypeConfiguration>();
        }

        /// <summary>
        /// Gets a type configuration based on type
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>AbstractTypeConfiguration.</returns>
        public AbstractTypeConfiguration this[Type type]
        {
            get
            {
                if (TypeConfigurations.ContainsKey(type))
                    return TypeConfigurations[type];
                else
                    return null;
            }
        }

        /// <summary>
        /// Loads the specified loaders.
        /// </summary>
        /// <param name="loaders">The list of configuration loaders to load into the context.</param>
        public void Load(params IConfigurationLoader[] loaders)
        {
            if (loaders.Any())
            {
                var typeConfigurations = loaders
                    .Select(loader => loader.Load()).Aggregate((x, y) => x.Union(y));

                
                

                //first we have to add each type config to the collection
                foreach (var typeConfig in typeConfigurations)
                {
                    //we now run the auto-mapping after all the static configuration is loaded
                    if(typeConfig.AutoMap)
                        typeConfig.AutoMapProperties();

                    TypeConfigurations.Add(typeConfig.Type, typeConfig);
                }
                //then process the properties.
                //this stops the problem of types not existing for certain data handlers
                foreach (var typeConfig in typeConfigurations)
                {
                    ProcessProperties(typeConfig.Properties);
                }
            }

        }

        /// <summary>
        /// Processes the properties.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <exception cref="System.NullReferenceException">Could not find data mapper for property {0} on type {1}
        ///                         .Formatted(property.PropertyInfo.Name,property.PropertyInfo.ReflectedType.FullName)</exception>
        private void ProcessProperties(IEnumerable<AbstractPropertyConfiguration> properties )
        {
            DataMapperResolver runner = new DataMapperResolver(DependencyResolver.ResolveAll<IDataMapperResolverTask>());

            foreach(var property in properties)
            {

                DataMapperResolverArgs args = new DataMapperResolverArgs(this, property);
                args.PropertyConfiguration = property;
                args.DataMappers = DependencyResolver.ResolveAll<AbstractDataMapper>();
                runner.Run(args);
                if(args.Result == null)
                {
                    throw new NullReferenceException(
                        "Could not find data mapper for property {0} on type {1}"
                        .Formatted(property.PropertyInfo.Name,property.PropertyInfo.ReflectedType.FullName));
                }
                property.Mapper = args.Result;
            }
        }

        /// <summary>
        /// Gets the type configuration.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>AbstractTypeConfiguration.</returns>
        public AbstractTypeConfiguration GetTypeConfiguration(object obj)
        {
            var type = obj.GetType();
            var config = TypeConfigurations.ContainsKey(type) ? TypeConfigurations[type] : null;

            if (config != null) return config;

            //check base type encase of proxy
            config = TypeConfigurations.ContainsKey(type.BaseType) ? TypeConfigurations[type.BaseType] : null;

            if (config != null) return config;

            //check interfaces encase this is an interface proxy
            string name = type.Name;
            var interfaceType = type.GetInterfaces().FirstOrDefault(x => name.Contains(x.Name));

            if (interfaceType != null)
                config = TypeConfigurations.ContainsKey(interfaceType) ? TypeConfigurations[interfaceType] : null;

            return config;
        }
    }
}



