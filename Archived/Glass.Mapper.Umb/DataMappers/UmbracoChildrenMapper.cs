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
using Glass.Mapper.Umb.Configuration;
using Umbraco.Core.Models;

namespace Glass.Mapper.Umb.DataMappers
{
    /// <summary>
    /// UmbracoChildrenMapper
    /// </summary>
    public class UmbracoChildrenMapper : AbstractDataMapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoChildrenMapper"/> class.
        /// </summary>
        public UmbracoChildrenMapper()
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
        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var umbContext = mappingContext as UmbracoDataMappingContext;
            var umbConfig = Configuration as UmbracoChildrenConfiguration;

            Type genericType = Utilities.GetGenericArgument(Configuration.PropertyInfo.PropertyType);

            Func<IEnumerable<IContent>> getItems = null;
            if (umbContext.PublishedOnly)
            {
                if (String.IsNullOrWhiteSpace(umbConfig.DocumentTypeAlias))
                {
                    getItems = () => umbContext.Service.ContentService.GetChildren(umbContext.Content.Id)
                                     .Select(c => umbContext.Service.ContentService.GetPublishedVersion(c.Id));
                }
                else
                {
                    getItems = () => umbContext.Service.ContentService.GetChildren(umbContext.Content.Id)
                        .Where(c => c.ContentType.Alias == umbConfig.DocumentTypeAlias)
                        .Select(c => umbContext.Service.ContentService.GetPublishedVersion(c.Id));
                }
            }
            else
            {
                if (String.IsNullOrWhiteSpace(umbConfig.DocumentTypeAlias))
                {
                    getItems = () => umbContext.Service.ContentService.GetChildren(umbContext.Content.Id);
                }
                else
                {
                    getItems = () => umbContext.Service.ContentService.GetChildren(umbContext.Content.Id).Where(c => c.ContentType.Alias == umbConfig.DocumentTypeAlias);
                }
            }

            return Utilities.CreateGenericType(
                typeof(LazyContentEnumerable<>),
                new[] {genericType},
                getItems,
                umbConfig.IsLazy,
                umbConfig.InferType,
                umbContext.Service
                );
        }

        /// <summary>
        /// Indicates that the data mapper will mapper to and from the property
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration,  Context context)
        {
            return configuration is UmbracoChildrenConfiguration;
        }
    }
}

