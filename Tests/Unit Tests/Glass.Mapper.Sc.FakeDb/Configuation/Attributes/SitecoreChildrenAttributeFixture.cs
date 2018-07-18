
using System.Linq;
using Glass.Mapper.Configuration.Attributes;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;

namespace Glass.Mapper.Sc.FakeDb.Configuation.Attributes
{
    [TestFixture]
    public class SitecoreChildrenAttributeFixture
    {
        [Test]
        public void Does_SitecoreChildrenAttribute_Extend_ChildrenAttribute()
        {
            Assert.IsTrue(typeof(ChildrenAttribute).IsAssignableFrom(typeof(SitecoreChildrenAttribute)));
        }

        [Test]
        [TestCase("InferType")]
        public void Does_SitecoreChildrenAttribute_Have_Properties(string propertyName)
        {
            var properties = typeof(SitecoreChildrenAttribute).GetProperties();
            Assert.IsTrue(properties.Any(x => x.Name == propertyName));
        }

        

        #region Method - Configure

        [Test]
        public void Configure_ConfigureCalled_SitecoreQueryConfigurationReturned()
        {
            //Assign
            SitecoreChildrenAttribute attr = new SitecoreChildrenAttribute();
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");


            //Act
            var result = attr.Configure(propertyInfo) as SitecoreChildrenConfiguration;

            //Assert
            Assert.IsNotNull(result);
        }

        #endregion

        #region Stubs

        public class StubClass
        {
            public string DummyProperty { get; set; }
        }

        private class TestSitecoreChildrenAttribute : SitecoreChildrenAttribute
        {
        }

        #endregion
    }
}




