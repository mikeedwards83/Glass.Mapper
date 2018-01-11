using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Configuration.Fluent;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Glass.Mapper.Sc.FakeDb.Issues.Issue247
{
    [TestFixture]
    public class Issue247
    {
        const string TemplateId1 = "8C429BCF-CAAE-4D06-B5C4-4309E7AE2B31";
        const string TemplateId2 = "8C429BCF-CAAE-4D06-B5C4-4309E7AE2B32";

        [Test]
        public void InferredTypeParentWithCache()
        {
            //Arrange
            var templateId1 = new ID(TemplateId1);
            var templateId2 = new ID(TemplateId2);
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId1)
                {
                    new Sitecore.FakeDb.DbItem("Child1", ID.NewID,templateId2)
                    {
                        new Sitecore.FakeDb.DbItem("Child2", ID.NewID,templateId1)
                    }
                }
            })
            {
                var resolver = Utilities.CreateStandardResolver(finalise: false);
                resolver.DataMapperFactory.Insert(0, () => new Issue145.Issue145.StubDataMapper());
                resolver.Finalise();

                var context = Context.Create(resolver);
                var service = new SitecoreService(database.Database, context);

                var loader = new SitecoreFluentConfigurationLoader();
                var inferBase = loader.Add<InterfaceBase>();
                inferBase.Parent(x => x.Parent).InferType();
                var type1 = loader.Add<Type1>();
                type1.TemplateId(templateId1);
                type1.Import(inferBase);
                var type2 = loader.Add<Type2>();
                type2.TemplateId(templateId1);
                type2.Import(inferBase);

                context.Load(loader);

                //Act

                var child = service.GetItem<InterfaceBase>("/sitecore/content/target/child1/child2");
                int count = 0;

                while (child != null)
                {
                    count++;
                    child = child.Parent;

                }


                //Assert
                Assert.AreEqual(3, count);

            }


        }

        public interface InterfaceBase
        {
            InterfaceBase Parent { get; set; }
        }

        public class Type1 : InterfaceBase
        {
           public virtual InterfaceBase Parent { get; set; }
        }

        public class Type2 : InterfaceBase
        {
            public virtual InterfaceBase Parent { get; set; }
        }
    }
}
