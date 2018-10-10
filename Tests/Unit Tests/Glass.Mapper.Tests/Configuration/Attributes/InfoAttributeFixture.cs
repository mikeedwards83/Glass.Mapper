using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Glass.Mapper.Configuration;
using NSubstitute;
using NUnit.Framework;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Tests.Configuration.Attributes
{
    [TestFixture]
    public class InfoAttributeFixture
    {
        [Test]
        public void Does_FieldAttribute_Extend_AbstractPropertyAttribute()
        {
            Assert.IsTrue(typeof(AbstractPropertyAttribute).IsAssignableFrom(typeof(InfoAttribute)));
        }

        #region Method - Configure

        [Test]
        public void Configure_DefaultValues_ConfigContainsDefaults()
        {
            //Assign
            var attr = new StubInfoAttribute();
            var config = new InfoConfiguration();
            var propertyInfo = typeof(StubItem).GetProperty("X");

            //Act
            attr.Configure(propertyInfo, config);

            //Assert
            Assert.AreEqual(propertyInfo, config.PropertyInfo);
        }

        #endregion

        #region Stub

        public class StubInfoAttribute : InfoAttribute
        {
            public override AbstractPropertyConfiguration Configure(PropertyInfo propertyInfo)
            {
                throw new NotImplementedException();
            }
        }

		public class StubItem
		{
			public object X { get; set; }
		}

        #endregion
    }
}




