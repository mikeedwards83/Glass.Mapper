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
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration
{
    /// <summary>
    /// Class SitecoreFieldConfiguration
    /// </summary>
    public class SitecoreFieldConfiguration : FieldConfiguration
    {
        /// <summary>
        /// The name of the field  to use if it is different to the property name
        /// </summary>
        /// <value>The name of the field.</value>
        public string FieldName { get; set; }

        /// <summary>
        /// The ID of the field when used in a code first scenario
        /// </summary>
        /// <value>The field id.</value>
        public ID FieldId { get; set; }

        /// <summary>
        /// Options to override the behaviour of certain fields.
        /// </summary>
        /// <value>The setting.</value>
        public SitecoreFieldSettings Setting { get; set; }

        #region Code First Properties

        /// <summary>
        /// Indicates the field should be used as part of a code first template
        /// </summary>
        /// <value><c>true</c> if [code first]; otherwise, <c>false</c>.</value>
        public bool CodeFirst { get; set; }

        /// <summary>
        /// The type of field to create when using Code First
        /// </summary>
        /// <value>The type of the field.</value>
        public SitecoreFieldType FieldType { get; set; }

        /// <summary>
        /// The name of the section this field will appear in when using code first.
        /// </summary>
        /// <value>The name of the section.</value>
        public string SectionName { get; set; }

        /// <summary>
        /// The title for the field if using Code First
        /// </summary>
        /// <value>The field title.</value>
        public string FieldTitle { get; set; }

        /// <summary>
        /// The source for the field if using Code First
        /// </summary>
        /// <value>The field source.</value>
        public string FieldSource { get; set; }

        /// <summary>
        /// Sets the field as shared if using Code First
        /// </summary>
        /// <value><c>true</c> if this instance is shared; otherwise, <c>false</c>.</value>
        public bool IsShared { get; set; }

        /// <summary>
        /// Sets the field as unversioned if using Code First
        /// </summary>
        /// <value><c>true</c> if this instance is unversioned; otherwise, <c>false</c>.</value>
        public bool IsUnversioned { get; set; }

        /// <summary>
        /// Overrides the field sort order if using Code First
        /// </summary>
        /// <value>The field sort order.</value>
        public int FieldSortOrder { get; set; }

        /// <summary>
        /// Overrides the section sort order if using Code First
        /// </summary>
        /// <value>The section sort order.</value>
        public int SectionSortOrder { get; set; }

        /// <summary>
        /// Overrides the field validation regular expression if using Code First
        /// </summary>
        /// <value>The validation regular expression.</value>
        public string ValidationRegularExpression { get; set; }

        /// <summary>
        /// Overrides the field validation error text if using Code First
        /// </summary>
        /// <value>The validation error text.</value>
        public string ValidationErrorText { get; set; }

        /// <summary>
        /// Sets the field as required if using Code First
        /// </summary>
        /// <value><c>true</c> if this instance is required; otherwise, <c>false</c>.</value>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the field value configs.
        /// </summary>
        /// <value>The field value configs.</value>
        public IEnumerable<SitecoreFieldFieldValueConfiguration> FieldValueConfigs { get; set; }

        /// <summary>
        /// Use with Glass.Mapper.Sc.Fields.Link type
        /// </summary>
        public SitecoreInfoUrlOptions UrlOptions { get; set; }

        /// <summary>
        /// Allows for custom types of field to create when using Code First
        /// </summary>
        /// <value>The type of the field.</value>
        public string CustomFieldType { get; set; }

        #endregion


        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new SitecoreFieldConfiguration();
        }

        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as SitecoreFieldConfiguration;
            config.CodeFirst = this.CodeFirst;
            config.FieldId = this.FieldId;
            config.FieldName = this.FieldName;
            config.FieldSource = this.FieldSource;
            config.FieldTitle = this.FieldTitle;
            config.FieldType = this.FieldType;
            config.IsShared = this.IsShared;
            config.IsUnversioned = this.IsUnversioned;
            config.SectionName = this.SectionName;
            config.Setting = this.Setting;
            config.CustomFieldType = this.CustomFieldType;
            base.Copy(copy);
        }

    }
}





