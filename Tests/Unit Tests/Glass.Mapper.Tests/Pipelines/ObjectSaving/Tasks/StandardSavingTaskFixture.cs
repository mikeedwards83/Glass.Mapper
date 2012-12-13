using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines;
using Glass.Mapper.Pipelines.ObjectSaving;
using Glass.Mapper.Pipelines.ObjectSaving.Tasks;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Pipelines.ObjectSaving.Tasks
{
    [TestFixture]
    public class StandardSavingTaskFixture
    {

        #region Method - Execute


        [Test]
        [ExpectedException(typeof(PipelineException))]
        public void Execute_ConfigurationNotSet_ThrowsException()
        {
            //Assign
            var target = new object();
            var savingContext = Substitute.For<AbstractTypeSavingContext>();
            var service = Substitute.For<IAbstractService>();
            var args = new ObjectSavingArgs(null, target, savingContext, service);
            var task = new StandardSavingTask();


            //Act
            task.Execute(args);

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

            var dataContext = Substitute.For<AbstractDataMappingContext>(target);
            service.CreateDataMappingContext(savingContext).Returns(dataContext);

            var property1 = Substitute.For<AbstractPropertyConfiguration>();
            var property1Called = false;
            property1.Mapper = Substitute.For<AbstractDataMapper>();
            property1.Mapper.When(x => x.MapToCms(dataContext)).Do(info => property1Called = true);
            property1.Mapper.Property = typeof(Stub).GetProperty("Property");

            var property2 = Substitute.For<AbstractPropertyConfiguration>();
            var property2Called = false;
            property2.Mapper = Substitute.For<AbstractDataMapper>();
            property2.Mapper.When(x => x.MapToCms(dataContext)).Do(info => property2Called = true);
            property2.Mapper.Property = typeof(Stub).GetProperty("Property");

            savingContext.Config = Substitute.For<AbstractTypeConfiguration>();
            savingContext.Config.AddProperty(property1);
            savingContext.Config.AddProperty(property2);

            //Act
            task.Execute(args);

            //Assert
            Assert.IsTrue(property1Called);
            Assert.IsTrue(property2Called);
        }

        #endregion

        #region Stubs

        public class Stub
        {
            public string Property { get; set; }
        }

        #endregion
    }
}
