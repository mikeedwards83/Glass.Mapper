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
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data.Fields;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class AbstractSitecoreFieldMapper
    /// </summary>
    public abstract class AbstractSitecoreFieldMapper : AbstractDataMapper
    {
        /// <summary>
        /// Gets the types handled.
        /// </summary>
        /// <value>The types handled.</value>
        public IEnumerable<Type> TypesHandled { get; private set; }

        /// <summary>
        /// The default value to return if the field isn't found
        /// </summary>
        protected virtual object DefaultValue { get { return null; } }


        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractSitecoreFieldMapper"/> class.
        /// </summary>
        /// <param name="typesHandled">The types handled.</param>
        public AbstractSitecoreFieldMapper(params Type [] typesHandled)
        {
            TypesHandled = typesHandled;
        }

        public override void MapCmsToProperty(AbstractDataMappingContext mappingContext)
        {
            var scConfig = Configuration as SitecoreFieldConfiguration;

            if ((scConfig.Setting & SitecoreFieldSettings.PageEditorOnly) == SitecoreFieldSettings.PageEditorOnly)
            {
                return;
            }

            base.MapCmsToProperty(mappingContext);
        }

        public override void MapPropertyToCms(AbstractDataMappingContext mappingContext)
        {
            var scConfig = Configuration as SitecoreFieldConfiguration;

            if ((scConfig.Setting & SitecoreFieldSettings.PageEditorOnly) == SitecoreFieldSettings.PageEditorOnly)
            {
                return;
            }

            base.MapPropertyToCms(mappingContext);
        }

        /// <summary>
        /// Maps data from the .Net property value to the CMS value
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns>The value to write</returns>
        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            var scConfig = Configuration as SitecoreFieldConfiguration;
            var scContext =  mappingContext  as SitecoreDataMappingContext ;

            var field = Utilities.GetField(scContext.Item, scConfig.FieldId, scConfig.FieldName);
            
            if(field ==null)
               return;
            
            object value = Configuration.PropertyInfo.GetValue(mappingContext.Object, null);


            SetField(field, value, scConfig, scContext);
        }

        /// <summary>
        /// Maps data from the CMS value to the .Net property value
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns>System.Object.</returns>
        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var scConfig = Configuration as SitecoreFieldConfiguration;
            var scContext = mappingContext as SitecoreDataMappingContext;

            var field = Utilities.GetField(scContext.Item, scConfig.FieldId, scConfig.FieldName);

            if (field == null)
                return DefaultValue;

            return GetField(field, scConfig, scContext);
        }


        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.Object.</returns>
        public virtual object GetField(Field field, SitecoreFieldConfiguration config,
                                       SitecoreDataMappingContext context)
        {
            
                var fieldValue = field.Value;
            try
            {
                return GetFieldValue(fieldValue, config, context);
            }
            catch (Exception ex)
            {
#if NCRUNCH
                throw new MapperException("Failed to map field {0} with value {1}".Formatted(field.ID, fieldValue), ex);

#else
                throw new MapperException("Failed to map field {0} with value {1}".Formatted(field.Name, fieldValue), ex);

#endif

            }
        }
        /// <summary>
        /// Sets the field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        public virtual void SetField(Field field, object value, SitecoreFieldConfiguration config,
                                      SitecoreDataMappingContext context)
        {
            field.Value = SetFieldValue(value, config, context);
        }

        /// <summary>
        /// Sets the field value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.String.</returns>
        public abstract string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context);
        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.Object.</returns>
        public abstract object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context);

        /// <summary>
        /// Indicates that the data mapper will mapper to and from the property
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if this instance can handle the specified configuration; otherwise, <c>false</c>.</returns>
        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is SitecoreFieldConfiguration &&
                   TypesHandled.Any(x => x == configuration.PropertyInfo.PropertyType);
        }

        public override void Setup(Mapper.Pipelines.DataMapperResolver.DataMapperResolverArgs args)
        {
            var scArgs = args.PropertyConfiguration as FieldConfiguration;
            this.ReadOnly = scArgs.ReadOnly;
            base.Setup(args);
        }
        
    }
}




