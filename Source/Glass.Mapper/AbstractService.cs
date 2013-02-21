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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectSaving;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Profilers;

namespace Glass.Mapper
{
    public abstract class AbstractService : IAbstractService
    {

        private IPerformanceProfiler _profiler;

        public IPerformanceProfiler Profiler
        {
            get { return _profiler; }
            set
            {
                _configurationResolver.Profiler = value;
                _objectConstruction.Profiler = value;
                _objectSaving.Profiler = value;
                _profiler = value;
            }
        }

        public  Context GlassContext { get; private set; }

        private ConfigurationResolver _configurationResolver;

        private ObjectConstruction _objectConstruction;

        private ObjectSaving _objectSaving;
        
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

            var objectConstructionTasks = glassContext.DependencyResolver.ResolveAll<IObjectConstructionTask>();
            _objectConstruction = new ObjectConstruction(objectConstructionTasks); 

            var configurationResolverTasks = glassContext.DependencyResolver.ResolveAll<IConfigurationResolverTask>();
            _configurationResolver = new ConfigurationResolver(configurationResolverTasks);

            var objectSavingTasks = glassContext.DependencyResolver.ResolveAll<IObjectSavingTask>();
            _objectSaving = new ObjectSaving(objectSavingTasks);

            Profiler = new NullProfiler();

        }

        public object InstantiateObject(AbstractTypeCreationContext abstractTypeCreationContext)
        {
            //Run the get type pipeline to get the type to load
           // var typeArgs = new TypeResolverArgs(GlassContext, abstractTypeCreationContext);
          //  _typeResolver.Run(typeArgs);

            //TODO: ME - make these exceptions more specific
         //   if (typeArgs.Result == null)
         //       throw new NullReferenceException("Type Resolver pipeline did not return type.");

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

        public void SaveObject(AbstractTypeSavingContext abstractTypeSavingContext)
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
        /// <param name="creationContext"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract AbstractDataMappingContext CreateDataMappingContext(AbstractTypeSavingContext creationContext);
    }

    public interface IAbstractService
    {
        Context GlassContext { get;  }

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


