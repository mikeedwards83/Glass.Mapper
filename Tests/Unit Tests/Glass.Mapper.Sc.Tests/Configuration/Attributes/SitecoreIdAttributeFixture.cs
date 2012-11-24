using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using NUnit.Framework;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Configuration.Attributes;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Tests.Configuration.Attributes
{
    [TestFixture]
    public abstract class SitecoreIdAttributeFixture
    {
        [Test]
        public void Does_SitecoreIdAttribute_Extend_IdAttribute()
        {
            Assert.IsTrue(typeof(IdAttribute).IsAssignableFrom(typeof(SitecoreIdAttribute)));
        }

        [Test]
        public void SitecoreIdAttribute_Is_ID_Type()
        {
            var sitecoreIdAttribute = new SitecoreIdAttribute();
            Assert.AreEqual(sitecoreIdAttribute.Type, typeof(ID));
        }

        #region Method - Configure

        [Test]
        public void Configure_ConfigureCalled_SitecoreIdConfigurationReturned()
        {
            //Assign
            SitecoreIdAttribute attr = new SitecoreIdAttribute();
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");


            //Act
            var result = attr.Configure(propertyInfo) as SitecoreIdConfiguration;

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
