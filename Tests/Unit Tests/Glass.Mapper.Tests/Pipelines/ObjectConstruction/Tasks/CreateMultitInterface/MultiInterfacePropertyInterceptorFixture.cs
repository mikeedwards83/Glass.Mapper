using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using Glass.Mapper.Configuration;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateMultiInterface;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Pipelines.ObjectConstruction.Tasks.CreateMulitInterface
{
    [TestFixture]
    public  class MultiInterfacePropertyInterceptorFixture
    {
        #region Method - Intercept

        [Test]
        public void Intercept_InterceptsMultiplePropertiesOnDifferentInterfaces_ReturnsExpectedPropertyValues()
        {
            //Arrange   
            var service = Substitute.For<IAbstractService>();
            var config1 = new StubAbstractTypeConfiguration();
            var config2 = new StubAbstractTypeConfiguration();

            var property1 = new StubAbstractPropertyConfiguration();
            var property2 = new StubAbstractPropertyConfiguration();

            var mapper1 = new StubAbstractDataMapper();
            var mapper2 = new StubAbstractDataMapper();

            var expected1 = "test random 1";
            var expected2 = "some other random";

            var propertyName1 = "Property1";
            var propertyName2 = "Property2";


            var info1 = new FakePropertyInfo(typeof(string), propertyName1, typeof(IStubTarget));
            var info2 = new FakePropertyInfo(typeof(string), propertyName2, typeof(IStubTarget2));

            config1.AddProperty(property1);
            config2.AddProperty(property2);

            property1.Mapper = mapper1;
            property2.Mapper = mapper2;
            property1.PropertyInfo = info1;
            property2.PropertyInfo = info2;

            mapper1.Value = expected1;
            mapper2.Value = expected2;

            var args = new ObjectConstructionArgs(null, null, config1, service);
            args.Parameters = new Dictionary<string, object>();
            args.Parameters[CreateMultiInferaceTask.MultiInterfaceConfigsKey] = new[] {config2};
            var interceptor = new MultiInterfacePropertyInterceptor(args);

            var invocation1 = Substitute.For<IInvocation>();
            var invocation2 = Substitute.For<IInvocation>();

            invocation1.Method.Returns(typeof(IStubTarget).GetProperty(propertyName1).GetGetMethod());
            invocation2.Method.Returns(typeof(IStubTarget2).GetProperty(propertyName2).GetGetMethod());

            //Act
            interceptor.Intercept(invocation1);
            interceptor.Intercept(invocation2);

            //Assert
            Assert.AreEqual(expected1, invocation1.ReturnValue);
            Assert.AreEqual(expected2, invocation2.ReturnValue);

        }

        [Test]
        public void Intercept_InterceptsMultiplePropertiesOnDifferentInterfaces_SetsExpectedPropertyValues()
        {
            //Arrange   
            var service = Substitute.For<IAbstractService>();
            var config1 = new StubAbstractTypeConfiguration();
            var config2 = new StubAbstractTypeConfiguration();

            var property1 = new StubAbstractPropertyConfiguration();
            var property2 = new StubAbstractPropertyConfiguration();

            var mapper1 = new StubAbstractDataMapper();
            var mapper2 = new StubAbstractDataMapper();

            var expected1 = "test random 1";
            var expected2 = "some other random";

            var propertyName1 = "Property1";
            var propertyName2 = "Property2";


            var info1 = new FakePropertyInfo(typeof(string), propertyName1, typeof(IStubTarget));
            var info2 = new FakePropertyInfo(typeof(string), propertyName2, typeof(IStubTarget2));

            config1.AddProperty(property1);
            config2.AddProperty(property2);

            property1.Mapper = mapper1;
            property2.Mapper = mapper2;
            property1.PropertyInfo = info1;
            property2.PropertyInfo = info2;

            var args = new ObjectConstructionArgs(null, null,  config1 , service);
            args.Parameters = new Dictionary<string, object>();
            args.Parameters[CreateMultiInferaceTask.MultiInterfaceConfigsKey] = new[] { config2 };
            var interceptor = new MultiInterfacePropertyInterceptor(args);


            var setInvocation1 = Substitute.For<IInvocation>();
            var setInvocation2 = Substitute.For<IInvocation>();

            setInvocation1.Arguments.Returns(new[] { expected1 });
            setInvocation2.Arguments.Returns(new[] { expected2 });

            setInvocation1.Method.Returns(typeof(IStubTarget).GetProperty(propertyName1).GetSetMethod());
            setInvocation2.Method.Returns(typeof(IStubTarget2).GetProperty(propertyName2).GetSetMethod());


            //Act

            interceptor.Intercept(setInvocation1);
            interceptor.Intercept(setInvocation2);

            //Assert

            var getInvocation1 = Substitute.For<IInvocation>();
            var getInvocation2 = Substitute.For<IInvocation>();

            getInvocation1.Method.Returns(typeof(IStubTarget).GetProperty(propertyName1).GetGetMethod());
            getInvocation2.Method.Returns(typeof(IStubTarget2).GetProperty(propertyName2).GetGetMethod());


          
            interceptor.Intercept(getInvocation1);
            interceptor.Intercept(getInvocation2);

           
            Assert.AreEqual(expected1, getInvocation1.ReturnValue);
            Assert.AreEqual(expected2, getInvocation2.ReturnValue);

        }


        #endregion
        #region Stubs

        public class StubAbstractTypeConfiguration : AbstractTypeConfiguration
        {
            
        }
        public class StubAbstractPropertyConfiguration : AbstractPropertyConfiguration
        {
            
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
        public interface IStubTarget2
        {
            string Property2 { get; set; }
        }

        #endregion
    }
}
