using FluentAssertions;
using Glass.Mapper.Umb.Configuration;
using Glass.Mapper.Umb.Configuration.Attributes;
using NUnit.Framework;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Umb.Tests.Configuration.Attributes
{
    [TestFixture]
    public class UmbracoIdAttributeFixture
    {
        [Test]
        public void Does_UmbracoIdAttribute_Extend_IdAttribute()
        {
            typeof(IdAttribute).IsAssignableFrom(typeof(UmbracoIdAttribute)).Should().BeTrue();
        }

       

        #region Method - Configure

        [Test]
        public void Configure_ConfigureCalled_UmbracoIdConfigurationReturned()
        {
            //Assign
            UmbracoIdAttribute attr = new UmbracoIdAttribute();
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");


            //Act
            var result = attr.Configure(propertyInfo) as UmbracoIdConfiguration;

            //Assert
            result.Should().NotBeNull();
        }

        #endregion

        #region Stubs

        public class StubClass
        {
            public int DummyProperty { get; set; }
        }

        #endregion
    }
}
