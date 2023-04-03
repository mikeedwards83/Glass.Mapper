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
            configuration.PropertyInfo = typeof(StubClass).GetProperty("StubMapperProperty");

            var args = new DataMapperResolverArgs(null, configuration);
            args.DataMappers = Enumerable.Empty<AbstractDataMapper>();

            //Act
            task.Execute(args);

            //Assert
            Assert.IsNotNull(args.Result);
            Assert.IsTrue(args.Result.GetType() == typeof(StubMapper));
        }

        [Test]
        public void Execute_DataMapperAttributeMapperMissingConstructor_SetsResultToSpecifiedMapper()
        {
            //Assign

            var task = new DataMapperAttributeResolverTask();
            var configuration = Substitute.For<AbstractPropertyConfiguration>();
            configuration.PropertyInfo = typeof(StubClass).GetProperty("MissingConstructorStubMapperProperty");

            var args = new DataMapperResolverArgs(null, configuration);
            args.DataMappers = Enumerable.Empty<AbstractDataMapper>();

            //Act
            Assert.Throws<NotSupportedException>(() =>
            {
                task.Execute(args);
            });

            //Assert
        }

        [Test]
        public void Execute_DataMapperAttributeLoadedFromMapperCollection_SetsResultToSpecifiedMapper()
        {
            //Assign

            var task = new DataMapperAttributeResolverTask();
            var configuration = Substitute.For<AbstractPropertyConfiguration>();
            configuration.PropertyInfo = typeof(StubClass).GetProperty("StubMapperProperty");

            var mapper=new StubMapper();

            var args = new DataMapperResolverArgs(null, configuration);
            args.DataMappers = new AbstractDataMapper[] {mapper};
            
            //Act
            task.Execute(args);

            //Assert
            Assert.IsNotNull(args.Result);
            Assert.AreEqual(mapper, args.Result);
        }

        [Test]
        public void Execute_DataMapperAttribute_InvokesSetupMethod()
        {
            //Assign
            var task = new DataMapperAttributeResolverTask();
            var configuration = Substitute.For<AbstractPropertyConfiguration>();
            configuration.PropertyInfo = typeof(StubClass).GetProperty("StubMapperProperty");

            var args = new DataMapperResolverArgs(null, configuration);
            args.DataMappers = Enumerable.Empty<AbstractDataMapper>();

            //Act
            task.Execute(args);

            //Assert
            var mapper = args.Result as StubMapper;
            Assert.IsNotNull(mapper);
            Assert.IsTrue(mapper.SetupInvoked);
        }

        public class StubClass
        {
            [DataMapper(typeof(StubMapper))]
            public string StubMapperProperty { get; set; }

            [DataMapper(typeof(StubMapperCannotHandle))]
            public string CannotHandleProperty { get; set; }

            [DataMapper(typeof(MissingConstructorStubMapper))]
            public string MissingConstructorStubMapperProperty { get; set; }

            [DataMapper(typeof(object))]
            public string InvalidMapperTypeProperty { get; set; }

            public string NoAttributeProperty { get; set; }
        }


        public class StubMapper : AbstractDataMapper
        {
            public AbstractDataMappingContext MappingContext { get; set; }

            public bool SetupInvoked { get; private set; }

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

            public override void Setup(DataMapperResolverArgs args)
            {
                base.Setup(args);
                SetupInvoked = true;
            }
        }

        public class StubMapperCannotHandle : StubMapper
        {
            public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
            {
                return false;
            }
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

    }
}
