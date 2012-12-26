using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.DataMappers;
using Glass.Mapper.Sc.Fields;
using NUnit.Framework;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    [TestFixture]
    public class SitecoreFieldLinkMapperFixture: AbstractMapperFixture
    {

        #region Method - GetField

        [Test]
        public void GetField_FieldContainsAnchor_ReturnsAnchorLink()
        {
            //Assign
            var mapper = new SitecoreFieldLinkMapper();
            var fieldValue =
                "<link text=\"Test description\" linktype=\"anchor\" url=\"testAnchor\" anchor=\"testAnchor\" title=\"test alternate\" class=\"testClass\" />";
           
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldLinkMapper/GetField");
            var field = item.Fields[FieldName];

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = mapper.GetField(field, null, null) as Link;

            //Assert
            Assert.AreEqual("testAnchor", result.Anchor);
            Assert.AreEqual("testClass", result.Class);
            Assert.AreEqual("", result.Query);
            Assert.AreEqual("", result.Target);
            Assert.AreEqual(Guid.Empty, result.TargetId);
            Assert.AreEqual("Test description", result.Text);
            Assert.AreEqual("test alternate", result.Title);
            Assert.AreEqual(LinkType.Anchor, result.Type);
            Assert.AreEqual("testAnchor", result.Url);
        }

        [Test]
        public void GetField_FieldContainsExternal_ReturnsExternalLink()
        {
            //Assign
            var mapper = new SitecoreFieldLinkMapper();
            var fieldValue =
                "<link text=\"Test description\" linktype=\"external\" url=\"http://www.google.com\" anchor=\"\" title=\"test alternative\" class=\"testClass\" target=\"_blank\" />";

            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldLinkMapper/GetField");
            var field = item.Fields[FieldName];

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = mapper.GetField(field, null, null) as Link;

            //Assert
            Assert.AreEqual("", result.Anchor);
            Assert.AreEqual("testClass", result.Class);
            Assert.AreEqual("", result.Query);
            Assert.AreEqual("_blank", result.Target);
            Assert.AreEqual(Guid.Empty, result.TargetId);
            Assert.AreEqual("Test description", result.Text);
            Assert.AreEqual("test alternative", result.Title);
            Assert.AreEqual(LinkType.External, result.Type);
            Assert.AreEqual("http://www.google.com", result.Url);
        }

        [Test]
        public void GetField_FieldContainsJavaScript_ReturnsJavascriptLink()
        {
            //Assign
            var mapper = new SitecoreFieldLinkMapper();
            var fieldValue =
                "<link text=\"Test description\" linktype=\"javascript\" url=\"javascript:alert('hello world');\" anchor=\"\" title=\"test alternate\" class=\"testClass\" />";

            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldLinkMapper/GetField");
            var field = item.Fields[FieldName];

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = mapper.GetField(field, null, null) as Link;

            //Assert
            Assert.AreEqual("", result.Anchor);
            Assert.AreEqual("testClass", result.Class);
            Assert.AreEqual("", result.Query);
            Assert.AreEqual("", result.Target);
            Assert.AreEqual(Guid.Empty, result.TargetId);
            Assert.AreEqual("Test description", result.Text);
            Assert.AreEqual("test alternate", result.Title);
            Assert.AreEqual(LinkType.JavaScript, result.Type);
            Assert.AreEqual("javascript:alert('hello world');", result.Url);
        }

        [Test]
        public void GetField_FieldContainsMailto_ReturnsMailtoLink()
        {
            //Assign
            var mapper = new SitecoreFieldLinkMapper();
            var fieldValue =
                "<link text=\"Test description\" linktype=\"mailto\" url=\"mailto:test@test.com\" anchor=\"\" title=\"test alternate\" class=\"testClass\" />";

            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldLinkMapper/GetField");
            var field = item.Fields[FieldName];

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = mapper.GetField(field, null, null) as Link;

            //Assert
            Assert.AreEqual("", result.Anchor);
            Assert.AreEqual("testClass", result.Class);
            Assert.AreEqual("", result.Query);
            Assert.AreEqual("", result.Target);
            Assert.AreEqual(Guid.Empty, result.TargetId);
            Assert.AreEqual("Test description", result.Text);
            Assert.AreEqual("test alternate", result.Title);
            Assert.AreEqual(LinkType.MailTo, result.Type);
            Assert.AreEqual("mailto:test@test.com", result.Url);
        }

        [Test]
        public void GetField_FieldContainsInternal_ReturnsInternalLink()
        {
            //Assign
            var mapper = new SitecoreFieldLinkMapper();
            var fieldValue =
                "<link text=\"Test description\" linktype=\"internal\" url=\"/Tests/DataMappers/SitecoreFieldLinkMapper/Target.aspx\" anchor=\"testAnchor\" querystring=\"q=s\" title=\"test alternative\" class=\"testClass\" target=\"testTarget\" id=\"{1AE37F34-3B6F-4F5F-A5C2-FD2B9D5A47A0}\" />";

            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldLinkMapper/GetField");
            var field = item.Fields[FieldName];

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = mapper.GetField(field, null, null) as Link;

            //Assert
            Assert.AreEqual("testAnchor", result.Anchor);
            Assert.AreEqual("testClass", result.Class);
            Assert.AreEqual("q=s", result.Query);
            Assert.AreEqual("testTarget", result.Target);
            Assert.AreEqual(new Guid("{1AE37F34-3B6F-4F5F-A5C2-FD2B9D5A47A0}"), result.TargetId);
            Assert.AreEqual("Test description", result.Text);
            Assert.AreEqual("test alternative", result.Title);
            Assert.AreEqual(LinkType.Internal, result.Type);
            Assert.AreEqual("/en/sitecore/content/Tests/DataMappers/SitecoreFieldLinkMapper/Target.aspx", result.Url);
        }

        [Test]
        public void GetField_FieldContainsInternalMissingItem_ReturnsNotSetLink()
        {
            //Assign
            var mapper = new SitecoreFieldLinkMapper();
            var fieldValue =
                "<link text=\"Test description\" linktype=\"internal\" url=\"/Tests/DataMappers/SitecoreFieldLinkMapper/Target.aspx\" anchor=\"testAnchor\" querystring=\"q=s\" title=\"test alternative\" class=\"testClass\" target=\"testTarget\" id=\"{11111111-3B6F-4F5F-A5C2-FD2B9D5A47A0}\" />";

            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldLinkMapper/GetField");
            var field = item.Fields[FieldName];

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = mapper.GetField(field, null, null) as Link;

            //Assert
            Assert.AreEqual("testAnchor", result.Anchor);
            Assert.AreEqual("testClass", result.Class);
            Assert.AreEqual("q=s", result.Query);
            Assert.AreEqual("testTarget", result.Target);
            Assert.AreEqual(new Guid("{11111111-3B6F-4F5F-A5C2-FD2B9D5A47A0}"), result.TargetId);
            Assert.AreEqual("Test description", result.Text);
            Assert.AreEqual("test alternative", result.Title);
            Assert.AreEqual(LinkType.Internal, result.Type);
            Assert.AreEqual("", result.Url);
        }
        [Test]
        public void GetField_FieldContainsMediaLink_ReturnsMediaLink()
        {
            //Assign
            var mapper = new SitecoreFieldLinkMapper();
            var fieldValue =
                "<link text=\"Test description\" linktype=\"media\" url=\"/Files/20121222_001405\" title=\"test alternative\" class=\"testClass\" target=\"_blank\" id=\"{8E1C32F3-CF15-4067-A3F4-85148606F9CD}\" />";

            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldLinkMapper/GetField");
            var field = item.Fields[FieldName];

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = mapper.GetField(field, null, null) as Link;

            //Assert
            Assert.AreEqual("", result.Anchor);
            Assert.AreEqual("testClass", result.Class);
            Assert.AreEqual("", result.Query);
            Assert.AreEqual("_blank", result.Target);
            Assert.AreEqual(new Guid("{8E1C32F3-CF15-4067-A3F4-85148606F9CD}"), result.TargetId);
            Assert.AreEqual("Test description", result.Text);
            Assert.AreEqual("test alternative", result.Title);
            Assert.AreEqual(LinkType.Media, result.Type);
            Assert.AreEqual("/~/media/8E1C32F3CF154067A3F485148606F9CD.ashx", result.Url);
        }

        [Test]
        public void GetField_FieldContainsMediaLinkMissingItem_ReturnsNotSetLink()
        {
            //Assign
            var mapper = new SitecoreFieldLinkMapper();
            var fieldValue =
                "<link text=\"Test description\" linktype=\"media\" url=\"/Files/20121222_001405\" title=\"test alternative\" class=\"testClass\" target=\"_blank\" id=\"{11111111-CF15-4067-A3F4-85148606F9CD}\" />";

            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldLinkMapper/GetField");
            var field = item.Fields[FieldName];

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = mapper.GetField(field, null, null) as Link;

            //Assert
            Assert.AreEqual("", result.Anchor);
            Assert.AreEqual("testClass", result.Class);
            Assert.AreEqual("", result.Query);
            Assert.AreEqual("_blank", result.Target);
            Assert.AreEqual(new Guid("{11111111-CF15-4067-A3F4-85148606F9CD}"), result.TargetId);
            Assert.AreEqual("Test description", result.Text);
            Assert.AreEqual("test alternative", result.Title);
            Assert.AreEqual(LinkType.Media, result.Type);
            Assert.AreEqual("", result.Url);
        }

        #endregion
        #region Method - SetField

        #endregion


    }
}
