using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.TypeResolver;

namespace Glass.Mapper
{
    public class ObjectFactory
    {
        public Context Context { get; private set; }

        /// <summary>
        /// The list of tasks to be performed by the Object Construction Pipeline. Called in the order specified.
        /// </summary>
        public IEnumerable<IObjectConstructionTask> ObjectConstructionTasks { get; private set; }

        /// <summary>
        /// The list of tasks to be performed by the Type Resolver Pipeline. Called in the order specified.
        /// </summary>
        public IEnumerable<ITypeResolverTask> TypeResolverTasks { get; private set; }

        /// <summary>
        /// The list of tasks to be performed by the Configuration Resolver Pipeline. Called in the order specified.
        /// </summary>
        public IEnumerable<IConfigurationResolverTask> ConfigurationResolverTasks { get; private set; }

        public ObjectFactory(
            Context context,
            IEnumerable<ITypeResolverTask> typeResolverTasks,
            IEnumerable<IConfigurationResolverTask> configurationResolverTasks,
            IEnumerable<IObjectConstructionTask> objectConstructionTasks)
        {
            Context = context;
            ObjectConstructionTasks = objectConstructionTasks;
            TypeResolverTasks = typeResolverTasks;
            ConfigurationResolverTasks = configurationResolverTasks;
        }

        public object InstantiateObject(IDataContext dataContext)
        {
            //Run the get type pipeline to get the type to load
            var typeRunner = new TypeResolver(TypeResolverTasks);
            var typeArgs = new TypeResolverArgs(Context, dataContext);
            typeRunner.Run(typeArgs);

            //TODO: ME - make these exceptions more specific
            if (typeArgs.Result == null)
                throw new NullReferenceException("Type Resolver pipeline did not return type.");

            //run the pipeline to get the configuration to load
            var configurationRunner = new ConfigurationResolver(ConfigurationResolverTasks);
            var configurationArgs = new ConfigurationResolverArgs(Context, dataContext, typeArgs.Result);
            configurationRunner.Run(configurationArgs);

            if (configurationArgs.Result == null)
                throw new NullReferenceException("Configuration Resolver pipeline did not return type.");

            //Run the object construction
            var objectRunner = new ObjectConstruction(ObjectConstructionTasks);
            var objectArgs = new ObjectConstructionArgs(Context, dataContext, configurationArgs.Result);
            objectRunner.Run(objectArgs);

            return objectArgs.Result;
        }
        

    }
}
