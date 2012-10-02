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
    public class SitecoreFieldAttributeFixture
    {
        [Test]
        public void Does_SitecoreFieldAttribute_Extend_FieldAttribute()
        {
            Assert.IsTrue(typeof(FieldAttribute).IsAssignableFrom(typeof(SitecoreFieldAttribute)));
        }


        [Test]
        [TestCase("FieldName")]
        [TestCase("Setting")]
        [TestCase("ReadOnly")]
        public void Does_SitecoreFieldAttribute_Have_Properties(string propertyName)
        {
            var properties = typeof(SitecoreFieldAttribute).GetProperties();
            Assert.IsTrue(properties.Any(x => x.Name == propertyName));
        }

        [Test]
        public void Default_Constructor_Set_Setting_To_Default()
        {
            var testSitecoreFieldAttribute = new SitecoreFieldAttribute();
            Assert.AreEqual(testSitecoreFieldAttribute.Setting, SitecoreFieldSettings.Default);
        }

        [Test]
        public void Constructor_Sets_FieldName()
        {
            var testFieldName = "testFieldName";
            var testSitecoreFieldAttribute = new SitecoreFieldAttribute(testFieldName);
            Assert.AreEqual(testSitecoreFieldAttribute.FieldName, testFieldName);
        }
    }
}
