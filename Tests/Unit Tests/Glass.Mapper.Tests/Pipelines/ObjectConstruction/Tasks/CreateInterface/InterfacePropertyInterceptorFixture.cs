using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.DynamicProxy;
using Glass.Mapper.Configuration;
using Glass.Mapper.Diagnostics;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface;
using NSubstitute;

namespace Glass.Mapper.Tests.Pipelines.ObjectConstruction.Tasks.CreateInterface
{
    [TestFixture]
    public class InterfacePropertyInterceptorFixture
    {
        #region Method - Intercept

        [Test]
        public void Intercept_WithMultiplePropertiesAndLazyLoading_ReturnsExpectedPropertyValues()
        {
            //Arrange   
            var service = Substitute.For<IAbstractService>();
            var config = new StubAbstractTypeConfiguration();
            var context = Substitute.For<AbstractTypeCreationContext>();

            config.Type = typeof(IStubTarget);
            context.IsLazy = true;

            var args = new ObjectConstructionArgs(null, context, config, service, new ModelCounter())
            {
                Parameters = new Dictionary<string, object>()
            };
            var interceptor = new InterfacePropertyInterceptor(args);
            var invocations = new List<IInvocation>();

            for (var i = 0; i < 100; i++)
            {
                invocations.Add(CreateInterceptedProperty(config, i.ToString()));
            }

            //Act
            foreach (var invocation in invocations)
            {
                interceptor.Intercept(invocation);

                // Assert
                Assert.AreEqual(invocation.Method.Name.Substring(4), invocation.ReturnValue);
            }
        }

        [Test]
        public void Intercept_InterceptsProperties_ReturnsExpectedPropertyValue()
        {
            //Arrange   
            var service = Substitute.For<IAbstractService>();
            var config = new StubAbstractTypeConfiguration();
            var context = Substitute.For<AbstractTypeCreationContext>();

            var property = new StubAbstractPropertyConfiguration();
            var mapper = new StubAbstractDataMapper();

            var expected = "test random 1";
            var propertyName = "Property1";

            var info = new FakePropertyInfo(typeof(string), propertyName, typeof(IStubTarget));

            config.AddProperty(property);
            config.Type = typeof(IStubTarget);

            property.Mapper = mapper;
            property.PropertyInfo = info;

            mapper.Value = expected;

            var args = new ObjectConstructionArgs(null, context, config, service, new ModelCounter())
            {
                Parameters = new Dictionary<string, object>()
            };
            var interceptor = new InterfacePropertyInterceptor(args);

            var invocation = Substitute.For<IInvocation>();

            invocation.Method.Returns(typeof(IStubTarget).GetProperty(propertyName).GetGetMethod());

            //Act
            interceptor.Intercept(invocation);

            //Assert
            Assert.AreEqual(expected, invocation.ReturnValue);
        }

        [Test]
        public void Intercept_InterceptsProperties_SetsExpectedPropertyValue()
        {
            //Arrange   
            var service = Substitute.For<IAbstractService>();
            var config = new StubAbstractTypeConfiguration();
            var context = Substitute.For<AbstractTypeCreationContext>();

            var property = new StubAbstractPropertyConfiguration();
            var mapper = new StubAbstractDataMapper();

            var expected = "test random 1";
            var propertyName = "Property1";

            var info1 = new FakePropertyInfo(typeof(string), propertyName, typeof(IStubTarget));

            config.AddProperty(property);
            config.Type = typeof(IStubTarget);

            property.Mapper = mapper;
            property.PropertyInfo = info1;

            var args = new ObjectConstructionArgs(null, context, config, service, new ModelCounter())
            {
                Parameters = new Dictionary<string, object>()
            };
            var interceptor = new InterfacePropertyInterceptor(args);

            var setInvocation = Substitute.For<IInvocation>();
            setInvocation.Arguments.Returns(new[] { expected });
            setInvocation.Method.Returns(typeof(IStubTarget).GetProperty(propertyName).GetSetMethod());

            //Act
            interceptor.Intercept(setInvocation);

            //Assert
            var getInvocation = Substitute.For<IInvocation>();

            getInvocation.Method.Returns(typeof(IStubTarget).GetProperty(propertyName).GetGetMethod());

            interceptor.Intercept(getInvocation);

            Assert.AreEqual(expected, getInvocation.ReturnValue);
        }


        #endregion

        #region Stubs

        public class StubAbstractTypeConfiguration : AbstractTypeConfiguration
        {

        }

        public class StubAbstractPropertyConfiguration : AbstractPropertyConfiguration
        {
            protected override AbstractPropertyConfiguration CreateCopy()
            {
                throw new NotImplementedException();
            }
        }

        public class StubAbstractDataMapper : AbstractDataMapper
        {
            public string Value { get; set; }
            public override void MapToCms(AbstractDataMappingContext mappingContext)
            {
                throw new NotImplementedException();
            }

            public override object MapToProperty(AbstractDataMappingContext mappingContext)
            {
                return Value;
            }

            public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
            {
                throw new NotImplementedException();
            }
        }

        public interface IStubTarget
        {
            string Property1 { get; set; }
        }

        #endregion

        #region Test Helpers


        private static IInvocation CreateInterceptedProperty(StubAbstractTypeConfiguration config, string propertyName)
        {
            var invocation = Substitute.For<IInvocation>();

            var property = new StubAbstractPropertyConfiguration();
            var mapper = new StubAbstractDataMapper();

            var getter = Substitute.For<MethodInfo>();
            getter.Attributes.Returns(MethodAttributes.SpecialName);
            getter.Name.Returns("get_" + propertyName);
            getter.ReturnType.Returns(typeof(string));

            var returnValue = propertyName;
            var info = Substitute.For<FakePropertyInfo>(typeof(string), propertyName, typeof(IStubTarget));
            info.CanWrite.Returns(false);
            info.DeclaringType.Returns(typeof(IStubTarget));
            info.Name.Returns(propertyName);

            property.Mapper = mapper;
            property.PropertyInfo = info;

            config.AddProperty(property);

            mapper.Value = returnValue;

            invocation.Method.Returns(getter);

            return invocation;
        }

        #endregion

    }
}
