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
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Umb.Configuration;

namespace Glass.Mapper.Umb.DataMappers
{
    /// <summary>
    /// UmbracoPropertyTypeMapper
    /// </summary>
    public class UmbracoPropertyTypeMapper : AbstractUmbracoPropertyMapper
    {
        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="propertyValue">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override object GetPropertyValue(object propertyValue, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context)
        {
            int id;
            if (propertyValue == null || !int.TryParse(propertyValue.ToString(), out id))
                return null;

            var item = context.PublishedOnly
                           ? context.Service.ContentService.GetPublishedVersion(id)
                           : context.Service.ContentService.GetById(id);

            return context.Service.CreateType(config.PropertyInfo.PropertyType, item, IsLazy, InferType);
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException">Could not find item to save value {0}</exception>
        public override object SetPropertyValue(object value, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context)
        {
            if (value == null)
                return string.Empty;
            
            var typeConfig = context.Service.GlassContext[value.GetType()] as UmbracoTypeConfiguration;

            var item = typeConfig.ResolveItem(value, context.Service.ContentService);
            if (item == null)
                throw new NullReferenceException("Could not find item to save value {0}");

            return item.Id;
        }

        /// <summary>
        /// Indicates that the data mapper will mapper to and from the property
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration,  Context context)
        {
            return configuration is UmbracoPropertyConfiguration; //context[configuration.PropertyInfo.PropertyType] != null &&
        }

        /// <summary>
        /// Sets up the data mapper for a particular property
        /// </summary>
        /// <param name="args"></param>
        public override void Setup(DataMapperResolverArgs args)
        {
            var config = args.PropertyConfiguration as UmbracoPropertyConfiguration;

            IsLazy = (config.Setting & UmbracoPropertySettings.DontLoadLazily) != UmbracoPropertySettings.DontLoadLazily;
            InferType = (config.Setting & UmbracoPropertySettings.InferType) == UmbracoPropertySettings.InferType;
            base.Setup(args);
        }

        /// <summary>
        /// Gets or sets a value indicating whether [infer type].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [infer type]; otherwise, <c>false</c>.
        /// </value>
        protected bool InferType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is lazy.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is lazy; otherwise, <c>false</c>.
        /// </value>
        protected bool IsLazy { get; set; }
    }
}




