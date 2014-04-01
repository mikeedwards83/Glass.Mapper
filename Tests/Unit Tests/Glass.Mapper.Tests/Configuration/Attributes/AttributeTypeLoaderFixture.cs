using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Configuration.Attributes;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Configuration.Attributes
{
    [TestFixture]
    public class AttributeTypeLoaderFixture
    {
        private StubAttributeTypeLoader _loader;

         [SetUp]
        public void Setup()
        {
            _loader = new StubAttributeTypeLoader(null);

        }

        #region Method - ProcessProperty

        [Test]
        public void ProcessProperty_PropertyOnClassWithAttribute_ReturnsConfigurationItem()
        {
            //Assign
            var propertyInfo = Utilities.GetProperty(typeof(StubClassWithProperties), "PropertyWithAttribute");

            //Act
            var result = AttributeTypeLoader.ProcessProperty(propertyInfo);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is StubPropertyConfiguration);
            Assert.AreEqual(propertyInfo, result.PropertyInfo);
        }

        [Test]
        public void ProcessProperty_PropertyOnSubClassWithAttribute_ReturnsConfigurationItem()
        {
            //Assign
            var propertyInfo = Utilities.GetProperty(typeof(StubSubClassWithProperties), "PropertyWithAttribute");

            //Act
            var result = AttributeTypeLoader.ProcessProperty(propertyInfo);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is StubPropertyConfiguration);
            Assert.AreEqual(propertyInfo, result.PropertyInfo);
        }

        [Test]
        public void ProcessProperty_PropertyOnInterfaceWithAttribute_ReturnsConfigurationItem()
        {
            //Assign
            var propertyInfo = Utilities.GetProperty(typeof(StubInterfaceWithProperties), "PropertyWithAttribute");

            //Act
            var result = AttributeTypeLoader.ProcessProperty(propertyInfo);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is StubPropertyConfiguration);
            Assert.AreEqual(propertyInfo, result.PropertyInfo);
        }

        [Test]
        public void ProcessProperty_PropertyOnSubInterfaceWithAttribute_ReturnsConfigurationItem()
        {
            //Assign
            var propertyInfo = Utilities.GetProperty(typeof(StubSubInterfaceWithProperties), "PropertyWithAttribute");

            //Act
            var result = AttributeTypeLoader.ProcessProperty(propertyInfo);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is StubPropertyConfiguration);
            Assert.AreEqual(propertyInfo, result.PropertyInfo);
        }

        [Test]
        public void ProcessProperty_PropertyOnClassFromInterfaceWithAttribute_ReturnsConfigurationItem()
        {
            //Assign
            var propertyInfo = Utilities.GetProperty(typeof(StubClassFromInterface), "PropertyWithAttribute");

            //Act
            var result = AttributeTypeLoader.ProcessProperty(propertyInfo);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is StubPropertyConfiguration);


            Assert.AreEqual(propertyInfo, result.PropertyInfo);
        }

        [Test]
        public void ProcessProperty_PropertyOnClassFromTwoInterfacesOneWithAttribute_ReturnsConfigurationItem()
        {
            //Assign
            var propertyInfo = Utilities.GetProperty(typeof(StubClassFromTwoInterfaces), "PropertyWithAttribute");

            //Act
            var result = AttributeTypeLoader.ProcessProperty(propertyInfo);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is StubPropertyConfiguration);


            Assert.AreEqual(propertyInfo, result.PropertyInfo);
        }

        #endregion


        #region Method - GetPropertyAttribute

        [Test]
        public void GetPropertyAttribute_PropertyHasAttribute_ReturnsAttribute()
        {
            //Assign
            PropertyInfo propertyInfo = typeof(StubClassWithProperties).GetProperty("PropertyWithAttribute");

            //Act
            var result = AttributeTypeLoader.GetPropertyAttribute(propertyInfo);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is StubAbstractPropertyAttribute);
        }

        [Test]
        public void GetPropertyAttribute_PropertyHasNoAttribute_ReturnsNull()
        {
            //Assign
            PropertyInfo propertyInfo = typeof(StubClassWithProperties).GetProperty("PropertyWithoutAttribute");

            //Act
            var result = AttributeTypeLoader.GetPropertyAttribute(propertyInfo);

            //Assert
            Assert.IsNull(result);
        }

        #endregion


        #region Method - LoadPropertiesFromType

        [Test]
        public void LoadPropertiesFromType_LoadsTypeWithProperty_ReturnsTypeConfigAndPropertyConfig()
        {
            //Assign
            var type = typeof(StubClassWithProperties);

            //Act
            var result = _loader.LoadPropertiesFromType(type);

            //Assert
            Assert.AreEqual(1, result.Count());

        }

        #endregion

        #region Stubs

        public class StubTypeConfiguration : AbstractTypeConfiguration
        {

        }

        public class StubPropertyConfiguration : AbstractPropertyConfiguration
        {

        }

        public class StubAttributeTypeLoader : AttributeTypeLoader
        {
            public StubAttributeTypeLoader(Type type)
                : base(type)
            {
            }
           
            public IEnumerable<AbstractPropertyConfiguration> LoadPropertiesFromType(Type type)
            {
                return base.LoadPropertiesFromType(type);
            }
        }

        public class StubAbstractTypeAttribute : AbstractTypeAttribute
        {
            public override AbstractTypeConfiguration Configure(Type type)
            {
                var config = new StubTypeConfiguration();
                base.Configure(type, config);
                return config;
            }
        }

        public class StubAbstractPropertyAttribute : AbstractPropertyAttribute
        {

            public override AbstractPropertyConfiguration Configure(PropertyInfo propertyInfo)
            {
                var config = new StubPropertyConfiguration();
                base.Configure(propertyInfo, config);
                return config;
            }
        }

        [StubAbstractTypeAttribute]
        public class StubClassWithTypeAttribute
        {

        }

        [StubAbstractTypeAttribute]
        public class StubClassWithTypeAttributeAndProperties
        {
            [StubAbstractPropertyAttribute]
            public string PropertyWithAttribute1 { get; set; }
            [StubAbstractPropertyAttribute]
            public string PropertyWithAttribute2 { get; set; }
        }

        public class StubClassWithProperties
        {
            [StubAbstractPropertyAttribute]
            public string PropertyWithAttribute { get; set; }

            public string PropertyWithoutAttribute { get; set; }

        }

        public class StubSubClassWithProperties : StubClassWithProperties
        {

        }

        public interface StubInterfaceWithProperties
        {
            [StubAbstractPropertyAttribute]
            string PropertyWithAttribute { get; set; }
        }

        public interface StubSubInterfaceWithProperties : StubInterfaceWithProperties
        {
        }

        public class StubClassFromInterface : StubSubInterfaceWithProperties
        {

            public string PropertyWithAttribute
            {
                get
                {
                    throw new System.NotImplementedException();
                }
                set
                {
                    throw new System.NotImplementedException();
                }
            }
        }

        public class StubClassFromTwoInterfaces : StubInferfaceOne, StubInferfaceTwo
        {
            public string PropertyWithAttribute { get; set; }
        }

        public interface StubInferfaceOne
        {
            [StubAbstractPropertyAttribute]
            string PropertyWithAttribute { get; set; }
        }

        public interface StubInferfaceTwo
        {
            string PropertyWithAttribute { get; set; }
        }



        #endregion
    }
}
