/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Glass.Mapper.Configuration;
using Glass.Mapper.Configuration.Attributes;
using NSubstitute;
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

        #region Method - Create

        [Test]
        public void Load_LoadsTypesUsingAssemblyNameWithDllAtEnd_TypeReturnedWithTwoProperties()
        {

            //Assign
            string assemblyName = "Glass.Mapper.Tests.dll";

            _loader = new StubAttributeConfigurationLoader<StubTypeConfiguration, StubPropertyConfiguration>(assemblyName);

            //Act
            var results = _loader.Load();

            //Assert
            Assert.IsTrue(results.Any());
            Assert.AreEqual(1, results.Count(x => x.Type == typeof(StubClassWithTypeAttributeAndProperties)));
            var config = results.First(x => x.Type == typeof(StubClassWithTypeAttributeAndProperties));
            Assert.AreEqual(2, config.Properties.Count());
        }

        [Test]
        public void Load_LoadsTypesUsingAssemblyNameWithoutDllAtEnd_TypeReturnedWithTwoProperties()
        {

            //Assign
            string assemblyName = "Glass.Mapper.Tests";

            _loader = new StubAttributeConfigurationLoader<StubTypeConfiguration, StubPropertyConfiguration>(assemblyName);

            //Act
            var results = _loader.Load();

            //Assert
            Assert.IsTrue(results.Any());
            Assert.AreEqual(1, results.Count(x => x.Type == typeof(StubClassWithTypeAttributeAndProperties)));
            var config = results.First(x => x.Type == typeof(StubClassWithTypeAttributeAndProperties));
            Assert.AreEqual(2, config.Properties.Count());
        }


        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void Load_AssemblyDoesntExist_ThrowsException()
        {
            //Assign
            string assemblyName = "DoesNotExist";

            _loader = new StubAttributeConfigurationLoader<StubTypeConfiguration, StubPropertyConfiguration>(assemblyName);

            //Act
            var results = _loader.Load();

            //Exception
        }

        #endregion

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
            Assert.AreEqual(1, results.Count(x=>x.Type == typeof(StubClassWithTypeAttribute)));

        }

        [Test]
        public void LoadFromAssembly_TypesDefinedInThisAssemblyWithTwoProperties_LoadsAtLeastOneTypeWithTwoProperties()
        {
            //Assign
            var assembly = Assembly.GetExecutingAssembly();

            //Act
            var results = _loader.LoadFromAssembly(assembly);

            //Assert
            Assert.IsTrue(results.Any());
            Assert.AreEqual(1, results.Count(x => x.Type == typeof(StubClassWithTypeAttributeAndProperties)));
            var config = results.First(x => x.Type == typeof (StubClassWithTypeAttributeAndProperties));
            Assert.AreEqual(2, config.Properties.Count());

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

        #region Method - LoadPropertiesFromType

        [Test]
        public void LoadPropertiesFromType_LoadsTypeWithProperty_ReturnsTypeConfigAndPropertyConfig()
        {
            //Assign
            var type = typeof (StubClassWithProperties);

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

        public class StubAttributeConfigurationLoader<T,K> : AttributeConfigurationLoader<T, K>
            where T : AbstractTypeConfiguration, new() 
            where K : AbstractPropertyConfiguration, new ()
        
        {

            public StubAttributeConfigurationLoader(params string [] assemblies):base(assemblies)
            {
            }

            public IEnumerable<T> LoadFromAssembly(Assembly assembly)
            {
                return base.LoadFromAssembly(assembly);
            }
            public AbstractPropertyConfiguration ProcessProperty(PropertyInfo property)
            {
                return base.ProcessProperty(property);
            }

            public IEnumerable<AbstractPropertyConfiguration> LoadPropertiesFromType(Type type)
            {
                return base.LoadPropertiesFromType(type);
            }
        }

        public class StubAbstractTypeAttribute : AbstractTypeAttribute
        {
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

        [StubAbstractType]
        public class StubClassWithTypeAttribute
        {

        }

        [StubAbstractType]
        public class StubClassWithTypeAttributeAndProperties
        {
            [StubAbstractProperty]
            public string PropertyWithAttribute1 { get; set; }
            [StubAbstractProperty]
            public string PropertyWithAttribute2 { get; set; }
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



