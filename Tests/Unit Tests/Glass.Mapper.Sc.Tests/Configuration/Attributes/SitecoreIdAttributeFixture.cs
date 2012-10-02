using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Configuration.Attributes;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Tests.Configuration.Attributes
{
    [TestFixture]
    public abstract class SitecoreIdAttributeFixture
    {
        [Test]
        public void Does_SitecoreIdAttribute_Extend_IdAttribute()
        {
            Assert.IsTrue(typeof(IdAttribute).IsAssignableFrom(typeof(SitecoreIdAttribute)));
        }

        [Test]
        public void SitecoreIdAttribute_Is_ID_Type()
        {
            var sitecoreIdAttribute = new SitecoreIdAttribute();
            Assert.AreEqual(sitecoreIdAttribute.IdType, typeof(ID));
        }
    }
}
