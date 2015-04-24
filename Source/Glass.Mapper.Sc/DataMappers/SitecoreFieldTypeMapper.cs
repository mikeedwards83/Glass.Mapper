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
using System.Text;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class SitecoreFieldTypeMapper
    /// </summary>
    public class SitecoreFieldTypeMapper : AbstractSitecoreFieldMapper
    {

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.Object.</returns>
        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {

            var item = context.Item;

            if (fieldValue.IsNullOrEmpty()) return null;

            Guid id = Guid.Empty;
            Item target;

            if (Guid.TryParse(fieldValue, out id)) {

                target = item.Database.GetItem(new ID(id), item.Language);
            }
            else
            {
                target = item.Database.GetItem(fieldValue, item.Language);
            }

            if (target == null) return null;
            return context.Service.CreateType(config.PropertyInfo.PropertyType, target, IsLazy, InferType, null);
        }

        /// <summary>
        /// Sets the field value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NullReferenceException">Could not find item to save value {0}.Formatted(Configuration)</exception>
        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context", "The context was incorrectly set");
            
            if(context.Service == null)
                throw new NullReferenceException("The context's service property was null");

            if (context.Service.GlassContext == null)
                throw new NullReferenceException("The service glass context is null");
            
            if (context.Service.Database == null)
                throw new NullReferenceException("The database is not set for the service");

            if (value == null)
                return string.Empty;

            var type = value.GetType();

            var typeConfig = context.Service.GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>(value);

            if(typeConfig == null)
                throw new NullReferenceException("The type {0} has not been loaded into context {1}".Formatted(type.FullName, context.Service.GlassContext.Name));

            var item = typeConfig.ResolveItem(value, context.Service.Database);
            if(item == null)
                throw new NullReferenceException("Could not find item to save value {0}".Formatted(Configuration));

            return item.ID.ToString();
        }

        /// <summary>
        /// Determines whether this instance can handle the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if this instance can handle the specified configuration; otherwise, <c>false</c>.</returns>
        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration,  Context context)
        {
            return configuration is SitecoreFieldConfiguration;// context[configuration.PropertyInfo.PropertyType] != null &&
                   
        }

        /// <summary>
        /// Sets up the data mapper for a particular property
        /// </summary>
        /// <param name="args">The args.</param>
        public override void Setup(DataMapperResolverArgs args)
        {
            var scConfig = args.PropertyConfiguration as SitecoreFieldConfiguration;

            IsLazy = (scConfig.Setting & SitecoreFieldSettings.DontLoadLazily) != SitecoreFieldSettings.DontLoadLazily;
            InferType = (scConfig.Setting & SitecoreFieldSettings.InferType) == SitecoreFieldSettings.InferType;
            base.Setup(args);
        }

        /// <summary>
        /// Gets or sets a value indicating whether [infer type].
        /// </summary>
        /// <value><c>true</c> if [infer type]; otherwise, <c>false</c>.</value>
        protected bool InferType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is lazy.
        /// </summary>
        /// <value><c>true</c> if this instance is lazy; otherwise, <c>false</c>.</value>
        protected bool IsLazy { get; set; }
    }
}




