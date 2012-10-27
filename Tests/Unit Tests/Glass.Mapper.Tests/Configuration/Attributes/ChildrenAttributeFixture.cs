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
        [TestCase("IsLazy")]
        [TestCase("InferType")]
        public void Does_ChildrenAttribute_Have_Properties(string propertyName)
        {
            var properties = typeof(ChildrenAttribute).GetProperties();
            Assert.IsTrue(properties.Any(x => x.Name == propertyName));
        }

        [Test]
        public void Does_Default_Constructor_Set_IsLazy_True()
        {
            Assert.IsTrue(new StubChildrenAttribute().IsLazy);
        }


        #region Method - Configure

        [Test]
        public void Configure_InferTypeSet_InferTypeSetOnConfig()
        {
            //Assign
            var attr = new StubChildrenAttribute();
            var config = new ChildrenConfiguration();
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
        public void Configure_IsLazyByDefault_IsLazyConfigTrue()
        {
            //Assign
            var attr = new StubChildrenAttribute();
            var config = new ChildrenConfiguration();
            var propertyInfo = Substitute.For<PropertyInfo>();

            //Act
            attr.Configure(propertyInfo, config);

            //Assert
            Assert.AreEqual(propertyInfo, config.PropertyInfo);
            Assert.IsTrue(config.IsLazy);
            Assert.IsFalse(config.InferType);
        }

        [Test]
        public void Configure_IsLazySetToFalse_IsLazyConfigFalse()
        {
            //Assign
            var attr = new StubChildrenAttribute();
            var config = new ChildrenConfiguration();
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

        private class StubChildrenAttribute : ChildrenAttribute
        {
            public override Mapper.Configuration.AbstractPropertyConfiguration Configure(System.Reflection.PropertyInfo propertyInfo)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
