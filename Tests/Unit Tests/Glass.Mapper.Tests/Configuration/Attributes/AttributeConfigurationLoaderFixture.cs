using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Glass.Mapper.Configuration;
using Glass.Mapper.Configuration.Attributes;
using NUnit.Framework;

namespace Glass.Mapper.Tests.Configuration.Attributes
{
    [TestFixture]
    public class AttributeConfigurationLoaderFixture
    {
        private StubAttributeConfigurationLoader<StubTypeConfiguration, StubPropertyConfiguration> _loader;


        [SetUp]
        public void Setup()
        {
            _loader = new StubAttributeConfigurationLoader<StubTypeConfiguration, StubPropertyConfiguration>();

        }

        #region Method - LoadFromAssembly

        [Test]
        public void LoadFromAssembly_TypesDefinedInThisAssembly_LoadsAtLeastOneType()
        {
            //Assign
            var assembly = Assembly.GetExecutingAssembly();

            //Act
            var results = _loader.LoadFromAssembly(assembly);

            //Assert
            Assert.IsTrue(results.Any());
            Assert.AreEqual(1, results.Count(x=>x.Type == typeof(StubClassWithClassAttribute)));

        }

        #endregion

        #region Method - GetPropertyAttribute

        [Test]
        public void GetPropertyAttribute_PropertyHasAttribute_ReturnsAttribute()
        {
            //Assign
            PropertyInfo propertyInfo = typeof (StubClassWithProperties).GetProperty("PropertyWithAttribute");

            //Act
            var result = AttributeConfigurationLoader<StubTypeConfiguration,StubPropertyConfiguration>.GetPropertyAttribute(propertyInfo);

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
            var result = AttributeConfigurationLoader<StubTypeConfiguration, StubPropertyConfiguration>.GetPropertyAttribute(propertyInfo);

            //Assert
            Assert.IsNull(result);
        }
        

        #endregion

        #region Method - ProcessProperty

        [Test]
        public void ProcessProperty_PropertyOnClassWithAttribute_ReturnsConfigurationItem()
        {
            //Assign
            var propertyInfo = Utilities.GetProperty(typeof (StubClassWithProperties),"PropertyWithAttribute");

            //Act
            var result = _loader.ProcessProperty(propertyInfo);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is StubPropertyConfiguration);
            Assert.AreEqual(propertyInfo, result.PropertyInfo);
        }

        [Test]
        public void ProcessProperty_PropertyOnSubClassWithAttribute_ReturnsConfigurationItem()
        {
            //Assign
            var propertyInfo = Utilities.GetProperty(typeof(StubSubClassWithProperties),"PropertyWithAttribute");

            //Act
            var result = _loader.ProcessProperty(propertyInfo);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is StubPropertyConfiguration);
            Assert.AreEqual(propertyInfo, result.PropertyInfo);
        }

        [Test]
        public void ProcessProperty_PropertyOnInterfaceWithAttribute_ReturnsConfigurationItem()
        {
            //Assign
            var propertyInfo = Utilities.GetProperty(typeof(StubInterfaceWithProperties),"PropertyWithAttribute");

            //Act
            var result = _loader.ProcessProperty(propertyInfo);

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
            var result = _loader.ProcessProperty(propertyInfo);

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
            var result = _loader.ProcessProperty(propertyInfo);

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
            var result = _loader.ProcessProperty(propertyInfo);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is StubPropertyConfiguration);


            Assert.AreEqual(propertyInfo, result.PropertyInfo);
        }

        #endregion

        #region Stubs

        public class StubTypeConfiguration : AbstractTypeConfiguration
        {
           
        }

        public class StubPropertyConfiguration : AbstractPropertyConfiguration
        {
           
        }

        public class StubAttributeConfigurationLoader<T,K> : AttributeConfigurationLoader<T, K>
            where T : AbstractTypeConfiguration, new() 
            where K : AbstractPropertyConfiguration, new ()
        
        {
            public IEnumerable<T> LoadFromAssembly(Assembly assembly)
            {
                return base.LoadFromAssembly(assembly);
            }
            public AbstractPropertyConfiguration ProcessProperty(PropertyInfo property)
            {
                return base.ProcessProperty(property);
            }
        }

        public class StubAbstractTypeAttribute : AbstractTypeAttribute
        {
        }

        public class StubAbstractPropertyAttribute : AbstractPropertyAttribute
        {

        }

        [StubAbstractType]
        public class StubClassWithClassAttribute
        {

        }

        public class StubClassWithProperties
        {
            [StubAbstractProperty]
            public string PropertyWithAttribute { get; set; }

            public string PropertyWithoutAttribute { get; set; }

        }

        public class StubSubClassWithProperties: StubClassWithProperties
        {

        }

        public interface StubInterfaceWithProperties
        {
            [StubAbstractProperty]
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
            [StubAbstractProperty]
            string PropertyWithAttribute { get; set; }
        }

        public interface StubInferfaceTwo
        {
            string PropertyWithAttribute { get; set; }
        }


        #endregion
    }
}
