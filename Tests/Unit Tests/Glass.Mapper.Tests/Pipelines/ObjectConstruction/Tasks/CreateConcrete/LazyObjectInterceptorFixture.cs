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
            Assert.Fail("Need to rewrite");
            //Assign
            //Type type = typeof (StubClass);

            //Context context = Context.Create();


            //context.ObjectConstructionTasks.Add(new CreateConcreteTask());
            //context.ObjectConstructionTasks.Add(new CreateInterfaceTask());
            //context.TypeResolverTasks.Add(new TypeStandardResolverTask());

            //ITypeContext typeContext = Substitute.For<ITypeContext>();
            //typeContext.RequestedType.Returns(type);
            //typeContext.IsLazy = true;

            //var configuration = Substitute.For<AbstractTypeConfiguration>();
            //configuration.ConstructorMethods = Utilities.CreateConstructorDelegates(type);
            //configuration.Type = type;

            //var configurationResolver = Substitute.For<IConfigurationResolverTask>();
            //configurationResolver
            //    .When(x=>x.Execute(Arg.Any<ConfigurationResolverArgs>()))
            //    .Do(info=>
            //            {
            //                var paras = info.Args();
            //                var resolverArgs = paras[0] as ConfigurationResolverArgs;
            //                resolverArgs.Result = configuration;
            //            });

            //context.ConfigurationResolverTasks.Add(configurationResolver);

            //ObjectConstructionArgs args = new ObjectConstructionArgs(context, typeContext, configuration);
         
            //LazyObjectInterceptor interceptor = new LazyObjectInterceptor(args);
            //IInvocation invocation = Substitute.For<IInvocation>();
            //invocation.Method.Returns(typeof (StubClass).GetMethod("CalledMe"));

            ////Act
            //interceptor.Intercept(invocation);

            ////Assert
            //Assert.IsTrue(invocation.ReturnValue is bool);
            //Assert.IsTrue((bool)invocation.ReturnValue);

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
