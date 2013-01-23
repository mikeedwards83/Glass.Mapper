/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-

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
    public class FieldAttributeFixture
    {
        [Test]
        public void Does_FieldAttribute_Extend_AbstractPropertyAttribute()
        {
            Assert.IsTrue(typeof(AbstractPropertyAttribute).IsAssignableFrom(typeof(FieldAttribute)));
        }

        [Test]
        [TestCase("ReadOnly")]
        public void Does_FieldAttribute_Have_Properties(string propertyName)
        {
            var properties = typeof(FieldAttribute).GetProperties();
            Assert.IsTrue(properties.Any(x => x.Name == propertyName));
        }

        #region Method - Configure

        [Test]
        public void Configure_DefaultValues_ConfigSetWithDefaults()
        {
            //Act
            var attr = new StubFieldAttribute();
            var config = new FieldConfiguration();
            var propertyInfo = Substitute.For<PropertyInfo>();

            //Act
            attr.Configure(propertyInfo, config);

            //Assert
            Assert.AreEqual(propertyInfo, config.PropertyInfo);
          //  Assert.IsNullOrEmpty(config.Name);
            Assert.IsFalse(config.ReadOnly);
        }

        [Test]
        public void Configure_FieldNameSet_FieldNameSetOnConfig()
        {
            //Act
            var attr = new StubFieldAttribute();
            var config = new FieldConfiguration();
            var propertyInfo = Substitute.For<PropertyInfo>();

         //   attr.Name = "test field name";

            //Act
            attr.Configure(propertyInfo, config);

            //Assert
            Assert.AreEqual(propertyInfo, config.PropertyInfo);
       //     Assert.AreEqual(attr.Name, config.Name);
            Assert.IsFalse(config.ReadOnly);
        }

        [Test]
        public void Configure_ReadOnlySet_ReadOnlySetOnConfig()
        {
            //Act
            var attr = new StubFieldAttribute();
            var config = new FieldConfiguration();
            var propertyInfo = Substitute.For<PropertyInfo>();

            attr.ReadOnly = true;

            //Act
            attr.Configure(propertyInfo, config);

            //Assert
            Assert.AreEqual(propertyInfo, config.PropertyInfo);
            //Assert.IsNullOrEmpty(config.Name);
            Assert.True(config.ReadOnly);
        }

        #endregion

        #region Stubs

        public class StubFieldAttribute : FieldAttribute
        {
            public override AbstractPropertyConfiguration Configure(PropertyInfo propertyInfo)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}



