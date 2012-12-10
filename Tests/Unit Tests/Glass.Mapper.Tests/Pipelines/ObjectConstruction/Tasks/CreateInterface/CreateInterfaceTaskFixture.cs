using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
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
            Type type = typeof(StubClass);
            var glassConfig = Substitute.For<IGlassConfiguration>();
            var service = Substitute.For<IAbstractService>();

            Context.ResolverFactory = Substitute.For<IDependencyResolverFactory>();
            Context.ResolverFactory.GetResolver().Returns(Substitute.For<IDependencyResolver>());
            Context context = Context.Create(glassConfig);

            AbstractTypeCreationContext abstractTypeCreationContext = Substitute.For<AbstractTypeCreationContext>();
            abstractTypeCreationContext.RequestedType.Returns(type);

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;

            ObjectConstructionArgs args = new ObjectConstructionArgs(context, abstractTypeCreationContext, configuration, service);

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsNull(args.Result);
            Assert.IsFalse(args.IsAborted);

        }

        [Test]
        public void Execute_ProxyInterface_ProxyGetsCreated()
        {
            //Assign
            Type type = typeof(IStubInterface);
            var glassConfig = Substitute.For<IGlassConfiguration>();
            var service = Substitute.For<IAbstractService>();

            Context.ResolverFactory = Substitute.For<IDependencyResolverFactory>();
            Context.ResolverFactory.GetResolver().Returns(Substitute.For<IDependencyResolver>());
            Context context = Context.Create(glassConfig);

            AbstractTypeCreationContext abstractTypeCreationContext = Substitute.For<AbstractTypeCreationContext>();
            abstractTypeCreationContext.RequestedType.Returns(typeof (IStubInterface));

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;

            ObjectConstructionArgs args = new ObjectConstructionArgs(context, abstractTypeCreationContext, configuration, service);

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsNotNull(args.Result);
            Assert.IsTrue(args.IsAborted);
            Assert.IsTrue(args.Result is IStubInterface);
            Assert.IsFalse(args.Result.GetType() == typeof(IStubInterface));
        }

        [Test]
        public void Execute_ResultAlreadySet_DoesNoWork()
        {
            //Assign
            Type type = typeof(IStubInterface);
            var glassConfig = Substitute.For<IGlassConfiguration>();
            var service = Substitute.For<IAbstractService>();

            Context context = Context.Create(glassConfig);

            AbstractTypeCreationContext abstractTypeCreationContext = Substitute.For<AbstractTypeCreationContext>();
            abstractTypeCreationContext.RequestedType.Returns(typeof(IStubInterface));

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;

            ObjectConstructionArgs args = new ObjectConstructionArgs(context, abstractTypeCreationContext, configuration, service);
            args.Result = string.Empty;

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsNotNull(args.Result);
            Assert.IsFalse(args.IsAborted);
            Assert.IsTrue(args.Result is string);
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
