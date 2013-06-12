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
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Configuration.Fluent;
using Glass.Mapper.Sc.Fields;
using NUnit.Framework;
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

#region Stubs
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

#endregion
    }
}
