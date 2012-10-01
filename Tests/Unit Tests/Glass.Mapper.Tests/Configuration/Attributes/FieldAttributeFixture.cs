using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Tests.Configuration.Attributes
{
    [TestFixture]
    public class FieldAttributeFixture
    {
        [Test]
        public void Does_FieldAttribute_Extend_AbstractPropertyAttribute()
        {
            Assert.IsTrue(typeof(AbstractPropertyAttribute).IsAssignableFrom(typeof(FieldAttribute)));
        }

        [Test]
        [TestCase("FieldName")]
        [TestCase("ReadOnly")]
        public void Does_FieldAttribute_Have_Properties(string propertyName)
        {
            var properties = typeof(FieldAttribute).GetProperties();
            Assert.IsTrue(properties.Any(x => x.Name == propertyName));
        }
    }
}
