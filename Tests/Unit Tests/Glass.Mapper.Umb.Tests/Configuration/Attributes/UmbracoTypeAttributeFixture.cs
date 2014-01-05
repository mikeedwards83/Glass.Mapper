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
using System.Linq;
using FluentAssertions;
using Glass.Mapper.Configuration;
using Glass.Mapper.Umb.Configuration;
using Glass.Mapper.Umb.Configuration.Attributes;
using NSubstitute;
using NUnit.Framework;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Umb.Tests.Configuration.Attributes
{
    [TestFixture]
    public class UmbracoTypeAttributeFixture
    {
        [Test]
        public void Does_UmbracoTypeAttribute_Extend_AbstractClassAttribute()
        {
            //Assign
            var type = typeof(AbstractTypeAttribute);
            //Act
            //Assert
            type.IsAssignableFrom(typeof(UmbracoTypeAttribute)).Should().BeTrue();
        }

        [Test]
        [TestCase("ContentTypeAlias")]
        [TestCase("ContentTypeName")]
        public void Does_UmbracoTypeAttribute_Have_Properties(string propertyName)
        {
            //Assign
            var properties = typeof(UmbracoTypeAttribute).GetProperties();
            //Act
            //Assert
            properties.Any(x => x.Name == propertyName).Should().BeTrue();
        }

        #region Method - Configure

        [Test]
        public void Configure_ConfigurationIsOfCorrectType_NoExceptionThrown()
        {
            //Assign
            var attr = new UmbracoTypeAttribute();
            var type = typeof(StubClass);
            
            //Act
            var config = attr.Configure(type);

            //Assert
            config.Type.ShouldBeEquivalentTo(type);
        }
        
        [Test]
        public void Configure_AttributeHasContentTypeAlias_ContentTypeAliasSetOnConfig()
        {
            //Assign
            var attr = new UmbracoTypeAttribute();
            var type = typeof(StubClass);

            var contentTypeAliasExpected = "test";

            attr.ContentTypeAlias = contentTypeAliasExpected;

            //Act
            var config = attr.Configure(type) as UmbracoTypeConfiguration;

            //Assert
            config.Type.ShouldBeEquivalentTo(type);
            config.ContentTypeAlias.ShouldBeEquivalentTo(contentTypeAliasExpected);
        }

        [Test]
        public void Configure_AttributeHasContentTypeName_ContentTypeNameSetOnConfig()
        {
            //Assign
            var attr = new UmbracoTypeAttribute();
            var type = typeof(StubClass);

            var contentTypeNameExpected = "test";

            attr.ContentTypeName = contentTypeNameExpected;

            //Act
            var config = attr.Configure(type) as UmbracoTypeConfiguration;

            //Assert
            config.Type.ShouldBeEquivalentTo(type);
            config.ContentTypeName.ShouldBeEquivalentTo(contentTypeNameExpected);
        }
        
        #endregion

        #region Stubs

        public class StubClass
        {

        }

        #endregion
    }
}




