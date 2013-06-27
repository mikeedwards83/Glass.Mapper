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
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectSaving;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Profilers;

namespace Glass.Mapper
{
    /// <summary>
    /// AbstractService
    /// </summary>
    public abstract class AbstractService : IAbstractService
    {


        /// <summary>
        /// Gets or sets the profiler.
        /// </summary>
        /// <value>
        /// The profiler.
        /// </value>
        public IPerformanceProfiler Profiler { get; set; }

        /// <summary>
        /// Gets the glass context.
        /// </summary>
        /// <value>
        /// The glass context.
        /// </value>
        public  Context GlassContext { get; private set; }

        private readonly ConfigurationResolver _configurationResolver;

        private readonly ObjectConstruction _objectConstruction;

        private readonly ObjectSaving _objectSaving;


        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractService"/> class.
        /// </summary>
        /// <param name="glassContext">The glass context.</param>
        /// <exception cref="System.NullReferenceException">Context is null</exception>
        protected AbstractService(
            Context glassContext,
            ObjectConstruction objectConstruction,
            ConfigurationResolver configurationResolver,
            ObjectSaving objectSaving
            )
        {


            GlassContext = glassContext;
            if (GlassContext == null) 
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
            //run the pipeline to get the configuration to load
            var configurationArgs = new ConfigurationResolverArgs(GlassContext, abstractTypeCreationContext);
            _configurationResolver.Run(configurationArgs);
            
            if (configurationArgs.Result == null)
                throw new NullReferenceException("Configuration Resolver pipeline did not return a type. Has the type been loaded by Glass.Mapper. Type: {0}".Formatted(abstractTypeCreationContext.RequestedType.FullName));

            var config = configurationArgs.Result;

            //Run the object construction
            var objectArgs = new ObjectConstructionArgs(GlassContext, abstractTypeCreationContext, config, this);
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
            var savingArgs = new ObjectSavingArgs(GlassContext, abstractTypeSavingContext.Object, abstractTypeSavingContext, this);
            _objectSaving.Run(savingArgs);
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

    /// <summary>
    /// IAbstractService
    /// </summary>
    public interface IAbstractService
    {
        /// <summary>
        /// Gets the glass context.
        /// </summary>
        /// <value>
        /// The glass context.
        /// </value>
        Context GlassContext { get;  }

        /// <summary>
        /// Instantiates the object.
        /// </summary>
        /// <param name="abstractTypeCreationContext">The abstract type creation context.</param>
        /// <returns></returns>
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



