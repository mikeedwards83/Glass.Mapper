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
using System.Reflection;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    /// <summary>
    /// Used to populate default values of a field's field
    /// </summary>
    public class SitecoreFieldFieldValueAttribute : Attribute
    {
        /// <summary>
        /// Indicates that the property should pull data from a Sitecore field.
        /// </summary>
        /// <param name="fieldId">The Id (Guid) of the field to load</param>
        /// <param name="fieldValue">The default field value</param>
        public SitecoreFieldFieldValueAttribute(string fieldId, string fieldValue)
        {
            FieldValue = fieldValue;
            FieldId = new Guid(fieldId);
        }

        /// <summary>
        /// The Id (Guid) of the field to load
        /// </summary>
        /// <value>The field id.</value>
        public Guid FieldId { get; set; }

        /// <summary>
        /// The title for the field if using Code First
        /// </summary>
        /// <value>The field value.</value>
        public string FieldValue { get; set; }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="fieldConfiguration">The field configuration.</param>
        /// <returns>SitecoreFieldFieldValueConfiguration.</returns>
        public SitecoreFieldFieldValueConfiguration Configure(PropertyInfo propertyInfo,
                                                              SitecoreFieldConfiguration fieldConfiguration)
        {
            var config = new SitecoreFieldFieldValueConfiguration();
            
            config.FieldId =  new ID(this.FieldId);
            config.FieldValue = this.FieldValue;
            
            return config;
        }
    }

}

