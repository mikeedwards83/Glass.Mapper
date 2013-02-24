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
    public class UmbracoPropertyAttributeFixture
    {
        [Test]
        public void Does_UmbracoPropertyAttribute_Extend_FieldAttribute()
        {
            typeof(FieldAttribute).IsAssignableFrom(typeof(UmbracoPropertyAttribute)).Should().BeTrue();
        }

        [Test]
        [TestCase("PropertyAlias")]
        [TestCase("Setting")]
        [TestCase("ReadOnly")]
        [TestCase("PropertyName")]
        [TestCase("PropertyDescription")]
        [TestCase("PropertyIsMandatory")]
        [TestCase("PropertyValidation")]
        [TestCase("ContentTab")]
        [TestCase("PropertyType")]
        public void Does_UmbracoPropertyAttribute_Have_Properties(string propertyName)
        {
            var properties = typeof(UmbracoPropertyAttribute).GetProperties();
            properties.Any(x => x.Name == propertyName).Should().BeTrue();
        }

        [Test]
        public void Default_Constructor_Set_Setting_To_Default()
        {
            var testUmbracoPropertyAttribute = new UmbracoPropertyAttribute();
            testUmbracoPropertyAttribute.Setting.ShouldBeEquivalentTo(UmbracoPropertySettings.Default);
        }

        [Test]
        public void Constructor_Sets_PropertyAlias()
        {
            var testPropertyAlias = "testPropertyAlias";
            var testUmbracoPropertyAttribute = new UmbracoPropertyAttribute(testPropertyAlias);
            testUmbracoPropertyAttribute.PropertyAlias.ShouldBeEquivalentTo(testPropertyAlias);
        }

        [Test]
        public void Constructor_Sets_PropertyIdAndPropertyType()
        {
            var testPropertyId = "test";
            var testUmbracoPropertyAttribute = new UmbracoPropertyAttribute("test", UmbracoPropertyType.NotSet);
            testUmbracoPropertyAttribute.PropertyAlias.ShouldBeEquivalentTo(testPropertyId);
            testUmbracoPropertyAttribute.PropertyType.ShouldBeEquivalentTo(UmbracoPropertyType.NotSet);
        }

        #region Method - Configure

        [Test]
        public void Configure_ConfigureCalled_UmbracoPropertyConfigurationReturned()
        {
            //Assign
            var attr = new UmbracoPropertyAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");


            //Act
            var result = attr.Configure(propertyInfo) as UmbracoPropertyConfiguration;

            //Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void Configure_SettingNotSet_SettingsReturnAsDefault()
        {
            //Assign
            var attr = new UmbracoPropertyAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");


            //Act
            var result = attr.Configure(propertyInfo) as UmbracoPropertyConfiguration;

            //Assert
            result.Should().NotBeNull();
            result.Setting.ShouldBeEquivalentTo(UmbracoPropertySettings.Default);
        }

        [Test]
        public void Configure_SettingSetToDontLoadLazily_SettingsReturnAsDontLoadLazily()
        {
            //Assign
            var attr = new UmbracoPropertyAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");
            
            attr.Setting = UmbracoPropertySettings.DontLoadLazily;

            //Act
            var result = attr.Configure(propertyInfo) as UmbracoPropertyConfiguration;

            //Assert
            result.Should().NotBeNull();
            result.Setting.ShouldBeEquivalentTo(UmbracoPropertySettings.DontLoadLazily);
        }

        [Test]
        [Sequential]
        public void Configure_SettingCodeFirst_CodeFirstIsSetOnConfiguration(
            [Values(true, false)] bool value,
            [Values(true, false)] bool expected)
        {
            //Assign
            var attr = new UmbracoPropertyAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");

            attr.CodeFirst = value;

            //Act
            var result = attr.Configure(propertyInfo) as UmbracoPropertyConfiguration;

            //Assert
            result.Should().NotBeNull();
            result.CodeFirst.ShouldBeEquivalentTo(expected);
        }

        [Test]
        [Sequential]
        public void Configure_SettingDataType_DataTypeIsSetOnConfiguration(
            [Values(UmbracoPropertyType.TextString)] UmbracoPropertyType value,
            [Values(UmbracoPropertyType.TextString)] UmbracoPropertyType expected)
        {
            //Assign
            var attr = new UmbracoPropertyAttribute(string.Empty);
            var propertyInfo = typeof (StubClass).GetProperty("DummyProperty");

            attr.PropertyType = value;

            //Act
            var result = attr.Configure(propertyInfo) as UmbracoPropertyConfiguration;

            //Assert
            result.Should().NotBeNull();
            result.PropertyType.ShouldBeEquivalentTo(expected);
        }

        [Test]
        [Sequential]
        public void Configure_SettingPropertyAlias_PropertyAliasIsSetOnConfiguration(
            [Values("property alias")] string value,
            [Values("property alias")] string expected)
        {
            //Assign
            var attr = new UmbracoPropertyAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");

            attr.PropertyAlias = value;

            //Act
            var result = attr.Configure(propertyInfo) as UmbracoPropertyConfiguration;

            //Assert
            result.Should().NotBeNull();
            result.PropertyAlias.ShouldBeEquivalentTo(expected);
        }

        [Test]
        [Sequential]
        public void Configure_SettingDocumentTab_DocumentTabIsSetOnConfiguration(
            [Values("document tab")] string value,
            [Values("document tab")] string expected)
        {
            //Assign
            var attr = new UmbracoPropertyAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");

            attr.ContentTab = value;

            //Act
            var result = attr.Configure(propertyInfo) as UmbracoPropertyConfiguration;

            //Assert
            result.Should().NotBeNull();
            result.ContentTab.ShouldBeEquivalentTo(expected);
        }

        [Test]
        [Sequential]
        public void Configure_SettingPropertyName_PropertyNameIsSetOnConfiguration(
            [Values("property name")] string value,
            [Values("property name")] string expected)
        {
            //Assign
            var attr = new UmbracoPropertyAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");

            attr.PropertyName = value;

            //Act
            var result = attr.Configure(propertyInfo) as UmbracoPropertyConfiguration;

            //Assert
            result.Should().NotBeNull();
            result.PropertyName.ShouldBeEquivalentTo(expected);
        }

        [Test]
        [Sequential]
        public void Configure_SettingPropertyDescription_PropertyDescriptionIsSetOnConfiguration(
            [Values("property description")] string value,
            [Values("property description")] string expected)
        {
            //Assign
            var attr = new UmbracoPropertyAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");

            attr.PropertyDescription = value;

            //Act
            var result = attr.Configure(propertyInfo) as UmbracoPropertyConfiguration;

            //Assert
            result.Should().NotBeNull();
            result.PropertyDescription.ShouldBeEquivalentTo(expected);
        }

        [Test]
        [Sequential]
        public void Configure_SettingPropertyValidation_PropertyValidationIsSetOnConfiguration(
            [Values("property validation")] string value,
            [Values("property validation")] string expected)
        {
            //Assign
            var attr = new UmbracoPropertyAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");

            attr.PropertyValidation = value;

            //Act
            var result = attr.Configure(propertyInfo) as UmbracoPropertyConfiguration;

            //Assert
            result.Should().NotBeNull();
            result.PropertyValidation.ShouldBeEquivalentTo(expected);
        }

        [Test]
        [Sequential]
        public void Configure_SettingPropertyMandatory_PropertyMandatoryIsSetOnConfiguration(
            [Values(true)] bool value,
            [Values(true)] bool expected)
        {
            //Assign
            var attr = new UmbracoPropertyAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");

            attr.PropertyIsMandatory = value;

            //Act
            var result = attr.Configure(propertyInfo) as UmbracoPropertyConfiguration;

            //Assert
            result.Should().NotBeNull();
            result.PropertyIsMandatory.ShouldBeEquivalentTo(expected);
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



