using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;
using Sitecore.Sites;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    [TestFixture]
    public class SitecoreFieldStringMapperFixture : AbstractMapperFixture
    {

        #region Method - GetField

        [Test]
        public void GetField_FieldContainsData_StringIsReturned()
        {
            //Assign
            var fieldValue = "hello world";

            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldStringMapper/GetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldStringMapper();
            var config = new SitecoreFieldConfiguration();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = mapper.GetField(field, config, null) as string;

            //Assert
            Assert.AreEqual(fieldValue, result);
        }


        [Test]
        public void GetField_RichText_StringIsReturnedWithScapedUrl()
        {
            //Assign
            var fieldValue = "<p>Test with <a href=\"~/link.aspx?_id=BFD7975DF42F41E19DDA9A38E971555F&amp;_z=z\">link</a></p>";
            var expected = "<p>Test with <a href=\"/en/Tests/DataMappers/SitecoreFieldStringMapper/GetField.aspx\">link</a></p>";
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldStringMapper/GetFieldRichText");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldStringMapper();
            var config = new SitecoreFieldConfiguration();

            Sitecore.Context.Site = Sitecore.Configuration.Factory.GetSite("website");
            Sitecore.Context.Site.SetDisplayMode(DisplayMode.Preview, DisplayModeDuration.Remember);
            
            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }



            //Act
            var result = mapper.GetField(field, config, null) as string;

            Sitecore.Context.Site = null;

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetField_RichTextSettingsIsRaw_StringIsReturnedWithoutEscaping()
        {
            //Assign
            var fieldValue = "<p>Test with <a href=\"~/link.aspx?_id=BFD7975DF42F41E19DDA9A38E971555F&amp;_z=z\">link</a></p>";
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldStringMapper/GetFieldRichText");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldStringMapper();
            var config = new SitecoreFieldConfiguration();
            config.Setting = SitecoreFieldSettings.RichTextRaw;

            Sitecore.Context.Site = Sitecore.Configuration.Factory.GetSite("website");

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }



            //Act
            var result = mapper.GetField(field, config, null) as string;

            Sitecore.Context.Site = null;

            //Assert
            Assert.AreEqual(fieldValue, result);
        }

        #endregion

        #region Method - SetField

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetField_RichText_ThrowsException()
        {
            //Assign
            var expected = "<p>Test with <a href=\"~/link.aspx?_id=BFD7975DF42F41E19DDA9A38E971555F&amp;_z=z\">link</a></p>";
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldStringMapper/SetFieldRichText");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldStringMapper();
            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = new FakePropertyInfo(typeof (string));

            Sitecore.Context.Site = Sitecore.Configuration.Factory.GetSite("website");

            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetField(field, expected, config, null);
            }

            Sitecore.Context.Site = null;

            //Assert
            Assert.AreEqual(expected, field.Value);
        }

        [Test]
        public void SetField_FielNonRichText_ValueWrittenToField()
        {
            //Assign
            var expected = "<p>Test with <a href=\"~/link.aspx?_id=BFD7975DF42F41E19DDA9A38E971555F&amp;_z=z\">link</a></p>";
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldStringMapper/SetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldStringMapper();
            var config = new SitecoreFieldConfiguration();
            config.Setting = SitecoreFieldSettings.RichTextRaw;
            
            Sitecore.Context.Site = Sitecore.Configuration.Factory.GetSite("website");

            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetField(field, expected, config, null);
            }

            Sitecore.Context.Site = null;

            //Assert
            Assert.AreEqual(expected, field.Value);
        }

        #endregion

        #region Method - CanHandle

        [Test]
        public void CanHandle_StreamType_ReturnsTrue()
        {
            //Assign
            var mapper = new SitecoreFieldStringMapper();
            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = new FakePropertyInfo(typeof(String));

            //Act
            var result = mapper.CanHandle(config, null);

            //Assert
            Assert.IsTrue(result);
        }

        #endregion
    }
}
