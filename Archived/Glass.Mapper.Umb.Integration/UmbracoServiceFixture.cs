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
using System.Linq;
using Glass.Mapper.Umb.CastleWindsor;
using Glass.Mapper.Umb.Configuration;
using Glass.Mapper.Umb.Configuration.Attributes;
using NUnit.Framework;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;

namespace Glass.Mapper.Umb.Integration
{
    [TestFixture]
    public class UmbracoServiceFixture
    {
        private ContentService _contentService;
        
        #region Method - GetItem

        [Test]
        public void GetItem_UsingItemId_ReturnsItem()
        {
            //Assign
            var context = Context.Create(DependencyResolver.CreateStandardResolver());
            context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration"));

            var service = new UmbracoService(_contentService, context);
            var id = new Guid("{24814F74-CE52-4975-B9F3-15ABCBB132D6}");

            //Act
            var result = (StubClass)service.GetItem<StubClass>(id);

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetItem_UsingItemIdInt_ReturnsItem()
        {
            //Assign
            var context = Context.Create(DependencyResolver.CreateStandardResolver());
            context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration"));

            var content = _contentService.GetById(new Guid("{24814F74-CE52-4975-B9F3-15ABCBB132D6}"));
            var service = new UmbracoService(_contentService, context);

            //Act
            var result = (StubClass)service.GetItem<StubClass>(content.Id);

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetItem_UsingItemId_ReturnsItemName()
        {
            //Assign
            var context = Context.Create(DependencyResolver.CreateStandardResolver());
            context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration"));

            var service = new UmbracoService(_contentService, context);
            var id = new Guid("{24814F74-CE52-4975-B9F3-15ABCBB132D6}");

            //Act
            var result = service.GetItem<StubClassWithProperty>(id);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("EmptyItem", result.Name);
        }
        
        #endregion

        //#region Method - Save

        [Test]
        public void Save_ItemDisplayNameChanged_SavesName()
        {
            //Assign
            string expected = "new name";
            var context = Context.Create(DependencyResolver.CreateStandardResolver());
            context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration"));
            var currentItem = _contentService.GetById(new Guid("{24814F74-CE52-4975-B9F3-15ABCBB132D6}"));
            var service = new UmbracoService(_contentService, context);
            var cls = new StubSaving();
            cls.Id = currentItem.Key;

            //setup item
            currentItem.Name = "old name";
            _contentService.Save(currentItem);

            //Act
            cls.Name = expected;
            service.Save(cls);

            //Assert
            var newItem = _contentService.GetById(new Guid("{24814F74-CE52-4975-B9F3-15ABCBB132D6}"));

            Assert.AreEqual(expected, newItem.Name);
        }

        //[Test]
        //public void Save_ItemDisplayNameChangedUsingProxy_SavesDisplayName()
        //{
        //    //Assign
        //    var itemPath = "/Umbraco/content/Tests/UmbracoService/Save/EmptyItem";
        //    string expected = "new name";

        //    var db = Umbraco.Configuration.Factory.GetDatabase("master");
        //    var context = Context.Create(DependencyResolver.CreateStandardResolver());
        //    context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration"));
        //    var currentItem = db.GetItem(itemPath);
        //    var service = new UmbracoService(db, context);


        //    //setup item
        //    using (new SecurityDisabler())
        //    {
        //        currentItem.Editing.BeginEdit();
        //        currentItem[Global.Fields.DisplayName] = "old name";
        //        currentItem.Editing.EndEdit();
        //    }

        //    var cls = service.GetItem<StubSaving>(itemPath, true);



        //    using (new SecurityDisabler())
        //    {
        //        //Act
        //        cls.Name = expected;
        //        service.Save(cls);

        //        //Assert
        //        var newItem = db.GetItem(itemPath);

        //        Assert.AreEqual(expected, newItem[Global.Fields.DisplayName]);

        //        Assert.IsTrue(cls is StubSaving);
        //        Assert.AreNotEqual(typeof(StubSaving), cls.GetType());
        //    }

        //}

        //[Test]
        //public void Save_ItemDisplayNameChangedUsingProxyUsingInterface_SavesDisplayName()
        //{
        //    //Assign
        //    var itemPath = "/Umbraco/content/Tests/UmbracoService/Save/EmptyItem";
        //    string expected = "new name";

        //    var db = Umbraco.Configuration.Factory.GetDatabase("master");
        //    var context = Context.Create(DependencyResolver.CreateStandardResolver());
        //    context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration"));
        //    var currentItem = db.GetItem(itemPath);
        //    var service = new UmbracoService(db, context);


        //    //setup item
        //    using (new SecurityDisabler())
        //    {
        //        currentItem.Editing.BeginEdit();
        //        currentItem[Global.Fields.DisplayName] = "old name";
        //        currentItem.Editing.EndEdit();
        //    }

        //    var cls = service.GetItem<IStubSaving>(itemPath);



        //    using (new SecurityDisabler())
        //    {
        //        //Act
        //        cls.Name = expected;
        //        service.Save(cls);

        //        //Assert
        //        var newItem = db.GetItem(itemPath);

        //        Assert.AreEqual(expected, newItem[Global.Fields.DisplayName]);

        //        Assert.IsTrue(cls is IStubSaving);
        //        Assert.AreNotEqual(typeof(IStubSaving), cls.GetType());
        //    }

        //}



        //#endregion

        //#region Method - CreateTypes

        //[Test]
        //public void CreateTypes_TwoItems_ReturnsTwoClasses()
        //{
        //    //Assign
        //    var context = Context.Create(DependencyResolver.CreateStandardResolver());
        //    context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration"));

        //    var db = Umbraco.Configuration.Factory.GetDatabase("master");
        //    var service = new UmbracoService(db, context);

        //    var result1 = db.GetItem("/Umbraco/content/Tests/UmbracoService/CreateTypes/Result1");
        //    var result2 = db.GetItem("/Umbraco/content/Tests/UmbracoService/CreateTypes/Result2");

        //    //Act
        //    var results =
        //        service.CreateTypes(false, false, typeof(StubClass), () => new[] { result1, result2 }) as
        //        IEnumerable<StubClass>;

        //    //Assert
        //    Assert.AreEqual(2, results.Count());
        //    Assert.IsTrue(results.Any(x => x.Id == result1.ID.Guid));
        //    Assert.IsTrue(results.Any(x => x.Id == result2.ID.Guid));
        //}

        //[Test]
        //public void CreateTypes_TwoItemsOnly1WithLanguage_ReturnsOneClasses()
        //{
        //    //Assign
        //    var language = LanguageManager.GetLanguage("af-ZA");

        //    var context = Context.Create(DependencyResolver.CreateStandardResolver());
        //    context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration"));

        //    var db = Umbraco.Configuration.Factory.GetDatabase("master");
        //    var service = new UmbracoService(db, context);

        //    var result1 = db.GetItem("/Umbraco/content/Tests/UmbracoService/CreateTypes/Result1", language);
        //    var result2 = db.GetItem("/Umbraco/content/Tests/UmbracoService/CreateTypes/Result2", language);

        //    //Act
        //    var results =
        //        service.CreateTypes(false, false, typeof(StubClass), () => new[] { result1, result2 }) as
        //        IEnumerable<StubClass>;

        //    //Assert
        //    Assert.AreEqual(1, results.Count());
        //    Assert.IsTrue(results.Any(x => x.Id == result1.ID.Guid));
        //}

        //#endregion

        //#region Method - CreateType

        //[Test]
        //public void CreateType_NoConstructorArgs_ReturnsItem()
        //{
        //    //Assign
        //    var context = Context.Create(DependencyResolver.CreateStandardResolver());
        //    context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration"));

        //    var db = Umbraco.Configuration.Factory.GetDatabase("master");
        //    var service = new UmbracoService(db);
        //    var item = db.GetItem("/Umbraco/content/Tests/UmbracoService/CreateType/Target");

        //    //Act
        //    var result = (StubClass)service.CreateType(typeof(StubClass), item, false, false);

        //    //Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(item.ID.Guid, result.Id);
        //}

        //[Test]
        //public void CreateType_NoConstructorArgsTyped_ReturnsItem()
        //{
        //    //Assign
        //    var context = Context.Create(DependencyResolver.CreateStandardResolver());
        //    context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration"));

        //    var db = Umbraco.Configuration.Factory.GetDatabase("master");
        //    var service = new UmbracoService(db);
        //    var item = db.GetItem("/Umbraco/content/Tests/UmbracoService/CreateType/Target");

        //    //Act
        //    var result = service.CreateType<StubClass>(item, false, false);

        //    //Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(item.ID.Guid, result.Id);
        //}

        //[Test]
        //public void CreateType_OneConstructorArgs_ReturnsItem()
        //{
        //    //Assign
        //    var context = Context.Create(DependencyResolver.CreateStandardResolver());
        //    context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration"));

        //    var db = Umbraco.Configuration.Factory.GetDatabase("master");
        //    var service = new UmbracoService(db);
        //    var item = db.GetItem("/Umbraco/content/Tests/UmbracoService/CreateType/Target");

        //    var param1 = 456;

        //    //Act
        //    var result = (StubClass)service.CreateType(typeof(StubClass), item, false, false, param1);

        //    //Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(item.ID.Guid, result.Id);
        //    Assert.AreEqual(param1, result.Param1);
        //}

        //[Test]
        //public void CreateType_OneConstructorArgsTyped_ReturnsItem()
        //{
        //    //Assign
        //    var context = Context.Create(DependencyResolver.CreateStandardResolver());
        //    context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration"));

        //    var db = Umbraco.Configuration.Factory.GetDatabase("master");
        //    var service = new UmbracoService(db);
        //    var item = db.GetItem("/Umbraco/content/Tests/UmbracoService/CreateType/Target");

        //    var param1 = 456;

        //    //Act
        //    var result = service.CreateType<StubClass, int>(item, param1);

        //    //Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(item.ID.Guid, result.Id);
        //    Assert.AreEqual(param1, result.Param1);
        //}

        //[Test]
        //public void CreateType_TwoConstructorArgs_ReturnsItem()
        //{
        //    //Assign
        //    var context = Context.Create(DependencyResolver.CreateStandardResolver());
        //    context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration"));

        //    var db = Umbraco.Configuration.Factory.GetDatabase("master");
        //    var service = new UmbracoService(db);
        //    var item = db.GetItem("/Umbraco/content/Tests/UmbracoService/CreateType/Target");

        //    var param1 = 456;
        //    var param2 = "hello world";

        //    //Act
        //    var result = (StubClass)service.CreateType(typeof(StubClass), item, false, false, param1, param2);

        //    //Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(item.ID.Guid, result.Id);
        //    Assert.AreEqual(param1, result.Param1);
        //    Assert.AreEqual(param2, result.Param2);
        //}

        //[Test]
        //public void CreateType_TwoConstructorArgsTyped_ReturnsItem()
        //{
        //    //Assign
        //    var context = Context.Create(DependencyResolver.CreateStandardResolver());
        //    context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration"));

        //    var db = Umbraco.Configuration.Factory.GetDatabase("master");
        //    var service = new UmbracoService(db);
        //    var item = db.GetItem("/Umbraco/content/Tests/UmbracoService/CreateType/Target");

        //    var param1 = 456;
        //    var param2 = "hello world";

        //    //Act
        //    var result = service.CreateType<StubClass, int, string>(item, param1, param2);

        //    //Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(item.ID.Guid, result.Id);
        //    Assert.AreEqual(param1, result.Param1);
        //    Assert.AreEqual(param2, result.Param2);
        //}

        //[Test]
        //public void CreateType_ThreeConstructorArgs_ReturnsItem()
        //{
        //    //Assign
        //    var context = Context.Create(DependencyResolver.CreateStandardResolver());
        //    context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration"));

        //    var db = Umbraco.Configuration.Factory.GetDatabase("master");
        //    var service = new UmbracoService(db);
        //    var item = db.GetItem("/Umbraco/content/Tests/UmbracoService/CreateType/Target");

        //    var param1 = 456;
        //    var param2 = "hello world";
        //    DateTime param3 = DateTime.Now;


        //    //Act
        //    var result = (StubClass)service.CreateType(typeof(StubClass), item, false, false, param1, param2, param3);

        //    //Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(item.ID.Guid, result.Id);
        //    Assert.AreEqual(param1, result.Param1);
        //    Assert.AreEqual(param2, result.Param2);
        //    Assert.AreEqual(param3, result.Param3);
        //}

        //[Test]
        //public void CreateType_ThreeConstructorArgsTyped_ReturnsItem()
        //{
        //    //Assign
        //    var context = Context.Create(DependencyResolver.CreateStandardResolver());
        //    context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration"));

        //    var db = Umbraco.Configuration.Factory.GetDatabase("master");
        //    var service = new UmbracoService(db);
        //    var item = db.GetItem("/Umbraco/content/Tests/UmbracoService/CreateType/Target");

        //    var param1 = 456;
        //    var param2 = "hello world";
        //    DateTime param3 = DateTime.Now;


        //    //Act
        //    var result = service.CreateType<StubClass, int, string, DateTime>(item, param1, param2, param3, false, false);

        //    //Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(item.ID.Guid, result.Id);
        //    Assert.AreEqual(param1, result.Param1);
        //    Assert.AreEqual(param2, result.Param2);
        //    Assert.AreEqual(param3, result.Param3);
        //}

        //[Test]
        //public void CreateType_FourConstructorArgs_ReturnsItem()
        //{
        //    //Assign
        //    var context = Context.Create(DependencyResolver.CreateStandardResolver());
        //    context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration"));

        //    var db = Umbraco.Configuration.Factory.GetDatabase("master");
        //    var service = new UmbracoService(db);
        //    var item = db.GetItem("/Umbraco/content/Tests/UmbracoService/CreateType/Target");

        //    var param1 = 456;
        //    var param2 = "hello world";
        //    DateTime param3 = DateTime.Now;
        //    var param4 = true;

        //    //Act
        //    var result = (StubClass)service.CreateType(typeof(StubClass), item, false, false, param1, param2, param3, param4);

        //    //Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(item.ID.Guid, result.Id);
        //    Assert.AreEqual(param1, result.Param1);
        //    Assert.AreEqual(param2, result.Param2);
        //    Assert.AreEqual(param3, result.Param3);
        //    Assert.AreEqual(param4, result.Param4);
        //}

        //[Test]
        //public void CreateType_FourConstructorArgsTyped_ReturnsItem()
        //{
        //    //Assign
        //    var context = Context.Create(DependencyResolver.CreateStandardResolver());
        //    context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration"));

        //    var db = Umbraco.Configuration.Factory.GetDatabase("master");
        //    var service = new UmbracoService(db);
        //    var item = db.GetItem("/Umbraco/content/Tests/UmbracoService/CreateType/Target");

        //    var param1 = 456;
        //    var param2 = "hello world";
        //    DateTime param3 = DateTime.Now;
        //    var param4 = true;

        //    //Act
        //    var result = service.CreateType<StubClass, int, string, DateTime, bool>(item, param1, param2, param3, param4, false, false);

        //    //Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(item.ID.Guid, result.Id);
        //    Assert.AreEqual(param1, result.Param1);
        //    Assert.AreEqual(param2, result.Param2);
        //    Assert.AreEqual(param3, result.Param3);
        //    Assert.AreEqual(param4, result.Param4);
        //}

        //#endregion

        //#region Method - Create

        //[Test]
        //public void Create_CreatesANewItem()
        //{
        //    //Assign
        //    string parentPath = "/Umbraco/content/Tests/UmbracoService/Create";
        //    string childPath = "/Umbraco/content/Tests/UmbracoService/Create/newChild";

        //    var db = Umbraco.Configuration.Factory.GetDatabase("master");
        //    var context = Context.Create(DependencyResolver.CreateStandardResolver());
        //    context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration"));
        //    var service = new UmbracoService(db);

        //    var parent = service.GetItem<StubClass>(parentPath);
        //    var child = new StubClass();
        //    child.Name = "newChild";

        //    //Act
        //    using (new SecurityDisabler())
        //    {
        //        service.Create(parent, child);
        //    }

        //    //Assert
        //    var newItem = db.GetItem(childPath);

        //    newItem.Delete();

        //    Assert.AreEqual(child.Name, newItem.Name);
        //    Assert.AreEqual(child.Id, newItem.ID.Guid);
        //}

        //#endregion

        //#region Method - Delete

        //[Test]
        //public void Delete_RemovesItemFromDatabse()
        //{
        //    //Assign
        //    string parentPath = "/Umbraco/content/Tests/UmbracoService/Delete";
        //    string childPath = "/Umbraco/content/Tests/UmbracoService/Delete/Target";

        //    var db = Umbraco.Configuration.Factory.GetDatabase("master");
        //    var context = Context.Create(DependencyResolver.CreateStandardResolver());
        //    context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration"));
        //    var service = new UmbracoService(db);

        //    var parent = db.GetItem(parentPath);
        //    var child = parent.Add("Target", new TemplateID(new ID(StubClass.TemplateId)));

        //    Assert.IsNotNull(child);

        //    var childClass = service.GetItem<StubClass>(childPath);
        //    Assert.IsNotNull(childClass);

        //    //Act
        //    using (new SecurityDisabler())
        //    {
        //        service.Delete(childClass);
        //    }

        //    //Assert
        //    var newItem = db.GetItem(childPath);

        //    Assert.IsNull(newItem);

        //}

        //#endregion

        //#region Method - Move

        //[Test]
        //public void Move_MovesItemFromParent1ToParent2()
        //{
        //    //Assign
        //    string parent1Path = "/Umbraco/content/Tests/UmbracoService/Move/Parent1";
        //    string parent2Path = "/Umbraco/content/Tests/UmbracoService/Move/Parent2";
        //    string targetPath = "/Umbraco/content/Tests/UmbracoService/Move/Parent1/Target";
        //    string targetNewPath = "/Umbraco/content/Tests/UmbracoService/Move/Parent2/Target";

        //    var db = Umbraco.Configuration.Factory.GetDatabase("master");
        //    var context = Context.Create(DependencyResolver.CreateStandardResolver());
        //    context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration"));
        //    var service = new UmbracoService(db);

        //    var parent1 = db.GetItem(parent1Path);
        //    var parent2 = db.GetItem(parent1Path);
        //    var target = db.GetItem(targetPath);

        //    Assert.AreEqual(parent1.ID, target.Parent.ID);

        //    var parent2Class = service.GetItem<StubClass>(parent2Path);
        //    var targetClass = service.GetItem<StubClass>(targetPath);

        //    //Act
        //    using (new SecurityDisabler())
        //    {
        //        service.Move(targetClass, parent2Class);
        //    }

        //    //Assert
        //    var targetNew = db.GetItem(targetNewPath);

        //    Assert.IsNotNull(targetNew);
        //    using (new SecurityDisabler())
        //    {
        //        targetNew.MoveTo(parent1);
        //    }


        //}

        //#endregion

        #region Stubs

        [TestFixtureSetUp]
        public void CreateStub()
        {
            string name = "EmptyItem";
            string contentTypeAlias = "TestType";
            string contentTypeName = "Test Type";

            var unitOfWork = Global.CreateUnitOfWork();
            var repoFactory = new RepositoryFactory();
            _contentService = new ContentService(unitOfWork, repoFactory);
            var contentTypeService = new ContentTypeService(unitOfWork, repoFactory,
                                                            new ContentService(unitOfWork),
                                                            new MediaService(unitOfWork, repoFactory));
            var dataTypeService = new DataTypeService(unitOfWork, repoFactory);

            var contentType = new ContentType(-1);
            contentType.Name = contentTypeName;
            contentType.Alias = contentTypeAlias;
            contentType.Thumbnail = string.Empty;
            contentTypeService.Save(contentType);
            Assert.Greater(contentType.Id, 0);

            var definitions = dataTypeService.GetDataTypeDefinitionByControlId(new Guid("ec15c1e5-9d90-422a-aa52-4f7622c63bea"));
            dataTypeService.Save(definitions.FirstOrDefault());
            var propertyType = new PropertyType(definitions.FirstOrDefault());
            propertyType.Alias = "Property";
            contentType.AddPropertyType(propertyType);
            contentTypeService.Save(contentType);
            Assert.Greater(contentType.Id, 0);

            var content = new Content(name, -1, contentType);
            content.Key = new Guid("{24814F74-CE52-4975-B9F3-15ABCBB132D6}");
            _contentService.Save(content);
        }

        [UmbracoType]
        public class StubSaving
        {
            [UmbracoId]
            public virtual Guid Id { get; set; }

            [UmbracoInfo(Type = UmbracoInfoType.Name)]
            public virtual string Name { get; set; }
        }

        [UmbracoType]
        public interface IStubSaving
        {
            [UmbracoId]
            Guid Id { get; set; }

            [UmbracoInfo(Type = UmbracoInfoType.Name)]
            string Name { get; set; }
        }

        [UmbracoType(ContentTypeAlias = ContentTypeAlias)]
        public class StubClass
        {
            public const string ContentTypeAlias = "TestType";

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

            [UmbracoId]
            public virtual Guid Id { get; set; }

            [UmbracoInfo(UmbracoInfoType.Path)]
            public virtual string Path { get; set; }

            [UmbracoInfo(UmbracoInfoType.Version)]
            public virtual Guid Version { get; set; }

            [UmbracoInfo(UmbracoInfoType.Name)]
            public virtual string Name { get; set; }
        }

        [UmbracoType]
        public class StubClassWithProperty
        {
            [UmbracoInfo(UmbracoInfoType.Name)]
            public virtual string Name { get; set; }
        }

        #endregion
    }
}

