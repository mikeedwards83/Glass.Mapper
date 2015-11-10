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
using Glass.Mapper.IoC;
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

        private IPerformanceProfiler _profiler;
        private ObjectConstruction _objectConstruction;
        private ConfigurationResolver _configurationResolver;
        private ObjectSaving _objectSaving;
        private bool factoryConfigLoaded;
        private readonly object lockObject = new object();

        /// <summary>
        /// Gets or sets the profiler.
        /// </summary>
        /// <value>
        /// The profiler.
        /// </value>
        public IPerformanceProfiler Profiler
        {
            get { return _profiler ?? ( _profiler = new NullProfiler() ); }
            set
            {
                _profiler = value;
            }
        }

        /// <summary>
        /// Gets the glass context.
        /// </summary>
        /// <value>
        /// The glass context.
        /// </value>
        public Context GlassContext { get; private set; }

        protected virtual ObjectConstruction ObjectConstruction
        {
            get
            {
                LoadFactoryConfig();
                return _objectConstruction;
            }
        }

        protected virtual ConfigurationResolver ConfigurationResolver
        {
            get
            {
                LoadFactoryConfig();
                return _configurationResolver;
            }
        }

        protected virtual ObjectSaving ObjectSaving
        {
            get
            {
                LoadFactoryConfig();
                return _objectSaving;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractService"/> class.
        /// </summary>
        protected AbstractService()
            : this(Context.Default)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractService"/> class.
        /// </summary>
        /// <param name="glassContextName">Name of the context.</param>
        protected AbstractService(string glassContextName) : this(Context.Contexts[glassContextName])
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractService"/> class.
        /// </summary>
        /// <param name="glassContext">The glass context.</param>
        /// <exception cref="System.NullReferenceException">Context is null</exception>
        protected AbstractService(Context glassContext)
        {
            GlassContext = glassContext;
            Initialise();
        }

        protected void Initialise()
        {
            if (GlassContext == null)
            {
                throw new NullReferenceException("Context is null");
            }

            // NM: at this point, the GlassContext is available and scaffolded

            // The original initiate logic still fires on the constructor
            Initiate(GlassContext.DependencyResolver);
        }

        public virtual void Initiate(IDependencyResolver resolver)
        {
            CacheEnabled = true;            
        }


        private void LoadFactoryConfig()
        {
            lock (lockObject)
            {
                if (factoryConfigLoaded)
                {
                    return;
                }

                // todo: consider if the items here could be loaded from a singleton / factory pattern taking the hit up front
                factoryConfigLoaded = true;
                var objectConstructionTasks = GlassContext.DependencyResolver.ObjectConstructionFactory.GetItems();
                _objectConstruction = new ObjectConstruction(objectConstructionTasks)
                {
                    Profiler = Profiler
                };

                var objectSavingTasks = GlassContext.DependencyResolver.ObjectSavingFactory.GetItems();
                _objectSaving = new ObjectSaving(objectSavingTasks)
                {
                    Profiler = Profiler
                };

                var configurationResolverTasks = GlassContext.DependencyResolver.ConfigurationResolverFactory.GetItems();
                _configurationResolver = new ConfigurationResolver(configurationResolverTasks)
                {
                    Profiler = Profiler
                };
            }
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
            var configurationArgs = RunConfigurationPipeline(abstractTypeCreationContext);
            if (configurationArgs.Result == null)
                throw new NullReferenceException("Configuration Resolver pipeline did not return a type. Has the type been loaded by Glass.Mapper. Type: {0}".Formatted(abstractTypeCreationContext.RequestedType));

            //Run the object construction
            var objectArgs = new ObjectConstructionArgs(GlassContext, abstractTypeCreationContext, configurationArgs.Result, this);
            objectArgs.Parameters = configurationArgs.Parameters;
            ObjectConstruction.Run(objectArgs);

            return objectArgs.Result;
        }

        public ConfigurationResolverArgs RunConfigurationPipeline(AbstractTypeCreationContext abstractTypeCreationContext)
        {
            var configurationArgs = new ConfigurationResolverArgs(GlassContext, abstractTypeCreationContext, abstractTypeCreationContext.RequestedType, this);
            configurationArgs.Parameters = abstractTypeCreationContext.Parameters;
            ConfigurationResolver.Run(configurationArgs);

            return configurationArgs;
        }

        /// <summary>
        /// Saves the object.
        /// </summary>
        /// <param name="abstractTypeSavingContext">The abstract type saving context.</param>
        public virtual void SaveObject(AbstractTypeSavingContext abstractTypeSavingContext)
        {
            //Run the object construction
            var savingArgs = new ObjectSavingArgs(GlassContext, abstractTypeSavingContext.Object, abstractTypeSavingContext, this);
            ObjectSaving.Run(savingArgs);
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_configurationResolver != null)
                {
                    _configurationResolver.Dispose();
                }

                if (_objectConstruction != null)
                {
                    _objectConstruction.Dispose();
                }

                if (_objectSaving != null)
                {
                    _objectSaving.Dispose();
                }

                _configurationResolver = null;
                _objectConstruction = null;
                _objectSaving = null;

            }
        }


        public bool CacheEnabled { get; set; }
    }

    /// <summary>
    /// IAbstractService
    /// </summary>
    public interface IAbstractService : IDisposable
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

        ConfigurationResolverArgs RunConfigurationPipeline(AbstractTypeCreationContext abstractTypeCreationContext);

        /// <summary>
        /// Indicates if the cache should be enable when using this service.
        /// </summary>
        bool CacheEnabled { get; set; }
    }
}



