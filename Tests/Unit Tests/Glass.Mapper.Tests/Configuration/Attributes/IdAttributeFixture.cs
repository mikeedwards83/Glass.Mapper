using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Tests.Configuration.Attributes
{
    [TestFixture]
    public class IdAttributeFixture
    {
        [Test]
        public void Does_IdAttribute_Extend_AbstractPropertyAttribute()
        {
            Assert.IsTrue(typeof(AbstractPropertyAttribute).IsAssignableFrom(typeof(IdAttribute)));
        }

        [Test]
        public void Can_Set_IDType()
        {
            var idAttribute = new IdAttribute(typeof(int));
            Assert.AreEqual(idAttribute.IdType, typeof(int));
        }

    }
}
