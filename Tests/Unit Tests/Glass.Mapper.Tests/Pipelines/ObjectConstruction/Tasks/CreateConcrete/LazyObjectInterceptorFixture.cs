using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface;
using Glass.Mapper.Pipelines.TypeResolver.Tasks.StandardResolver;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Pipelines.ObjectConstruction.Tasks.CreateConcrete
{
    [TestFixture]
    public class LazyObjectInterceptorFixture
    {
        #region Method - Intercept

        [Test]
        public void Intercept_CreatesObjectLazily_CallsInvokeMethod()
        {
            //Assign
            var typeContext = Substitute.For<AbstractTypeCreationContext>();
            var config = Substitute.For<AbstractTypeConfiguration>();
            var service = Substitute.For<IAbstractService>();

            var args = new ObjectConstructionArgs(
                null,
                typeContext, 
                config,
                service
                );

            var invocation = Substitute.For<IInvocation>();
            invocation.Method.Returns(typeof (StubClass).GetMethod("CalledMe"));
            service.InstantiateObject(typeContext).Returns(new StubClass());

            var interceptor = new LazyObjectInterceptor(args);

            //Act
            interceptor.Intercept(invocation);

            //Assert
            Assert.IsTrue((bool)invocation.ReturnValue);


        }

        #endregion

        #region Stubs

        public class StubClass
        {

            public bool CalledMe()
            {
                return true;
            }

        }

        #endregion
    }
}
