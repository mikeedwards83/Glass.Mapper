using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Configuration.Fluent;
using Glass.Mapper.Sc.DataMappers;
using Glass.Mapper.Sc.FakeDb.Issues.Issue311;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;
using Sitecore.Sites;
using Sitecore.Web;

namespace Glass.Mapper.Sc.FakeDb.Issues.Issue299
{
    public class Issue299Fixture
    {

        [Test]
        public void SitecoreService_NotLazyLoadingChildren_EnumerationBeforeAndAfterServiceDisposal()
        {
            //Arrange

            var fieldName = "Field1";

            using (Db db = new Db()
            {
               
                new DbItem("Target")
                {
                    new DbItem("Child1"),
                    new DbItem("Child2")
                }
            })
            {

                var resolver = Utilities.CreateStandardResolver();

                var context = Context.Create(resolver);

                var service = new SitecoreService(db.Database, context);

                //Act
                var result = service.GetItem<StubNotLazy>("/sitecore/content/Target", x => x.LazyDisabled());

                Assert.AreEqual(2, result.Children.Count());

                service.Dispose();

                //Assert
                Assert.AreEqual(2, result.Children.Count());


            }
        }

        [Test]
        public void SitecoreService_NotLazyLoadingChildren_EnumerationAfterServiceDisposal()
        {
            //Arrange

            var fieldName = "Field1";

            using (Db db = new Db()
            {

                new DbItem("Target")
                {
                    new DbItem("Child1"),
                    new DbItem("Child2")
                }
            })
            {

                var resolver = Utilities.CreateStandardResolver();

                var context = Context.Create(resolver);

                var service = new SitecoreService(db.Database, context);

                //Act
                var result = service.GetItem<StubNotLazy>("/sitecore/content/Target", x=>x.LazyDisabled());

                service.Dispose();

                //Assert
                Assert.AreEqual(2, result.Children.Count());


            }
        }


        [Test]
        public void SitecoreService_LazyLoadingChildren_EnumerationBeforeAndAfterServiceDisposal()
        {
            //Arrange

            var fieldName = "Field1";

            using (Db db = new Db()
            {

                new DbItem("Target")
                {
                    new DbItem("Child1"),
                    new DbItem("Child2")
                }
            })
            {

                var resolver = Utilities.CreateStandardResolver();

                var context = Context.Create(resolver);

                var service = new SitecoreService(db.Database, context);

                //Act
                var result = service.GetItem<StubNotLazy>("/sitecore/content/Target", x => x.LazyDisabled());

                Assert.AreEqual(2, result.Children.Count());

                service.Dispose();

                //Assert
                Assert.AreEqual(2, result.Children.Count());


            }
        }


        [Test]
        public void SitecoreService_LazyLoadingChildren_EnumerationAfterServiceDisposalThrowsException()
        {
            //Arrange

            var fieldName = "Field1";

            using (Db db = new Db()
            {

                new DbItem("Target")
                {
                    new DbItem("Child1"),
                    new DbItem("Child2")
                }
            })
            {

                var resolver = Utilities.CreateStandardResolver();

                var context = Context.Create(resolver);

                var service = new SitecoreService(db.Database, context);

                //Act
                var result = service.GetItem<StubNotLazy>("/sitecore/content/Target");

                service.Dispose();

                //Assert

                Assert.Throws<MapperException>(() =>
                {
                    result.Children.Count();
                });


            }
        }

        public class StubNotLazy
        {
            public virtual IEnumerable<StubChild> Children { get; set; }
            public virtual Guid Id { get; set; }
        }

        public class StubLazy
        {
            public virtual IEnumerable<StubChild> Children { get; set; }
            public virtual Guid Id { get; set; }
        }
        public class StubChild
        {

            public virtual Guid Id { get; set; }
            
        }
    }
}
