using Glass.Mapper.Sc.IoC;
using NUnit.Framework;
using SimpleInjector;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Glass.Mapper.Sc.FakeDb.ThirdParty.SimpleInjector
{
    [TestFixture]
    public class SimpleInjectorConstructionFixture
    {
        [Test]
        public void Execute_RequestInstanceOfClassWithServiceButLazy_ReturnsInstanceWithService()
        {
            //Assign
            var templateId = ID.NewID;
            var targetId = ID.NewID;
            var fieldName1 = "Field1";
            var fieldName2 = "Field2";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName1, ""},
                    {fieldName2, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                SimpleInjectorTask.Container = new Container();
                SimpleInjectorTask.Container.Register<StubServiceInterface, StubService>();

                var path = "/sitecore/content/Target";

                var resolver = Integration.Utilities.CreateStandardResolver() as DependencyResolver;
                resolver.ObjectConstructionFactory.Insert(0, () => new SimpleInjectorTask());


                var context = Context.Create(resolver);
                var db = database.Database;
                var service = new SitecoreService(db, context);

                var item = db.GetItem(path);

                string field1Expected = "hello world";
                string field2 = item.ID.ToString();

                using (new ItemEditing(item, true))
                {
                    item[fieldName1] = field1Expected;
                    item[fieldName2] = field2;
                }


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

        }

        [Test]
        public void Execute_RequestInstanceOfClassWithServiceNotLazy_ReturnsInstanceWithService()
        {
            //Assign
            var templateId = ID.NewID;
            var targetId = ID.NewID;
            var fieldName1 = "Field1";
            var fieldName2 = "Field2";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName1, ""},
                    {fieldName2, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                SimpleInjectorTask.Container = new Container();
                SimpleInjectorTask.Container.Register<StubServiceInterface, StubService>();

                var path = "/sitecore/content/Target";
                var resolver = Integration.Utilities.CreateStandardResolver() as DependencyResolver;
                resolver.ObjectConstructionFactory.Insert(0, () => new SimpleInjectorTask());

                var context = Context.Create(resolver);

                var db = database.Database;
                var service = new SitecoreService(db, context);

                var item = db.GetItem(path);

                string field1Expected = "hello world";
                string field2 = item.ID.ToString();

                using (new ItemEditing(item, true))
                {
                    item["Field1"] = field1Expected;
                    item["Field2"] = field2;
                }


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
