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
using Sitecore.Data;

namespace Glass.Mapper.Sc.CodeFirst
{
    public class FieldInfo
    {
        public FieldInfo(ID fieldId, ID sectionId, string name, SitecoreFieldType type, string source, string title, bool isShared, bool isUnversioned, int fieldSortOrder, string validationRegularExpression, string validationErrorText, bool isRequired)
        {
            FieldId = fieldId;
            SectionId = sectionId;
            Name = name;
            Type = type;
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

        public ID FieldId { get; set; }
        public ID SectionId { get; set; }
        public string Name { get; set; }
        public SitecoreFieldType Type { get; set; }
        public string Source { get; set; }
        public string Title { get; set; }
        public bool IsShared { get; set; }
        public bool IsUnversioned { get; set; }

        public Dictionary<ID,string> FieldFieldValues { get; protected set; }

        public int FieldSortOrder { get; set; }
        public string ValidationRegularExpression { get; set; }
        public string ValidationErrorText { get; set; }
        public bool IsRequired { get; set; }

        public static string GetFieldType(SitecoreFieldType type)
        {
            switch (type)
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
                default:
                    return "Single-Line Text";
            }
        }
    }
}
