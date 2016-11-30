using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.Diagnostics;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Fluent;
using NUnit.Framework;
using Sitecore.FakeDb;

namespace Glass.Mapper.Sc.FakeDb.Pipelines.ObjectConstruction
{
    [TestFixture]
    public class ModelCounterFixture
    {
        [Test]
        public void ModelCounter_ConcreteModelCount_Created()
        {
            //Arrange
            string path = "/sitecore/content/target";

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var config = new Config();
                config.Debug.Enabled = true;

                var context = Context.Create(Utilities.CreateStandardResolver(config));
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreService(database.Database);
                var modelCounter = context.DependencyResolver.GetModelCounter();
                modelCounter.Reset();

                //Act
                var item = service.GetItem<StubClass>("/sitecore/content/target");


                
                //Assert
                Assert.AreEqual(1, modelCounter.ConcreteModelCreated);
                Assert.AreEqual(0, modelCounter.ProxyModelsCreated);
                Assert.AreEqual(0, modelCounter.CachedModels);
                Assert.AreEqual(1, modelCounter.ModelsMapped);
                Assert.AreEqual(1, modelCounter.ModelsRequested);
            }
        }

        [Test]
        public void ModelCounter_ConcreteModelCount_Cached()
        {
            //Arrange
            string path = "/sitecore/content/target";

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var config = new Config();
                config.Debug.Enabled = true;

                var context = Context.Create(Utilities.CreateStandardResolver(config));
                var fluentloader = new SitecoreFluentConfigurationLoader();
                var stubLoader = fluentloader.Add<StubClass>();
                stubLoader.Cachable();

                context.Load(fluentloader);

                var service = new SitecoreService(database.Database);
                var modelCounter = context.DependencyResolver.GetModelCounter();
                modelCounter.Reset();

                //Act
                var item1 = service.GetItem<StubClass>("/sitecore/content/target");
                var item2 = service.GetItem<StubClass>("/sitecore/content/target");



                //Assert
                Assert.AreEqual(1, modelCounter.ConcreteModelCreated);
                Assert.AreEqual(0, modelCounter.ProxyModelsCreated);
                Assert.AreEqual(1, modelCounter.CachedModels);
                Assert.AreEqual(1, modelCounter.ModelsMapped);
                Assert.AreEqual(2, modelCounter.ModelsRequested);
            }
        }

        [Test]
        public void ModelCounter_ProxyModelCount_Created()
        {
            //Arrange
            string path = "/sitecore/content/target";

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var config = new Config();
                config.Debug.Enabled = true;

                var context = Context.Create(Utilities.CreateStandardResolver(config));
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(IStubClass)));

                var service = new SitecoreService(database.Database);
                var modelCounter = context.DependencyResolver.GetModelCounter();
                modelCounter.Reset();

                //Act
                var item = service.GetItem<IStubClass>("/sitecore/content/target", isLazy:true);

                //Assert
                Assert.AreEqual(0, modelCounter.ConcreteModelCreated);
                Assert.AreEqual(1, modelCounter.ProxyModelsCreated);
                Assert.AreEqual(0, modelCounter.CachedModels);
                Assert.AreEqual(0, modelCounter.ModelsMapped);
                Assert.AreEqual(1, modelCounter.ModelsRequested);
            }
        }
        [Test]
        public void ModelCounter_ProxyModelCount_Mapped()
        {
            //Arrange
            string path = "/sitecore/content/target";

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var config = new Config();
                config.Debug.Enabled = true;

                var context = Context.Create(Utilities.CreateStandardResolver(config));
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(IStubClass)));

                var service = new SitecoreService(database.Database);
                var modelCounter = context.DependencyResolver.GetModelCounter();
                modelCounter.Reset();

                //Act
                var item = service.GetItem<IStubClass>("/sitecore/content/target");

                var id = item.Id;


                //Assert
                Assert.AreEqual(0, modelCounter.ConcreteModelCreated);
                Assert.AreEqual(1, modelCounter.ProxyModelsCreated);
                Assert.AreEqual(0, modelCounter.CachedModels);
                Assert.AreEqual(1, modelCounter.ModelsMapped);
                Assert.AreEqual(1, modelCounter.ModelsRequested);
            }
        }

        [Test]
        public void ModelCounter_ProxyModelCount_Cached()
        {
            //Arrange
            string path = "/sitecore/content/target";

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var config = new Config();
                config.Debug.Enabled = true;

                var context = Context.Create(Utilities.CreateStandardResolver(config));
                var fluentloader = new SitecoreFluentConfigurationLoader();
                var stubLoader = fluentloader.Add<IStubClass>();
                stubLoader.Cachable();
                stubLoader.Id(x => x.Id);

                context.Load(fluentloader);


                var service = new SitecoreService(database.Database);
                var modelCounter = context.DependencyResolver.GetModelCounter();
                modelCounter.Reset();

                //Act
                var item1 = service.GetItem<IStubClass>("/sitecore/content/target");
                var id1 = item1.Id;
                var item2 = service.GetItem<IStubClass>("/sitecore/content/target");
                var id2 = item2.Id;


                //Assert
                Assert.AreEqual(0, modelCounter.ConcreteModelCreated);
                Assert.AreEqual(1, modelCounter.ProxyModelsCreated);
                Assert.AreEqual(1, modelCounter.CachedModels);
                Assert.AreEqual(1, modelCounter.ModelsMapped);
                Assert.AreEqual(2, modelCounter.ModelsRequested);
            }
        }

        public class StubClass
        {
        }

        public interface IStubClass
        {
            Guid Id { get; set; }
        }

        public class StubWithChildrenClass
        {
            public virtual IEnumerable<StubClass> Children { get; set; }
        }

    }
}