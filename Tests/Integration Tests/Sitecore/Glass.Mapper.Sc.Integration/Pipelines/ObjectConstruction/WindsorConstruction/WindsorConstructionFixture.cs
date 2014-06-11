using System.Data.Common;
using Castle.MicroKernel.Registration;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.CastleWindsor;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Integration.Pipelines.ObjectConstruction.WindsorConstruction
{
    [TestFixture]
    public class WindsorConstructionFixture
    {
        [Test]
        public void Execute_RequestInstanceOfClassWithServiceButLazy_ReturnsInstanceWithService()
        {
            //Assign
            var path = "/sitecore/content/Tests/Pipelines/ObjectConstruction/WindsorConstruction/Target";
            var resolver = Utilities.CreateStandardResolver(true) as DependencyResolver;
            var context = Context.Create(resolver);
            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db, context);
            
            var item = db.GetItem(path);
            
            string field1Expected = "hello world";
            string field2 = item.ID.ToString();

            using (new ItemEditing(item, true))
            {
                item["Field1"] = field1Expected;
                item["Field2"] = field2;
            }
            

            resolver.Container.Register(
                Component.For<StubServiceInterface>().ImplementedBy<StubService>().LifestyleTransient()
                );
            
            //Act
            var result = service.GetItem<StubClassWithService>(path, true);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is StubClassWithService);
            Assert.AreNotEqual(typeof(StubClassWithService), result.GetType());

            var stub = result as StubClassWithService;


            Assert.AreEqual(field1Expected, stub.Field1);
            Assert.IsNotNull(stub.Service);
            Assert.IsTrue(stub.Service is StubService);


            var field2Class = stub.Field2;
            Assert.AreEqual(field1Expected, field2Class.Field1);
            Assert.IsNotNull(field2Class.Service);
            Assert.IsTrue(field2Class.Service is StubService);


        }

        [Test]
        public void Execute_RequestInstanceOfClassWithServiceNotLazy_ReturnsInstanceWithService()
        {
            //Assign
            var path = "/sitecore/content/Tests/Pipelines/ObjectConstruction/WindsorConstruction/Target";
            var resolver = Utilities.CreateStandardResolver(true) as DependencyResolver;
            var context = Context.Create(resolver);
            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db, context);

            var item = db.GetItem(path);

            string field1Expected = "hello world";
            string field2 = item.ID.ToString();

            using (new ItemEditing(item, true))
            {
                item["Field1"] = field1Expected;
                item["Field2"] = field2;
            }


            resolver.Container.Register(
                Component.For<StubServiceInterface>().ImplementedBy<StubService>().LifestyleTransient()
                );

            //Act
            var result = service.GetItem<StubClassWithService>(path);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is StubClassWithService);
            Assert.AreEqual(typeof(StubClassWithService), result.GetType());

            var stub = result as StubClassWithService;


            Assert.AreEqual(field1Expected, stub.Field1);
            Assert.IsNotNull(stub.Service);
            Assert.IsTrue(stub.Service is StubService);


            var field2Class = stub.Field2;
            Assert.AreEqual(field1Expected, field2Class.Field1);
            Assert.IsNotNull(field2Class.Service);
            Assert.IsTrue(field2Class.Service is StubService);
        }

        #region Stubs
        public interface StubServiceInterface
        {

        }
        public class StubService : StubServiceInterface
        {
            public string DoWork()
            {
                return "Work Done";
            }

        }
        public class StubClassWithService
        {
            public StubServiceInterface Service { get; set; }

            public StubClassWithService(StubServiceInterface service)
            {
                Service = service;
            }

            public virtual string Field1 { get; set; }
            public virtual StubClassWithService Field2 { get; set; }
        }
        #endregion
    }
}
