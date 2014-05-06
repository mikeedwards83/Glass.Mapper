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
/*
   Copyright 2011 Michael Edwards
 
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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using global::Sitecore.Data;

namespace Glass.Mapper.Sc.CodeFirst
{
    /// <summary>
    /// Class FieldInfo
    /// </summary>
    public class FieldInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldInfo"/> class.
        /// </summary>
        /// <param name="fieldId">The field id.</param>
        /// <param name="sectionId">The section id.</param>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="customFieldType">The custom field type.</param>
        /// <param name="source">The source.</param>
        /// <param name="title">The title.</param>
        /// <param name="isShared">if set to <c>true</c> [is shared].</param>
        /// <param name="isUnversioned">if set to <c>true</c> [is unversioned].</param>
        /// <param name="fieldSortOrder">The field sort order.</param>
        /// <param name="validationRegularExpression">The validation regular expression.</param>
        /// <param name="validationErrorText">The validation error text.</param>
        /// <param name="isRequired">if set to <c>true</c> [is required].</param>
        public FieldInfo(ID fieldId, ID sectionId, string name, SitecoreFieldType type, string customFieldType, string source, string title, bool isShared, bool isUnversioned, int fieldSortOrder, string validationRegularExpression, string validationErrorText, bool isRequired)
        {
            FieldId = fieldId;
            SectionId = sectionId;
            Name = name;
            FieldType = type;
            CustomFieldType = customFieldType;
            Source = source;
            Title = title;
            IsShared = isShared;
            IsUnversioned = isUnversioned;
            FieldFieldValues = new Dictionary<ID, string>();
            FieldSortOrder = fieldSortOrder;
            ValidationRegularExpression = validationRegularExpression;
            ValidationErrorText = validationErrorText;
            IsRequired = isRequired;
        }

        /// <summary>
        /// Gets or sets the field id.
        /// </summary>
        /// <value>The field id.</value>
        public ID FieldId { get; set; }
        /// <summary>
        /// Gets or sets the section id.
        /// </summary>
        /// <value>The section id.</value>
        public ID SectionId { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the custom field type.
        /// </summary>
        /// <value>The field type.</value>
        public SitecoreFieldType FieldType { get; set; }
        /// <summary>
        /// Gets or sets the field type.
        /// </summary>
        /// <value>The custom field type.</value>
        public string CustomFieldType { get; set; }
        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public string Source { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is shared.
        /// </summary>
        /// <value><c>true</c> if this instance is shared; otherwise, <c>false</c>.</value>
        public bool IsShared { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is unversioned.
        /// </summary>
        /// <value><c>true</c> if this instance is unversioned; otherwise, <c>false</c>.</value>
        public bool IsUnversioned { get; set; }

        /// <summary>
        /// Gets or sets the field field values.
        /// </summary>
        /// <value>The field field values.</value>
        public Dictionary<ID,string> FieldFieldValues { get; protected set; }

        /// <summary>
        /// Gets or sets the field sort order.
        /// </summary>
        /// <value>The field sort order.</value>
        public int FieldSortOrder { get; set; }
        /// <summary>
        /// Gets or sets the validation regular expression.
        /// </summary>
        /// <value>The validation regular expression.</value>
        public string ValidationRegularExpression { get; set; }
        /// <summary>
        /// Gets or sets the validation error text.
        /// </summary>
        /// <value>The validation error text.</value>
        public string ValidationErrorText { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is required.
        /// </summary>
        /// <value><c>true</c> if this instance is required; otherwise, <c>false</c>.</value>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetFieldType()
        {
            switch (FieldType)
            {
                case SitecoreFieldType.Checkbox:
                    return "Checkbox";
                case SitecoreFieldType.Date:
                    return "Date";
                case SitecoreFieldType.DateTime:
                    return "Datetime";
                case SitecoreFieldType.File:
                    return "File";
                case SitecoreFieldType.Image:
                    return "Image";
                case SitecoreFieldType.Integer:
                    return "Integer";
                case SitecoreFieldType.MultiLineText:
                    return "Multi-Line Text";
                case SitecoreFieldType.Number:
                    return "Number";
                case SitecoreFieldType.Password:
                    return "Password";
                case SitecoreFieldType.RichText:
                    return "Rich Text";
                case SitecoreFieldType.SingleLineText:
                    return "Single-Line Text";
                case SitecoreFieldType.Checklist:
                    return "Checklist";
                case SitecoreFieldType.Droplist:
                    return "Droplist";
                case SitecoreFieldType.GroupedDroplink:
                    return "Grouped Droplink";
                case SitecoreFieldType.GroupedDroplist:
                    return "Grouped Droplist";
                case SitecoreFieldType.Multilist:
                    return "Multilist";
                case SitecoreFieldType.Treelist:
                    return "Treelist";
                case SitecoreFieldType.TreelistEx:
                    return "TreelistEx";
                case SitecoreFieldType.Droplink:
                    return "Droplink";
                case SitecoreFieldType.DropTree:
                    return "Droptree";
                case SitecoreFieldType.GeneralLink:
                    return "General Link";
                case SitecoreFieldType.Custom:
                    return CustomFieldType;
                default:
                    return "Single-Line Text";
            }
        }
    }
}

