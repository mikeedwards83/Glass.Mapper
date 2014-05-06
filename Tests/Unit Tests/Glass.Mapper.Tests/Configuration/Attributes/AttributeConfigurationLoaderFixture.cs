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
        private StubAttributeConfigurationLoader _loader;


        [SetUp]
        public void Setup()
        {
            _loader = new StubAttributeConfigurationLoader();

        }

        #region Method - Create

        [Test]
        public void Load_LoadsTypesUsingAssemblyNameWithDllAtEnd_TypeReturnedWithTwoProperties()
        {

            //Assign
            string assemblyName = "Glass.Mapper.Tests.dll";

            _loader = new StubAttributeConfigurationLoader(assemblyName);

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

            _loader = new StubAttributeConfigurationLoader(assemblyName);

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

            _loader = new StubAttributeConfigurationLoader(assemblyName);

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

      
        


       



        #region Stubs

        public class StubTypeConfiguration : AbstractTypeConfiguration
        {
           
        }

        public class StubPropertyConfiguration : AbstractPropertyConfiguration
        {
           
        }

        public class StubAttributeConfigurationLoader : AttributeConfigurationLoader
        {

            public StubAttributeConfigurationLoader(params string [] assemblies):base(assemblies)
            {
            }

            public IEnumerable<AbstractTypeConfiguration> LoadFromAssembly(Assembly assembly)
            {
                return base.LoadFromAssembly(assembly);
            }
        }

        public class StubAbstractTypeAttribute : AbstractTypeAttribute
        {
            public override AbstractTypeConfiguration Configure(Type type)
            {
                var config =  new StubTypeConfiguration();
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




