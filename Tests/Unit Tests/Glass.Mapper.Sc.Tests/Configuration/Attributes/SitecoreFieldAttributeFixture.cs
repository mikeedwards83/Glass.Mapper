using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Glass.Mapper.Configuration.Attributes;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.Tests.Configuration.Attributes
{
    [TestFixture]
    public class SitecoreFieldAttributeFixture
    {
        [Test]
        public void Does_SitecoreFieldAttribute_Extend_FieldAttribute()
        {
            Assert.IsTrue(typeof(FieldAttribute).IsAssignableFrom(typeof(SitecoreFieldAttribute)));
        }


        [Test]
        [TestCase("FieldName")]
        [TestCase("Setting")]
        [TestCase("ReadOnly")]
        public void Does_SitecoreFieldAttribute_Have_Properties(string fieldName)
        {
            var properties = typeof(SitecoreFieldAttribute).GetProperties();
            Assert.IsTrue(properties.Any(x => x.Name == fieldName));
        }

        [Test]
        public void Default_Constructor_Set_Setting_To_Default()
        {
            var testSitecoreFieldAttribute = new SitecoreFieldAttribute();
            Assert.AreEqual(testSitecoreFieldAttribute.Setting, SitecoreFieldSettings.Default);
        }

        [Test]
        public void Constructor_Sets_FieldName()
        {
            var testFieldName = "testFieldName";
            var testSitecoreFieldAttribute = new SitecoreFieldAttribute(testFieldName);
            Assert.AreEqual(testSitecoreFieldAttribute.FieldName, testFieldName);
        }

        #region Method - Configure

        [Test]
        public void Configure_ConfigureCalled_SitecoreQueryConfigurationReturned()
        {
            //Assign
            SitecoreFieldAttribute attr = new SitecoreFieldAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");


            //Act
            var result = attr.Configure(propertyInfo) as SitecoreFieldConfiguration;

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Configure_SettingNotSet_SettingsReturnAsDefault()
        {
            //Assign
            SitecoreFieldAttribute attr = new SitecoreFieldAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");


            //Act
            var result = attr.Configure(propertyInfo) as SitecoreFieldConfiguration;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(SitecoreFieldSettings.Default, result.Setting);
        }

        [Test]
        public void Configure_SettingSetToDontLoadLazily_SettingsReturnAsDontLoadLazily()
        {
            //Assign
            SitecoreFieldAttribute attr = new SitecoreFieldAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");
            
            attr.Setting = SitecoreFieldSettings.DontLoadLazily;

            //Act
            var result = attr.Configure(propertyInfo) as SitecoreFieldConfiguration;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(SitecoreFieldSettings.DontLoadLazily, result.Setting);
        }

        [Test]
        [Sequential]
        public void Configure_SettingCodeFirst_CodeFirstIsSetOnConfiguration(
            [Values(true, false)] bool value,
            [Values(true, false)] bool expected)
        {
            //Assign
            SitecoreFieldAttribute attr = new SitecoreFieldAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");

            attr.CodeFirst = value;

            //Act
            var result = attr.Configure(propertyInfo) as SitecoreFieldConfiguration;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.CodeFirst);
        }

        [Test]
        [Sequential]
        public void Configure_SettingFieldType_FieldTypeIsSetOnConfiguration(
            [Values(SitecoreFieldType.DateTime)] SitecoreFieldType value,
            [Values(SitecoreFieldType.DateTime)] SitecoreFieldType expected)
        {
            //Assign
            SitecoreFieldAttribute attr = new SitecoreFieldAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");

            attr.FieldType = value;

            //Act
            var result = attr.Configure(propertyInfo) as SitecoreFieldConfiguration;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.FieldType);
        }

        [Test]
        [Sequential]
        public void Configure_SettingSectionName_SectionNameIsSetOnConfiguration(
            [Values("section name")] string value,
            [Values("section name")] string expected)
        {
            //Assign
            SitecoreFieldAttribute attr = new SitecoreFieldAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");

            attr.SectionName = value;

            //Act
            var result = attr.Configure(propertyInfo) as SitecoreFieldConfiguration;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.SectionName);
        }

        [Test]
        [Sequential]
        public void Configure_SettingFieldTitle_FieldTitleIsSetOnConfiguration(
            [Values("field name")] string value,
            [Values("field name")] string expected)
        {
            //Assign
            SitecoreFieldAttribute attr = new SitecoreFieldAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");

            attr.FieldTitle = value;

            //Act
            var result = attr.Configure(propertyInfo) as SitecoreFieldConfiguration;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.FieldTitle);
        }

        [Test]
        [Sequential]
        public void Configure_SettingFieldSource_FieldSourceIsSetOnConfiguration(
            [Values("field Source")] string value,
            [Values("field Source")] string expected)
        {
            //Assign
            SitecoreFieldAttribute attr = new SitecoreFieldAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");

            attr.FieldSource = value;

            //Act
            var result = attr.Configure(propertyInfo) as SitecoreFieldConfiguration;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.FieldSource);
        }

        [Test]
        [Sequential]
        public void Configure_SettingIsShared_IsSharedIsSetOnConfiguration(
            [Values(true, false)] bool value,
            [Values(true, false)] bool expected)
        {
            //Assign
            SitecoreFieldAttribute attr = new SitecoreFieldAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");

            attr.IsShared = value;

            //Act
            var result = attr.Configure(propertyInfo) as SitecoreFieldConfiguration;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.IsShared);
        }

        [Test]
        [Sequential]
        public void Configure_SettingIsUnversioned_IsUnversionedIsSetOnConfiguration(
            [Values(true, false)] bool value,
            [Values(true, false)] bool expected)
        {
            //Assign
            SitecoreFieldAttribute attr = new SitecoreFieldAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");

            attr.IsUnversioned = value;

            //Act
            var result = attr.Configure(propertyInfo) as SitecoreFieldConfiguration;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.IsUnversioned);
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
