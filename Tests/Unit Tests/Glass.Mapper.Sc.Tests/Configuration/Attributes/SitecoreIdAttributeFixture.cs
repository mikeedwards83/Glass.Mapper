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
    public class SitecoreIdAttributeFixture
    {
        [Test]
        public void Does_SitecoreIdAttribute_Extend_IdAttribute()
        {
            Assert.IsTrue(typeof(IdAttribute).IsAssignableFrom(typeof(SitecoreIdAttribute)));
        }



        #region Method - Configure

        [Test]
        public void Configure_ConfigureCalled_WithID_SitecoreIdConfigurationReturned()
        {
            //Assign
            SitecoreIdAttribute attr = new SitecoreIdAttribute();
            var propertyInfo = typeof(StubClass).GetProperty("DummyPropertyID");


            //Act
            var result = attr.Configure(propertyInfo) as SitecoreIdConfiguration;

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Configure_ConfigureCalled_WithGuid_SitecoreIdConfigurationReturned()
        {
            //Assign
            SitecoreIdAttribute attr = new SitecoreIdAttribute();
            var propertyInfo = typeof(StubClass).GetProperty("DummyPropertyGuid");


            //Act
            var result = attr.Configure(propertyInfo) as SitecoreIdConfiguration;

            //Assert
            Assert.IsNotNull(result);
        }

        #endregion

        #region Stubs

        public class StubClass
        {
            public ID DummyPropertyID { get; set; }
            public Guid DummyPropertyGuid { get; set; }
        }

        #endregion
    }
}
