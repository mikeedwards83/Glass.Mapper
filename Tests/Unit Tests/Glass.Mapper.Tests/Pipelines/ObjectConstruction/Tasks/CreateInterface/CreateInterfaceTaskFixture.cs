using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Pipelines.ObjectConstruction.Tasks.CreateInterface
{
    [TestFixture]
    public class CreateInterfaceTaskFixture
    {
        private CreateInterfaceTask _task;

        [SetUp]
        public void Setup()
        {
            _task = new CreateInterfaceTask();
        }

        #region Method - Execute

        [Test]
        public void Execute_ConcreteClass_ObjectNotCreated()
        {
            //Assign
            AbstractItemContext context = Substitute.For<AbstractItemContext>();
            context.Type = typeof (StubClass);

            ObjectConstructionArgs args = new ObjectConstructionArgs(context);

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsNull(args.Object);
            Assert.IsFalse(args.AbortPipeline);

        }

        [Test]
        public void Execute_ProxyInterface_ProxyGetsCreated()
        {
            //Assign
            AbstractItemContext context = Substitute.For<AbstractItemContext>();
            context.Type = typeof(IStubInterface);

            ObjectConstructionArgs args = new ObjectConstructionArgs(context);

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsNotNull(args.Object);
            Assert.IsTrue(args.AbortPipeline);
            Assert.IsTrue(args.Object is IStubInterface);
            Assert.IsFalse(args.Object.GetType() == typeof(IStubInterface));
        }



        #endregion

        #region Stubs

        public class StubClass
        {

        }

        public interface IStubInterface
        {

        }

        #endregion
    }
}
