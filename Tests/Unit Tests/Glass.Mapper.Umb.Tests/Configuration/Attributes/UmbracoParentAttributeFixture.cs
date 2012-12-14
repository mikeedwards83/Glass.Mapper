using System.Linq;
using FluentAssertions;
using Glass.Mapper.Umb.Configuration;
using Glass.Mapper.Umb.Configuration.Attributes;
using NUnit.Framework;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Umb.Tests.Configuration.Attributes
{
    [TestFixture]
    public class UmbracoParentAttributeFixture
    {
        [Test]
        public void Does_UmbracoParentAttribute_Extend_ParentAttribute()
        {
            typeof(ParentAttribute).IsAssignableFrom(typeof(UmbracoParentAttribute)).Should().BeTrue();
        }

        [Test]
        [TestCase("IsLazy")]
        [TestCase("InferType")]
        public void Does_UmbracoNodeAttributee_Have_Properties(string propertyName)
        {
            var properties = typeof(UmbracoParentAttribute).GetProperties();
            properties.Any(x => x.Name == propertyName).Should().BeTrue();
        }

        [Test]
        public void Does_Constructor_Set_IsLazy_True()
        {
            new UmbracoParentAttribute().IsLazy.Should().BeTrue();
        }

        [Test]
        public void Does_Constructor_Set_InferType_False()
        {
            new UmbracoParentAttribute().InferType.Should().BeFalse();
        }

        #region Method - Configure

        [Test]
        public void Configure_ConfigureCalled_UmbracoInfoConfigurationReturned()
        {
            //Assign
            var attr = new UmbracoParentAttribute();
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");


            //Act
            var result = attr.Configure(propertyInfo) as UmbracoParentConfiguration;

            //Assert
            result.Should().NotBeNull();
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
