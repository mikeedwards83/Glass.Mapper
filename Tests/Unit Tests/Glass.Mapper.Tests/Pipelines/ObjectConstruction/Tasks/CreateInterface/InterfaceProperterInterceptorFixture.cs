using System;
using Castle.DynamicProxy;
using Glass.Mapper.Configuration;
using Glass.Mapper.IoC;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Pipelines.ObjectConstruction.Tasks.CreateInterface
{
    [TestFixture]
    public class InterfacePropertyInterceptorFixture
    {
        private ObjectConstructionArgs _args;


        [SetUp]
        public void Setup()
        {
            Type type = typeof(IStubInterfaceWithProp);
            var service = Substitute.For<IAbstractService>();

            Context context = Context.Create(Substitute.For<IDependencyResolver>());

            var abstractCreationContext = Substitute.For<AbstractTypeCreationContext>();
            abstractCreationContext.RequestedType = typeof(IStubInterfaceWithProp);
            abstractCreationContext.IsLazy = false;

            var configuration = Substitute.For<AbstractTypeConfiguration>();
            configuration.Type = type;

            _args = new ObjectConstructionArgs(context, abstractCreationContext, configuration, service);
        }

        #region Method - cctor & Intercept

        [Test]
        public void Execute_LazyCreationContext_ValuesLoadedLazily()
        {
            //Assign
            _args.AbstractTypeCreationContext.IsLazy = true;
            var interceptor = new TestInterfacePropertyInterceptor(_args);

            var invocation = Substitute.For<IInvocation>();
            invocation.Method.Returns(typeof(IStubInterfaceWithProp).GetProperty("StubProp").GetMethod);

            //Preconditional Assert; ensure that we haven't yet received any calls to the underlying datasource
            Assert.IsFalse(interceptor.ValuesCalled);

            //Act
            interceptor.Intercept(invocation);
            
            //Assert
            Assert.IsTrue(interceptor.ValuesCalled);
        }

        [Test]
        public void Execute_LazyContextDisabled_ValuesLoadedEagerly()
        {
            //Assign
            _args.AbstractTypeCreationContext.IsLazy = false;
            var interceptor = new TestInterfacePropertyInterceptor(_args);

            var invocation = Substitute.For<IInvocation>();
            invocation.Method.Returns(typeof(IStubInterfaceWithProp).GetProperty("StubProp").GetMethod);

            //Preconditional Assert; ensure that we haven't yet received any calls to the underlying datasource
            Assert.IsTrue(interceptor.ValuesCalled);

            //Act
            interceptor.Intercept(invocation);

            //Assert
            Assert.IsTrue(interceptor.ValuesCalled);
        }

        #endregion

        #region Stubs

        public interface IStubInterfaceWithProp
        {
            string StubProp { get; }
        }

        #endregion

        #region Test Helpers
        
        public class TestInterfacePropertyInterceptor : InterfacePropertyInterceptor
        {
            public bool ValuesCalled => LazyValues.IsValueCreated;
            
            public TestInterfacePropertyInterceptor(ObjectConstructionArgs args) : base(args)
            {
            }
        }

        #endregion
    }
}
