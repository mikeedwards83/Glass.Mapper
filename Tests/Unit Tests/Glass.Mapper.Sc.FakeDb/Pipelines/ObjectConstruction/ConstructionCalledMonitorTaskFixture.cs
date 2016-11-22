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
    //[TestFixture]
    //public class ConstructionCalledMonitorTaskFixture
    //{
    //    [Test]
    //    public void ConstructionCalled_CounterIncrementedBy()
    //    {
    //        //Arrange
    //        string path = "/sitecore/content/target";

    //        using (Db database = new Db
    //        {
    //            new Sitecore.FakeDb.DbItem("Target")
    //        })
    //        {
    //            ThreadData.SetValue(ConstructionCalledMonitorTask.CalledKey, 0);

    //            var config = new Config();
    //            config.Debug.Enabled = true;

    //            var context = Context.Create(Utilities.CreateStandardResolver(config));
    //            context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

    //            var service = new SitecoreService(database.Database);

    //            //Act
    //            var item = service.GetItem<StubClass>("/sitecore/content/target");

    //            //Assert
    //            var counter = ConstructionCalledMonitorTask.GetCounter();

    //            Assert.AreEqual(1, counter);
    //        }
    //    }

    //    [Test]
    //    public void ConstructionCalled_WithChildren_IncrementsForEachChild()
    //    {
    //        //Arrange
    //        string path = "/sitecore/content/target";

    //        using (Db database = new Db
    //        {
    //            new Sitecore.FakeDb.DbItem("Target")
    //            {
    //                new DbItem("Child1"),
    //                new DbItem("Child2")
    //            }
    //        })
    //        {
    //            ThreadData.SetValue(ConstructionCalledMonitorTask.CalledKey,0);

    //            var config = new Config();
    //            config.Debug.Enabled = true;

    //            var context = Context.Create(Utilities.CreateStandardResolver(config));
    //            context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubWithChildrenClass)));

    //            var service = new SitecoreService(database.Database);

    //            //Act
    //            var item = service.GetItem<StubWithChildrenClass>("/sitecore/content/target");
    //            foreach (var child in item.Children)
    //            {
                    
    //            }

    //            //Assert
    //            var counter = ConstructionCalledMonitorTask.GetCounter();

    //            Assert.AreEqual(3, counter);

    //        }
    //    }


    //    [Test]
    //    public void ConstructionCalled_CachedItem_Increments()
    //    {
    //        //Arrange
    //        string path = "/sitecore/content/target";

    //        using (Db database = new Db
    //        {
    //            new Sitecore.FakeDb.DbItem("Target")
    //            {
    //            }
    //        })
    //        {
    //            ThreadData.SetValue(ConstructionCalledMonitorTask.CalledKey,0);

    //            var config = new Config();
    //            config.Debug.Enabled = true;

    //            var context = Context.Create(Utilities.CreateStandardResolver(config));

    //            var loader = new SitecoreFluentConfigurationLoader();
    //            loader.Add<StubClass>().Cachable();
                
    //            context.Load(loader);

    //            var service = new SitecoreService(database.Database);

    //            //Act
    //            var item = service.GetItem<StubClass>("/sitecore/content/target");
    //            var item2 = service.GetItem<StubClass>("/sitecore/content/target");
                
    //            //Assert
    //            var counter = ConstructionCalledMonitorTask.GetCounter();

    //            Assert.AreEqual(2, counter);
    //        }


    //    }


    //    [Test]
    //    public void ConstructionCalled_CachedItemWithChildren_SingleIncrements()
    //    {
    //        //Arrange
    //        string path = "/sitecore/content/target";

    //        using (Db database = new Db
    //        {
    //            new Sitecore.FakeDb.DbItem("Target")
    //            {
    //                  new DbItem("Child1"),
    //                new DbItem("Child2")
    //            }
    //        })
    //        {
    //            ThreadData.SetValue(ConstructionCalledMonitorTask.CalledKey, 0);

    //            var config = new Config();
    //            config.Debug.Enabled = true;

    //            var context = Context.Create(Utilities.CreateStandardResolver(config));

    //            var loader = new SitecoreFluentConfigurationLoader();
    //            var stubConfig = loader.Add<StubWithChildrenClass>().Cachable();
    //            stubConfig.Children(x => x.Children);

    //            context.Load(loader);

    //            var service = new SitecoreService(database.Database);

    //            //Act
    //            var item1 = service.GetItem<StubWithChildrenClass>("/sitecore/content/target");
    //            var childCount1 = 0;
    //            foreach (var child in item1.Children)
    //            {
    //                childCount1++;
    //            }

    //            var counterFirst = ConstructionCalledMonitorTask.GetCounter();

    //            //reset counter
    //            ThreadData.SetValue(ConstructionCalledMonitorTask.CalledKey, 0);

    //            var item2 = service.GetItem<StubWithChildrenClass>("/sitecore/content/target");
    //            var childCount2 = 0;
    //            foreach (var child in item2.Children)
    //            {
    //                childCount2++;
    //            }

    //            var counterSecond = ConstructionCalledMonitorTask.GetCounter();
    //            //Assert

    //            Assert.AreEqual(3, counterFirst);
    //            Assert.AreEqual(2, childCount1);

    //            Assert.AreEqual(1, counterSecond);
    //            Assert.AreEqual(2, childCount2);

    //        }
    //    }

    //    public class StubClass
    //    {
    //    }

    //    public class StubWithChildrenClass
    //    {
    //        public virtual  IEnumerable<StubClass> Children { get; set; }
    //    }

    //}
}