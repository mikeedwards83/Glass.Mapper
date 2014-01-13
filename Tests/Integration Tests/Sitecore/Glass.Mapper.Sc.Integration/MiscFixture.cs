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
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Configuration.Fluent;
using Glass.Mapper.Sc.Fields;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.layouts.testing;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sc.Integration
{
    [TestFixture]
    public class MiscFixture
    {
        [Test]
        public void InterfaceIssueInPageEditorWhenInterfaceInheritsFromAnInterfaceWithSimilarName()
        {
            /*
             * This test is in response to issue 53 raised on the Glass.Sitecore.Mapper
             * project. When two interfaces have similar names are created as proxies
             * the method GetTypeConfiguration returns the wrong config.
             */
            
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var scContext = new SitecoreContext(db);

            var glassHtml = new GlassHtml(scContext);
            var instance = scContext.GetItem<IBasePage>("/sitecore");

            //Act
            glassHtml.Editable(instance, x => x.Title);

            //This method should execute without error


        }

        [Test]
        public void ItemPropertySave_SavesItemOnProperty_SetsField()
        {
            /*
             * Tests that we can save to an item property.
             */

            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var scContext = new SitecoreContext(db);
            string path = "/sitecore/content/Tests/Misc/ItemPropertySave";

            var expected = "some expected value";
            var item = db.GetItem(path);

            using (new ItemEditing(item, true))
            {
                item["Field1"] = string.Empty;
            }

            var instance = scContext.GetItem<ItemPropertySaveStub>(path);

            //Act
            instance.Field1 = expected;

            using (new SecurityDisabler())
            {
                scContext.Save(instance);
            }

            //Assert
            Assert.AreEqual(expected, instance.Item["Field1"]);

        }

        [Test]
        public void FieldWithSpacesReturningNullIssue()
        {
            /*
             * This test is in response to issue 53 raised on the Glass.Sitecore.Mapper
             * project. When two interfaces have similar names are created as proxies
             * the method GetTypeConfiguration returns the wrong config.
             */


            //Assign
            string path = "/sitecore/content/Tests/Misc/FieldWithSpace";
            string expected = "Hello space";
            string imageValue =
                "<image mediaid=\"{C2CE5623-1E36-4535-9A01-669E1541DDAF}\" mediapath=\"/Tests/Dayonta\" src=\"~/media/C2CE56231E3645359A01669E1541DDAF.ashx\" />";

            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var db = Sitecore.Configuration.Factory.GetDatabase("master");

            var item = db.GetItem(path);

            using (new ItemEditing(item, true))
            {
                item["Field With Space"] = expected;
                item["Image Field"] = imageValue;
            }
            
            var scContext = new SitecoreContext(db);

            var glassHtml = new GlassHtml(scContext);

            //Act
            var instance = scContext.GetItem<FieldWithSpaceIssue>(path);
           

            //Assert
            Assert.AreEqual(expected, instance.FieldWithSpace);
            Assert.IsNotNull(instance.ImageSpace);

        }

        [Test]
        public void OrderOfIgnoreIssue1_ConfiguredShouldBeSet_TitleShouldBeIgnored()
        {
            //Assign
            string path = "/sitecore/content/Tests/Misc/FieldConfigOrder";
            string expected = "Hello space";


            var fluentConfig = new SitecoreFluentConfigurationLoader();

            var typeConfig = fluentConfig.Add<FieldOrderOnIgnore>();
            typeConfig.AutoMap();
            typeConfig.Ignore(x => x.Title);
            typeConfig.Field(x => x.ConfiguredTitle).FieldName("Title");

            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(fluentConfig);

            var db = Sitecore.Configuration.Factory.GetDatabase("master");

            var item = db.GetItem(path);

            using (new ItemEditing(item, true))
            {
                item["Title"] = expected;
            }
            
            var scContext = new SitecoreContext(db);


            //Act
            var instance = scContext.GetItem<FieldOrderOnIgnore>(path);
           

            //Assert
            Assert.AreEqual(expected, instance.ConfiguredTitle);
            Assert.IsNullOrEmpty(instance.Title);
        }

        [Test]
        public void OrderOfIgnoreIssue2_ConfiguredShouldBeSet_TitleShouldBeIgnored()
        {
            //Assign
            string path = "/sitecore/content/Tests/Misc/FieldConfigOrder";
            string expected = "Hello space";


            var fluentConfig = new SitecoreFluentConfigurationLoader();

            var typeConfig = fluentConfig.Add<FieldOrderOnIgnore>();
            typeConfig.AutoMap();
            typeConfig.Field(x => x.ConfiguredTitle).FieldName("Title");
            typeConfig.Ignore(x => x.Title);

            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(fluentConfig);

            var db = Sitecore.Configuration.Factory.GetDatabase("master");

            var item = db.GetItem(path);

            using (new ItemEditing(item, true))
            {
                item["Title"] = expected;
            }

            var scContext = new SitecoreContext(db);


            //Act
            var instance = scContext.GetItem<FieldOrderOnIgnore>(path);


            //Assert
            Assert.AreEqual(expected, instance.ConfiguredTitle);
            Assert.IsNullOrEmpty(instance.Title);
        }

        [Test]
        public void FieldLoopIssue()
        {
            //Arrange
            Guid itemId = new Guid("{6603A3A7-C1E2-42FE-9DC1-34367D3F6187}");

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var item = db.GetItem(new ID(itemId));

            using (new ItemEditing(item, true))
            {
                item["RelatedItems"] = itemId.ToString();
            }

            var context = Context.Create(Utilities.CreateStandardResolver());
            var scContext = new SitecoreService(db, context);

            //Act

            var fieldLoop = scContext.GetItem<FieldLoop>(itemId);

            //Assert
            Assert.AreEqual(itemId, fieldLoop.Id);
            Assert.AreEqual(itemId, fieldLoop.RelatedItems.First().Id);
            Assert.AreEqual(itemId, fieldLoop.RelatedItems.First().RelatedItems.First().Id);

        }

        [Test]
        public void GetItem_ClassHasItemChildrenCollectionAndParent_ReturnsItem()
        {
            //Assign
            var context = Context.Create(Utilities.CreateStandardResolver());
            var path = "/sitecore/content/Tests/Misc/ClassWithItemProperties";

            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);

            var item = db.GetItem(path);

            //Act
            var result = service.GetItem<ItemWithItemProperties>(path);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(item.ParentID, result.Parent.ID);
            Assert.AreEqual(item.Children.Count, result.Children.Count());
        }


#region Stubs


        public class ItemWithItemProperties
        {
            public Item Parent { get; set; }
            public IEnumerable<Item> Children { get; set; } 
        }

        [SitecoreType]
        public interface IBase
        {
            [SitecoreId]
            Guid Id { get; set; }
        }

        [SitecoreType]
        public interface IBasePage :IBase
        {
            [SitecoreField]
            string Title { get; set; }
        }

        [SitecoreType]
        public class FieldWithSpaceIssue
        {
            [SitecoreField("Field With Space")]
            public virtual string FieldWithSpace { get; set; }

            [SitecoreField("Image Space")]
            public virtual Image ImageSpace { get; set; }
        }

        public class FieldOrderOnIgnore
        {
            public virtual string Title { get; set; }
            public virtual string ConfiguredTitle { get; set; }
        }

        [SitecoreType(AutoMap = true)]
        public class FieldLoop
        {
            public virtual Guid Id { get; set; }

            public virtual IEnumerable<FieldLoop> RelatedItems { get; set; } 
        }

        public class ItemPropertySaveStub
        {
            public virtual Item Item { get; set; }
            public virtual string Field1 { get; set; }
        }

#endregion
    }
}
