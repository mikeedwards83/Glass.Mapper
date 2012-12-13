using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectSaving;
using Glass.Mapper.Pipelines.TypeResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver;

namespace Glass.Mapper
{
    public abstract class AbstractService<TK> : IAbstractService
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

        /// <summary>
        /// The list of tasks to be performed by the Object Saving Pipeline. Called in the order specified.
        /// </summary>
        private IEnumerable<IObjectSavingTask> ObjectSavingTasks { get; set; }



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
            if (GlassContext == null) 
                throw new NullReferenceException("Context is null");

            ObjectConstructionTasks = glassContext.DependencyResolver.ResolveAll<IObjectConstructionTask>();
            TypeResolverTasks = glassContext.DependencyResolver.ResolveAll<ITypeResolverTask>();
            ConfigurationResolverTasks = glassContext.DependencyResolver.ResolveAll<IConfigurationResolverTask>();
            ObjectSavingTasks = glassContext.DependencyResolver.ResolveAll<IObjectSavingTask>();
        }

        public object InstantiateObject(AbstractTypeCreationContext abstractTypeCreationContext)
        {
            //Run the get type pipeline to get the type to load
            var typeRunner = new TypeResolver(TypeResolverTasks);
            var typeArgs = new TypeResolverArgs(GlassContext, abstractTypeCreationContext);
            typeRunner.Run(typeArgs);

            //TODO: ME - make these exceptions more specific
            if (typeArgs.Result == null)
                throw new NullReferenceException("Type Resolver pipeline did not return type.");

            //run the pipeline to get the configuration to load
            var configurationRunner = new ConfigurationResolver(ConfigurationResolverTasks);
            var configurationArgs = new ConfigurationResolverArgs(GlassContext, abstractTypeCreationContext, typeArgs.Result);
            configurationRunner.Run(configurationArgs);

            if (configurationArgs.Result == null)
                throw new NullReferenceException("Configuration Resolver pipeline did not return type.");

            var config = configurationArgs.Result;

            //Run the object construction
            var objectRunner = new ObjectConstruction(ObjectConstructionTasks);
            var objectArgs = new ObjectConstructionArgs(GlassContext, abstractTypeCreationContext, config, this);
            objectRunner.Run(objectArgs);

           

            return objectArgs.Result;
        }

        public void SaveObject(AbstractTypeSavingContext abstractTypeSavingContext)
        {
            //Run the object construction
            var savingRunner = new ObjectSaving(ObjectSavingTasks);
            var savingArgs = new ObjectSavingArgs(GlassContext, abstractTypeSavingContext.Object, abstractTypeSavingContext, this);
            savingRunner.Run(savingArgs);
        }

        /// <summary>
        /// Used to create the context used by DataMappers to map data to a class
        /// </summary>
        /// <param name="creationContext"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract AbstractDataMappingContext CreateDataMappingContext(AbstractTypeCreationContext creationContext, object obj);


        /// <summary>
        /// Used to create the context used by DataMappers to map data from a class
        /// </summary>
        /// <param name="creationContext"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract AbstractDataMappingContext CreateDataMappingContext(AbstractTypeSavingContext creationContext);
    }

    public interface IAbstractService
    {
        object InstantiateObject(AbstractTypeCreationContext abstractTypeCreationContext);
          /// <summary>
        /// Used to create the context used by DataMappers to map data to a class
        /// </summary>
        /// <param name="creationContext">The Type Creation Context used to create the instance</param>
        /// <param name="obj">The newly instantiated object without any data mapped</param>
        /// <returns></returns>
        AbstractDataMappingContext CreateDataMappingContext(AbstractTypeCreationContext creationContext, object obj);


        /// <summary>
        /// Used to create the context used by DataMappers to map data from a class
        /// </summary>
        /// <param name="creationContext">The Saving Context</param>
        /// <returns></returns>
        AbstractDataMappingContext CreateDataMappingContext(AbstractTypeSavingContext creationContext);
    }
}