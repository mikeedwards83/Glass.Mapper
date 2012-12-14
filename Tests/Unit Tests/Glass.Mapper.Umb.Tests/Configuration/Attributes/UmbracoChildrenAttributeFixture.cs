using System.Linq;
using FluentAssertions;
using Glass.Mapper.Umb.Configuration;
using Glass.Mapper.Umb.Configuration.Attributes;
using NUnit.Framework;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Umb.Tests.Configuration.Attributes
{
    [TestFixture]
    public class UmbracoChildrenAttributeFixture
    {
        [Test]
        public void Does_UmbracoChildrenAttribute_Extend_ChildrenAttribute()
        {
            typeof(ChildrenAttribute).IsAssignableFrom(typeof(UmbracoChildrenAttribute)).Should().BeTrue();
        }

        [Test]
        [TestCase("IsLazy")]
        [TestCase("InferType")]
        public void Does_UmbracoChildrenAttribute_Have_Properties(string propertyName)
        {
            var properties = typeof(UmbracoChildrenAttribute).GetProperties();
            properties.Any(x => x.Name == propertyName).Should().BeTrue();
        }

        [Test]
        public void Does_Default_Constructor_Set_IsLazy_True()
        {
            new TestUmbracoChildrenAttribute().IsLazy.Should().BeTrue();
        }

        #region Method - Configure

        [Test]
        public void Configure_ConfigureCalled_UmbracoQueryConfigurationReturned()
        {
            //Assign
            var attr = new UmbracoChildrenAttribute();
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");


            //Act
            var result = attr.Configure(propertyInfo) as UmbracoChildrenConfiguration;

            //Assert
            result.Should().NotBeNull();
        }

        #endregion

        #region Stubs

        public class StubClass
        {
            public string DummyProperty { get; set; }
        }

        private class TestUmbracoChildrenAttribute : UmbracoChildrenAttribute
        {
        }

        #endregion
    }
}
