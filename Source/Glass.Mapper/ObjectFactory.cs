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
    public class ObjectFactory
    {
          /// <summary>
        /// Gets or sets the profiler.
        /// </summary>
        /// <value>
        /// The profiler.
        /// </value>
        public IPerformanceProfiler Profiler { get; set; }

     

        private readonly ConfigurationResolver _configurationResolver;
        private readonly Context _glassContext;
        private readonly ObjectConstruction _objectConstruction;
        private readonly ObjectSaving _objectSaving;
        private readonly AbstractService _service;


        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractService"/> class.
        /// </summary>
        /// <param name="glassContext">The glass context.</param>
        /// <exception cref="System.NullReferenceException">Context is null</exception>
        public ObjectFactory(
            Context glassContext,
            ObjectConstruction objectConstruction,
            ConfigurationResolver configurationResolver,
            ObjectSaving objectSaving,
            AbstractService service
            )
        {


            _glassContext = glassContext;
            if (_glassContext == null) 
                throw new NullReferenceException("Context is null");

            _objectConstruction = objectConstruction;
            _configurationResolver = configurationResolver;
            _objectSaving = objectSaving;
            _service = service;

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
            //run the pipeline to get the configuration to load
            var configurationArgs = new ConfigurationResolverArgs(_glassContext, abstractTypeCreationContext);
            _configurationResolver.Run(configurationArgs);
            
            if (configurationArgs.Result == null)
                throw new NullReferenceException("Configuration Resolver pipeline did not return a type. Has the type been loaded by Glass.Mapper. Type: {0}".Formatted(abstractTypeCreationContext.RequestedType.FullName));

            var config = configurationArgs.Result;

            //Run the object construction
            var objectArgs = new ObjectConstructionArgs(_glassContext, abstractTypeCreationContext, config, _service);
            _objectConstruction.Run(objectArgs);

            return objectArgs.Result;
        }

        /// <summary>
        /// Saves the object.
        /// </summary>
        /// <param name="abstractTypeSavingContext">The abstract type saving context.</param>
        public virtual void SaveObject(AbstractTypeSavingContext abstractTypeSavingContext)
        {
            //Run the object construction
            var savingArgs = new ObjectSavingArgs(_glassContext, abstractTypeSavingContext.Object, abstractTypeSavingContext, _service);
            _objectSaving.Run(savingArgs);
        }
    }
}
