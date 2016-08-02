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
using System.Linq.Expressions;

namespace Glass.Mapper.Umb.Configuration.Fluent
{
    /// <summary>
    /// Used to populate the property with data from a umbraco property
    /// </summary>
    public class UmbracoProperty<T> : AbstractPropertyBuilder<T, UmbracoPropertyConfiguration>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoProperty{T}"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public UmbracoProperty(Expression<Func<T, object>> ex)
            : base(ex)
        {
            Configuration.PropertyAlias = Configuration.PropertyInfo.Name;
        }

        /// <summary>
        /// Indicate that the property can be used with Code First
        /// </summary>
        /// <returns></returns>
        public UmbracoProperty<T> IsCodeFirst()
        {
            Configuration.CodeFirst = true;
            return this;
        }
        
        /// <summary>
        /// The alias of the property to load or create in code first
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        public UmbracoProperty<T> PropertyAlias(string alias)
        {
            Configuration.PropertyAlias = alias;
            return this;
        }
        
        /// <summary>
        /// The name of the property  to use if it is different to the property name
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public UmbracoProperty<T> PropertyName(string name)
        {
            Configuration.PropertyName = name;
            return this;
        }

        /// <summary>
        /// The description for a code first property
        /// </summary>
        /// <param name="propertyDescription">The property description.</param>
        /// <returns></returns>
        public UmbracoProperty<T> PropertyDescription(string propertyDescription)
        {
            Configuration.PropertyDescription = propertyDescription;
            return this;
        }

        /// <summary>
        /// The validation for a code first property
        /// </summary>
        /// <param name="propertyValidation">The property validation.</param>
        /// <returns></returns>
        public UmbracoProperty<T> PropertyValidation(string propertyValidation)
        {
            Configuration.PropertyValidation = propertyValidation;
            return this;
        }

        /// <summary>
        /// The umbraco property type for a code first property
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <returns></returns>
        public UmbracoProperty<T> PropertyType(UmbracoPropertyType propertyType)
        {
            Configuration.PropertyType = propertyType;
            return this;
        }

        /// <summary>
        /// The property should be mandatory
        /// </summary>
        /// <returns></returns>
        public UmbracoProperty<T> IsMandatory()
        {
            Configuration.PropertyIsMandatory = true;
            return this;
        }

        /// <summary>
        /// Options to override the behaviour of certain properties.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns></returns>
        public UmbracoProperty<T> Setting(UmbracoPropertySettings setting)
        {
            Configuration.Setting = setting;
            return this;
        }

        /// <summary>
        /// The name of the tab the property should appear in
        /// </summary>
        /// <param name="contentTab">The content tab.</param>
        /// <returns></returns>
        public UmbracoProperty<T> ContentTab(string contentTab)
        {
            Configuration.ContentTab = contentTab;
            return this;
        }

        /// <summary>
        /// Indicate that the property can not be written to umbraco
        /// </summary>
        /// <returns></returns>
        public UmbracoProperty<T> ReadOnly()
        {
            Configuration.ReadOnly = true;
            return this;
        }
    }
}




