using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Glass.Mapper.Configuration.Attributes;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace Glass.Mapper.Sc.Tests.Configuration.Attributes
{
    [TestFixture]
    public class SitecoreTypeAttributeFixture
    {
        [Test]
        public void Does_SitecoreTypeAttribute_Extend_AbstractClassAttribute()
        {
            Assert.IsTrue(typeof(AbstractTypeAttribute).IsAssignableFrom(typeof(SitecoreTypeAttribute)));
        }

        [Test]
        [TestCase("TemplateId")]
        [TestCase("BranchId")]
        public void Does_SitecoreTypeAttribute_Have_Properties(string propertyName)
        {
            var properties = typeof(SitecoreTypeAttribute).GetProperties();
            Assert.IsTrue(properties.Any(x => x.Name == propertyName));
        }
    }
}
