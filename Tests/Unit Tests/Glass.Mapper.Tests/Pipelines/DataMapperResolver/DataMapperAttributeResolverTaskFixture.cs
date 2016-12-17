using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Configuration;
using Glass.Mapper.Configuration.Attributes;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.DataMapperResolver.Tasks;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Pipelines.DataMapperResolver
{
    public class DataMapperAttributeResolverTaskFixture
    {

        [Test]
        public void Execute_DataMapperAttribute_SetsResultToSpecifiedMapper()
        {
            //Assign

            var task = new DataMapperAttributeResolverTask();
            var configuration = Substitute.For<AbstractPropertyConfiguration>();
            configuration.PropertyInfo = typeof(DataMapperAttributeResolverTaskFixture.StubClass).GetProperty("StubMapperProperty");

            var args = new DataMapperResolverArgs(null, configuration);
            args.DataMappers = Enumerable.Empty<AbstractDataMapper>();

            //Act
            task.Execute(args);

            //Assert
            Assert.IsNotNull(args.Result);
            Assert.IsTrue(args.Result.GetType() == typeof(DataMapperStandardResolverTaskFixture.StubMapper));
        }

        public class StubClass
        {
            [DataMapper(typeof(DataMapperStandardResolverTaskFixture.StubMapper))]
            public string StubMapperProperty { get; set; }

            [DataMapper(typeof(DataMapperStandardResolverTaskFixture.StubMapperCannotHandle))]
            public string CannotHandleProperty { get; set; }

            [DataMapper(typeof(DataMapperStandardResolverTaskFixture.MissingConstructorStubMapper))]
            public string MissingConstructorStubMapperProperty { get; set; }

            [DataMapper(typeof(object))]
            public string InvalidMapperTypeProperty { get; set; }

            public string NoAttributeProperty { get; set; }
        }
    }
}
