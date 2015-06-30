using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;
using Sitecore.Configuration;

namespace Glass.Mapper.Sc.Integration.Issues.Issue145
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
            var db = Factory.GetDatabase("master");
            var resolver = Utilities.CreateStandardResolver();
            resolver.DataMapperFactory.Insert(0,()=> new StubDataMapper());

            var context = Context.Create(resolver);
            var service = new SitecoreService(db, context);


            //Act
            var result = service.GetItem<Stub>("/sitecore");


            //Assert
            Assert.AreEqual("property test", result.Property1.Value);

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
