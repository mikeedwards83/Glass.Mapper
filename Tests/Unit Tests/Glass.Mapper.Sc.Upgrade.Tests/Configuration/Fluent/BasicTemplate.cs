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
using Glass.Mapper.Sc.Fields;

namespace Glass.Mapper.Sc.Upgrade.Tests.Configuration.Fluent
{
    public class BasicTemplate
    {
        #region SitecoreId

        public virtual Guid Id { get; set; }

        #endregion

        #region Fields
        #region Simple Types

        public virtual bool Checkbox { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual DateTime DateTime { get; set; }
        public virtual File File { get; set; }
        public virtual Image Image { get; set; }
        public virtual int Integer { get; set; }
        public virtual float Float { get; set; }
        public virtual double Double { get; set; }
        public virtual decimal Decimal { get; set; }
        public virtual string MultiLineText { get; set; }
        public virtual int Number { get; set; }
        public virtual string Password { get; set; }
        public virtual string RichText { get; set; }
        public virtual string SingleLineText { get; set; }

        #endregion

        #region List Types

        public virtual IEnumerable<SubClass> CheckList { get; set; }
        public virtual TestEnum DropList { get; set; }
        public virtual SubClass GroupedDropLink { get; set; }
        public virtual TestEnum GroupedDropList { get; set; }
        public virtual IEnumerable<SubClass> MultiList { get; set; }
        public virtual IEnumerable<SubClass> Treelist { get; set; }
        public virtual IEnumerable<SubClass> TreeListEx { get; set; }

        #endregion

        #region Link Types

        public virtual SubClass DropLink { get; set; }
        public virtual SubClass DropTree { get; set; }
        public virtual Link GeneralLink { get; set; }

        #endregion

        #region Developer Types

        public virtual string Icon { get; set; }
        public virtual TriState TriState { get; set; }

        #endregion

        #region SystemType

        public virtual System.IO.Stream Attachment { get; set; }

        #endregion

        #endregion

        #region SitecoreInfo

        public virtual string ContentPath { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string FullPath { get; set; }
        public virtual string Key { get; set; }
        public virtual string MediaUrl { get; set; }
        public virtual string Path { get; set; }
        public virtual Guid TemplateId { get; set; }
        public virtual string TemplateName { get; set; }
        public virtual string Url { get; set; }
        public virtual int Version { get; set; }

        #endregion

        #region SitecoreChildren

        public virtual IEnumerable<SubClass> Children { get; set; }

        #endregion

        #region SitecoreParent

        public virtual SubClass Parent { get; set; }

        #endregion

        #region SitecoreQuery

        public virtual IEnumerable<SubClass> Query { get; set; }

        #endregion

    }

    public class SubClass
    {

        public virtual Guid Id { get; set; }

    }

    public enum TestEnum
    {
        Test1,
        Test2,
        Test3
    }
}

