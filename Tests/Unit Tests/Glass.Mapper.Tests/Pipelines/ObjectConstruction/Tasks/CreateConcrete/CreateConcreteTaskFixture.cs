using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete;
using NUnit.Framework;
using NSubstitute;

namespace Glass.Mapper.Tests.Pipelines.ObjectConstruction.Tasks.CreateConcrete
{
    [TestFixture]
    public class CreateConcreteTaskFixture
    {
        private CreateConcreteTask _task;

        [SetUp]
        public void Setup()
        {
            _task = new CreateConcreteTask();
        }

        #region Method - Execute

        [Test]
        public void Execute_TypeIsInterface_NoObjectCreated()
        {
            //Assign
            AbstractItemContext context = Substitute.For<AbstractItemContext>();
            context.Type = typeof (IStubInterface);

            ObjectConstructionArgs args = new ObjectConstructionArgs(context);

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsFalse(args.AbortPipeline);
            Assert.IsNull(args.Object);

        }

        [Test]
        public void Execute_LazyType_LazyTypeCreated()
        {
            //Assign
            AbstractItemContext context = Substitute.For<AbstractItemContext>();
            context.Type = typeof(StubClass);
           
            ObjectConstructionArgs args = new ObjectConstructionArgs(context);
            args.IsLazy = true;

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsTrue(args.AbortPipeline);
            Assert.IsNotNull(args.Object);
            Assert.IsTrue(args.Object is StubClass);
            Assert.IsFalse(args.Object.GetType() == typeof(StubClass));
        }

        [Test]
        public void Execute_ConcreteType_TypeCreated()
        {
            //Assign
            AbstractItemContext context = Substitute.For<AbstractItemContext>();
            context.Type = typeof(StubClass);


            context.ConstructorMethods = Utilities.CreateConstructorDelegates(typeof (StubClass));
                

            ObjectConstructionArgs args = new ObjectConstructionArgs(context);
            args.IsLazy = false;

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsTrue(args.AbortPipeline);
            Assert.IsNotNull(args.Object);
            Assert.IsTrue(args.Object is StubClass);
            Assert.IsTrue(args.Object.GetType() == typeof(StubClass));
        }

        #endregion


        public interface IStubInterface
        {

        }

        public class StubClass
        {

        }
    }
}
