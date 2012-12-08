using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.TypeResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver;

namespace Glass.Mapper
{
    public abstract class AbstractService<T, TK> : IAbstractService
        where T : ITypeContext
        where TK : AbstractDataMappingContext
    {
        protected Context GlassContext { get; set; }

        /// <summary>
        /// The list of tasks to be performed by the Object Construction Pipeline. Called in the order specified.
        /// </summary>
        private IEnumerable<IObjectConstructionTask> ObjectConstructionTasks { get; set; }

        /// <summary>
        /// The list of tasks to be performed by the Type Resolver Pipeline. Called in the order specified.
        /// </summary>
        private IEnumerable<ITypeResolverTask> TypeResolverTasks { get; set; }

        /// <summary>
        /// The list of tasks to be performed by the Configuration Resolver Pipeline. Called in the order specified.
        /// </summary>
        private IEnumerable<IConfigurationResolverTask> ConfigurationResolverTasks { get; set; }

        public AbstractService()
            : this(Context.Default)
        {

        }

        public AbstractService(string contextName)
            : this(Context.Contexts[contextName])
        {
        }

        public AbstractService(Context glassContext)
        {
            GlassContext = glassContext;
            if (GlassContext == null) throw new NullReferenceException("Context is null");

            var args = new Dictionary<string, object>() {{"context", GlassContext}};

            ObjectConstructionTasks = glassContext.DependencyResolver.ResolveAll<IObjectConstructionTask>();
            TypeResolverTasks = glassContext.DependencyResolver.ResolveAll<ITypeResolverTask>();
            ConfigurationResolverTasks = glassContext.DependencyResolver.ResolveAll<IConfigurationResolverTask>();
        }

        public object InstantiateObject(ITypeContext typeContext)
        {
            //Run the get type pipeline to get the type to load
            var typeRunner = new TypeResolver(TypeResolverTasks);
            var typeArgs = new TypeResolverArgs(GlassContext, typeContext);
            typeRunner.Run(typeArgs);

            //TODO: ME - make these exceptions more specific
            if (typeArgs.Result == null)
                throw new NullReferenceException("Type Resolver pipeline did not return type.");

            //run the pipeline to get the configuration to load
            var configurationRunner = new ConfigurationResolver(ConfigurationResolverTasks);
            var configurationArgs = new ConfigurationResolverArgs(GlassContext, typeContext, typeArgs.Result);
            configurationRunner.Run(configurationArgs);

            if (configurationArgs.Result == null)
                throw new NullReferenceException("Configuration Resolver pipeline did not return type.");

            var config = configurationArgs.Result;

            //Run the object construction
            var objectRunner = new ObjectConstruction(ObjectConstructionTasks);
            var objectArgs = new ObjectConstructionArgs(GlassContext, typeContext, config, this);
            objectRunner.Run(objectArgs);

           

            return objectArgs.Result;
        }

        public void SaveObject(ITypeContext typeContext)
        {
            //TODO: ME - make this a pipeline
          //  typeContext.

        }

        /// <summary>
        /// Used to create the context used by DataMappers to map data to or from a class
        /// </summary>
        /// <param name="context"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract AbstractDataMappingContext CreateDataMappingContext(ITypeContext context, object obj);



    }

    public interface IAbstractService
    {
        object InstantiateObject(ITypeContext typeContext);
        AbstractDataMappingContext CreateDataMappingContext(ITypeContext context, object obj);
    }
}