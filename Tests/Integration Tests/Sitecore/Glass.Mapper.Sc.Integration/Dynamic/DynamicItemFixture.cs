using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Dynamic;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.Data.Items;

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


}
