using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Configuration;
using Glass.Mapper.Configuration.Attributes;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.DataMapperResolver.Tasks;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Pipelines.DataMapperResolver
{
    [TestFixture]
    public class DataMapperStandardResolverTaskFixture
    {
        private DataMapperStandardResolverTask _task;

        [SetUp]
        public void Setup()
        {
            _task = new DataMapperStandardResolverTask();
        }

        #region Method - Execute

        [Test]
        public void Execute_InvokesMapperSetup()
        {
            //Assign
            var configuration = Substitute.For<AbstractPropertyConfiguration>();
            configuration.PropertyInfo = typeof (StubClass).GetProperty("NoAttributeProperty");

            var mapper = Substitute.For<AbstractDataMapper>();
            mapper.CanHandle(configuration, null).Returns(true);

            var args = new DataMapperResolverArgs(null, configuration);
            args.DataMappers = new List<AbstractDataMapper> { mapper };

            //Act
            _task.Execute(args);

            //Assert
            mapper.Received().Setup(args);
        }

        [Test]
        public void Execute_NoDataMapperAttribute_SetsResultToMatchingRegisteredMapper()
        {
            //Assign
            var configuration = Substitute.For<AbstractPropertyConfiguration>();
            configuration.PropertyInfo = typeof (StubClass).GetProperty("NoAttributeProperty");

            var mapper = Substitute.For<AbstractDataMapper>();
            mapper.CanHandle(configuration, null).Returns(true);

            var args = new DataMapperResolverArgs(null, configuration);
            args.DataMappers = new List<AbstractDataMapper> { mapper };

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsNotNull(args.Result);
            Assert.AreEqual(mapper, args.Result);
        }

        [Test]
        public void Execute_NoDataMapperAttribute_NoMatchingMapper_ThrowsMapperException()
        {
            //Assign
            var configuration = Substitute.For<AbstractPropertyConfiguration>();
            configuration.PropertyInfo = typeof (StubClass).GetProperty("NoAttributeProperty");

            var args = new DataMapperResolverArgs(null, configuration);
            args.DataMappers = Enumerable.Empty<AbstractDataMapper>();

            //Act
            Assert.Throws<MapperException>(() => _task.Execute(args));

            //Assert
        }

      

        [Test]
        public void Execute_DataMapperAttribute_SetsResultToMatchingRegisteredMapper()
        {
            //Assign
            var configuration = Substitute.For<AbstractPropertyConfiguration>();
            configuration.PropertyInfo = typeof (StubClass).GetProperty("StubMapperProperty");

            var mapper = new StubMapper();
            var args = new DataMapperResolverArgs(null, configuration);
            args.DataMappers = new List<AbstractDataMapper> {mapper};

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsNotNull(args.Result);
            Assert.AreEqual(mapper, args.Result);
        }


        [Test]
        public void Execute_DataMapperAttribute_CannotHandle_ThrowsMapperException()
        {
            //Assign
            var configuration = Substitute.For<AbstractPropertyConfiguration>();
            configuration.PropertyInfo = typeof (StubClass).GetProperty("CannotHandleProperty");

            var args = new DataMapperResolverArgs(null, configuration);
            args.DataMappers = Enumerable.Empty<AbstractDataMapper>();

            //Act
            Assert.Throws<MapperException>(() => _task.Execute(args));

            //Assert
        }

        [Test]
        public void Execute_DataMapperAttribute_MapperMissingConstructor_ThrowsMapperException()
        {
            //Assign
            var configuration = Substitute.For<AbstractPropertyConfiguration>();
            configuration.PropertyInfo = typeof (StubClass).GetProperty("MissingConstructorStubMapperProperty");

            var args = new DataMapperResolverArgs(null, configuration);
            args.DataMappers = Enumerable.Empty<AbstractDataMapper>();

            //Act
            Assert.Throws<MapperException>(() => _task.Execute(args));

            //Assert
        }

        [Test]
        public void Execute_DataMapperAttribute_InvalidType_ThrowsMapperException()
        {
            //Assign
            var configuration = Substitute.For<AbstractPropertyConfiguration>();
            configuration.PropertyInfo = typeof (StubClass).GetProperty("InvalidMapperTypeProperty");

            var args = new DataMapperResolverArgs(null, configuration);
            args.DataMappers = Enumerable.Empty<AbstractDataMapper>();

            //Act
            Assert.Throws<MapperException>(() => _task.Execute(args));

            //Assert
        }

        #endregion

        #region Stubs

        public class StubClass
        {
            public string StubMapperProperty { get; set; }

            public string CannotHandleProperty { get; set; }

            public string MissingConstructorStubMapperProperty { get; set; }

            public string InvalidMapperTypeProperty { get; set; }

            public string NoAttributeProperty { get; set; }
        }

        public class MissingConstructorStubMapper : AbstractDataMapper
        {
            public MissingConstructorStubMapper(object obj)
            {
            }

            public override void MapToCms(AbstractDataMappingContext mappingContext)
            {
                throw new NotImplementedException();
            }

            public override object MapToProperty(AbstractDataMappingContext mappingContext)
            {
                throw new NotImplementedException();
            }

            public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
            {
                throw new NotImplementedException();
            }
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
                throw new NotImplementedException();
            }

            public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
            {
                return true;
            }
        }

        public class StubMapperCannotHandle : StubMapper
        {
            public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
            {
                return false;
            }
        }

        #endregion
    }
}
