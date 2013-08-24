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
using Glass.Mapper.Sc.CastleWindsor;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sc.Integration
{
    [TestFixture]
    public class SitecoreServiceFixture
    {

        #region Method - AddVersion

        [Test]
        public void AddVersion_NewVersionCreated()
        {
            //Assign
            string path = "/sitecore/content/Tests/SitecoreService/AddVersion/Target2";
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);

            //clean up everything 
            using (new SecurityDisabler())
            {
                var item = db.GetItem(path);
                item.Versions.RemoveAll(true);
                var firstVersion = item.Versions.AddVersion();
                Assert.AreEqual(1, firstVersion.Version.Number);
            }

            var oldVersion = service.GetItem<StubClass>(path);

            //Act
            using (new SecurityDisabler())
            {
                var newVersion = service.AddVersion(oldVersion);

                //clean up
                var item = db.GetItem(path, newVersion.Language, new Sitecore.Data.Version(newVersion.Version));
                item.Versions.RemoveVersion();
                //Assert
                Assert.AreEqual(oldVersion.Version + 1, newVersion.Version);
            }
        }

        #endregion

        #region Method - GetItem

        [Test]
        public void GetItem_UsingItemId_ReturnsItem()
        {
             //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration") );

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            Guid id = new Guid("{6DE18DBD-0AF3-404A-8018-02B8A19515C1}");

            //Act
            var result =(StubClass)  service.GetItem<StubClass>(id);

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetItem_UsingItemId_ReturnsItemName()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            Guid id = new Guid("{6DE18DBD-0AF3-404A-8018-02B8A19515C1}");

            //Act
            var result = service.GetItem<StubClassWithProperty>(id);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("EmptyItem", result.Name);
        }

        [Test]
        public void GetItem_UsingItemIdLanguage_ReturnsItemName()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            Guid id = new Guid("{6DE18DBD-0AF3-404A-8018-02B8A19515C1}");
            var language = LanguageManager.GetLanguage("af-ZA");

            //Act
            var result = service.GetItem<StubClass>(id, language);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(language, result.Language);
        }

        [Test]
        public void GetItem_UsingItemIdLanguage1Parameter_ReturnsItemName()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            Guid id = new Guid("{6DE18DBD-0AF3-404A-8018-02B8A19515C1}");
            var language = LanguageManager.GetLanguage("af-ZA");
            int param1 = 1;
            
            //Act
            var result = service.GetItem<StubClass, int>(id, language, param1);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(language, result.Language);
            Assert.AreEqual(param1, result.Param1);
        }

        [Test]
        public void GetItem_UsingItemIdLanguage2Parameter_ReturnsItemName()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            Guid id = new Guid("{6DE18DBD-0AF3-404A-8018-02B8A19515C1}");
            var language = LanguageManager.GetLanguage("af-ZA");
            int param1 = 1;
            string param2 = "2param";

            //Act
            var result = service.GetItem<StubClass, int, string>(id, language, param1, param2);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(language, result.Language);
            Assert.AreEqual(param1, result.Param1);
            Assert.AreEqual(param2, result.Param2);
        }

        [Test]
        public void GetItem_UsingItemIdLanguage3Parameter_ReturnsItemName()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            Guid id = new Guid("{6DE18DBD-0AF3-404A-8018-02B8A19515C1}");
            var language = LanguageManager.GetLanguage("af-ZA");
            int param1 = 1;
            string param2 = "2param";
            DateTime param3 = DateTime.Now;

            //Act
            var result = service.GetItem<StubClass, int, string, DateTime>(id, language, param1, param2, param3);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(language, result.Language);
            Assert.AreEqual(param1, result.Param1);
            Assert.AreEqual(param2, result.Param2);
            Assert.AreEqual(param3, result.Param3);
        }

        [Test]
        public void GetItem_UsingItemIdLanguage4Parameter_ReturnsItemName()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            Guid id = new Guid("{6DE18DBD-0AF3-404A-8018-02B8A19515C1}");
            var language = LanguageManager.GetLanguage("af-ZA");
            int param1 = 1;
            string param2 = "2param";
            DateTime param3 = DateTime.Now;
            bool param4 = true;

            //Act
            var result = service.GetItem<StubClass, int, string, DateTime, bool>(id, language, param1, param2, param3, param4);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(language, result.Language);
            Assert.AreEqual(param1, result.Param1);
            Assert.AreEqual(param2, result.Param2);
            Assert.AreEqual(param3, result.Param3);
            Assert.AreEqual(param4, result.Param4);
        }

        [Test]
        public void GetItem_UsingItemPathLanguage_ReturnsItemName()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            string path = "/sitecore/content/Tests/SitecoreService/GetItem/EmptyItem";
            var language = LanguageManager.GetLanguage("af-ZA");

            //Act
            var result = service.GetItem<StubClass>(path, language);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(path, result.Path);
            Assert.AreEqual(language, result.Language);
        }

        [Test]
        public void GetItem_UsingItemPathLanguage1Parameter_ReturnsItemName()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            string path = "/sitecore/content/Tests/SitecoreService/GetItem/EmptyItem";
            var language = LanguageManager.GetLanguage("af-ZA");
            int param1 = 1;

            //Act
            var result = service.GetItem<StubClass, int>(path, language, param1);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(path, result.Path);
            Assert.AreEqual(language, result.Language);
            Assert.AreEqual(param1, result.Param1);
        }

        [Test]
        public void GetItem_UsingItemPathLanguage2Parameter_ReturnsItemName()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            string path = "/sitecore/content/Tests/SitecoreService/GetItem/EmptyItem";
            var language = LanguageManager.GetLanguage("af-ZA");
            int param1 = 1;
            string param2 = "2param";

            //Act
            var result = service.GetItem<StubClass, int, string>(path, language, param1, param2);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(path, result.Path);
            Assert.AreEqual(language, result.Language);
            Assert.AreEqual(param1, result.Param1);
            Assert.AreEqual(param2, result.Param2);
        }

        [Test]
        public void GetItem_UsingItemPathLanguage3Parameter_ReturnsItemName()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            string path = "/sitecore/content/Tests/SitecoreService/GetItem/EmptyItem";
            var language = LanguageManager.GetLanguage("af-ZA");
            int param1 = 1;
            string param2 = "2param";
            DateTime param3 = DateTime.Now;

            //Act
            var result = service.GetItem<StubClass, int, string, DateTime>(path, language, param1, param2, param3);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(path, result.Path);
            Assert.AreEqual(language, result.Language);
            Assert.AreEqual(param1, result.Param1);
            Assert.AreEqual(param2, result.Param2);
            Assert.AreEqual(param3, result.Param3);
        }

        [Test]
        public void GetItem_UsingItemPathLanguage4Parameter_ReturnsItemName()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            string path = "/sitecore/content/Tests/SitecoreService/GetItem/EmptyItem";
            var language = LanguageManager.GetLanguage("af-ZA");
            int param1 = 1;
            string param2 = "2param";
            DateTime param3 = DateTime.Now;
            bool param4 = true;

            //Act
            var result = service.GetItem<StubClass, int, string, DateTime, bool>(path, language, param1, param2, param3, param4);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(path, result.Path);
            Assert.AreEqual(language, result.Language);
            Assert.AreEqual(param1, result.Param1);
            Assert.AreEqual(param2, result.Param2);
            Assert.AreEqual(param3, result.Param3);
            Assert.AreEqual(param4, result.Param4);
        }

        [Test]
        public void GetItem_UsingItemIdLanguageVersion_ReturnsItemName()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            Guid id = new Guid("{6DE18DBD-0AF3-404A-8018-02B8A19515C1}");
            var language = LanguageManager.GetLanguage("af-ZA");
            Sitecore.Data.Version version = new  Sitecore.Data.Version(1);
            //Act
            var result = service.GetItem<StubClass>(id, language, version);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(language, result.Language);
            Assert.AreEqual(version.Number, result.Version);
        }

        [Test]
        public void GetItem_UsingItemIdLanguage1ParameterVersion_ReturnsItemName()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            Guid id = new Guid("{6DE18DBD-0AF3-404A-8018-02B8A19515C1}");
            Sitecore.Data.Version version = new Sitecore.Data.Version(1);
            var language = LanguageManager.GetLanguage("af-ZA");
            int param1 = 1;

            //Act
            var result = service.GetItem<StubClass, int>(id, language, version, param1);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(language, result.Language);
            Assert.AreEqual(param1, result.Param1);
            Assert.AreEqual(version.Number, result.Version);
        }

        [Test]
        public void GetItem_UsingItemIdLanguage2ParameterVersion_ReturnsItemName()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            Guid id = new Guid("{6DE18DBD-0AF3-404A-8018-02B8A19515C1}");
            var language = LanguageManager.GetLanguage("af-ZA");
            Sitecore.Data.Version version = new Sitecore.Data.Version(1);
            int param1 = 1;
            string param2 = "2param";

            //Act
            var result = service.GetItem<StubClass, int, string>(id, language, version, param1, param2);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(language, result.Language);
            Assert.AreEqual(param1, result.Param1);
            Assert.AreEqual(param2, result.Param2);
            Assert.AreEqual(version.Number, result.Version);
        }

        [Test]
        public void GetItem_UsingItemIdLanguage3ParameterVersion_ReturnsItemName()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            Guid id = new Guid("{6DE18DBD-0AF3-404A-8018-02B8A19515C1}");
            var language = LanguageManager.GetLanguage("af-ZA");
            Sitecore.Data.Version version = new Sitecore.Data.Version(1);
            int param1 = 1;
            string param2 = "2param";
            DateTime param3 = DateTime.Now;

            //Act
            var result = service.GetItem<StubClass, int, string, DateTime>(id, language, version, param1, param2, param3);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(language, result.Language);
            Assert.AreEqual(param1, result.Param1);
            Assert.AreEqual(param2, result.Param2);
            Assert.AreEqual(param3, result.Param3);
            Assert.AreEqual(version.Number, result.Version);
        }

        [Test]
        public void GetItem_UsingItemIdLanguage4ParameterVersion_ReturnsItemName()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            Guid id = new Guid("{6DE18DBD-0AF3-404A-8018-02B8A19515C1}");
            var language = LanguageManager.GetLanguage("af-ZA");
            Sitecore.Data.Version version = new Sitecore.Data.Version(1);
            int param1 = 1;
            string param2 = "2param";
            DateTime param3 = DateTime.Now;
            bool param4 = true;

            //Act
            var result = service.GetItem<StubClass, int, string, DateTime, bool>(id, language, version, param1, param2, param3, param4);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(language, result.Language);
            Assert.AreEqual(param1, result.Param1);
            Assert.AreEqual(param2, result.Param2);
            Assert.AreEqual(param3, result.Param3);
            Assert.AreEqual(param4, result.Param4);
            Assert.AreEqual(version.Number, result.Version);
        }

        [Test]
        public void GetItem_UsingItemPathLanguageVersion_ReturnsItemName()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            string path = "/sitecore/content/Tests/SitecoreService/GetItem/EmptyItem";
            var language = LanguageManager.GetLanguage("af-ZA");
            Sitecore.Data.Version version = new Sitecore.Data.Version(1);

            //Act
            var result = service.GetItem<StubClass>(path, language, version);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(path, result.Path);
            Assert.AreEqual(language, result.Language);
            Assert.AreEqual(version.Number, result.Version);
        }

        [Test]
        public void GetItem_UsingItemPathLanguage1ParameterVersion_ReturnsItemName()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            string path = "/sitecore/content/Tests/SitecoreService/GetItem/EmptyItem";
            var language = LanguageManager.GetLanguage("af-ZA");
            Sitecore.Data.Version version = new Sitecore.Data.Version(1);
            int param1 = 1;

            //Act
            var result = service.GetItem<StubClass, int>(path, language, version, param1);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(path, result.Path);
            Assert.AreEqual(language, result.Language);
            Assert.AreEqual(param1, result.Param1);
            Assert.AreEqual(version.Number, result.Version);
        }

        [Test]
        public void GetItem_UsingItemPathLanguage2ParameterVersion_ReturnsItemName()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            string path = "/sitecore/content/Tests/SitecoreService/GetItem/EmptyItem";
            var language = LanguageManager.GetLanguage("af-ZA");
            Sitecore.Data.Version version = new Sitecore.Data.Version(1);
            int param1 = 1;
            string param2 = "2param";

            //Act
            var result = service.GetItem<StubClass, int, string>(path, language, version, param1, param2);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(path, result.Path);
            Assert.AreEqual(language, result.Language);
            Assert.AreEqual(param1, result.Param1);
            Assert.AreEqual(param2, result.Param2);
            Assert.AreEqual(version.Number, result.Version);
        }

        [Test]
        public void GetItem_UsingItemPathLanguage3ParameterVersion_ReturnsItemName()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            string path = "/sitecore/content/Tests/SitecoreService/GetItem/EmptyItem";
            var language = LanguageManager.GetLanguage("af-ZA");
            Sitecore.Data.Version version = new Sitecore.Data.Version(1);
            int param1 = 1;
            string param2 = "2param";
            DateTime param3 = DateTime.Now;

            //Act
            var result = service.GetItem<StubClass, int, string, DateTime>(path, language, version, param1, param2, param3);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(path, result.Path);
            Assert.AreEqual(language, result.Language);
            Assert.AreEqual(param1, result.Param1);
            Assert.AreEqual(param2, result.Param2);
            Assert.AreEqual(param3, result.Param3);
            Assert.AreEqual(version.Number, result.Version);
        }

        [Test]
        public void GetItem_UsingItemPathLanguage4ParameterVersion_ReturnsItemName()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            string path = "/sitecore/content/Tests/SitecoreService/GetItem/EmptyItem";
            var language = LanguageManager.GetLanguage("af-ZA");
            Sitecore.Data.Version version = new Sitecore.Data.Version(1);
            int param1 = 1;
            string param2 = "2param";
            DateTime param3 = DateTime.Now;
            bool param4 = true;

            //Act
            var result = service.GetItem<StubClass, int, string, DateTime, bool>(path, language, version, param1, param2, param3, param4);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(path, result.Path);
            Assert.AreEqual(language, result.Language);
            Assert.AreEqual(param1, result.Param1);
            Assert.AreEqual(param2, result.Param2);
            Assert.AreEqual(param3, result.Param3);
            Assert.AreEqual(param4, result.Param4);
            Assert.AreEqual(version.Number, result.Version);
        }



        #endregion

        #region Method - Query

        [Test]
        public void Query_ReturnsItemsBeneathFolder_ThreeItemsReturned()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            string  query = "/sitecore/content/Tests/SitecoreService/Query/*";

            //Act
            var result = service.Query<StubClass>(query);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void Query_ReturnsItemsBeneathFolderBasedOnLanguage_TwoItemsReturned()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            string query = "/sitecore/content/Tests/SitecoreService/Query/*";
            var language = LanguageManager.GetLanguage("af-ZA");

            //Act
            var result = service.Query<StubClass>(query, language);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }
        #endregion

        #region Method - Save

        [Test]
        public void Save_ItemDisplayNameChanged_SavesDisplayName()
        {
            //Assign
            var itemPath = "/sitecore/content/Tests/SitecoreService/Save/EmptyItem";
            string expected = "new name";
            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var currentItem = db.GetItem(itemPath);
            var service = new SitecoreService(db);
            var cls = new StubSaving();
            cls.Id = currentItem.ID.Guid;

            //setup item
            using (new SecurityDisabler())
            {
                currentItem.Editing.BeginEdit();
                currentItem[Global.Fields.DisplayName] = "old name";
                currentItem.Editing.EndEdit();
            }


            using (new SecurityDisabler())
            {
                //Act
                cls.Name = expected;
                service.Save(cls);

                //Assert
                var newItem = db.GetItem(itemPath);

                Assert.AreEqual(expected, newItem[Global.Fields.DisplayName]);
            }

        }

        [Test]
        public void Save_ItemDisplayNameChangedUsingProxy_SavesDisplayName()
        {
            //Assign
            var itemPath = "/sitecore/content/Tests/SitecoreService/Save/EmptyItem";
            string expected = "new name";

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var currentItem = db.GetItem(itemPath);
            var service = new SitecoreService(db, context);


            //setup item
            using (new SecurityDisabler())
            {
                currentItem.Editing.BeginEdit();
                currentItem[Global.Fields.DisplayName] = "old name";
                currentItem.Editing.EndEdit();
            }

            var cls = service.GetItem<StubSaving>(itemPath, true);



            using (new SecurityDisabler())
            {
                //Act
                cls.Name = expected;
                service.Save(cls);

                //Assert
                var newItem = db.GetItem(itemPath);

                Assert.AreEqual(expected, newItem[Global.Fields.DisplayName]);

                Assert.IsTrue(cls is StubSaving);
                Assert.AreNotEqual(typeof(StubSaving), cls.GetType());
            }

        }

        [Test]
        public void Save_ItemDisplayNameChangedUsingProxyUsingInterface_SavesDisplayName()
        {
            //Assign
            var itemPath = "/sitecore/content/Tests/SitecoreService/Save/EmptyItem";
            string expected = "new name";

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var currentItem = db.GetItem(itemPath);
            var service = new SitecoreService(db, context);


            //setup item
            using (new SecurityDisabler())
            {
                currentItem.Editing.BeginEdit();
                currentItem[Global.Fields.DisplayName] = "old name";
                currentItem.Editing.EndEdit();
            }

            var cls = service.GetItem<IStubSaving>(itemPath);



            using (new SecurityDisabler())
            {
                //Act
                cls.Name = expected;
                service.Save(cls);

                //Assert
                var newItem = db.GetItem(itemPath);

                Assert.AreEqual(expected, newItem[Global.Fields.DisplayName]);

                Assert.IsTrue(cls is IStubSaving);
                Assert.AreNotEqual(typeof(IStubSaving), cls.GetType());
            }

        }

     

        #endregion

        #region Method - CreateTypes

        [Test]
        public void CreateTypes_TwoItems_ReturnsTwoClasses()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db, context);

            var result1 = db.GetItem("/sitecore/content/Tests/SitecoreService/CreateTypes/Result1");
            var result2 = db.GetItem("/sitecore/content/Tests/SitecoreService/CreateTypes/Result2");

            //Act
            var results =
                service.CreateTypes( typeof (StubClass), () => new[] {result1, result2}, false, false) as
                IEnumerable<StubClass>;

            //Assert
            Assert.AreEqual(2, results.Count());
            Assert.IsTrue(results.Any(x => x.Id == result1.ID.Guid));
            Assert.IsTrue(results.Any(x => x.Id == result2.ID.Guid));
        }

        [Test]
        public void CreateTypes_TwoItemsOnly1WithLanguage_ReturnsOneClasses()
        {
            //Assign
            var language = LanguageManager.GetLanguage("af-ZA");

            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db, context);

            var result1 = db.GetItem("/sitecore/content/Tests/SitecoreService/CreateTypes/Result1", language);
            var result2 = db.GetItem("/sitecore/content/Tests/SitecoreService/CreateTypes/Result2", language);

            //Act
            var results =
                service.CreateTypes(typeof(StubClass), () => new[] { result1, result2 }, false, false) as
                IEnumerable<StubClass>;

            //Assert
            Assert.AreEqual(1, results.Count());
            Assert.IsTrue(results.Any(x => x.Id == result1.ID.Guid));
        }

        #endregion

        #region Method - CreateType

        [Test]
        public void CreateType_NoConstructorArgs_ReturnsItem()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            var item = db.GetItem("/sitecore/content/Tests/SitecoreService/CreateType/Target");

            //Act
            var result = (StubClass) service.CreateType(typeof (StubClass), item, false, false);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(item.ID.Guid, result.Id);
        }

        [Test]
        public void CreateType_NoConstructorArgsTyped_ReturnsItem()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            var item = db.GetItem("/sitecore/content/Tests/SitecoreService/CreateType/Target");

            //Act
            var result = service.CreateType<StubClass>(item, false, false);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(item.ID.Guid, result.Id);
        }

        [Test]
        public void CreateType_OneConstructorArgs_ReturnsItem()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            var item = db.GetItem("/sitecore/content/Tests/SitecoreService/CreateType/Target");

            var param1 = 456;

            //Act
            var result = (StubClass)service.CreateType(typeof(StubClass), item, false, false, param1);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(item.ID.Guid, result.Id);
            Assert.AreEqual(param1, result.Param1);
        }

        [Test]
        public void CreateType_OneConstructorArgsTyped_ReturnsItem()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            var item = db.GetItem("/sitecore/content/Tests/SitecoreService/CreateType/Target");

            var param1 = 456;

            //Act
            var result =service.CreateType<StubClass, int>(item, param1);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(item.ID.Guid, result.Id);
            Assert.AreEqual(param1, result.Param1);
        }

        [Test]
        public void CreateType_TwoConstructorArgs_ReturnsItem()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            var item = db.GetItem("/sitecore/content/Tests/SitecoreService/CreateType/Target");

            var param1 = 456;
            var param2 = "hello world";

            //Act
            var result = (StubClass)service.CreateType(typeof(StubClass), item, false, false, param1, param2);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(item.ID.Guid, result.Id);
            Assert.AreEqual(param1, result.Param1);
            Assert.AreEqual(param2, result.Param2);
        }

        [Test]
        public void CreateType_TwoConstructorArgsTyped_ReturnsItem()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            var item = db.GetItem("/sitecore/content/Tests/SitecoreService/CreateType/Target");

            var param1 = 456;
            var param2 = "hello world";

            //Act
            var result =service.CreateType<StubClass, int, string>(item, param1, param2);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(item.ID.Guid, result.Id);
            Assert.AreEqual(param1, result.Param1);
            Assert.AreEqual(param2, result.Param2);
        }

        [Test]
        public void CreateType_ThreeConstructorArgs_ReturnsItem()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            var item = db.GetItem("/sitecore/content/Tests/SitecoreService/CreateType/Target");

            var param1 = 456;
            var param2 = "hello world";
            DateTime param3 = DateTime.Now;


            //Act
            var result = (StubClass)service.CreateType(typeof(StubClass), item, false, false, param1, param2, param3);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(item.ID.Guid, result.Id);
            Assert.AreEqual(param1, result.Param1);
            Assert.AreEqual(param2, result.Param2);
            Assert.AreEqual(param3, result.Param3);
        }

        [Test]
        public void CreateType_ThreeConstructorArgsTyped_ReturnsItem()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            var item = db.GetItem("/sitecore/content/Tests/SitecoreService/CreateType/Target");

            var param1 = 456;
            var param2 = "hello world";
            DateTime param3 = DateTime.Now;


            //Act
            var result = service.CreateType<StubClass, int, string, DateTime>(item, param1, param2, param3, false, false);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(item.ID.Guid, result.Id);
            Assert.AreEqual(param1, result.Param1);
            Assert.AreEqual(param2, result.Param2);
            Assert.AreEqual(param3, result.Param3);
        }

        [Test]
        public void CreateType_FourConstructorArgs_ReturnsItem()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            var item = db.GetItem("/sitecore/content/Tests/SitecoreService/CreateType/Target");

            var param1 = 456;
            var param2 = "hello world";
            DateTime param3 = DateTime.Now;
            var param4 = true;

            //Act
            var result = (StubClass)service.CreateType(typeof(StubClass), item, false, false, param1, param2, param3, param4);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(item.ID.Guid, result.Id);
            Assert.AreEqual(param1, result.Param1);
            Assert.AreEqual(param2, result.Param2);
            Assert.AreEqual(param3, result.Param3);
            Assert.AreEqual(param4, result.Param4);
        }

        [Test]
        public void CreateType_FourConstructorArgsTyped_ReturnsItem()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            var item = db.GetItem("/sitecore/content/Tests/SitecoreService/CreateType/Target");

            var param1 = 456;
            var param2 = "hello world";
            DateTime param3 = DateTime.Now;
            var param4 = true;

            //Act
            var result = service.CreateType<StubClass, int, string, DateTime, bool>(item, param1, param2, param3, param4, false, false);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(item.ID.Guid, result.Id);
            Assert.AreEqual(param1, result.Param1);
            Assert.AreEqual(param2, result.Param2);
            Assert.AreEqual(param3, result.Param3);
            Assert.AreEqual(param4, result.Param4);
        }

        #endregion

          #region Method - Create

        [Test]
        public void Create_CreatesANewItem()
        {
            //Assign
            string parentPath = "/sitecore/content/Tests/SitecoreService/Create";
            string childPath = "/sitecore/content/Tests/SitecoreService/Create/newChild";

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreService(db);

            using (new SecurityDisabler())
            {
                var parentItem = db.GetItem(parentPath);
                parentItem.DeleteChildren();
            }

            var parent = service.GetItem<StubClass>(parentPath);
            
                        var child = new StubClass();
            child.Name = "newChild";

            //Act
            using (new SecurityDisabler())
            {
                service.Create(parent, child);
            }

            //Assert
            var newItem = db.GetItem(childPath);

            using (new SecurityDisabler())
            {
                newItem.Delete();
            }

            Assert.AreEqual(child.Name,newItem.Name);
            Assert.AreEqual(child.Id, newItem.ID.Guid);
        }

        [Test]
        public void Create_AutoMappedClass_CreatesANewItem()
        {
            //Assign
            string parentPath = "/sitecore/content/Tests/SitecoreService/Create";
            string childPath = "/sitecore/content/Tests/SitecoreService/Create/newChildAutoMapped";

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreService(db);

            using (new SecurityDisabler())
            {
                var parentItem = db.GetItem(parentPath);
                parentItem.DeleteChildren();
            }

            var parent = service.GetItem<StubClassAutoMapped>(parentPath);

            var child = new StubClassAutoMapped();
            child.Name = "newChildAutoMapped";

            //Act
            using (new SecurityDisabler())
            {
                service.Create(parent, child);
            }

            //Assert
            var newItem = db.GetItem(childPath);

            using (new SecurityDisabler())
            {
                newItem.Delete();
            }

            Assert.AreEqual(child.Name, newItem.Name);
            Assert.AreEqual(child.Id, newItem.ID.Guid);
        }
        #endregion

        #region Method - Delete

        [Test]
        public void Delete_RemovesItemFromDatabase()
        {
            //Assign
            string parentPath = "/sitecore/content/Tests/SitecoreService/Delete";
            string childPath = "/sitecore/content/Tests/SitecoreService/Delete/Target";

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreService(db);

            var parent = db.GetItem(parentPath);

            //clean up any outstanding items
            Item child;
            using (new SecurityDisabler())
            {
                parent.DeleteChildren();


                child = parent.Add("Target", new TemplateID(new ID(StubClass.TemplateId)));
            }
            Assert.IsNotNull(child);

            var childClass = service.GetItem<StubClass>(childPath);
            Assert.IsNotNull(childClass);

            //Act
            using (new SecurityDisabler())
            {
              service.Delete(childClass);
            }

            //Assert
            var newItem = db.GetItem(childPath);

            Assert.IsNull(newItem);
           
        }

        #endregion

        #region Method - Move

        [Test]
        public void Move_MovesItemFromParent1ToParent2()
        {
            //Assign
            string parent1Path = "/sitecore/content/Tests/SitecoreService/Move/Parent1";
            string parent2Path = "/sitecore/content/Tests/SitecoreService/Move/Parent2";
            string targetPath = "/sitecore/content/Tests/SitecoreService/Move/Parent1/Target";
            string targetNewPath = "/sitecore/content/Tests/SitecoreService/Move/Parent2/Target";

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreService(db);

            var parent1 = db.GetItem(parent1Path);
            var parent2 = db.GetItem(parent1Path);
            var target = db.GetItem(targetPath);

            Assert.AreEqual(parent1.ID, target.Parent.ID);

            var parent2Class = service.GetItem<StubClass>(parent2Path);
            var targetClass = service.GetItem<StubClass>(targetPath);

            //Act
            using (new SecurityDisabler())
            {
                service.Move(targetClass, parent2Class);
            }

            //Assert
            var targetNew = db.GetItem(targetNewPath);

            Assert.IsNotNull(targetNew);
            using (new SecurityDisabler())
            {
                targetNew.MoveTo(parent1);
            }


        }

        #endregion

        #region Stubs

        [SitecoreType]
        public class StubSaving
        {
            [SitecoreId]
            public virtual Guid Id { get; set; }

            [SitecoreInfo(Type = SitecoreInfoType.DisplayName)]
            public virtual string Name { get; set; }
        }

        [SitecoreType]
        public interface IStubSaving
        {
            [SitecoreId]
            Guid Id { get; set; }

            [SitecoreInfo(Type = SitecoreInfoType.DisplayName)]
            string Name { get; set; }
        }

        [SitecoreType(TemplateId = StubClass.TemplateId)]
        public class StubClass
        {

            public const string TemplateId = "{ABE81623-6250-46F3-914C-6926697B9A86}";

            public DateTime Param3 { get; set; }
            public bool Param4 { get; set; }
            public string Param2 { get; set; }
            public int Param1 { get; set; }

            public StubClass()
            {
            }
            public StubClass(int param1)
            {
                Param1 = param1;
            }

            public StubClass(int param1, string param2)
                : this(param1)
            {
                Param2 = param2;
            }

            public StubClass(int param1, string param2, DateTime param3)
                : this(param1, param2)
            {
                Param3 = param3;
            }
            public StubClass(int param1, string param2, DateTime param3, bool param4)
                : this(param1, param2, param3)
            {
                Param4 = param4;
            }

            [SitecoreId]
            public virtual Guid Id { get; set; }

            [SitecoreInfo(SitecoreInfoType.Language)]
            public virtual Language Language { get; set; }

            [SitecoreInfo(SitecoreInfoType.FullPath)]
            public virtual string Path { get; set; }

            [SitecoreInfo(SitecoreInfoType.Version)]
            public virtual int Version { get; set; }

            [SitecoreInfo(SitecoreInfoType.Name)]
            public virtual string Name { get; set; }
        }


        [SitecoreType]
        public class StubClassWithProperty
        {
            [SitecoreInfo(SitecoreInfoType.Name)]
            public virtual string Name { get; set; }
        }


        public class OnDemandMapping
        {
            public virtual string StringField { get; set; }
            public virtual DateTime DateField { get; set; }
        }

        [SitecoreType(TemplateId = StubClassAutoMapped.TemplateId, AutoMap = true)]
        public class StubClassAutoMapped
        {
            public const string TemplateId = "{ABE81623-6250-46F3-914C-6926697B9A86}";


            public virtual Guid Id { get; set; }

            public virtual Language Language { get; set; }

            public virtual string Path { get; set; }

            public virtual int Version { get; set; }

            public virtual string Name { get; set; }
        }

        #endregion

        #region OnDemand Mapping

        [Test]
        public void OnDemandMapping_AutomaticallyMapsProperties()
        {
            //Assign
            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            string text = "test text 1";
            string path = "/sitecore/content/Tests/SitecoreService/OnDemand/Target";
            DateTime date = new DateTime(2013,04,03,12,15,10);

            var item = db.GetItem(path);
            using (new ItemEditing(item, true))
            {
                item["StringField"] = text;
                item["DateField"] = date.ToString("yyyyMMddThhmmss");
            }

            var context = Context.Create(Utilities.CreateStandardResolver());
            var service = new SitecoreService(db, context); 

            //Act
            var result = service.GetItem<OnDemandMapping>(path);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(text, result.StringField);
            Assert.AreEqual(date, result.DateField);


        }

        #endregion
    }
}




