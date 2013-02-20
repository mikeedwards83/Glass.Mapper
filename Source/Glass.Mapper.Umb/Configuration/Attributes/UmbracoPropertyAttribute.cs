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
    public class UmbracoPropertyAttribute : FieldAttribute
    {
        public UmbracoPropertyAttribute(string propertyAlias)
        {
            PropertyAlias = propertyAlias;
        }

        public UmbracoPropertyAttribute(int propertyId, UmbracoDataType propertyType, string documentTab = "General Properties", bool codeFirst = true)
        {
            PropertyId = propertyId;
            DocumentTab = documentTab;
            CodeFirst = codeFirst;
            DataType = propertyType;
        }
        
        /// <summary>
        /// The alias for the property  to use if it is different to the property name
        /// </summary>
        public string PropertyAlias { get; set; }

        /// <summary>
        /// The ID of the property when used in a code first scenario 
        /// </summary>
        public int PropertyId { get; set; } 

        /// <summary>
        /// Options to override the behaviour of certain properties.
        /// </summary>
        public UmbracoPropertySettings Setting { get; set; }

        #region Code First Properties

        /// <summary>
        /// The name of the property
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Indicates the property should be used as part of a code first template
        /// </summary>
        public bool CodeFirst { get; set; }
        
        /// <summary>
        /// The type of property to create when using Code First
        /// </summary>
        public UmbracoDataType DataType { get; set; }

        /// <summary>
        /// The name of the tab this property will appear in when using code first.
        /// </summary>
        public string DocumentTab { get; set; }

        /// <summary>
        /// The description for the property
        /// </summary>
        public string PropertyDescription { get; set; }

        /// <summary>
        /// Indicates that the property is mandatory
        /// </summary>
        public bool PropertyIsMandatory { get; set; }

        /// <summary>
        /// The validation for the property
        /// </summary>
        public string PropertyValidation { get; set; }

        #endregion

        /// <summary>
        /// Indicates that the property should pull data from an Umbraco property.
        /// </summary>
        public UmbracoPropertyAttribute()
        {
        }

        public override AbstractPropertyConfiguration Configure(PropertyInfo propertyInfo)
        {
            var config = new UmbracoPropertyConfiguration();
            Configure(propertyInfo, config);
            return config;
        }

        public void Configure(PropertyInfo propertyInfo, UmbracoPropertyConfiguration config)
        {
            config.PropertyAlias = this.PropertyAlias;
            config.Setting = this.Setting;
            config.CodeFirst = this.CodeFirst;
            
            config.PropertyId = this.PropertyId;
            config.PropertyName = this.PropertyName;
            config.PropertyDescription = this.PropertyDescription;
            config.PropertyIsMandatory = this.PropertyIsMandatory;
            config.PropertyValidation = this.PropertyValidation;
            config.DocumentTab = this.DocumentTab;
            config.DataType = this.DataType;
            config.Setting = this.Setting;
            base.Configure(propertyInfo, config);
        }
    }
}


