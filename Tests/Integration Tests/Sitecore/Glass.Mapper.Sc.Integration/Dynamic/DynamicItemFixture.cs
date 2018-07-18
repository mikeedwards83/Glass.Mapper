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
using Glass.Mapper.Sc.Dynamic;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Links;

namespace Glass.Mapper.Sc.Integration.Dynamic
{
    [TestFixture]
    public class DynamicItemFixture
    {
        private const string TargetPath = "/sitecore/content/Tests/Dynamic/DynamicItem/Target";

        Database _db;

        [SetUp]
        public void Setup()
        {
            _db = global::Sitecore.Configuration.Factory.GetDatabase("master");

            global::Sitecore.Context.Site = global::Sitecore.Sites.SiteContext.GetSite("website");
        }
        #region INTERFACE TEST

        [Test]
        public void InterfaceTest()
        {
            //Assign
            Item item = _db.GetItem(TargetPath);

            using (new ItemEditing(item, true))
            {
                item["DateTime"] = "20120204T150015";
                item["SingleLineText"] = "some awesome dynamic content";
            }

            dynamic d = new DynamicItem(item);
            IDynamicItem i = d as IDynamicItem;


            

            //Act
            string result = d.DateTime;
            string path = i.Path;

            //Assert
            Assert.AreEqual("04/02/2012 15:00:15", result);
            Assert.AreEqual(item.Paths.Path, path);
        }

      

        #endregion


        #region PROPERTY ContentPath

        [Test]
        public void ContentPath_ReturnsContentPath()
        {
            //Arrange
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var result = d.ContentPath;

            //Assert
            Assert.AreEqual(item.Paths.ContentPath, result);
        }

        #endregion

        #region PROPERTY DisplayName

        [Test]
        public void DisplayName_ReturnsDisplayName()
        {
            //Arrange
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var result = d.DisplayName;

            //Assert
            Assert.AreEqual(item["DisplayName"], result);
        }

        #endregion

        #region PROPERTY FullPath

        [Test]
        public void FullPath_ReturnsFullPath()
        {
            //Arrange
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var result = d.FullPath;

            //Assert
            Assert.AreEqual(item.Paths.FullPath, result);
        }

        #endregion

        #region PROPERTY Key

        [Test]
        public void Key_ReturnsKey()
        {
            //Arrange
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var result = d.Key;

            //Assert
            Assert.AreEqual(item.Key, result);
        }

        #endregion

        #region PROPERTY MediaUrl

        [Test]
        public void MediaUrl_ReturnsMediaUrl()
        {
            //Arrange
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var result = d.MediaUrl;

            //Assert
            Assert.AreEqual(Sitecore.Resources.Media.MediaManager.GetMediaUrl(new Sitecore.Data.Items.MediaItem(item)), result);
        }

        #endregion
        #region PROPERTY Path

        [Test]
        public void Path_ReturnsPath()
        {
            //Arrange
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var result = d.Path;

            //Assert
            Assert.AreEqual(item.Paths.Path, result);
        }

        #endregion

        #region PROPERTY TemplateId

        [Test]
        public void TemplateId_ReturnsTemplateId()
        {
            //Arrange
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var result = d.TemplateId;

            //Assert
            Assert.AreEqual(item.TemplateID.Guid, result);
        }

        #endregion
        #region PROPERTY TemplateName

        [Test]
        public void TemplateName_ReturnsTemplateName()
        {
            //Arrange
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var result = d.TemplateName;

            //Assert
            Assert.AreEqual(item.TemplateName, result);
        }

        #endregion
        #region PROPERTY Url

        [Test]
        public void Url_ReturnsUrl()
        {
            //Arrange
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var result = d.Url;

            //Assert
            Assert.AreEqual(LinkManager.GetItemUrl(item), result);
        }

        #endregion
        #region PROPERTY Version

        [Test]
        public void Version_ReturnsVersion()
        {
            //Arrange
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var result = d.Version;

            //Assert
            Assert.AreEqual(item.Version.Number, result);
        }

        #endregion
        #region PROPERTY Name

        [Test]
        public void Name_ReturnsName()
        {
            //Arrange
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var result = d.Name;

            //Assert
            Assert.AreEqual(item.Name, result);
        }


        #endregion

        #region PROPERTY Language

        [Test]
        public void Language_ReturnsLanguage()
        {
            //Arrange
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var result = d.Language;

            //Assert
            Assert.AreEqual(item.Language, result);
        }


        #endregion

        #region PROPERTY Language

        [Test]
        public void Language_ReturnsContentPath()
        {
            //Arrange
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var result = d.Language;

            //Assert
            Assert.AreEqual(item.Language, result);
        }


        #endregion

        #region PROPERTY BaseTemplateIds

        [Test]
        public void BaseTemplateIds_ReturnsBaseTemplateIds()
        {
            //Arrange
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var result = d.BaseTemplateIds as IEnumerable<Guid>;

            //Assert
            Assert.Greater(result.Count(), 10);
        }


        #endregion


        #region

        [Test]
        public void DynamicFields_ReturnsFields()
        {
            //Assign
            Item item = _db.GetItem(TargetPath);

            using (new ItemEditing(item, true))
            {
                item["DateTime"] = "20120204T150015";
                item["SingleLineText"] = "some awesome dynamic content";
            }
            dynamic d = new DynamicItem(item);

            //Act
            string dateTime = d.DateTime;

            string text = d.SingleLineText;


            //Assert
            Assert.AreEqual("some awesome dynamic content", text);
            Assert.AreEqual("04/02/2012 15:00:15", dateTime);
        }

        [Test]
        public void DynamicInfo_ReturnsItemInfo()
        {
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            string path = d.Path;
            string name = d.Name;

            //Assert
            Assert.AreEqual(TargetPath, path);
            Assert.AreEqual("Target", name);


        }

        [Test]
        public void DynamicParent_ReturnsParent()
        {
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act
            var parent = d.Parent;
            string path = parent.Path;

            //Assert
            Assert.AreEqual(item.Parent.Paths.FullPath, path);

        }

        [Test]
        public void DynamicParent_ReturnsChildren()
        {
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act

            var children = d.Children;

            //Assert
            Assert.AreEqual(3, children.Count());

            foreach (var child in d.Children)
            {
                string path = child.Path;
                Assert.IsTrue(path.StartsWith(TargetPath));
            }
        }

        [Test]
        public void DynamicParent_ReturnsLastChild()
        {
            //Assign
            Item item = _db.GetItem(TargetPath);

            dynamic d = new DynamicItem(item);

            //Act

            var children = d.Children;

            //Assert
            Assert.AreEqual(3, children.Count());

            var child = d.Children.Last();

                string path = child.Path;
                Assert.IsTrue(path.StartsWith(TargetPath));
        }
        #endregion
    }

    public interface IDynamicTitle : IDynamicItem
    {
        string Title { get; set; }
    }


}

