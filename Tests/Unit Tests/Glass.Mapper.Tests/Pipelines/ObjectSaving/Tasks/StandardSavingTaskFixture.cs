using System;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.ObjectSaving;
using Glass.Mapper.Pipelines.ObjectSaving.Tasks;
using Glass.Mapper.Tests.Configuration.Attributes;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Pipelines.ObjectSaving.Tasks
{
    [TestFixture]
    public class StandardSavingTaskFixture
    {

        #region Method - Execute


        [Test]
        public void Execute_ConfigurationNotSet_ThrowsException()
        {
            //Assign
            var target = new object();
            var savingContext = Substitute.For<AbstractTypeSavingContext>();
            var service = Substitute.For<IAbstractService>();
            var args = new ObjectSavingArgs(null, target, savingContext, service);
            var task = new StandardSavingTask();


            //Act
            Assert.Throws<PipelineException>(() =>
            {
                task.Execute(args);
            });

            //Assert

        }

        [Test]
        public void Execute_RunnerCorrectlyConfigured_CallsEachDataMapper()
        {
            //Assign
            var target = new Stub();
            var savingContext = Substitute.For<AbstractTypeSavingContext>();
            var service = Substitute.For<IAbstractService>();
            var args = new ObjectSavingArgs(null, target, savingContext, service);
            var task = new StandardSavingTask();
            var options = new GetOptions();


            var dataContext = Substitute.For<AbstractDataMappingContext>(target, options);
            savingContext.CreateDataMappingContext(Arg.Is<IAbstractService>(x=>x == service)).Returns(dataContext);

            var property1 = Substitute.For<AbstractPropertyConfiguration>();
            var config1 = Substitute.For<AbstractPropertyConfiguration>();
            var mapper1 = new StubMapper();

            property1.Mapper = mapper1;
            config1.PropertyInfo = typeof (Stub).GetProperty("Property");
            property1.PropertyInfo = config1.PropertyInfo;
            mapper1.Setup(new DataMapperResolverArgs(null, config1));

            var property2 = Substitute.For<AbstractPropertyConfiguration>();
            var config2 = Substitute.For<AbstractPropertyConfiguration>();
            var mapper2 = new StubMapper();

            property2.Mapper = mapper2;
            config2.PropertyInfo = typeof (Stub).GetProperty("Property2");
            property2.PropertyInfo = config2.PropertyInfo;
            mapper2.Setup(new DataMapperResolverArgs(null, config2));

            savingContext.Config = new AttributeConfigurationLoaderFixture.StubTypeConfiguration();
            savingContext.Config.AddProperty(property1);
            savingContext.Config.AddProperty(property2);

            //Act
            task.Execute(args);

            //Assert
            Assert.IsTrue(mapper1.MapCalled);
            Assert.IsTrue(mapper2.MapCalled);
        }

        #endregion

        #region Stubs

        public class Stub
        {
            public string Property { get; set; }
            public string Property2 { get; set; }
        }

        public class StubMapper : AbstractDataMapper
        {

            public bool MapCalled { get; set; }

            public override void MapToCms(AbstractDataMappingContext mappingContext)
            {
                MapCalled = true;
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

        public class StubAbstractTypeConfiguration : AbstractTypeConfiguration
        {
        }
    

    #endregion
    }
}




