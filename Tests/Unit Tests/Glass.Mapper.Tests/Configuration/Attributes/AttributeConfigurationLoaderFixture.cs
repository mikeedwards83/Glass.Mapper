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
            Assert.AreEqual(1, results.Count(x=>x.Type == typeof(DummyClass)));

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
        }

        public class StubAbstractTypeAttribute : AbstractTypeAttribute
        {
        }

        [StubAbstractType]
        public class DummyClass
        {

        }

        #endregion
    }
}
