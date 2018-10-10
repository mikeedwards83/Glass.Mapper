
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Diagnostics;
using Glass.Mapper.IoC;
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
            _task = new CreateInterfaceTask(new LazyLoadingHelper());
        }

        #region Method - Execute

        [Test]
        public void Execute_ConcreteClass_ObjectNotCreated()
        {
            //Assign
            Type type = typeof(StubClass);
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
        public void Execute_ProxyInterface_ProxyGetsCreated()
        {
            //Assign
            Type type = typeof(IStubInterface);
            var glassConfig = Substitute.For<IGlassConfiguration>();
            var service = Substitute.For<IAbstractService>();

            Context context = Context.Create(Substitute.For<IDependencyResolver>());

            AbstractTypeCreationContext abstractTypeCreationContext = new TestTypeCreationContext();
            abstractTypeCreationContext.Options = new TestGetOptions()
            {
                Type = typeof (IStubInterface)
            };
           
            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;

            ObjectConstructionArgs args = new ObjectConstructionArgs(context, abstractTypeCreationContext, configuration, service);

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsNotNull(args.Result);
            Assert.IsTrue(args.Result is IStubInterface);
            Assert.IsFalse(args.Result.GetType() == typeof(IStubInterface));
        }

        [Test]
        public void Execute_TwoClassesWithTheSameName_ProxyGetsCreated()
        {
            //Assign
            var glassConfig = Substitute.For<IGlassConfiguration>();
            var service = Substitute.For<IAbstractService>();

            Context context = Context.Create(Substitute.For<IDependencyResolver>());


            AbstractTypeCreationContext abstractTypeCreationContext1 = new TestTypeCreationContext();
            abstractTypeCreationContext1.Options = new TestGetOptions()
            {
                Type = typeof(NS1.ProxyTest1)
            };

            var configuration1 = Substitute.For<AbstractTypeConfiguration>();
            configuration1.Type = typeof(NS1.ProxyTest1);

            ObjectConstructionArgs args1 = new ObjectConstructionArgs(context, abstractTypeCreationContext1, configuration1, service);


            AbstractTypeCreationContext abstractTypeCreationContext2 = new TestTypeCreationContext();
            abstractTypeCreationContext2.Options = new TestGetOptions()
            {
                Type = typeof(NS2.ProxyTest1)
            };

            var configuration2 = Substitute.For<AbstractTypeConfiguration>();
            configuration2.Type = typeof(NS2.ProxyTest1); ;

            ObjectConstructionArgs args2 = new ObjectConstructionArgs(context, abstractTypeCreationContext2, configuration2, service);

            //Act
            _task.Execute(args1);
            _task.Execute(args2);

            //Assert
            Assert.IsNotNull(args1.Result);
            Assert.IsTrue(args1.Result is NS1.ProxyTest1);
            Assert.IsFalse(args1.Result.GetType() == typeof(NS1.ProxyTest1));

            Assert.IsNotNull(args2.Result);
            Assert.IsTrue(args2.Result is NS2.ProxyTest1);
            Assert.IsFalse(args2.Result.GetType() == typeof(NS2.ProxyTest1));
        }

        
        [Test]
        public void Execute_ResultAlreadySet_DoesNoWork()
        {
            //Assign
            Type type = typeof(IStubInterface);
            var resolver = Substitute.For<IDependencyResolver>();
            var service = Substitute.For<IAbstractService>();

            Context context = Context.Create(resolver);

            AbstractTypeCreationContext abstractTypeCreationContext = new TestTypeCreationContext();
            abstractTypeCreationContext.Options = new TestGetOptions()
            {
                Type = typeof(IStubInterface)
            };


            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;

            ObjectConstructionArgs args = new ObjectConstructionArgs(context, abstractTypeCreationContext, configuration, service);
            args.Result = string.Empty;

            //Act
            _task.Execute(args);

            //Assert
            Assert.IsNotNull(args.Result);
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


        #endregion
    }

    namespace NS1
    {
        public interface ProxyTest1 { }
    }
    namespace NS2
    {
        public interface ProxyTest1 { }
    }
}




