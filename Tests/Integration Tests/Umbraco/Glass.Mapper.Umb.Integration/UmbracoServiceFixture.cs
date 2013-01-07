using System;
using Glass.Mapper.Umb.Configuration.Attributes;
using NUnit.Framework;
using Umbraco.Tests.TestHelpers;
using Umbraco.Web;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.web;

namespace Glass.Mapper.Umb.Integration
{
    [TestFixture]
    public class UmbracoServiceFixture : BaseWebTest
    {
        public override void Initialize()
        {
            base.Initialize();

            //set the current umbraco context and a published content store
          //  PublishedContentStoreResolver.Current = new PublishedContentStoreResolver(
          //     new DefaultPublishedContentStore());

           // var routingContext = GetRoutingContext("/test", 1234);
            UmbracoContext.Current = GetUmbracoContext("/test", 1234);
        }
        #region Method - GetItem

        [Test]
        public void GetItem_UsingItemId_ReturnsItem()
        {
            var dt = DocumentType.MakeNew(new User(0), "asd");
             //Assign
            var context = Context.Create(new GlassConfig());
            context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration") );

            var service = new UmbracoService();
            int id = 2;

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
            context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration"));

            var service = new UmbracoService();
            int id = 2;

            //Act
            var result = service.GetItem<StubClassWithProperty>(id);

            //Assert
            Assert.IsNotNull(result);
        //    Assert.AreEqual("EmptyItem", result.Name);
        }

        #endregion

        #region Method - Save

        //[Test]
        //public void Save_ItemDisplayNameChanged_SavesDisplayName()
        //{
        //    //Assign
        //    var itemPath = "/Umbraco/content/Tests/UmbracoService/Save/EmptyItem";
        //    string expected = "new name";
        //    var context = Context.Create(new GlassConfig());
        //    context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
        //    var currentItem = db.GetItem(itemPath);
        //    var service = new UmbracoService(db);
        //    var cls = new StubSaving();
        //    cls.Id = currentItem.ID.Guid;

        //    //setup item
        //    using (new SecurityDisabler())
        //    {
        //        currentItem.Editing.BeginEdit();
        //        currentItem[Global.Fields.DisplayName] = "old name";
        //        currentItem.Editing.EndEdit();
        //    }


        //    using (new SecurityDisabler())
        //    {
        //        //Act
        //        cls.Name = expected;
        //        service.Save(cls);

        //        //Assert
        //        var newItem = db.GetItem(itemPath);

        //        Assert.AreEqual(expected, newItem[Global.Fields.DisplayName]);
        //    }

        //}

        #endregion

        #region Stubs

        [UmbracoType]
        public class StubSaving
        {
            [UmbracoId]
            public virtual int Id { get; set; }

           // [UmbracoInfo(Type = UmbracoInfoType.DisplayName)]
           // public virtual string Name { get; set; }
        }

        [UmbracoType]
        public class StubClass{
        }


        [UmbracoType]
        public class StubClassWithProperty
        {
           // [UmbracoInfo(UmbracoInfoType.Name)]
         //   public virtual string Name { get; set; }
        }

        #endregion
    }
}
