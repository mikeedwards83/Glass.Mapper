using System.Linq;
using Glass.Mapper.Umb.Configuration;
using Glass.Mapper.Umb.Configuration.Attributes;
using NUnit.Framework;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Umb.Tests.Configuration.Attributes
{
    [TestFixture]
    public class UmbracoFieldAttributeFixture
    {
        [Test]
        public void Does_SitecoreFieldAttribute_Extend_FieldAttribute()
        {
            Assert.IsTrue(typeof(FieldAttribute).IsAssignableFrom(typeof(UmbracoFieldAttribute)));
        }


        [Test]
        [TestCase("FieldName")]
        [TestCase("Setting")]
        [TestCase("ReadOnly")]
        public void Does_SitecoreFieldAttribute_Have_Properties(string propertyName)
        {
            var properties = typeof(UmbracoFieldAttribute).GetProperties();
            Assert.IsTrue(properties.Any(x => x.Name == propertyName));
        }

        [Test]
        public void Default_Constructor_Set_Setting_To_Default()
        {
            var testSitecoreFieldAttribute = new UmbracoFieldAttribute();
            Assert.AreEqual(testSitecoreFieldAttribute.Setting, UmbracoFieldSettings.Default);
        }

        [Test]
        public void Constructor_Sets_FieldName()
        {
            var testFieldName = "testFieldName";
            var testSitecoreFieldAttribute = new UmbracoFieldAttribute(testFieldName);
            Assert.AreEqual(testSitecoreFieldAttribute.FieldName, testFieldName);
        }

        #region Method - Configure

        [Test]
        public void Configure_ConfigureCalled_SitecoreQueryConfigurationReturned()
        {
            //Assign
            UmbracoFieldAttribute attr = new UmbracoFieldAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");


            //Act
            var result = attr.Configure(propertyInfo) as UmbracoFieldConfiguration;

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Configure_SettingNotSet_SettingsReturnAsDefault()
        {
            //Assign
            UmbracoFieldAttribute attr = new UmbracoFieldAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");


            //Act
            var result = attr.Configure(propertyInfo) as UmbracoFieldConfiguration;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(UmbracoFieldSettings.Default, result.Setting);
        }

        [Test]
        public void Configure_SettingSetToDontLoadLazily_SettingsReturnAsDontLoadLazily()
        {
            //Assign
            UmbracoFieldAttribute attr = new UmbracoFieldAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");
            
            attr.Setting = UmbracoFieldSettings.DontLoadLazily;

            //Act
            var result = attr.Configure(propertyInfo) as UmbracoFieldConfiguration;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(UmbracoFieldSettings.DontLoadLazily, result.Setting);
        }

        [Test]
        [Sequential]
        public void Configure_SettingCodeFirst_CodeFirstIsSetOnConfiguration(
            [Values(true, false)] bool value,
            [Values(true, false)] bool expected)
        {
            //Assign
            UmbracoFieldAttribute attr = new UmbracoFieldAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");

            attr.CodeFirst = value;

            //Act
            var result = attr.Configure(propertyInfo) as UmbracoFieldConfiguration;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.CodeFirst);
        }

      /*  [Test]
        [Sequential]
        public void Configure_SettingFieldType_FieldTypeIsSetOnConfiguration(
            [Values(UmbracoFieldType.DateTime)] UmbracoFieldType value,
            [Values(UmbracoFieldType.DateTime)] UmbracoFieldType expected)
        {
            //Assign
            UmbracoFieldAttribute attr = new UmbracoFieldAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");

            attr.FieldType = value;

            //Act
            var result = attr.Configure(propertyInfo) as UmbracoFieldConfiguration;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.FieldType);
        }*/

        [Test]
        [Sequential]
        public void Configure_SettingSectionName_SectionNameIsSetOnConfiguration(
            [Values("section name")] string value,
            [Values("section name")] string expected)
        {
            //Assign
            UmbracoFieldAttribute attr = new UmbracoFieldAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");

            attr.FieldTab = value;

            //Act
            var result = attr.Configure(propertyInfo) as UmbracoFieldConfiguration;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.FieldTab);
        }

        [Test]
        [Sequential]
        public void Configure_SettingFieldTitle_FieldTitleIsSetOnConfiguration(
            [Values("field alia")] string value,
            [Values("field alia")] string expected)
        {
            //Assign
            UmbracoFieldAttribute attr = new UmbracoFieldAttribute(string.Empty);
            var propertyInfo = typeof(StubClass).GetProperty("DummyProperty");

            attr.FieldAlias = value;

            //Act
            var result = attr.Configure(propertyInfo) as UmbracoFieldConfiguration;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.FieldAlias);
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
