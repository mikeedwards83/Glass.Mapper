using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using NUnit.Framework;
using Glass.Mapper.Configuration.Attributes;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace Glass.Mapper.Sc.Tests.Configuration.Attributes
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
        [TestCase("IsLazy")]
        [TestCase("InferType")]
        public void Does_SitecoreChildrenAttribute_Have_Properties(string propertyName)
        {
            var properties = typeof(SitecoreChildrenAttribute).GetProperties();
            Assert.IsTrue(properties.Any(x => x.Name == propertyName));
        }

        [Test]
        public void Does_Default_Constructor_Set_IsLazy_True()
        {
            Assert.IsTrue(new TestSitecoreChildrenAttribute().IsLazy);
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
