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
            var glassConfig = Substitute.For<GlassCastleConfigBase>();
            var service = Substitute.For<IAbstractService>();

            Type type = typeof(IStubInterface);

            Context context = Context.Create(glassConfig);

            AbstractTypeCreationContext abstractTypeCreationContext = Substitute.For<AbstractTypeCreationContext>();
            abstractTypeCreationContext.RequestedType.Returns(type);

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;

            ObjectConstructionArgs args = new ObjectConstructionArgs(context, abstractTypeCreationContext, configuration, service);

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
            var glassConfig = Substitute.For<GlassCastleConfigBase>();
            var service = Substitute.For<IAbstractService>();

            Context context = Context.Create(glassConfig);

            AbstractTypeCreationContext abstractTypeCreationContext = Substitute.For<AbstractTypeCreationContext>();
            abstractTypeCreationContext.RequestedType.Returns(typeof (StubClass));
            abstractTypeCreationContext.IsLazy = true;

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;
            configuration.ConstructorMethods = Utilities.CreateConstructorDelegates(type);

            ObjectConstructionArgs args = new ObjectConstructionArgs(context, abstractTypeCreationContext, configuration, service);

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
            var glassConfig = Substitute.For<GlassCastleConfigBase>();
            var service = Substitute.For<IAbstractService>();

            Context context = Context.Create(glassConfig);

            AbstractTypeCreationContext abstractTypeCreationContext = Substitute.For<AbstractTypeCreationContext>();
            abstractTypeCreationContext.RequestedType.Returns(typeof (StubClass));

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.ConstructorMethods = Utilities.CreateConstructorDelegates(type);
            configuration.Type = type;

            ObjectConstructionArgs args = new ObjectConstructionArgs(context, abstractTypeCreationContext, configuration, service);

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
            var glassConfig = Substitute.For<GlassCastleConfigBase>();
            var service = Substitute.For<IAbstractService>();

            Context context = Context.Create(glassConfig);

            AbstractTypeCreationContext abstractTypeCreationContext = Substitute.For<AbstractTypeCreationContext>();
            abstractTypeCreationContext.RequestedType.Returns(typeof (StubClass));

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.ConstructorMethods = Utilities.CreateConstructorDelegates(type);
            configuration.Type = type;

            ObjectConstructionArgs args = new ObjectConstructionArgs(context, abstractTypeCreationContext, configuration, service);
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
