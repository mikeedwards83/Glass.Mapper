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


using System.Linq;
using System.Xml;
using Glass.Mapper.Sc.CastleWindsor;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.SecurityModel;
using Sitecore.Sites;
using Sitecore.Web;

namespace Glass.Mapper.Sc.Integration
{
    [TestFixture]
    public class SitecoreContextFixture
    {
        #region GetCurrentItem

        [Test]
        public void GetCurrentItem_NonGeneric()
        {
            //Assign
            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var path = "/sitecore/content/Tests/SitecoreContext/GetCurrentItem/Target";
            var scContext = new SitecoreContext();

            Sitecore.Context.Item = db.GetItem(path);

            //Act
            var result = scContext.GetCurrentItem(typeof (StubClass)) as StubClass;

            //Assert
            Assert.AreEqual(Sitecore.Context.Item.ID, result.Id);

        }
        [Test]
        public void GetCurrentItem_NoParameters()
        {
            //Assign
            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var path = "/sitecore/content/Tests/SitecoreContext/GetCurrentItem/Target";
            var scContext = new SitecoreContext();

            Sitecore.Context.Item = db.GetItem(path);

            //Act
            var result = scContext.GetCurrentItem<StubClass>();

            //Assert
            Assert.AreEqual(Sitecore.Context.Item.ID, result.Id);

        }

        [Test]
        public void GetCurrentItem_OneParameters()
        {
            //Assign
            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            string param1 = "1para";

            var path = "/sitecore/content/Tests/SitecoreContext/GetCurrentItem/Target";
            var scContext = new SitecoreContext();

            Sitecore.Context.Item = db.GetItem(path);

            //Act
            var result = scContext.GetCurrentItem<StubClass, string>(param1);

            //Assert
            Assert.AreEqual(Sitecore.Context.Item.ID, result.Id);
        }

        [Test]
        public void GetCurrentItem_TwoParameters()
        {
            //Assign
            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            string param1 = "1para";
            string param2 = "2para";

            var path = "/sitecore/content/Tests/SitecoreContext/GetCurrentItem/Target";
            var scContext = new SitecoreContext();

            Sitecore.Context.Item = db.GetItem(path);

            //Act
            var result = scContext.GetCurrentItem<StubClass, string, string>(param1, param2);

            //Assert
            Assert.AreEqual(Sitecore.Context.Item.ID, result.Id);
        }

        [Test]
        public void GetCurrentItem_ThreeParameters()
        {
            //Assign
            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            string param1 = "1para";
            string param2 = "2para";
            string param3 = "3para";

            var path = "/sitecore/content/Tests/SitecoreContext/GetCurrentItem/Target";
            var scContext = new SitecoreContext();

            Sitecore.Context.Item = db.GetItem(path);

            //Act
            var result = scContext.GetCurrentItem<StubClass, string, string, string>(param1, param2, param3);

            //Assert
            Assert.AreEqual(Sitecore.Context.Item.ID, result.Id);
        }

        [Test]
        public void GetCurrentItem_FourParameters()
        {
            //Assign
            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            string param1 = "1para";
            string param2 = "2para";
            string param3 = "3para";
            string param4 = "4para";

            var path = "/sitecore/content/Tests/SitecoreContext/GetCurrentItem/Target";
            var scContext = new SitecoreContext();

            Sitecore.Context.Item = db.GetItem(path);

            //Act
            var result = scContext.GetCurrentItem<StubClass,string, string, string,string>(param1, param2, param3, param4);

            //Assert
            Assert.AreEqual(Sitecore.Context.Item.ID, result.Id);
        }

        #endregion


        #region GetHomeItem

        [Test]
        public void GetHomeItem_ReturnsHomeItem()
        {
            //Assign
            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var doc = new XmlDocument();
            doc.LoadXml("<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

            Sitecore.Context.Site = new SiteContext(
                new SiteInfo(
                    doc.FirstChild
                    )
                );
            //Sitecore.Context.Site = Sitecore.Configuration.Factory.GetSite("GetHomeItem");

            var scContext = new SitecoreContext();

            var target1 = db.GetItem("/sitecore/content/Tests/SitecoreContext/GetHomeItem/Target1");

            //Act
            var result = scContext.GetHomeItem<StubClass>();

            //Assert
            Assert.AreEqual(target1.ID, result.Id);

        }

        #endregion

        #region QueryRelative

        [Test]
        public void QueryRelative_RetrievesSiblings()
        {
            //Assign
            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var path = "/sitecore/content/Tests/SitecoreContext/QueryRelative/Source";
            var scContext = new SitecoreContext();

            Sitecore.Context.Item = db.GetItem(path);

            var target1 = db.GetItem("/sitecore/content/Tests/SitecoreContext/QueryRelative/Target1");
            var target2 = db.GetItem("/sitecore/content/Tests/SitecoreContext/QueryRelative/Target2");

            //Act
            var results = scContext.QueryRelative<StubClass>("../*");

            //Assert
            Assert.AreEqual(3, results.Count());
            Assert.IsTrue(results.Any(x => x.Id == target1.ID));
            Assert.IsTrue(results.Any(x => x.Id == target1.ID));
            Assert.IsTrue(results.Any(x => x.Id == Sitecore.Context.Item.ID));

        }

        [Test]
        public void QueryRelative_NoResultsReturnsEmptyResult()
        {
            //Assign
            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var path = "/sitecore/content/Tests/SitecoreContext/QueryRelative/Source";
            var scContext = new SitecoreContext();

            Sitecore.Context.Item = db.GetItem(path);

            var target1 = db.GetItem("/sitecore/content/Tests/SitecoreContext/QueryRelative/Target1");
            var target2 = db.GetItem("/sitecore/content/Tests/SitecoreContext/QueryRelative/Target2");

            using (new SecurityDisabler())
            {
                //Act
                var results = scContext.QueryRelative<StubClass>("/*[@@templatename='notthere']");

                //Assert
                Assert.AreEqual(0, results.Count());
            }
        }

        #endregion

        #region QueryRelative

        [Test]
        public void QuerySingleRelative_RetrievesSibling()
        {
            //Assign
            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var path = "/sitecore/content/Tests/SitecoreContext/QueryRelative/Source";
            var scContext = new SitecoreContext();

            Sitecore.Context.Item = db.GetItem(path);

            var target1 = db.GetItem("/sitecore/content/Tests/SitecoreContext/QueryRelative/Target1");

            //Act
            var result = scContext.QuerySingleRelative<StubClass>("../*[@@name='Target1']");

            //Assert
            Assert.AreEqual(target1.ID, result.Id);

        }

        #endregion



        #region Stub

        [SitecoreType]
        public class StubClass
        {
            public string Param4 { get; set; }
            public string Param3 { get; set; }
            public string Param2 { get; set; }
            public string Param1 { get; set; }

            public StubClass()
            {

            }
            public StubClass(string param1)
            {
                Param1 = param1;
            }
            public StubClass(string param1, string param2)
            {
                Param1 = param1;
                Param2 = param2;
            }
            public StubClass(string param1, string param2, string param3)
            {
                Param1 = param1;
                Param2 = param2;
                Param3 = param3;
            }

            public StubClass(string param1, string param2, string param3, string param4)
            {
                Param1 = param1;
                Param2 = param2;
                Param3 = param3;
                Param4 = param4;
            }

            [SitecoreId]
            public ID Id { get; set; }
        }

        #endregion
    }
}




