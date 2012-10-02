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
    public class SitecoreInfoAttributeFixture
    {
        [Test]
        public void Does_SitecoreInfoAttribute_Extend_InfoAttribute()
        {
            Assert.IsTrue(typeof(InfoAttribute).IsAssignableFrom(typeof(SitecoreInfoAttribute)));
        }
    }
}
