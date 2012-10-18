using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete;
using NUnit.Framework;
using NSubstitute;
using Glass.Mapper.Configuration;

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
            Type type = typeof (IStubInterface);

            Context context = Context.Load();

            IDataContext dataContext = Substitute.For<IDataContext>();
            dataContext.RequestedType.Returns(type);

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;

            ObjectConstructionArgs args = new ObjectConstructionArgs(context, dataContext, configuration);

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsFalse(args.IsAborted);
            Assert.IsNull(args.Result);

        }

        [Test]
        public void Execute_LazyType_LazyTypeCreated()
        {
            //Assign
            Type type = typeof (StubClass);

            Context context = Context.Load();

            IDataContext dataContext = Substitute.For<IDataContext>();
            dataContext.RequestedType.Returns(typeof (StubClass));
            dataContext.IsLazy = true;

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;
            configuration.ConstructorMethods = Utilities.CreateConstructorDelegates(type);

            ObjectConstructionArgs args = new ObjectConstructionArgs(context, dataContext, configuration);

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsTrue(args.IsAborted);
            Assert.IsNotNull(args.Result);
            Assert.IsTrue(args.Result is StubClass);
            Assert.IsFalse(args.Result.GetType() == typeof(StubClass));
        }

        [Test]
        public void Execute_ConcreteType_TypeCreated()
        {
            //Assign
            Type type = typeof (StubClass);

            Context context = Context.Load();

            IDataContext dataContext = Substitute.For<IDataContext>();
            dataContext.RequestedType.Returns(typeof (StubClass));

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.ConstructorMethods = Utilities.CreateConstructorDelegates(type);
            configuration.Type = type;

            ObjectConstructionArgs args = new ObjectConstructionArgs(context, dataContext, configuration);

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsTrue(args.IsAborted);
            Assert.IsNotNull(args.Result);
            Assert.IsTrue(args.Result is StubClass);
            Assert.IsTrue(args.Result.GetType() == typeof(StubClass));
        }

        [Test]
        public void Execute_ResultAlreadySet_DoesNoWork()
        {
            //Assign
            Type type = typeof (StubClass);

            Context context = Context.Load();

            IDataContext dataContext = Substitute.For<IDataContext>();
            dataContext.RequestedType.Returns(typeof (StubClass));

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.ConstructorMethods = Utilities.CreateConstructorDelegates(type);
            configuration.Type = type;

            ObjectConstructionArgs args = new ObjectConstructionArgs(context, dataContext, configuration);
            args.Result = string.Empty;

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsFalse(args.IsAborted);
            Assert.IsNotNull(args.Result);
            Assert.IsTrue(args.Result is string);
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
