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


using System.Collections.Generic;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Umb.Configuration
{
    /// <summary>
    /// UmbracoPropertyConfiguration
    /// </summary>
    public class UmbracoPropertyConfiguration : FieldConfiguration
    {
        /// <summary>
        /// The alias for the property  to use if it is different to the property name
        /// </summary>
        /// <value>
        /// The property alias.
        /// </value>
        public string PropertyAlias { get; set; }

        /// <summary>
        /// Options to override the behaviour of certain properties.
        /// </summary>
        /// <value>
        /// The setting.
        /// </value>
        public UmbracoPropertySettings Setting { get; set; }

        #region Code First Properties

        /// <summary>
        /// Indicates the property should be used as part of a code first template
        /// </summary>
        /// <value>
        ///   <c>true</c> if [code first]; otherwise, <c>false</c>.
        /// </value>
        public bool CodeFirst { get; set; }

        /// <summary>
        /// The type of property to create when using Code First
        /// </summary>
        /// <value>
        /// The type of the property.
        /// </value>
        public UmbracoPropertyType PropertyType { get; set; }

        /// <summary>
        /// The name of the tab this property will appear in when using code first.
        /// </summary>
        /// <value>
        /// The content tab.
        /// </value>
        public string ContentTab { get; set; }

        /// <summary>
        /// The name of the property
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; set; }

        /// <summary>
        /// The description for the property
        /// </summary>
        /// <value>
        /// The property description.
        /// </value>
        public string PropertyDescription { get; set; }

        /// <summary>
        /// Indicates that the property is mandatory
        /// </summary>
        /// <value>
        ///   <c>true</c> if [property is mandatory]; otherwise, <c>false</c>.
        /// </value>
        public bool PropertyIsMandatory { get; set; }

        /// <summary>
        /// The validation for the property
        /// </summary>
        /// <value>
        /// The property validation.
        /// </value>
        public string PropertyValidation { get; set; }

        /// <summary>
        /// Gets or sets the property value configs.
        /// </summary>
        /// <value>
        /// The property value configs.
        /// </value>
        public IEnumerable<UmbracoPropertyValueConfiguration> PropertyValueConfigs { get; set; }

        #endregion

        /// <summary>
        /// Makes a copy of the UmbracoPropertyConfiguration
        /// </summary>
        /// <returns></returns>
        public UmbracoPropertyConfiguration Copy()
        {
            return new UmbracoPropertyConfiguration()
            {
                CodeFirst = this.CodeFirst,
                PropertyAlias = this.PropertyAlias,
                PropertyDescription = this.PropertyDescription,
                ContentTab = this.ContentTab,
                PropertyIsMandatory = this.PropertyIsMandatory,
                PropertyName = this.PropertyName,
                PropertyValidation = this.PropertyValidation,
                PropertyInfo = this.PropertyInfo,
                ReadOnly = this.ReadOnly,
                PropertyType = this.PropertyType,
                Setting = this.Setting
            };
        }
    }
}




