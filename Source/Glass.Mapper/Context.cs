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

        public static IGlassConfiguration GlassConfig { get; set; }

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
        public static Context Create()
        {
            return Context.Create(DefaultContextName, true);
        }

        /// <summary>
        /// Creates a new context and adds it to the Contexts dictionary.
        /// </summary>
        /// <param name="contextName">The context name, used as the key in the Contexts dictionary.</param>
        /// <param name="isDefault">Indicates if this is the default context. If it is the context is assigned to the Default static property.</param>
        /// <returns></returns>
        public static Context Create(string contextName, bool isDefault = false)
        {

            if(GlassConfig == null)
                throw new NullReferenceException("No GlassConfig set. Set the static property Context.GlassConfig");

            var context = new Context();
            context.DependencyResolver = ResolverFactory.GetResolver();
            context.DependencyResolver.Load(contextName, GlassConfig);
            
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
        /// The list of DataMappers used when loading configurations
        /// </summary>
        public IList<AbstractDataMapper> DataMappers { get; set; }

        /// <summary>
        /// The dependency resolver used by services using the context
        /// </summary>
        public IDependencyResolver DependencyResolver { get; set; }

        private Context()
        {
            TypeConfigurations = new Dictionary<Type, AbstractTypeConfiguration>();
            DataMappers = new List<AbstractDataMapper>();
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

                DataMapperResolver runner = new DataMapperResolver(this.DataMappers);
                foreach (var typeConfig in typeConfigurations)
                {
                    ProcessProperties(runner, typeConfig.Properties);
                    TypeConfigurations.Add(typeConfig.Type, typeConfig);
                }
            }

        }

        private void ProcessProperties(DataMapperResolver runner, IEnumerable<AbstractPropertyConfiguration> properties )
        {
            foreach(var property in properties)
            {
                DataMapperResolverArgs args = new DataMapperResolverArgs(this, property);
                args.PropertyConfiguration = property;

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
    }
}
