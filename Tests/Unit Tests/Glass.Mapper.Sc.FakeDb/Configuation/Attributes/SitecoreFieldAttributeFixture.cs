


using System.Linq;
using Glass.Mapper.Configuration.Attributes;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;

namespace Glass.Mapper.Sc.FakeDb.Configuation.Attributes
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

        #endregion

        #region Stubs

        public class StubClass
        {
            public string DummyProperty { get; set; }
        }

        #endregion
    }
}




