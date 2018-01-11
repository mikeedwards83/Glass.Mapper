using System;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Glass.Mapper.Sc.FakeDb.Issues.Issue145
{
    /// <summary>
    /// Test for https://github.com/mikeedwards83/Glass.Mapper/issues/145
    /// </summary>
    [TestFixture]
    public class Issue145
    {

        [Test]
        public void CustomDataMapper()
        {
            //Arrange
            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    new Sitecore.FakeDb.DbItem("Child1")
                }
            })
            {
                var resolver = Utilities.CreateStandardResolver(finalise: false);
                resolver.DataMapperFactory.Insert(0, () => new StubDataMapper());
                resolver.Finalise();

                var context = Context.Create(resolver);
                var service = new SitecoreService(database.Database, context);

                //Act
                var result = service.GetItem<Stub>("/sitecore");

                //Assert
                Assert.AreEqual("property test", result.Property1.Value);
            }
        }



        public class Stub
        {
            [SitecoreField("__Display Name")]
            public virtual Property Property1 { get; set; }
        }

        public class Property
        {
            public Property()
            {
                Value = "property test";
            }
            public string Value { get; set; }
        }

        public class StubDataMapper : AbstractSitecoreFieldMapper
        {
            public StubDataMapper() : base(typeof (Property))
            {
                
            }

            public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
            {
                throw new NotImplementedException();
            }

            public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
            {
                return new Property();
            }
        }
    }
}
