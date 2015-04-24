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
using Glass.Mapper.Sc.Configuration;
using Sitecore.Rules;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class SitecoreFieldRulesMapper
    /// </summary>
    public class SitecoreFieldRulesMapper : AbstractSitecoreFieldMapper
    {
        /// <summary>
        /// Sets the field value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            throw new NotImplementedException();
           
        }

        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.Object.</returns>
        public override object GetField(Sitecore.Data.Fields.Field field, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            Type ruleFactory = typeof(RuleFactory);
            var method = ruleFactory.GetMethod("GetRules",
                                                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static,
                                                null,
                                                new Type[] { typeof(global::Sitecore.Data.Fields.Field) },
                                                null);
            Type[] genericArgs = Configuration.PropertyInfo.PropertyType.GetGenericArguments();
            method = method.MakeGenericMethod(genericArgs);
            object rules = method.Invoke(null, new object[] { field });
            return rules;
        }
        /// <summary>
        /// Determines whether this instance can handle the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if this instance can handle the specified configuration; otherwise, <c>false</c>.</returns>
        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            var type = configuration.PropertyInfo.PropertyType;
            if (!type.IsGenericType)
            {
                return false;
            }
            var baseType = type.GetGenericTypeDefinition();
            if (baseType == typeof(RuleList<>))
            {
                return true;
            }
            return false;
        }
    }
}




