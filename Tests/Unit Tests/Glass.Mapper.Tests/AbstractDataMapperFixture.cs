using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.DataMapperResolver;
using NUnit.Framework;
using NSubstitute;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Tests
{
    [TestFixture]
    public class AbstractDataMapperFixture
    {

        [SetUp]
        public void Setup()
        {


        }

        #region Method - MapCmsToProperty

        [Test]
        public void MapCmsToProperty_ValueFromCms_WritesToProperty()
        {
            //Assign
            var obj = new StubClass();
            var config = Substitute.For<AbstractPropertyConfiguration>();
            var  dataMapper = new StubMapper();

            AbstractDataMappingContext context = Substitute.For<AbstractDataMappingContext>(obj);
           dataMapper.Setup(new DataMapperResolverArgs(null, config));
            config.PropertyInfo = typeof(StubClass).GetProperties().First(x => x.Name == "AProperty");

            //Act
            dataMapper.MapCmsToProperty(context);

            //Assert
            Assert.AreEqual("Hello world", obj.AProperty);
            Assert.AreEqual(context, dataMapper.MappingContext);

        }

        #endregion

        #region Stubs

        public class StubClass
        {
            public string AProperty { get; set; }
        }

        public class StubMapper : AbstractDataMapper
        {

            public AbstractDataMappingContext MappingContext { get; set; }
            public override void MapToCms(AbstractDataMappingContext mappingContext)
            {
                throw new NotImplementedException();

            }

            public override object MapToProperty(AbstractDataMappingContext mappingContext)
            {
                this.MappingContext = mappingContext;
                return "Hello world";
            }

            public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
            {
                throw new NotImplementedException();
            }
        }
        #endregion

    }
}
