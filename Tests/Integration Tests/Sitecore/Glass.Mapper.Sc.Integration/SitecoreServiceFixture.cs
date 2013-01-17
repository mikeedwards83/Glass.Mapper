using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;
using Sitecore.Data;
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
            string path = "/sitecore/content/Tests/SitecoreService/AddVersion/Target";
            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);

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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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

        #region Method - Save

        [Test]
        public void Save_ItemDisplayNameChanged_SavesDisplayName()
        {
            //Assign
            var itemPath = "/sitecore/content/Tests/SitecoreService/Save/EmptyItem";
            string expected = "new name";
            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db, context);

            var result1 = db.GetItem("/sitecore/content/Tests/SitecoreService/CreateTypes/Result1");
            var result2 = db.GetItem("/sitecore/content/Tests/SitecoreService/CreateTypes/Result2");

            //Act
            var results =
                service.CreateTypes(false, false, typeof (StubClass), () => new[] {result1, result2}) as
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

            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db, context);

            var result1 = db.GetItem("/sitecore/content/Tests/SitecoreService/CreateTypes/Result1", language);
            var result2 = db.GetItem("/sitecore/content/Tests/SitecoreService/CreateTypes/Result2", language);

            //Act
            var results =
                service.CreateTypes(false, false, typeof(StubClass), () => new[] { result1, result2 }) as
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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
            var context = Context.Create(new GlassConfig());
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

        [SitecoreType]
        public class StubClass{
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
        }


        [SitecoreType]
        public class StubClassWithProperty
        {
            [SitecoreInfo(SitecoreInfoType.Name)]
            public virtual string Name { get; set; }
        }

       

        #endregion

       
    }
}
