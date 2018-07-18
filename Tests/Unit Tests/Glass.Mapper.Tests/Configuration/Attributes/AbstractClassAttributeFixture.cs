using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Tests.Configuration.Attributes
{
    [TestFixture]
    public class AbstractClassAttributeFixture
    {
        [Test]
        public void Is_Attribute_Multiple_False()
        {
            var attributes = (IList<AttributeUsageAttribute>)typeof(AbstractTypeAttribute).GetCustomAttributes(typeof(AttributeUsageAttribute), false);
            Assert.AreEqual(1, attributes.Count);

            var attribute = attributes[0];
            Assert.IsFalse(attribute.AllowMultiple);
        }

        [Test]
        public void Is_Attribute_Calss()
        {
            var attributes = (IList<AttributeUsageAttribute>)typeof(AbstractTypeAttribute).GetCustomAttributes(typeof(AttributeUsageAttribute), false);
            Assert.AreEqual(1, attributes.Count);

            var attribute = attributes[0];
            Assert.IsTrue(attribute.ValidOn == (AttributeTargets.Class | AttributeTargets.Interface));
        }
    }
}




