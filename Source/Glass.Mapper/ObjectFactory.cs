using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectSaving;
using Glass.Mapper.Profilers;

namespace Glass.Mapper
{
    public abstract class AbstractObjectFactory
    {
          /// <summary>
        /// Gets or sets the profiler.
        /// </summary>
        /// <value>
        /// The profiler.
        /// </value>
        public IPerformanceProfiler Profiler { get; set; }
        public Context Context { get; private set; }

     

        private readonly ConfigurationResolver _configurationResolver;
        private readonly ObjectConstruction _objectConstruction;
        private readonly ObjectSaving _objectSaving;


        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractService"/> class.
        /// </summary>
        /// <param name="glassContext">The glass context.</param>
        /// <exception cref="System.NullReferenceException">Context is null</exception>
        public AbstractObjectFactory(
            Context context,
            ObjectConstruction objectConstruction,
            ConfigurationResolver configurationResolver,
            ObjectSaving objectSaving
            )
        {
            Context = context;
            if (Context == null) 
                throw new NullReferenceException("Context is null");

            _objectConstruction = objectConstruction;
            _configurationResolver = configurationResolver;
            _objectSaving = objectSaving;

            Profiler = new NullProfiler();
        }

        /// <summary>
        /// Instantiates the object.
        /// </summary>
        /// <param name="abstractTypeCreationContext">The abstract type creation context.</param>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException">Configuration Resolver pipeline did not return a type. Has the type been loaded by Glass.Mapper. Type: {0}.Formatted(abstractTypeCreationContext.RequestedType.FullName)</exception>
        public object InstantiateObject(AbstractTypeCreationContext abstractTypeCreationContext)
        {
            string profileName = "GM: Instantiate object {0} {1}".Formatted(abstractTypeCreationContext.RequestedType.FullName, abstractTypeCreationContext.NodeIndentifier);

            try
            {
                Profiler.Start(profileName);

                //run the pipeline to get the configuration to load
                var configurationArgs = new ConfigurationResolverArgs(Context, abstractTypeCreationContext);
                _configurationResolver.Run(configurationArgs);

                if (configurationArgs.Result == null)
                    throw new NullReferenceException("Configuration Resolver pipeline did not return a type. Has the type been loaded by Glass.Mapper. Type: {0}".Formatted(abstractTypeCreationContext.RequestedType.FullName));

                var config = configurationArgs.Result;

                //Run the object construction
                var objectArgs = new ObjectConstructionArgs(Context, abstractTypeCreationContext, config);
                _objectConstruction.Run(objectArgs);

                return objectArgs.Result;

            }
            finally
            {
                Profiler.End(profileName);
            }

        }

        /// <summary>
        /// Saves the object.
        /// </summary>
        /// <param name="abstractTypeSavingContext">The abstract type saving context.</param>
        public virtual void SaveObject(AbstractTypeSavingContext abstractTypeSavingContext)
        {
            
            string profilerName = "GM: SaveObject {0}".Formatted(abstractTypeSavingContext.Object.GetType().FullName);
            
            Profiler.Start(profilerName);

            //Run the object construction
            var savingArgs = new ObjectSavingArgs(Context, abstractTypeSavingContext.Object, abstractTypeSavingContext);
            _objectSaving.Run(savingArgs);

            Profiler.End(profilerName);

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
        /// <param name="creationContext">The Saving Context</param>
        /// <returns></returns>
        public abstract AbstractDataMappingContext CreateDataMappingContext(AbstractTypeSavingContext creationContext);
    }
}
