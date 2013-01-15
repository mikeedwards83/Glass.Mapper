using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Glass.Mapper.Tests
{
    [TestFixture]
    public class UtilitiesFixture
    {

        #region Method - CreateConstructorDelegates

        [Test]
        public void CreateConstructorDelegates_NoParameters_CreatesSingleConstructor()
        {
            //Assign
            Type type = typeof (StubNoParameters);

            //Act
            var result = Utilities.CreateConstructorDelegates(type);

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(0, result.First().Key.GetParameters().Length);
        }

        [Test]
        public void CreateConstructorDelegates_OneParameters_CreatesSingleConstructor()
        {
            //Assign
            Type type = typeof(StubOneParameter);

            //Act
            var result = Utilities.CreateConstructorDelegates(type);

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result.First().Key.GetParameters().Length);
        }

        [Test]
        public void CreateConstructorDelegates_OneParametersInvoked_CreatesSingleConstructor()
        {
            //Assign
            Type type = typeof(StubOneParameter);
            var param1 = "hello world";
            //Act
            var result = Utilities.CreateConstructorDelegates(type);

            var obj = result.First().Value.DynamicInvoke(param1) as StubOneParameter;

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result.First().Key.GetParameters().Length);
            Assert.AreEqual(param1, obj.Param1);
            
        }

        [Test]
        public void CreateConstructorDelegates_TwoParameters_CreatesSingleConstructor()
        {
            //Assign
            Type type = typeof(StubTwoParameters);

            //Act
            var result = Utilities.CreateConstructorDelegates(type);

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(2, result.First().Key.GetParameters().Length);
        }

        [Test]
        public void CreateConstructorDelegates_TwoParametersInvoke_CreatesSingleConstructor()
        {
            //Assign
            Type type = typeof(StubTwoParameters);
            var param1 = "hello world";
            var param2 = 456;
            //Act
            var result = Utilities.CreateConstructorDelegates(type);
            var obj = result.First().Value.DynamicInvoke(param1, param2) as StubTwoParameters;

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(2, result.First().Key.GetParameters().Length);
            Assert.AreEqual(param1, obj.Param1);
            Assert.AreEqual(param2, obj.Param2);
        }

        [Test]
        public void CreateConstructorDelegates_ThreeParameters_CreatesSingleConstructor()
        {
            //Assign
            Type type = typeof(StubThreeParameters);

            //Act
            var result = Utilities.CreateConstructorDelegates(type);

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(3, result.First().Key.GetParameters().Length);
        }

        [Test]
        public void CreateConstructorDelegates_FourParameters_CreatesSingleConstructor()
        {
            //Assign
            Type type = typeof(StubFourParameters);

            //Act
            var result = Utilities.CreateConstructorDelegates(type);

            //Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(4, result.First().Key.GetParameters().Length);
        }

        [Test]
        [ExpectedException(typeof(MapperException))]
        public void CreateConstructorDelegates_FiveParameters_CreatesSingleConstructor()
        {
            //Assign
            Type type = typeof(StubFiveParameters);

            //Act
            var result = Utilities.CreateConstructorDelegates(type);

            //Assert
        }


        #endregion

        #region Method - GetProperty

        [Test]
        public void GetProperty_GetPropertyOnClass_ReturnsProperty()
        {
            //Assign
            string name = "Property";
            Type type = typeof (StubClass);

            //Act
            var result = Utilities.GetProperty(type, name);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(name, result.Name);
        }

        [Test]
        public void GetProperty_GetPropertyOnSubClass_ReturnsProperty()
        {
            //Assign
            string name = "Property";
            Type type = typeof(StubSubClass);

            //Act
            var result = Utilities.GetProperty(type, name);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(name, result.Name);
        }

        [Test]
        public void GetProperty_GetPropertyOnInterface_ReturnsProperty()
        {
            //Assign
            string name = "Property";
            Type type = typeof (StubInterface);

            //Act
            var result = Utilities.GetProperty(type, name);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(name, result.Name);
        }

        [Test]
        public void GetProperty_GetPropertyOnSubInterface_ReturnsProperty()
        {
            //Assign
            string name = "Property";
            Type type = typeof(StubSubInterface);

            //Act
            var result = Utilities.GetProperty(type, name);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(name, result.Name);
        }

        #endregion

        #region Stubs


        public class StubNoParameters
        {
            public StubNoParameters()
            {

            }
        }

        public class StubOneParameter
        {
            public string Param1 { get; set; }

            public StubOneParameter(string param1)
            {
                Param1 = param1;
            }
        }

        public class StubTwoParameters
        {
            public string Param1 { get; set; }
            public int Param2 { get; set; }

            public StubTwoParameters(string param1, int param2)
            {
                Param1 = param1;
                Param2 = param2;
            }
        }

        public class StubThreeParameters
        {
            public StubThreeParameters(string param1, string param2, string param3)
            {

            }
        }

        public class StubFourParameters
        {
            public StubFourParameters(string param1, string param2, string param3, string param4)
            {

            }
        }

        public class StubFiveParameters
        {
            public StubFiveParameters(string param1, string param2, string param3, string param4, string param5)
            {

            }
        }

        public class StubClass
        {
            public string Property { get; set; }
        }

        public class StubSubClass : StubClass
        {
        }

        public interface StubInterface
        {
            string Property { get; set; }
        }

        public interface StubSubInterface : StubInterface
        {
        }

        #endregion

    }
}
