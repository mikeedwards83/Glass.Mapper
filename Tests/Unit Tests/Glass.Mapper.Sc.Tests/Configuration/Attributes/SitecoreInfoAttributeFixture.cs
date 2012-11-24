using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Glass.Mapper.Configuration.Attributes;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.Tests.Configuration.Attributes
{
    [TestFixture]
    public class SitecoreInfoAttributeFixture
    {
        [Test]
        public void Does_SitecoreInfoAttribute_Extend_InfoAttribute()
        {
            Assert.IsTrue(typeof(InfoAttribute).IsAssignableFrom(typeof(SitecoreInfoAttribute)));
        }

        [Test]
        [TestCase("Type")]
        [TestCase("UrlOptions")]
        public void Does_ChildrenAttribute_Have_Properties(string propertyName)
        {
            var properties = typeof(SitecoreInfoAttribute).GetProperties();
            Assert.IsTrue(properties.Any(x => x.Name == propertyName));
        }

        [Test]
        public void Does_Constructor_Set_Type_NotSet()
        {
            Assert.AreEqual(new SitecoreInfoAttribute().Type, SitecoreInfoType.NotSet);
        }

        [Test]
        public void Does_Constructor_Set_UrlOptions_Default()
        {
            Assert.AreEqual(new SitecoreInfoAttribute().UrlOptions, SitecoreInfoUrlOptions.Default);
        }


        #region Method - Configure

        [Test]
        public void Configure_ConfigureCalled_SitecoreInfoConfigurationReturned()
        {
            //Assign
            SitecoreInfoAttribute attr = new SitecoreInfoAttribute();
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");


            //Act
            var result = attr.Configure(propertyInfo) as SitecoreInfoConfiguration;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(SitecoreInfoType.NotSet, result.Type);
            Assert.AreEqual(SitecoreInfoUrlOptions.Default, result.UrlOptions);
        }

        [Test]
        public void Configure_TypeSet_TypeSetOnConfiguration()
        {
            //Assign
            SitecoreInfoAttribute attr = new SitecoreInfoAttribute();
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");
            attr.Type = SitecoreInfoType.Language;

            //Act
            var result = attr.Configure(propertyInfo) as SitecoreInfoConfiguration;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(SitecoreInfoType.Language, result.Type);
        }

        [Test]
        public void Configure_UrlOptionsSet_UrlOptionsSetOnConfiguration()
        {
            //Assign
            SitecoreInfoAttribute attr = new SitecoreInfoAttribute();
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");
            attr.UrlOptions = SitecoreInfoUrlOptions.LanguageEmbeddingAsNeeded;

            //Act
            var result = attr.Configure(propertyInfo) as SitecoreInfoConfiguration;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(SitecoreInfoUrlOptions.LanguageEmbeddingAsNeeded, result.UrlOptions);
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
