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
using System.Reflection;
using System.Linq.Expressions;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration.Fluent
{
    /// <summary>
    /// Used to populate the property with data from a Sitecore field
    /// </summary>
    public class SitecoreField<T> : AbstractPropertyBuilder<T, SitecoreFieldConfiguration>
    {

        public SitecoreField(Expression<Func<T, object>> ex)
            : base(ex)
        {
            Configuration.FieldName = Configuration.PropertyInfo.Name;
        }

        /// <summary>
        /// Indicate that the field can be used with Code First
        /// </summary>
        public SitecoreField<T> IsCodeFirst()
        {
            Configuration.CodeFirst = true;
            return this;
        }


        /// <summary>
        /// The ID of the field to load or create in code first
        /// </summary>
        public SitecoreField<T> FieldId(string id)
        {
            return FieldId(new ID(id));
        }
        /// <summary>
        /// The ID of the field to load or create in code first
        /// </summary>
        public SitecoreField<T> FieldId(Guid id)
        {
            return FieldId(new ID(id));
        }

        /// <summary>
        /// The ID of the field to load or create in code first
        /// </summary>
        public SitecoreField<T> FieldId(ID id)
        {
            Configuration.FieldId = id;
            return this;
        }


        /// <summary>
        /// The name of the field  to use if it is different to the property name
        /// </summary>
        public SitecoreField<T> FieldName(string name)
        {
            Configuration.FieldName = name;
            return this;
        }

        /// <summary>
        /// The source for a code first field
        /// </summary>
        public SitecoreField<T> FieldSource(string fieldSource)
        {
            Configuration.FieldSource = fieldSource;
            return this;
        }

        /// <summary>
        /// The title for a code first field
        /// </summary>
        public SitecoreField<T> FieldTitle(string fieldTitle)
        {
            Configuration.FieldTitle = fieldTitle;
            return this;
        }

        /// <summary>
        /// The Sitecore field type for a code first field
        /// </summary>
        public SitecoreField<T> FieldType(SitecoreFieldType fieldType)
        {
            Configuration.FieldType = fieldType;
            return this;
        }

        /// <summary>
        /// The field should be shared between languages
        /// </summary>
        public SitecoreField<T> IsShared()
        {
            Configuration.IsShared = true;
            return this;
        }

        /// <summary>
        /// The field should be shared between versions
        /// </summary>
        public SitecoreField<T> IsUnversioned()
        {
            Configuration.IsUnversioned = true;
            return this;
        }



        /// <summary>
        /// Options to override the behaviour of certain fields.
        /// </summary>
        public SitecoreField<T> Setting(SitecoreFieldSettings setting)
        {
            Configuration.Setting = setting;
            return this;
        }

        /// <summary>
        /// The name of the section the field should appear in
        /// </summary>
        public SitecoreField<T> SectionName(string sectionName)
        {
            Configuration.SectionName = sectionName;
            return this;
        }

        /// <summary>
        /// Indicate that the field can not be written to Sitecore
        /// </summary>
        public SitecoreField<T> ReadOnly()
        {
            Configuration.ReadOnly = true;
            return this;
        }

        public SitecoreField<T> FieldSortOrder(int order)
        {
            Configuration.FieldSortOrder = order;
            return this;
        }

        public SitecoreField<T> SectionSortOrder(int order)
        {
            Configuration.SectionSortOrder = order;
            return this;
        }

        public SitecoreField<T> ValidationErrorText(string text)
        {
            Configuration.ValidationErrorText = text;
            return this;
        }

        public SitecoreField<T> ValidationRegularExpression(string regex)
        {
            Configuration.ValidationRegularExpression = regex;
            return this;
        }

        public SitecoreField<T> IsRequired()
        {
            Configuration.IsRequired = true;
           return this;
        }
    }
}



