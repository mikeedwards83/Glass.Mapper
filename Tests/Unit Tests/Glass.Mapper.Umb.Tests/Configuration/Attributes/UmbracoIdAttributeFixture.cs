using Glass.Mapper.Umb.Configuration;
using Glass.Mapper.Umb.Configuration.Attributes;
using NUnit.Framework;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Umb.Tests.Configuration.Attributes
{
    [TestFixture]
    public abstract class UmbracoIdAttributeFixture
    {
        [Test]
        public void Does_SitecoreIdAttribute_Extend_IdAttribute()
        {
            Assert.IsTrue(typeof(IdAttribute).IsAssignableFrom(typeof(UmbracoIdAttribute)));
        }

       

        #region Method - Configure

        [Test]
        public void Configure_ConfigureCalled_SitecoreIdConfigurationReturned()
        {
            //Assign
            UmbracoIdAttribute attr = new UmbracoIdAttribute();
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");


            //Act
            var result = attr.Configure(propertyInfo) as UmbracoIdConfiguration;

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
