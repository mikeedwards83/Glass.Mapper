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
using System.Reflection;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Umb.Configuration.Attributes
{
    /// <summary>
    /// UmbracoPropertyAttribute
    /// </summary>
    public class UmbracoPropertyAttribute : FieldAttribute
    {
        /// <summary>
        /// Indicates that the property should pull data from an Umbraco property.
        /// </summary>
        public UmbracoPropertyAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoPropertyAttribute"/> class.
        /// </summary>
        /// <param name="propertyAlias">The property alias.</param>
        public UmbracoPropertyAttribute(string propertyAlias)
        {
            PropertyAlias = propertyAlias;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoPropertyAttribute" /> class.
        /// </summary>
        /// <param name="propertyAlias">The property alias.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="contentTab">The content tab.</param>
        /// <param name="codeFirst">if set to <c>true</c> [code first].</param>
        public UmbracoPropertyAttribute(string propertyAlias, UmbracoPropertyType propertyType, string contentTab = "General Properties", bool codeFirst = true)
        {
            PropertyAlias = propertyAlias;
            ContentTab = contentTab;
            CodeFirst = codeFirst;
            PropertyType = propertyType;
        }

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
        /// The name of the property
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; set; }

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

        #endregion

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns></returns>
        public override AbstractPropertyConfiguration Configure(PropertyInfo propertyInfo)
        {
            var config = new UmbracoPropertyConfiguration();
            Configure(propertyInfo, config);
            return config;
        }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="config">The config.</param>
        public void Configure(PropertyInfo propertyInfo, UmbracoPropertyConfiguration config)
        {
            config.PropertyAlias = this.PropertyAlias;
            config.Setting = this.Setting;
            config.CodeFirst = this.CodeFirst;
            
            config.PropertyName = this.PropertyName;
            config.PropertyDescription = this.PropertyDescription;
            config.PropertyIsMandatory = this.PropertyIsMandatory;
            config.PropertyValidation = this.PropertyValidation;
            config.ContentTab = this.ContentTab;
            config.PropertyType = this.PropertyType;
            config.Setting = this.Setting;

			// if the property name / alias has not been set, use the name of the 
			// reflected property
	        if (String.IsNullOrEmpty(config.PropertyAlias))
	        {
		        config.PropertyAlias = propertyInfo.Name;
	        }

	        base.Configure(propertyInfo, config);
        }
    }
}



