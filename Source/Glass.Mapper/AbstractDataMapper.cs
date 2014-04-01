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
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.DataMapperResolver;

namespace Glass.Mapper
{

    /***** 
    /// 
    /// I have assumed that all CMSs at the moment will using Strings for data storage
    /// 
    *****/

    /// <summary>
    /// A data mapper converts data from the CMS stored value to the .Net data type
    /// </summary>
    public abstract class AbstractDataMapper
    {
        /// <summary>
        /// Gets or sets a value indicating whether [read only].
        /// </summary>
        /// <value><c>true</c> if [read only]; otherwise, <c>false</c>.</value>
        public bool ReadOnly { get;  set; }

        /// <summary>
        /// The property this Data Mapper will populate
        /// </summary>
        /// <value>The configuration.</value>
        public AbstractPropertyConfiguration Configuration { get; private set; }

        /// <summary>
        /// Takes CMS data and writes it to the property
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
        public virtual void MapCmsToProperty(AbstractDataMappingContext mappingContext)
        {
            object result;

            try
            {
                 result = MapToProperty(mappingContext);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to map to property '{0}' on type '{1}'".Formatted(Configuration.PropertyInfo.Name, Configuration.PropertyInfo.ReflectedType.FullName), ex);
            }
           

            if (result != null)
				Configuration.PropertySetter(mappingContext.Object, result);
        }

        /// <summary>
        /// Takes a Property value and writes it to a CMS value
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
        public virtual  void MapPropertyToCms(AbstractDataMappingContext mappingContext)
        {
            if (ReadOnly) return;

            try
            {
                mappingContext.PropertyValue = Configuration.PropertyGetter(mappingContext.Object);
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to map to CMS '{0}' on type '{1}'".Formatted(Configuration.PropertyInfo.Name, Configuration.PropertyInfo.ReflectedType.FullName), ex);
            }

            MapToCms(mappingContext);
        }



        /// <summary>
        /// Maps data from the .Net property value to the CMS value
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns>The value to write</returns>
        public abstract void MapToCms(AbstractDataMappingContext mappingContext);

        /// <summary>
        /// Maps data from the CMS value to the .Net property value
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns>System.Object.</returns>
        public abstract object MapToProperty(AbstractDataMappingContext mappingContext);


        /// <summary>
        /// Sets up the data mapper for a particular property
        /// </summary>
        /// <param name="args">The args.</param>
        public virtual void Setup(DataMapperResolverArgs args)
        {
            Configuration = args.PropertyConfiguration;
        }


        /// <summary>
        /// Indicates that the data mapper will mapper to and from the property
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if this instance can handle the specified configuration; otherwise, <c>false</c>.</returns>
        public abstract bool CanHandle(AbstractPropertyConfiguration configuration, Context context);

        
    }
}




