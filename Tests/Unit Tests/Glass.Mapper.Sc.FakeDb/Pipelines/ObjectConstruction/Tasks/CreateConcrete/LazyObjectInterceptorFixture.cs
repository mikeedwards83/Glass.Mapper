using System;
using Glass.Mapper.Diagnostics;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Sc.Configuration;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Glass.Mapper.Sc.FakeDb.Pipelines.ObjectConstruction.Tasks.CreateConcrete
{
    [TestFixture]
    public class LazyObjectInterceptorFixture
    {
        #region Method - Intercept

        [Test]
        public void Intercept_CreatesObjectLazily_CallsInvokeMethod()
        {
            //Assign
            //Arrange
            string path = "/sitecore/content/target";
            ID itemId = ID.NewID;

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", itemId)
            })
            {
                var config = new Config();

                var context = Context.Create(Utilities.CreateStandardResolver(config));
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreService(database.Database);
                var modelCounter = ModelCounter.Instance;
                modelCounter.Reset();

                var item = service.GetItem<StubClass>("/sitecore/content/target");

                //Act
                var result = item.CalledMe();

                //Assert
                Assert.IsTrue(result);
                Assert.AreEqual(itemId.Guid, item.Id);
            }
        }

        #endregion

        #region Stubs

        public class StubClass
        {
            public virtual Guid Id { get; set; }

            public bool CalledMe()
            {
                return true;
            }

        }

        #endregion
    }
}
