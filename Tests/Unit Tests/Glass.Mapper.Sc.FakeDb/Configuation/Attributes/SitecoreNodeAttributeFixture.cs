using System.Linq;
using Glass.Mapper.Configuration.Attributes;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;

namespace Glass.Mapper.Sc.FakeDb.Configuation.Attributes
{
    [TestFixture]
    public class SitecoreNodeAttributeFixture
    {
        [Test]
        public void Does_SitecoreNodeAttribute_Extend_NodeAttribute()
        {
            Assert.IsTrue(typeof(NodeAttribute).IsAssignableFrom(typeof(SitecoreNodeAttribute)));
        }

        [Test]
        [TestCase("IsLazy")]
        [TestCase("Path")]
        [TestCase("Id")]
        public void Does_SitecoreNodeAttributee_Have_Properties(string propertyName)
        {
            var properties = typeof(SitecoreNodeAttribute).GetProperties();
            Assert.IsTrue(properties.Any(x => x.Name == propertyName));
        }

        


        #region Method - Configure

        [Test]
        public void Configure_ConfigureCalled_SitecoreNodeConfigurationReturned()
        {
            //Assign
            var attr = new SitecoreNodeAttribute();
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");


            //Act
            var result = attr.Configure(propertyInfo) as SitecoreNodeConfiguration;

            //Assert
            Assert.IsNotNull(result);
            
        }


        #endregion

        #region Stubs

        public class StubClass
        {
            public string DummyProperty { get; set; }
        }

        #endregion
    }
}




