using System.Linq;
using System.Xml;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.FakeDb;
using Sitecore.SecurityModel;
using Sitecore.Sites;
using Sitecore.Web;

namespace Glass.Mapper.Sc.FakeDb
{
    [TestFixture]
    public class SitecoreContextFixture
    {
        #region GetCurrentItem

        [Test]
        public void GetCurrentItem_NonGeneric()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var path = "/sitecore/content/Target";
                var scContext = new SitecoreContext();

                Sitecore.Context.Item = database.GetItem(path);

                //Act
                var result = scContext.GetCurrentItem(typeof(StubClass)) as StubClass;

                //Assert
                Assert.AreEqual(Sitecore.Context.Item.ID, result.Id);
            }
        }
        [Test]
        public void GetCurrentItem_NoParameters()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var path = "/sitecore/content/Target";
                var scContext = new SitecoreContext();

                Sitecore.Context.Item = database.GetItem(path);

                //Act
                var result = scContext.GetCurrentItem<StubClass>();

                //Assert
                Assert.AreEqual(Sitecore.Context.Item.ID, result.Id);
            }
        }

        [Test]
        public void GetCurrentItem_OneParameters()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                string param1 = "1para";

                var path = "/sitecore/content/Target";
                var scContext = new SitecoreContext();

                Sitecore.Context.Item = database.GetItem(path);

                //Act
                var result = scContext.GetCurrentItem<StubClass, string>(param1);

                //Assert
                Assert.AreEqual(Sitecore.Context.Item.ID, result.Id);
                Assert.AreEqual(param1, result.Param1);
            }
        }

        [Test]
        public void GetCurrentItem_TwoParameters()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                string param1 = "1para";
                string param2 = "2para";

                var path = "/sitecore/content/Target";
                var scContext = new SitecoreContext();

                Sitecore.Context.Item = database.GetItem(path);

                //Act
                var result = scContext.GetCurrentItem<StubClass, string, string>(param1, param2);

                //Assert
                Assert.AreEqual(Sitecore.Context.Item.ID, result.Id);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(param2, result.Param2);
            }
        }

        [Test]
        public void GetCurrentItem_ThreeParameters()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                string param1 = "1para";
                string param2 = "2para";
                string param3 = "3para";

                var path = "/sitecore/content/Target";
                var scContext = new SitecoreContext();

                Sitecore.Context.Item = database.GetItem(path);

                //Act
                var result = scContext.GetCurrentItem<StubClass, string, string, string>(param1, param2, param3);

                //Assert
                Assert.AreEqual(Sitecore.Context.Item.ID, result.Id);

                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(param2, result.Param2);
                Assert.AreEqual(param3, result.Param3);
            }
        }

        [Test]
        public void GetCurrentItem_FourParameters()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                string param1 = "1para";
                string param2 = "2para";
                string param3 = "3para";
                string param4 = "4para";

                var path = "/sitecore/content/Target";
                var scContext = new SitecoreContext();

                Sitecore.Context.Item = database.GetItem(path);

                //Act
                var result = scContext.GetCurrentItem<StubClass, string, string, string, string>(param1, param2, param3,
                    param4);

                //Assert
                Assert.AreEqual(Sitecore.Context.Item.ID, result.Id);
                Assert.AreEqual(param1, result.Param1);
                Assert.AreEqual(param2, result.Param2);
                Assert.AreEqual(param3, result.Param3);
                Assert.AreEqual(param4, result.Param4);
            }
        }

        #endregion


        #region GetHomeItem

        [Test]
        public void GetHomeItem_ReturnsHomeItem()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content' startItem='/Target' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                Sitecore.Context.Site = new SiteContext(
                    new SiteInfo(
                        doc.FirstChild
                    )
                );
                //Sitecore.Context.Site = Sitecore.Configuration.Factory.GetSite("GetHomeItem");

                using (new SiteContextSwitcher(new SiteContext(new SiteInfo(
                    doc.FirstChild
                ))))
                {
                    var scContext = new SitecoreContext();

                    var target1 = database.GetItem("/sitecore/content/Target");

                    //Act
                    var result = scContext.GetHomeItem<StubClass>();

                    //Assert
                    Assert.AreEqual(target1.ID, result.Id);
                }
            }

        }

        #endregion

        #region QueryRelative

        [Test]
        public void QueryRelative_RetrievesSiblings()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("QueryRelative")
                {
                    new Sitecore.FakeDb.DbItem("Source"),
                    new Sitecore.FakeDb.DbItem("Target1"),
                    new Sitecore.FakeDb.DbItem("Target2")
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var path = "/sitecore/content/QueryRelative/Source";
                var scContext = new SitecoreContext();

                Sitecore.Context.Item = database.GetItem(path);

                var target1 = database.GetItem("/sitecore/content/QueryRelative/Target1");
                var target2 = database.GetItem("/sitecore/content/QueryRelative/Target2");

                //Act
                var results = scContext.QueryRelative<StubClass>("../*");

                //Assert
                Assert.AreEqual(3, results.Count());
                Assert.IsTrue(results.Any(x => x.Id == target1.ID));
                Assert.IsTrue(results.Any(x => x.Id == target2.ID));
                Assert.IsTrue(results.Any(x => x.Id == Sitecore.Context.Item.ID));
            }
        }

        [Test]
        public void QueryRelative_NoResultsReturnsEmptyResult()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("QueryRelative")
                {
                    new Sitecore.FakeDb.DbItem("Source"),
                    new Sitecore.FakeDb.DbItem("Target1"),
                    new Sitecore.FakeDb.DbItem("Target2")
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var path = "/sitecore/content/QueryRelative/Source";
                var scContext = new SitecoreContext();

                Sitecore.Context.Item = database.GetItem(path);

                var target1 = database.GetItem("/sitecore/content/QueryRelative/Target1");
                var target2 = database.GetItem("/sitecore/content/QueryRelative/Target2");

                using (new SecurityDisabler())
                {
                    //Act
                    var results = scContext.QueryRelative<StubClass>("/*[@@templatename='notthere']");

                    //Assert
                    Assert.AreEqual(0, results.Count());
                }
            }
        }

        #endregion

        #region QueryRelative

        [Test]
        public void QuerySingleRelative_RetrievesSibling()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("QueryRelative")
                {
                    new Sitecore.FakeDb.DbItem("Source"),
                    new Sitecore.FakeDb.DbItem("Target1"),
                    new Sitecore.FakeDb.DbItem("Target2")
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var path = "/sitecore/content/QueryRelative/Source";
                var scContext = new SitecoreContext();

                Sitecore.Context.Item = database.GetItem(path);

                var target1 = database.GetItem("/sitecore/content/QueryRelative/Target1");

                //Act
                var result = scContext.QuerySingleRelative<StubClass>("../*[@@name='Target1']");

                //Assert
                Assert.AreEqual(target1.ID, result.Id);
            }

        }

        #endregion



        #region Stub

        [SitecoreType]
        public class StubClass
        {
            public virtual string Param4 { get; set; }
            public virtual string Param3 { get; set; }
            public virtual string Param2 { get; set; }
            public virtual string Param1 { get; set; }

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
            public virtual ID Id { get; set; }
        }

        #endregion
    }
}



