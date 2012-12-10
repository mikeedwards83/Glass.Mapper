using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NSubstitute;
using NUnit.Framework;
using Glass.Mapper.Configuration.Attributes;
using Glass.Mapper.Configuration;

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

        #region Configure

        [Test]
        public void Configure_DefaultValues_ConfigContainsDefaults()
        {
            //Assign
            var type = typeof (int);
            var attr = new StubIdAttribute(type);
            var config = new IdConfiguration();
            var propertyInfo = typeof (StubClass).GetProperty("Id");

            //Act
            attr.Configure(propertyInfo, config);

            //Assert
            Assert.AreEqual(propertyInfo, config.PropertyInfo);
            Assert.AreEqual(type, config.Type);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationException))]
        public void Configure_RequiredTypeDifferentFromPropertyType_ExceptionThrown()
        {
            //Assign
            var type = typeof(string);
            var attr = new StubIdAttribute(type);
            var config = new IdConfiguration();
            var propertyInfo = typeof(StubClass).GetProperty("Id");

            //Act
            attr.Configure(propertyInfo, config);

            //Assert
        }


        #endregion

        #region Stubs

        public class StubClass
        {
            public int Id { get; set; }
        }

        public class StubIdAttribute : IdAttribute
        {
            public StubIdAttribute(Type type)
                : base(new []{ type })
            {

            }

            public override Mapper.Configuration.AbstractPropertyConfiguration Configure(System.Reflection.PropertyInfo propertyInfo)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
