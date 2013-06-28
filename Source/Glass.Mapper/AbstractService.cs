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
    public abstract class AbstractService //: IAbstractService
    {
        public ObjectFactory ObjectFactory { get; private set; }

        /// <summary>
        /// Gets the glass context.
        /// </summary>
        /// <value>
        /// The glass context.
        /// </value>
        public Context GlassContext { get; private set; }

        public AbstractService(
            Context glassContext,
            ObjectFactory objectFactory 
            )
        {
            ObjectFactory = objectFactory;
            GlassContext = glassContext;
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

    ///// <summary>
    ///// IAbstractService
    ///// </summary>
    //public interface IAbstractService
    //{
    //    /// <summary>
    //    /// Gets the glass context.
    //    /// </summary>
    //    /// <value>
    //    /// The glass context.
    //    /// </value>
    //    Context GlassContext { get;  }
        
    //    /// <summary>
    //    /// Used to create the context used by DataMappers to map data to a class
    //    /// </summary>
    //    /// <param name="creationContext">The Type Creation Context used to create the instance</param>
    //    /// <param name="obj">The newly instantiated object without any data mapped</param>
    //    /// <returns></returns>
    //    AbstractDataMappingContext CreateDataMappingContext(AbstractTypeCreationContext creationContext, object obj);


    //    /// <summary>
    //    /// Used to create the context used by DataMappers to map data from a class
    //    /// </summary>
    //    /// <param name="creationContext">The Saving Context</param>
    //    /// <returns></returns>
    //    AbstractDataMappingContext CreateDataMappingContext(AbstractTypeSavingContext creationContext);
    //}
}



