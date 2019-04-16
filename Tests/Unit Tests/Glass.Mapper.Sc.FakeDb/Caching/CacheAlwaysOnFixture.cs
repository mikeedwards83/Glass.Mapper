using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;
using Sitecore.FakeDb;
using Sitecore.Globalization;
using Sitecore.SecurityModel;
using Sitecore.Sites;
using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static Glass.Mapper.Sc.FakeDb.Caching.CacheFixtures;

namespace Glass.Mapper.Sc.FakeDb.Caching
{
    public class CacheAlwaysOnFixture
    {
        [Test]
        public void Cache_AlwaysOnTrue_ReturnsTheSameInstance()
        {
            //Arrange
            string path = "/sitecore/content/target";

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var config = new Config();
                config.Cache.AlwaysOn = true;

                var context = Context.Create(Utilities.CreateStandardResolver(config));

                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreService(database.Database);
                var lang1 = Language.Parse("en");


                //Act
                var item1 = service.GetItem<StubClass>(path, x => x.Language(lang1));
                var item2 = service.GetItem<StubClass>(path, x => x.Language(lang1));

                //Assert
                Assert.AreEqual(item1, item2);
            }

        }

        [Test]
        public void Cache_AlwaysOnTrue_RecursiveModelLazyLoadingTest_ReturnsTheSameInstance()
        {
            //Arrange
            string path = "/sitecore/content/target";

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
                {
                    new Sitecore.FakeDb.DbItem("Child")
                    {

                    }
                }
            })
            {
                    var config = new Config();
                    config.Cache.AlwaysOn = true;

                    var context = Context.Create(Utilities.CreateStandardResolver(config));
                    context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClassRecursive)));
                context.Config.Cache.AlwaysOn = true;

                var service = new SitecoreService(database.Database);
                var lang1 = Language.Parse("en");


                //Act
                var item1 = service.GetItem<StubClassRecursive>(path, x => x.Language(lang1));
                var item2 = service.GetItem<StubClassRecursive>(path, x => x.Language(lang1));

                var child = item1.Children.First();
                var childParent = item1.Children.First().Parent;

                //Assert
                Assert.AreEqual(item1, item2);
                Assert.AreEqual(item1, childParent);
            }

        }

        [Test]
        public void Cache_AlwaysOnTrue_CacheClear_ReturnsDiffInstance()
        {
            //Arrange
            string path = "/sitecore/content/target";

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                    var config = new Config();
                    config.Cache.AlwaysOn = true;

                    var context = Context.Create(Utilities.CreateStandardResolver(config));
                    context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreService(database.Database);
                var lang1 = Language.Parse("en");


                //Act
                var item1 = service.GetItem<StubClass>(path, x => x.Language(lang1));
                context.DependencyResolver.CacheFactory.ClearAll();
                var item2 = service.GetItem<StubClass>(path, x => x.Language(lang1));

                //Assert
                Assert.AreNotEqual(item1, item2);

            }

        }

        [Test]
        public void Cache_AlwaysOnTrue_TwoDiffSites_ReturnsDiffInstance()
        {
            //Arrange
            string path = "/sitecore/content/target";

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                    var config = new Config();
                    config.Cache.AlwaysOn = true;

                    var context = Context.Create(Utilities.CreateStandardResolver(config));
                    context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreService(database.Database);
                var lang1 = Language.Parse("en");


                //Act
                var doc1 = new XmlDocument();
                doc1.LoadXml(
                    "<site name='Site1' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content' startItem='/Target' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");
                var siteContext1 = new SiteContext(
                    new SiteInfo(
                        doc1.FirstChild
                    )
                );

                var doc2 = new XmlDocument();
                doc2.LoadXml(
                    "<site name='Site2' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content' startItem='/Target' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");
                var siteContext2 = new SiteContext(
                    new SiteInfo(
                        doc2.FirstChild
                    )
                );

                Sitecore.Context.Site = siteContext1;

                var item1 = service.GetItem<StubClass>(path, x => x.Language(lang1));
                Sitecore.Context.Site = siteContext2;
                var item2 = service.GetItem<StubClass>(path, x => x.Language(lang1));

                Sitecore.Context.Site = siteContext1;

                var item3 = service.GetItem<StubClass>(path, x => x.Language(lang1));

                //Assert
                Assert.AreNotEqual(item1, item2);
                Assert.AreEqual(item1, item3);

            }

        }
        [Test]
        public void Cache_AlwaysOnFalse_ReturnsDiffInstance()
        {
            //Arrange
            string path = "/sitecore/content/target";

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var config = new Config();
                config.Cache.AlwaysOn = false;

                var context = Context.Create(Utilities.CreateStandardResolver(config));
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreService(database.Database);
                var lang1 = Language.Parse("en");


                //Act
                var item1 = service.GetItem<StubClass>(path, x => x.Language(lang1));
                var item2 = service.GetItem<StubClass>(path, x => x.Language(lang1));

                //Assert
                Assert.AreNotEqual(item1, item2);

            }

        }

        [Test]
        public void Cache_AlwaysOnFalse_CacheOnModel_ReturnsSameInstance()
        {
            //Arrange
            string path = "/sitecore/content/target";

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var config = new Config();
                config.Cache.AlwaysOn = false;

                var context = Context.Create(Utilities.CreateStandardResolver(config));
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreService(database.Database);
                var lang1 = Language.Parse("en");


                //Act
                var item1 = service.GetItem<StubClass>(path, x => x.Language(lang1).CacheEnabled());
                var item2 = service.GetItem<StubClass>(path, x => x.Language(lang1).CacheEnabled());
                var item3 = service.GetItem<StubClass>(path, x => x.Language(lang1));

                //Assert
                Assert.AreEqual(item1, item2);
                Assert.AreNotEqual(item3, item2);

            }

        }
        [Test]
        public void Cache_AlwaysOnTrueModelDisabled_ReturnsTheSameInstance()
        {
            //Arrange
            string path = "/sitecore/content/target";

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var config = new Config();
                config.Cache.AlwaysOn = true;

                var context = Context.Create(Utilities.CreateStandardResolver(config));
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreService(database.Database);
                var lang1 = Language.Parse("en");


                //Act
                var item1 = service.GetItem<StubClass>(path, x => x.Language(lang1));
                var item2 = service.GetItem<StubClass>(path, x => x.Language(lang1));
                var item3 = service.GetItem<StubClass>(path, x => x.Language(lang1).CacheDisabled());

                //Assert
                Assert.AreEqual(item1, item2);
                Assert.AreNotEqual(item1, item3);

            }

        }


        [Test]
        public void Cache_Children_AlwaysOnTrue_ReturnsTheSameInstance()
        {
            //Arrange
            string path = "/sitecore/content/target";
            string pathChild = "/sitecore/content/target/Child";

            using (new SecurityDisabler())
            {
                using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
                {
                    new Sitecore.FakeDb.DbItem("Child")
                    {

                    }
                }
            })
                {
                    var config = new Config();
                    config.Cache.AlwaysOn = true;

                    var context = Context.Create(Utilities.CreateStandardResolver(config));
                    context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                    var service = new SitecoreService(database.Database);
                    var lang1 = Language.Parse("en");


                    //Act
                    var item1 = service.GetItem<StubClass>(path, x => x.Language(lang1));
                    var item2 = service.GetItem<StubClass>(path, x => x.Language(lang1));
                    var child = service.GetItem<StubClass>(pathChild, x => x.Language(lang1));

                    //Assert
                    Assert.AreEqual(item1, item2);
                    Assert.AreEqual(item1.Children.First(), item2.Children.First());
                    Assert.AreEqual(item1.Children.First(), child);

                }
            }

        }

        [Test]
        public void Cache_Children_AlwaysOnTrueModelDisabled_ReturnsTheSameInstance()
        {
            //Arrange
            string path = "/sitecore/content/target";
            string pathChild = "/sitecore/content/target/Child";

            using (new SecurityDisabler())
            {
                using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
                {
                    new Sitecore.FakeDb.DbItem("Child")
                    {

                    }
                }
            })
                {
                    var config = new Config();
                    config.Cache.AlwaysOn = true;

                    var context = Context.Create(Utilities.CreateStandardResolver(config));
                    context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                    var service = new SitecoreService(database.Database);
                    var lang1 = Language.Parse("en");


                    //Act
                    var item1 = service.GetItem<StubClass>(path, x => x.Language(lang1));
                    var item2 = service.GetItem<StubClass>(path, x => x.Language(lang1).CacheDisabled());
                    var child1 = service.GetItem<StubClass>(pathChild, x => x.Language(lang1));
                    var child2 = service.GetItem<StubClass>(pathChild, x => x.Language(lang1).CacheDisabled());

                    //Assert
                    Assert.AreNotEqual(item1, item2);
                    Assert.AreNotEqual(item1.Children.First(), item2.Children.First());
                    Assert.AreEqual(item1.Children.First(), child1);
                    Assert.AreNotEqual(item1.Children.First(), child2);

                }
            }

        }

        [Test]
        public void Cache_Children_AlwaysOnFalseModelEnabled_ReturnsTheSameInstance()
        {
            //Arrange
            string path = "/sitecore/content/target";
            string pathChild = "/sitecore/content/target/Child";

            using (new SecurityDisabler())
            {
                using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
                {
                    new Sitecore.FakeDb.DbItem("Child")
                    {

                    }
                }
            })
                {
                    var config = new Config();
                    config.Cache.AlwaysOn = false;

                    var context = Context.Create(Utilities.CreateStandardResolver(config));
                    context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                    var service = new SitecoreService(database.Database);
                    var lang1 = Language.Parse("en");


                    //Act
                    var item1 = service.GetItem<StubClass>(path, x => x.Language(lang1));
                    var item2 = service.GetItem<StubClass>(path, x => x.Language(lang1).CacheEnabled());
                    var child1 = service.GetItem<StubClass>(pathChild, x => x.Language(lang1));
                    var child2 = service.GetItem<StubClass>(pathChild, x => x.Language(lang1).CacheEnabled());

                    //Assert
                    Assert.AreNotEqual(item1, item2);
                    Assert.AreNotEqual(item1.Children.First(), item2.Children.First());
                    Assert.AreNotEqual(item1.Children.First(), child1);
                    Assert.AreNotEqual(item1.Children.First(), child2);
                    Assert.AreNotEqual(item2.Children.First(), child1);
                    Assert.AreEqual(item2.Children.First(), child2);

                }
            }

        }


        [SitecoreType]
        public class StubClass
        {
            [SitecoreChildren]
            public virtual IEnumerable<StubClass> Children { get; set; }
        }

        [SitecoreType]
        public class StubClassRecursive
        {
            [SitecoreChildren]
            public virtual IEnumerable<StubClassRecursive> Children { get; set; }

            [SitecoreParent]
            public virtual StubClassRecursive Parent { get; set; }
        }
    }
}
