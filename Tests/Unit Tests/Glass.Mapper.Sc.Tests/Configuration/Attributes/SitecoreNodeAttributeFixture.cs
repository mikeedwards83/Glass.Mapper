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

        [Test]
        public void Does_Constructor_Set_IsLazy_True()
        {
            Assert.IsTrue(new SitecoreNodeAttribute().IsLazy);
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
