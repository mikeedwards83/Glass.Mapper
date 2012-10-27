using System;
using System.Collections.Generic;
using System.Linq;
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
        public const string DefaultName = "Default";

        #region STATICS

        static Context()
        {
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
            return Context.Create(DefaultName, true);
        }

        /// <summary>
        /// Creates a new context and adds it to the Contexts dictionary.
        /// </summary>
        /// <param name="contextName">The context name, used as the key in the Contexts dictionary.</param>
        /// <param name="isDefault">Indicates if this is the default context. If it is the context is assigned to the Default static property.</param>
        /// <returns></returns>
        public static Context Create(string contextName, bool isDefault = false)
        {
            var context = new Context();

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
        /// The list of tasks to be performed by the Object Construction Pipeline. Called in the order specified.
        /// </summary>
        public IList<IObjectConstructionTask> ObjectConstructionTasks { get; private set; }
        
        /// <summary>
        /// The list of tasks to be performed by the Type Resolver Pipeline. Called in the order specified.
        /// </summary>
        public IList<ITypeResolverTask> TypeResolverTasks { get; private set; }

        /// <summary>
        /// The list of tasks to be performed by the Configuration Resolver Pipeline. Called in the order specified.
        /// </summary>
        public IList<IConfigurationResolverTask> ConfigurationResolverTasks { get; private set; }

        /// <summary>
        /// The list of DataMappers used when loading configurations
        /// </summary>
        public IList<AbstractDataMapper> DataMappers { get; set; } 

        private Context()
        {
            ObjectConstructionTasks = new List<IObjectConstructionTask>();
            TypeResolverTasks = new List<ITypeResolverTask>();
            ConfigurationResolverTasks = new List<IConfigurationResolverTask>();
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
