using Castle.DynamicProxy;
using Glass.Mapper.Configuration;
using Glass.Mapper.Diagnostics;
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
            var typeContext = Substitute.For<AbstractTypeCreationContext>();
            var options = new GetOptions();
            typeContext.Options = options;

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

            var interceptor = new LazyObjectInterceptor( args, new LazyLoadingHelper());

            //Act
            interceptor.Intercept(invocation);

            //Assert
            invocation.Received(1).Proceed();



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




