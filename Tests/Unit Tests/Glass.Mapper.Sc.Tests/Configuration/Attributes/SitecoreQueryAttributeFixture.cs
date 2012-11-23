using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.Tests.Configuration.Attributes
{
    [TestFixture]
    public class SitecoreQueryAttributeFixture
    {

        

        #region Method - Configure

        [Test]
        public void Configure_ConfigureCalled_SitecoreQueryConfigurationReturned()
        {
            //Assign
            SitecoreQueryAttribute attr = new SitecoreQueryAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("QueryContextProperty");


            //Act
            var result = attr.Configure(propertyInfo) as SitecoreQueryConfiguration;

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        [Sequential]
        public void Configure_ConfigureQueryContext_QueryContextSetOnConfigObject(
            [Values(true, false)] bool queryContextValue,
            [Values(true, false)] bool expectedValue)
        {
            //Assign
            SitecoreQueryAttribute attr = new SitecoreQueryAttribute(string.Empty);
            var propertyInfo = typeof (StubClass).GetProperty("QueryContextProperty");
            
            attr.UseQueryContext = queryContextValue;
            
            //Act
            var result = attr.Configure(propertyInfo) as SitecoreQueryConfiguration;

            //Assert
            Assert.AreEqual(expectedValue, result.UseQueryContext);
        }

        #endregion

        #region Stubs

        public class StubClass
        {
            public string QueryContextProperty { get; set; }
        }

        #endregion
    }
}
