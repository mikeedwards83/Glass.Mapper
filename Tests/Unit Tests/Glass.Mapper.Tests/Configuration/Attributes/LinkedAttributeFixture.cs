using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Tests.Configuration.Attributes
{
    [TestFixture]
    public class LinkedAttributeFixture
    {
        [Test]
        public void Does_QueryAttribute_Extend_AbstractPropertyAttribute()
        {
            Assert.IsTrue(typeof(AbstractPropertyAttribute).IsAssignableFrom(typeof(LinkedAttribute)));
        }

        [Test]
        [TestCase("IsLazy")]
        public void Does_QueryAttribute_Have_Properties(string propertyName)
        {
            var properties = typeof(LinkedAttribute).GetProperties();
            Assert.IsTrue(properties.Any(x => x.Name == propertyName));
        }


        [Test]
        public void Does_Constructor_Set_IsLazy_True()
        {
            Assert.IsTrue(new TestLinkedAttribute().IsLazy);
        }

        private class TestLinkedAttribute : LinkedAttribute
        {
        }
    }
}
