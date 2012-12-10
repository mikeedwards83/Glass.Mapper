using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sc.Integration
{
    [TestFixture]
    public class SitecoreServiceFixture
    {
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
        public class StubClass{
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
