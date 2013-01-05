using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.CastleWindsor;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface;
using Glass.Mapper.Pipelines.TypeResolver;
using Glass.Mapper.Pipelines.TypeResolver.Tasks.StandardResolver;
using Glass.Mapper.Pipelines.DataMapperResolver;

namespace Glass.Mapper
{
    /// <summary>
    /// The context contains the configuration of Glass.Mapper
    /// </summary>
    public class Context
    {
        public const string DefaultContextName = "Default";

        #region STATICS

        public static IDependencyResolverFactory ResolverFactory { get; set; }

       

        static Context()
        {
            ResolverFactory = new CastleDependencyResolverFactory();
            Contexts = new Dictionary<string, Context>();
        }

        /// <summary>
        /// The default Context. Used by services if no Context is specified.
        /// </summary>
        public static Context Default { get; private set; }

        /// <summary>
        /// Contains the list of Contexts currently loaded. 
        /// </summary>
        public static IDictionary<string, Context> Contexts { get; private set; }

        

        /// <summary>
        /// Creates a Context and creates it as the default Context. This is assigned to the Default static property.
        /// </summary>
        public static Context Create(IGlassConfiguration glassConfig)
        {
            return Context.Create(glassConfig, DefaultContextName, true);
        }

        /// <summary>
        /// Creates a new context and adds it to the Contexts dictionary.
        /// </summary>
        /// <param name="contextName">The context name, used as the key in the Contexts dictionary.</param>
        /// <param name="isDefault">Indicates if this is the default context. If it is the context is assigned to the Default static property.</param>
        /// <returns></returns>
        public static Context Create(IGlassConfiguration glassConfig, string contextName, bool isDefault = false)
        {

            if (glassConfig == null)
                throw new NullReferenceException("No GlassConfig set. Set the static property Context.GlassConfig");

            var context = new Context();
            context.DependencyResolver = ResolverFactory.GetResolver();
            context.DependencyResolver.Load(contextName, glassConfig);
            
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
        public IDictionary<Type, AbstractTypeConfiguration> TypeConfigurations { get; private set; }

        /// <summary>
        /// The dependency resolver used by services using the context
        /// </summary>
        public IDependencyResolver DependencyResolver { get; set; }

        private Context()
        {
            TypeConfigurations = new Dictionary<Type, AbstractTypeConfiguration>();
        }

        /// <summary>
        /// Gets a type configuration based on type
        /// </summary>
        /// <param name="type">The configuration </param>
        /// <returns></returns>
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
        
        /// <param name="loaders">The list of configuration loaders to load into the context.</param>
        public void Load(params IConfigurationLoader[] loaders)
        {
            if (loaders.Any())
            {
                var typeConfigurations = loaders
                    .Select(loader => loader.Load()).Aggregate((x, y) => x.Union(y));

                foreach (var typeConfig in typeConfigurations)
                {
                    ProcessProperties(typeConfig.Properties);
                    TypeConfigurations.Add(typeConfig.Type, typeConfig);
                }
            }

        }

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
