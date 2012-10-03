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
    }
}
