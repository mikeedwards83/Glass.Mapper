using System;
using System.Collections.Generic;
using Glass.Mapper.Sc.Configuration.Fluent;
using NUnit.Framework;
using Sitecore.FakeDb;

namespace Glass.Mapper.Sc.FakeDb
{
    [TestFixture]
    public class LazyLoadingFixture
    {
        #region Basic Lazy Loading

        [Test]
        public void LazyLoading_LazyEnabledGettingParent_ReturnsProxy()
        {
            //Arrange
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Parent") {
                    new Sitecore.FakeDb.DbItem("Target")
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());

                var fluent = new SitecoreFluentConfigurationLoader();
                var parentConfig = fluent.Add<Stub2>();
                var lazy1Config = fluent.Add<Stub1>();
                lazy1Config.Parent(x => x.Stub2);
                context.Load(fluent);

                var service = new SitecoreService(database.Database, context);

                //Act
                var target = service.GetItem<Stub1>("/sitecore/content/parent/target");
                var parent = target.Stub2;

                //Assert
                Assert.AreNotEqual(typeof(Stub2), parent.GetType());
                Assert.True(parent is Stub2);
            }
        }

        [Test]
        public void LazyLoading_LazyDisabledGettingParent_ReturnsConcrete()
        {
            //Arrange
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Parent") {
                    new Sitecore.FakeDb.DbItem("Target")
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());

                var fluent = new SitecoreFluentConfigurationLoader();
                var parentConfig = fluent.Add<Stub2>();
                var lazy1Config = fluent.Add<Stub1>();
                lazy1Config.Parent(x => x.Stub2).IsNotLazy();
                context.Load(fluent);

                var service = new SitecoreService(database.Database, context);

                //Act
                var target = service.GetItem<Stub1>("/sitecore/content/parent/target");
                var parent = target.Stub2;

                //Assert
                Assert.AreEqual(typeof(Stub2), parent.GetType());
                Assert.True(parent is Stub2);
            }
        }

        [Test]
        public void LazyLoading_LazyOnPrimaryDisabledButSecondaryEnabledGettingParent_ReturnsProxy()
        {
            //Arrange
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Parent") {
                    new Sitecore.FakeDb.DbItem("Target")
                    {
                        new Sitecore.FakeDb.DbItem("Child1"),
                    }
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());

                var fluent = new SitecoreFluentConfigurationLoader();
                var stub1Config = fluent.Add<Stub1>();
                stub1Config.Children(x => x.Stub3s).IsNotLazy();
                var stub2Config = fluent.Add<Stub2>();
                var stub3Config = fluent.Add<Stub3>();
                stub3Config.Parent(x => x.Stub2);


                context.Load(fluent);

                var service = new SitecoreService(database.Database, context);

                //Act
                var target = service.GetItem<Stub1>("/sitecore/content/parent/target");

                //Assert
                foreach (var child in target.Stub3s)
                {
                    Assert.IsTrue(child is Stub3);
                    Assert.AreEqual(typeof(Stub3), child.GetType());
                    Assert.IsTrue(child.Stub2 is Stub2);
                    Assert.AreNotEqual(typeof(Stub2), child.Stub2.GetType());
                }
            }
        }

        #endregion

        #region Lazy With Caching

        [Test]
        public void LazyLoadingWithCaching_LazyEnabledGettingParent_ReturnsProxy()
        {
            //Arrange
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Parent") {
                    new Sitecore.FakeDb.DbItem("Target")
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());

                var fluent = new SitecoreFluentConfigurationLoader();
                var stub2Config = fluent.Add<Stub2>();
                var stub1Config = fluent.Add<Stub1>();
                stub1Config.Parent(x => x.Stub2);
                stub1Config.Cachable();

                context.Load(fluent);

                var service = new SitecoreService(database.Database, context);

                //Act
                var target = service.GetItem<Stub1>("/sitecore/content/parent/target");
                var parent = target.Stub2;

                //Assert
                Assert.AreEqual(typeof(Stub2), parent.GetType());
                Assert.True(parent is Stub2);
            }
        }

        [Test]
        public void LazyLoadingWithCaching_LazyDisabledGettingParent_ReturnsConcrete()
        {
            //Arrange
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Parent") {
                    new Sitecore.FakeDb.DbItem("Target")
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());

                var fluent = new SitecoreFluentConfigurationLoader();
                var stub2Config = fluent.Add<Stub2>();
                var stub1Config = fluent.Add<Stub1>();
                stub1Config.Parent(x => x.Stub2).IsNotLazy();
                stub1Config.Cachable();
                context.Load(fluent);

                var service = new SitecoreService(database.Database, context);

                //Act
                var target = service.GetItem<Stub1>("/sitecore/content/parent/target");
                var parent = target.Stub2;

                //Assert
                Assert.AreEqual(typeof(Stub2), parent.GetType());
                Assert.True(parent is Stub2);
            }
        }

        [Test]
        public void LazyLoadingWithCaching_LazyOnPrimaryDisabledButSecondaryEnabledGettingParent_ReturnsProxy()
        {
            //Arrange
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Parent") {
                    new Sitecore.FakeDb.DbItem("Target")
                    {
                        new Sitecore.FakeDb.DbItem("Child1"),
                    }
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());

                var fluent = new SitecoreFluentConfigurationLoader();
                var stub1Config = fluent.Add<Stub1>();
                stub1Config.Children(x => x.Stub3s).IsNotLazy();
                stub1Config.Cachable();
                var stub2Config = fluent.Add<Stub2>();
                var stub3Config = fluent.Add<Stub3>();
                stub3Config.Parent(x => x.Stub2);


                context.Load(fluent);

                var service = new SitecoreService(database.Database, context);

                //Act
                var target = service.GetItem<Stub1>("/sitecore/content/parent/target");

                //Assert
                foreach (var child in target.Stub3s)
                {
                    Assert.IsTrue(child is Stub3);
                    Assert.AreEqual(typeof(Stub3), child.GetType());
                    Assert.IsTrue(child.Stub2 is Stub2);
                    Assert.AreEqual(typeof(Stub2), child.Stub2.GetType());
                }
            }
        }

        [Test]
        public void LazyLoadingWithCaching_LazyLoadingLoop_ThrowsException()
        {
            //Arrange
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Parent") {
                    new Sitecore.FakeDb.DbItem("Target")
                    {
                        new Sitecore.FakeDb.DbItem("Child1"),
                    }
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());

                var fluent = new SitecoreFluentConfigurationLoader();
                var stub4Config = fluent.Add<Stub4>();
                stub4Config.Parent(x => x.Stub5).IsNotLazy();
                stub4Config.Cachable();
                var stub5Config = fluent.Add<Stub5>();
                stub5Config.Children(x => x.Stub4s);

                context.Load(fluent);

                var service = new SitecoreService(database.Database, context);

                //Act
                Exception ex = Assert.Throws<MapperStackException>(() =>
                {
                    var target = service.GetItem<Stub4>("/sitecore/content/parent/target");
                });

                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                Assert.AreEqual(Constants.Errors.ErrorLazyLoop, ex.Message);
                //Assert

            }
        }



        #endregion

        public class Stub1
        {
            public virtual Stub2 Stub2 { get; set; }
            public virtual IEnumerable<Stub3> Stub3s { get; set; }
        }
        public class Stub2
        {

        }
        public class Stub3
        {
            public virtual Stub2 Stub2 { get; set; }
        }

        public class Stub4
        {
            public virtual Stub5 Stub5
            {
                get; set;
            }
        }

        public class Stub5
        {
            public virtual IEnumerable<Stub4> Stub4s
            {
                get;
                set;
            }
        }
    }
}
