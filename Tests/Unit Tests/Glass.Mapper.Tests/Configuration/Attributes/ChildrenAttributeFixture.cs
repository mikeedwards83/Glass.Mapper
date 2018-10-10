
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
    public class ChildrenAttributeFixture
    {
        [Test]
        public void Does_ChildrenAttribute_Extend_AbstractPropertyAttribute()
        {
            Assert.IsTrue(typeof(AbstractPropertyAttribute).IsAssignableFrom(typeof(ChildrenAttribute)));
        }

        [Test]
        [TestCase("InferType")]
        public void Does_ChildrenAttribute_Have_Properties(string propertyName)
        {
            var properties = typeof(ChildrenAttribute).GetProperties();
            Assert.IsTrue(properties.Any(x => x.Name == propertyName));
        }

        #region Method - Configure

        [Test]
        public void Configure_InferTypeSet_InferTypeSetOnConfig()
        {
            //Assign
            var attr = new StubChildrenAttribute();
            var config = new ChildrenConfiguration();
			var propertyInfo = typeof(StubItem).GetProperty("X");

            attr.InferType = true;

            //Act
            attr.Configure(propertyInfo, config);

            //Assert
            Assert.AreEqual(propertyInfo, config.PropertyInfo);
            Assert.IsTrue(config.InferType);
        }

        #endregion

        #region Stubs 

        private class StubChildrenAttribute : ChildrenAttribute
        {
            public override Mapper.Configuration.AbstractPropertyConfiguration Configure(System.Reflection.PropertyInfo propertyInfo)
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




