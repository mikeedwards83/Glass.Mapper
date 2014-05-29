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
using Umbraco.Core.Models;

namespace Glass.Mapper.Umb.DataMappers
{
    /// <summary>
    /// UmbracoParentMapper
    /// </summary>
    public class UmbracoParentMapper :AbstractDataMapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoParentMapper"/> class.
        /// </summary>
        public UmbracoParentMapper()
        {
            ReadOnly = true;
        }

        /// <summary>
        /// Maps data from the .Net property value to the CMS value
        /// </summary>
        /// <param name="mappingContext"></param>
        /// <exception cref="System.NotSupportedException"></exception>
        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Maps data from the CMS value to the .Net property value
        /// </summary>
        /// <param name="mappingContext"></param>
        /// <returns></returns>
        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var umbContext = mappingContext as UmbracoDataMappingContext;
            var umbConfig = Configuration as UmbracoParentConfiguration;

            IContent content = umbContext.PublishedOnly
                                   ? umbContext.Service.ContentService.GetPublishedVersion(umbContext.Content.ParentId)
                                   : umbContext.Service.ContentService.GetById(umbContext.Content.ParentId);



            return umbContext.Service.CreateType(
                umbConfig.PropertyInfo.PropertyType,
                content,
                umbConfig.IsLazy,
                umbConfig.InferType);
        }

        /// <summary>
        /// Indicates that the data mapper will mapper to and from the property
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is UmbracoParentConfiguration;
        }
    }
}

