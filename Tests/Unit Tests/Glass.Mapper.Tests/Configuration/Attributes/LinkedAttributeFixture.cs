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
    public class LinkedAttributeFixture
    {
        [Test]
        public void Does_QueryAttribute_Extend_AbstractPropertyAttribute()
        {
            Assert.IsTrue(typeof(AbstractPropertyAttribute).IsAssignableFrom(typeof(LinkedAttribute)));
        }

        [Test]
        [TestCase("IsLazy")]
        [TestCase("InferType")]
        public void Does_QueryAttribute_Have_Properties(string propertyName)
        {
            var properties = typeof(LinkedAttribute).GetProperties();
            Assert.IsTrue(properties.Any(x => x.Name == propertyName));
        }


        [Test]
        public void Does_Constructor_Set_IsLazy_True()
        {
            Assert.IsTrue(new StubLinkedAttribute().IsLazy);
        }

        #region Method - Configure

        [Test]
        public void Configure_DefaultValues_ConfigSetToDefaults()
        {
            //Assign
            var attr = new StubLinkedAttribute();
            var config = new LinkedConfiguration();
            var propertyInfo = Substitute.For<PropertyInfo>();

            //Act
            attr.Configure(propertyInfo, config);

            //Assert
            Assert.AreEqual(propertyInfo, config.PropertyInfo);
            Assert.IsTrue(config.IsLazy);
            Assert.IsFalse(config.InferType);
        }

        [Test]
        public void Configure_InferTypeIsTrue_ConfigInferTypeIsTrue()
        {
            //Assign
            var attr = new StubLinkedAttribute();
            var config = new LinkedConfiguration();
            var propertyInfo = Substitute.For<PropertyInfo>();

            attr.InferType = true;

            //Act
            attr.Configure(propertyInfo, config);

            //Assert
            Assert.AreEqual(propertyInfo, config.PropertyInfo);
            Assert.IsTrue(config.IsLazy);
            Assert.IsTrue(config.InferType);
        }

        [Test]
        public void Configure_IsLazyIsFalse_ConfigIsLazyIsFalse()
        {
            //Assign
            var attr = new StubLinkedAttribute();
            var config = new LinkedConfiguration();
            var propertyInfo = Substitute.For<PropertyInfo>();

            attr.IsLazy = false;

            //Act
            attr.Configure(propertyInfo, config);

            //Assert
            Assert.AreEqual(propertyInfo, config.PropertyInfo);
            Assert.IsFalse(config.IsLazy);
            Assert.IsFalse(config.InferType);
        }

        #endregion


        #region Stubs

        private class StubLinkedAttribute : LinkedAttribute
        {
            public override Mapper.Configuration.AbstractPropertyConfiguration Configure(System.Reflection.PropertyInfo propertyInfo)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
