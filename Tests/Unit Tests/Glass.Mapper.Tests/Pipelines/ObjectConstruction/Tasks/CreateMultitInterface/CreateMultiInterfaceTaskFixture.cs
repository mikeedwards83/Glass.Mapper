using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateMultiInterface;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Pipelines.ObjectConstruction.Tasks.CreateMulitInterface
{
    [TestFixture]
    public class CreateMultiInterfaceTaskFixture
    {
        #region Method - Execute

        [Test]
        public void Execute_ArgsNotNull_ReturnsAndDoesNothing()
        {
            //Arrange
            var args = new ObjectConstructionArgs(null,null, null, null);
            var expected = new object();
            args.Result = expected;

            var task = new CreateMultiInferaceTask();
            
            //Act
            task.Execute(args);

            //Assert
            Assert.AreEqual(expected, args.Result);
        }

        [Test]
        public void Execute_ArgsNotNullMultipleInterface_ReturnsMultiInterfaceProxy()
        {
            //Arrange

            var config1 = new StubAbstractTypeConfiguration();
            var config2 = new StubAbstractTypeConfiguration();

            config1.Type = typeof(IStubTarget);
            config2.Type = typeof(IStubTarget2);

          //  var args = new ObjectConstructionArgs(null, null, new[] { config1, config2 }, null);
            var args = new ObjectConstructionArgs(null, null, config1,  null);
            args.Parameters[CreateMultiInferaceTask.MultiInterfaceConfigsKey] = new[] {config2};
            var task = new CreateMultiInferaceTask();

            //Act
            task.Execute(args);

            //Assert
            Assert.IsNotNull(args.Result);
            Assert.IsTrue(args.Result is IStubTarget);
            Assert.IsTrue(args.Result is IStubTarget2);
        }

        [Test]
        public void Execute_ArgsNotNullOneInterface_ReturnsNull()
        {
            //Arrange

            var config1 = new StubAbstractTypeConfiguration();

            config1.Type = typeof(IStubTarget);

            var args = new ObjectConstructionArgs(null, null,  config1 , null);

            var task = new CreateMultiInferaceTask();

            //Act
            task.Execute(args);

            //Assert
            Assert.IsNull(args.Result);
        }

        [Test]
        public void Execute_ArgsNotNullMultipleTypesNotAllInterfaces_ReturnsNull()
        {
            //Arrange

            var config1 = new StubAbstractTypeConfiguration();
            var config2 = new StubAbstractTypeConfiguration();

            config1.Type = typeof(IStubTarget);
            config2.Type = typeof(StubClass);

            var args = new ObjectConstructionArgs(null, null, config2 , null);
            //var args = new ObjectConstructionArgs(null, null, new[] { config1, config2 }, null);

            var task = new CreateMultiInferaceTask();

            //Act
            task.Execute(args);

            //Assert
            Assert.IsNull(args.Result);
        }

        #endregion

        #region Stubs

        public class StubAbstractTypeConfiguration : AbstractTypeConfiguration
        {

        }

        public interface IStubTarget
        {
            string Property1 { get; set; }
        }
        public interface IStubTarget2
        {
            string Property2 { get; set; }
        }
        public class StubClass
        {
            string Property2 { get; set; }
        }

        #endregion
    }
}
