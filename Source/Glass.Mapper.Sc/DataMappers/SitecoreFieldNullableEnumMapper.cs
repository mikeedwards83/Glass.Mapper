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
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class SitecoreFieldNullableMapper
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TMapper">The type of the T mapper.</typeparam>
    public class SitecoreFieldNullableEnumMapper : AbstractSitecoreFieldMapper 
    {
        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.Object.</returns>
        public override object GetField(Sitecore.Data.Fields.Field field, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            if (string.IsNullOrWhiteSpace(field.Value))
            {
                return null;
            }

            return base.GetField(field, config, context);
        }

        /// <summary>
        /// Sets the field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        public override void SetField(Sitecore.Data.Fields.Field field, object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            if (value == null)
            {
                field.Value = null;
                return;
            }

            base.SetField(field, value, config, context);
        }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="Glass.Mapper.MapperException">
        /// Can not convert value {0} to enum type {1}.Formatted(fieldValue, enumType.FullName)
        /// or
        /// Can not convert value {0} to enum type {1}.Formatted(fieldValue, enumType.FullName)
        /// </exception>
        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            Type enumType = Nullable.GetUnderlyingType(config.PropertyInfo.PropertyType);

            int intValue;
            if (int.TryParse(fieldValue, out intValue))
            {
                return Enum.ToObject(enumType, intValue);
            }
            else
            {
                if (Enum.GetNames(enumType).Any(x => x.Equals(fieldValue, StringComparison.InvariantCultureIgnoreCase)))
                    return Enum.Parse(enumType, fieldValue, true);
                else if (!fieldValue.IsNullOrEmpty())
                    throw new MapperException("Can not convert value {0} to enum type {1}".Formatted(fieldValue,
                                                                                                     enumType.FullName));
                else
                    return Enum.GetValues(enumType).GetValue(0);

            }
        }

        /// <summary>
        /// Sets the field value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.String.</returns>
        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            Type enumType = Nullable.GetUnderlyingType(config.PropertyInfo.PropertyType);

            return Enum.GetName(enumType, value);
        }

        public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
        {
            var underlyingType = Nullable.GetUnderlyingType(configuration.PropertyInfo.PropertyType);

            return underlyingType != null && underlyingType.IsEnum;
        }

    }
}




