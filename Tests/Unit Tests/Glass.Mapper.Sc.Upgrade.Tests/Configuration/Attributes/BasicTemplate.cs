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
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Glass.Mapper.Sc.Upgrade.Configuration.Attribute;

namespace Glass.Mapper.Sc.Upgrade.Tests.Configuration.Attributes
{
    [SitecoreClass(TemplateId = "{1D0EE1F5-21E0-4C5B-8095-EDE2AF3D3300}")]
    public class BasicTemplate
    {
        #region SitecoreId

        [SitecoreId]
        public virtual Guid Id { get; set; }

        #endregion

        #region Fields
        #region Simple Types

        [SitecoreField]
        public virtual bool Checkbox { get; set; }
        [SitecoreField]
        public virtual DateTime Date { get; set; }
        [SitecoreField]
        public virtual DateTime DateTime { get; set; }
        [SitecoreField]
        public virtual File File { get; set; }
        [SitecoreField]
        public virtual Image Image { get; set; }
        [SitecoreField]
        public virtual int Integer { get; set; }
        [SitecoreField]
        public virtual float Float { get; set; }
        [SitecoreField]
        public virtual double Double { get; set; }
        [SitecoreField]
        public virtual decimal Decimal { get; set; }

        [SitecoreField]
        public virtual string MultiLineText { get; set; }
        [SitecoreField]
        public virtual int Number { get; set; }
        [SitecoreField]
        public virtual string Password { get; set; }
        [SitecoreField(Setting = SitecoreFieldSettings.RichTextRaw)]
        public virtual string RichText { get; set; }
        [SitecoreField]
        public virtual string SingleLineText { get; set; }

        #endregion

        #region List Types

        [SitecoreField]
        public virtual IEnumerable<SubClass> CheckList { get; set; }
        [SitecoreField]
        public virtual TestEnum DropList { get; set; }
        [SitecoreField]
        public virtual SubClass GroupedDropLink { get; set; }
        [SitecoreField]
        public virtual TestEnum GroupedDropList { get; set; }
        [SitecoreField]
        public virtual IEnumerable<SubClass> MultiList { get; set; }
        [SitecoreField]
        public virtual IEnumerable<SubClass> Treelist { get; set; }
        [SitecoreField]
        public virtual IEnumerable<SubClass> TreeListEx { get; set; }

        #endregion

        #region Link Types

        [SitecoreField]
        public virtual SubClass DropLink { get; set; }
        [SitecoreField]
        public virtual SubClass DropTree { get; set; }
        [SitecoreField]
        public virtual Link GeneralLink { get; set; }

        #endregion

        #region Developer Types

        [SitecoreField]
        public virtual string Icon { get; set; }

        [SitecoreField]
        public virtual TriState TriState { get; set; }

        #endregion

        #region SystemType

        [SitecoreField]
        public virtual System.IO.Stream Attachment { get; set; }

        #endregion

        #endregion

        #region SitecoreInfo

        [SitecoreInfo(SitecoreInfoType.ContentPath)]
        public virtual string ContentPath { get; set; }
        [SitecoreInfo(SitecoreInfoType.DisplayName)]
        public virtual string DisplayName { get; set; }
        [SitecoreInfo(SitecoreInfoType.FullPath)]
        public virtual string FullPath { get; set; }
        [SitecoreInfo(SitecoreInfoType.Key)]
        public virtual string Key { get; set; }
        [SitecoreInfo(SitecoreInfoType.MediaUrl)]
        public virtual string MediaUrl { get; set; }
        [SitecoreInfo(SitecoreInfoType.Path)]
        public virtual string Path { get; set; }
        [SitecoreInfo(SitecoreInfoType.TemplateId)]
        public virtual Guid TemplateId { get; set; }
        [SitecoreInfo(SitecoreInfoType.TemplateName)]
        public virtual string TemplateName { get; set; }
        [SitecoreInfo(SitecoreInfoType.Url)]
        public virtual string Url { get; set; }
        [SitecoreInfo(SitecoreInfoType.Version)]
        public virtual int Version { get; set; }

        #endregion

        #region SitecoreChildren

        [SitecoreChildren]
        public virtual IEnumerable<SubClass> Children { get; set; }

        #endregion

        #region SitecoreParent

        [SitecoreParent]
        public virtual SubClass Parent { get; set; }

        #endregion

        #region SitecoreQuery

        [SitecoreQuery("/sitecore/content/Glass/*[@@TemplateName='BasicTemplate']")]
        public virtual IEnumerable<SubClass> Query { get; set; }

        #endregion

    }

    [SitecoreClass]
    public class SubClass
    {

        [SitecoreId]
        public virtual Guid Id { get; set; }

    }

    public enum TestEnum
    {
        Test1,
        Test2,
        Test3
    }
}

