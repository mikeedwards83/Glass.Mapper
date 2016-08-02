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
using Glass.Mapper.Umb.Configuration;

namespace Glass.Mapper.Umb.DataMappers
{
    /// <summary>
    /// UmbracoIdMapper
    /// </summary>
    public class UmbracoIdMapper : AbstractDataMapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoIdMapper"/> class.
        /// </summary>
        public UmbracoIdMapper()
        {
            ReadOnly = true;
        }

        /// <summary>
        /// Maps data from the .Net property value to the CMS value
        /// </summary>
        /// <param name="mappingContext"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Maps data from the CMS value to the .Net property value
        /// </summary>
        /// <param name="mappingContext"></param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException">The type {0} on {0}.{1} is not supported by UmbracoIdMapper.Formatted
        ///                                                 (umbConfig.PropertyInfo.ReflectedType.FullName,
        ///                                                  umbConfig.PropertyInfo.Name)</exception>
        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            UmbracoDataMappingContext context = mappingContext as UmbracoDataMappingContext;
            var node = context.Content;

            var umbConfig = Configuration as UmbracoIdConfiguration;

            if (umbConfig.PropertyInfo.PropertyType == typeof(int))
                return node.Id;
            if (umbConfig.PropertyInfo.PropertyType == typeof(Guid))
                return node.Key;
            
            throw new NotSupportedException("The type {0} on {0}.{1} is not supported by UmbracoIdMapper".Formatted
                                                (umbConfig.PropertyInfo.ReflectedType.FullName,
                                                 umbConfig.PropertyInfo.Name));
        }

        /// <summary>
        /// Indicates that the data mapper will mapper to and from the property
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is UmbracoIdConfiguration;
        }
    }
}

