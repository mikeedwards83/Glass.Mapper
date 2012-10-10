using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete;
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
            AbstractItemContext context = Substitute.For<AbstractItemContext>();
            context.Type = typeof (StubClass);

            context.Configuration = Substitute.For<AbstractTypeConfiguration>();
            context.Configuration.ConstructorMethods = Utilities.CreateConstructorDelegates(typeof(StubClass));

            ObjectConstructionArgs args = new ObjectConstructionArgs(context);
            args.IsLazy = true;
         
            LazyObjectInterceptor interceptor = new LazyObjectInterceptor(args);
            IInvocation invocation = Substitute.For<IInvocation>();
            invocation.Method.Returns(typeof (StubClass).GetMethod("CalledMe"));





            //Act
           interceptor.Intercept(invocation);

            //Assert
            Assert.IsTrue(invocation.ReturnValue is bool);
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
