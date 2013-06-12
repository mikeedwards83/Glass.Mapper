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
using System.Threading.Tasks;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Fluent;
using Glass.Mapper.Sc.Upgrade.Configuration.Fluent;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Upgrade.Tests.Configuration.Fluent
{
    [TestFixture]
    public class MiscFixture
    {
        [Test]
        public void BasicTemplate_LoadedByAttributeLoader()
        {
            //Assign
            var loader = new SitecoreFluentConfigurationLoader();
            Map(loader);


            //Act
            var configs = loader.Load();

            //Assert
            var basicTemplate = configs.First(x => x.Type == typeof(BasicTemplate));
            var subClass = configs.First(x => x.Type == typeof(SubClass));
            Assert.IsNotNull(basicTemplate);
            Assert.IsNotNull(subClass);

            #region SitecoreId

            CheckProperty(basicTemplate, "Id", typeof(SitecoreIdConfiguration));

            #endregion
            #region Fields
            #region Simple Types

            CheckProperty(basicTemplate, "Checkbox", typeof(SitecoreFieldConfiguration));
            CheckProperty(basicTemplate, "Date", typeof(SitecoreFieldConfiguration));
            CheckProperty(basicTemplate, "DateTime", typeof(SitecoreFieldConfiguration));
            CheckProperty(basicTemplate, "File", typeof(SitecoreFieldConfiguration));
            CheckProperty(basicTemplate, "Image", typeof(SitecoreFieldConfiguration));
            CheckProperty(basicTemplate, "Integer", typeof(SitecoreFieldConfiguration));
            CheckProperty(basicTemplate, "Float", typeof(SitecoreFieldConfiguration));
            CheckProperty(basicTemplate, "Double", typeof(SitecoreFieldConfiguration));
            CheckProperty(basicTemplate, "Decimal", typeof(SitecoreFieldConfiguration));
            CheckProperty(basicTemplate, "MultiLineText", typeof(SitecoreFieldConfiguration));
            CheckProperty(basicTemplate, "Number", typeof(SitecoreFieldConfiguration));
            CheckProperty(basicTemplate, "Password", typeof(SitecoreFieldConfiguration));
            CheckProperty(basicTemplate, "RichText", typeof(SitecoreFieldConfiguration));
            CheckProperty(basicTemplate, "SingleLineText", typeof(SitecoreFieldConfiguration));

            #endregion
            #region List Types

            CheckProperty(basicTemplate, "CheckList", typeof(SitecoreFieldConfiguration));
            CheckProperty(basicTemplate, "DropList", typeof(SitecoreFieldConfiguration));
            CheckProperty(basicTemplate, "GroupedDropLink", typeof(SitecoreFieldConfiguration));
            CheckProperty(basicTemplate, "GroupedDropList", typeof(SitecoreFieldConfiguration));
            CheckProperty(basicTemplate, "MultiList", typeof(SitecoreFieldConfiguration));
            CheckProperty(basicTemplate, "Treelist", typeof(SitecoreFieldConfiguration));
            CheckProperty(basicTemplate, "TreeListEx", typeof(SitecoreFieldConfiguration));

            #endregion
            #region Link Types

            CheckProperty(basicTemplate, "DropLink", typeof(SitecoreFieldConfiguration));
            CheckProperty(basicTemplate, "DropTree", typeof(SitecoreFieldConfiguration));
            CheckProperty(basicTemplate, "GeneralLink", typeof(SitecoreFieldConfiguration));

            #endregion
            #region Developer Types

            CheckProperty(basicTemplate, "Icon", typeof(SitecoreFieldConfiguration));
            CheckProperty(basicTemplate, "TriState", typeof(SitecoreFieldConfiguration));

            #endregion
            #region SystemType

            CheckProperty(basicTemplate, "Attachment", typeof(SitecoreFieldConfiguration));

            #endregion
            #endregion

            #region SitecoreInfo

            CheckProperty(basicTemplate, "ContentPath", typeof(SitecoreInfoConfiguration));
            CheckProperty(basicTemplate, "DisplayName", typeof(SitecoreInfoConfiguration));
            CheckProperty(basicTemplate, "FullPath", typeof(SitecoreInfoConfiguration));
            CheckProperty(basicTemplate, "Key", typeof(SitecoreInfoConfiguration));
            CheckProperty(basicTemplate, "MediaUrl", typeof(SitecoreInfoConfiguration));
            CheckProperty(basicTemplate, "Path", typeof(SitecoreInfoConfiguration));
            CheckProperty(basicTemplate, "TemplateId", typeof(SitecoreInfoConfiguration));
            CheckProperty(basicTemplate, "TemplateName", typeof(SitecoreInfoConfiguration));
            CheckProperty(basicTemplate, "Url", typeof(SitecoreInfoConfiguration));
            CheckProperty(basicTemplate, "Version", typeof(SitecoreInfoConfiguration));

            #endregion

            #region SitecoreChildren

            CheckProperty(basicTemplate, "Children", typeof(SitecoreChildrenConfiguration));

            #endregion

            #region SitecoreParent

            CheckProperty(basicTemplate, "Parent", typeof(SitecoreParentConfiguration));

            #endregion

            #region SitecoreQuery

            CheckProperty(basicTemplate, "Query", typeof(SitecoreQueryConfiguration));

            #endregion
        }

        public static void CheckProperty(AbstractTypeConfiguration config, string name, Type configType)
        {
            var pConfig = config.Properties.FirstOrDefault(x => x.PropertyInfo.Name == name);

            Assert.IsNotNull(pConfig, "Could not find property with name {0}".Formatted(name));
            Assert.AreEqual(configType, pConfig.GetType(), "Property {0} is not of expected type".Formatted(name));
        }

        public static void Map(SitecoreFluentConfigurationLoader loader)
        {
            var basic = new SitecoreClass<BasicTemplate>()
            .Fields(x =>
            {
                x.Field(y => y.Checkbox);
                x.Field(y => y.Date);
            })
            .Infos(x =>
            {
                x.Info(y => y.ContentPath).InfoType(SitecoreInfoType.ContentPath);
                x.Info(y => y.DisplayName).InfoType(SitecoreInfoType.DisplayName);
            })
            .Queries(x =>
            {
                x.Query(y => y.Query).Query("/sitecore/content/Configuration/Fluent/GeneralFluent/Query/*[@@TemplateName='BasicTemplate']");

            });


            basic.Id(x => x.Id);

            //basic.Field(x => x.Checkbox);
            //basic.Field(x => x.Date);
            basic.Field(x => x.DateTime);
            basic.Field(x => x.File);
            basic.Field(x => x.Image);
            basic.Field(x => x.Integer);
            basic.Field(x => x.Float);
            basic.Field(x => x.Double);
            basic.Field(x => x.Decimal);
            basic.Field(x => x.MultiLineText);
            basic.Field(x => x.Number);
            basic.Field(x => x.Password);
            basic.Field(x => x.RichText).Setting(SitecoreFieldSettings.RichTextRaw);
            basic.Field(x => x.SingleLineText);

            basic.Field(x => x.CheckList);
            basic.Field(x => x.DropList);
            basic.Field(x => x.GroupedDropLink);
            basic.Field(x => x.GroupedDropList);
            basic.Field(x => x.MultiList);
            basic.Field(x => x.Treelist);
            basic.Field(x => x.TreeListEx);

            basic.Field(x => x.DropLink);
            basic.Field(x => x.DropTree);
            basic.Field(x => x.GeneralLink);

            basic.Field(x => x.Icon);
            basic.Field(x => x.TriState);

            basic.Field(x => x.Attachment);


            basic.Info(x => x.FullPath).InfoType(SitecoreInfoType.FullPath);
            basic.Info(x => x.Key).InfoType(SitecoreInfoType.Key);
            basic.Info(x => x.MediaUrl).InfoType(SitecoreInfoType.MediaUrl);
            basic.Info(x => x.Path).InfoType(SitecoreInfoType.Path);
            basic.Info(x => x.TemplateId).InfoType(SitecoreInfoType.TemplateId);
            basic.Info(x => x.TemplateName).InfoType(SitecoreInfoType.TemplateName);
            basic.Info(x => x.Url).InfoType(SitecoreInfoType.Url);
            basic.Info(x => x.Version).InfoType(SitecoreInfoType.Version);

            basic.Children(x => x.Children);

            basic.Parent(x => x.Parent);


            var subClass = new SitecoreClass<SubClass>();
            subClass.Id(x => x.Id);

            loader.Add(subClass);
            loader.Add(basic);
        }

    }
}

