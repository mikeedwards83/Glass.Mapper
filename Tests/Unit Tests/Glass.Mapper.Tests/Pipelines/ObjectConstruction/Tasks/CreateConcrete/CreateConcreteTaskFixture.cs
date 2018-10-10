using System;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete;
using NUnit.Framework;
using NSubstitute;
using Glass.Mapper.Configuration;
using Glass.Mapper.Diagnostics;
using Glass.Mapper.IoC;

namespace Glass.Mapper.Tests.Pipelines.ObjectConstruction.Tasks.CreateConcrete
{
    [TestFixture]
    public class CreateConcreteTaskFixture
    {
        private CreateConcreteTask _task;

        [SetUp]
        public void Setup()
        {
            _task = new CreateConcreteTask(new LazyLoadingHelper());
        }

        #region Method - Execute

        [Test]
        public void Execute_TypeIsInterface_NoObjectCreated()
        {
            //Assign
            Type type = typeof(IStubInterface);
            var service = Substitute.For<IAbstractService>();
            Context context = Context.Create(Substitute.For<IDependencyResolver>());

            AbstractTypeCreationContext abstractTypeCreationContext = new TestTypeCreationContext();
            abstractTypeCreationContext.Options = new TestGetOptions()
            {
                Type = type
            };

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;

            ObjectConstructionArgs args = new ObjectConstructionArgs(context, abstractTypeCreationContext, configuration, service);

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsNull(args.Result);

        }

        [Test]
        public void Execute_LazyType_LazyTypeCreated()
        {
            //Assign
            Type type = typeof (StubClass);
          
            var service = Substitute.For<IAbstractService>();

            var dependencyResolver = Substitute.For<IDependencyResolver>();
            Context context = Context.Create(dependencyResolver);

            AbstractTypeCreationContext abstractTypeCreationContext = new TestTypeCreationContext();
            abstractTypeCreationContext.Options = new TestGetOptions()
            {
                Type = typeof(StubClass),
                Lazy = LazyLoading.Enabled
            };

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;
            configuration.ConstructorMethods = Utilities.CreateConstructorDelegates(type);

            ObjectConstructionArgs args = new ObjectConstructionArgs(context, abstractTypeCreationContext, configuration, service);

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsNotNull(args.Result);
            Assert.IsTrue(args.Result is StubClass);
            Assert.IsFalse(args.Result.GetType() == typeof(StubClass));
        }

        [Test]
        public void Execute_ConcreteType_TypeCreated()
        {
            //Assign
            Type type = typeof (StubClass);
            
            var service = Substitute.For<IAbstractService>();

            Context context = Context.Create(Substitute.For<IDependencyResolver>());

            AbstractTypeCreationContext abstractTypeCreationContext = new TestTypeCreationContext();
            abstractTypeCreationContext.Options = new TestGetOptions()
            {
                Type = typeof(StubClass),
                Lazy = LazyLoading.Disabled
            };



            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.ConstructorMethods = Utilities.CreateConstructorDelegates(type);
            configuration.Type = type;


            ObjectConstructionArgs args = new ObjectConstructionArgs(context, abstractTypeCreationContext,configuration, service);

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsNotNull(args.Result);
            Assert.IsTrue(args.Result is StubClass);
            Assert.IsTrue(args.Result.GetType() == typeof(StubClass));
        }

        [Test]
        public void Execute_ResultAlreadySet_DoesNoWork()
        {
            //Assign
            Type type = typeof (StubClass);
          
            var service = Substitute.For<IAbstractService>();

            Context context = Context.Create(Substitute.For<IDependencyResolver>());

            AbstractTypeCreationContext abstractTypeCreationContext = new TestTypeCreationContext();
            abstractTypeCreationContext.Options = new TestGetOptions()
            {
                Type = typeof(StubClass),
            };

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.ConstructorMethods = Utilities.CreateConstructorDelegates(type);
            configuration.Type = type;

            ObjectConstructionArgs args = new ObjectConstructionArgs(context, abstractTypeCreationContext,configuration, service);
            args.Result = string.Empty;

            //Act
            _task.Execute(args);

            //Assert
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

        public class StubAbstractDataMappingContext : AbstractDataMappingContext
        {
            public StubAbstractDataMappingContext(object obj, GetOptions options) 
                : base(obj, options)
            {
            }
        }

        public class TestTypeCreationContext : AbstractTypeCreationContext
        {
            public override bool CacheEnabled { get; }
            public override AbstractDataMappingContext CreateDataMappingContext(object obj)
            {
                return new StubAbstractDataMappingContext(obj, Options);
            }
        }
    }
}




