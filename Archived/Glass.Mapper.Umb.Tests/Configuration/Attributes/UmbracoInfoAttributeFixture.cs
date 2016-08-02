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


using System.Linq;
using FluentAssertions;
using Glass.Mapper.Umb.Configuration;
using Glass.Mapper.Umb.Configuration.Attributes;
using NUnit.Framework;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Umb.Tests.Configuration.Attributes
{
    [TestFixture]
    public class UmbracoInfoAttributeFixture
    {
        [Test]
        public void Does_UmbracoInfoAttribute_Extend_InfoAttribute()
        {
            //Assign
            var type = typeof(InfoAttribute);

            //Act

            //Assert
            type.IsAssignableFrom(typeof(UmbracoInfoAttribute)).Should().BeTrue();
        }

        [Test]
        [TestCase("Type")]
        public void Does_ChildrenAttribute_Have_Properties(string propertyName)
        {
            //Assign
            var properties = typeof(UmbracoInfoAttribute).GetProperties();

            //Act

            //Assert
            properties.Any(x => x.Name == propertyName).Should().BeTrue();
        }

        [Test]
        public void Does_Constructor_Set_Type_NotSet()
        {
            //Assign
            var infoAttribute = new UmbracoInfoAttribute();

            //Act

            //Assert
            infoAttribute.Type.ShouldBeEquivalentTo(UmbracoInfoType.NotSet);
        }
        
        #region Method - Configure

        [Test]
        public void Configure_ConfigureCalled_UmbracoInfoConfigurationReturned()
        {
            //Assign
            var attr = new UmbracoInfoAttribute();
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");
            
            //Act
            var result = attr.Configure(propertyInfo) as UmbracoInfoConfiguration;

            //Assert
            result.Should().NotBeNull();
            result.Type.ShouldBeEquivalentTo(UmbracoInfoType.NotSet);
        }

        [Test]
        public void Configure_TypeSet_TypeSetOnConfiguration()
        {
            //Assign
            var attr = new UmbracoInfoAttribute();
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");
            attr.Type = UmbracoInfoType.ContentTypeName;

            //Act
            var result = attr.Configure(propertyInfo) as UmbracoInfoConfiguration;

            //Assert
            result.Should().NotBeNull();
            result.Type.ShouldBeEquivalentTo(UmbracoInfoType.ContentTypeName);
        }

        #endregion

        #region Stubs

        public class StubClass
        {
            public string DummyProperty { get; set; }
        }

        #endregion
    }
}

