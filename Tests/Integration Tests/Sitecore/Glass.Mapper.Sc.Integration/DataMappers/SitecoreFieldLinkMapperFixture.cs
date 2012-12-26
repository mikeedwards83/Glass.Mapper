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


        [Test]
        public void SetField_AnchorLink_AnchorLinkSetOnField()
        {
            //Assign
            var mapper = new SitecoreFieldLinkMapper();
            var expected =
                "<link title=\"test alternative\" linktype=\"anchor\" url=\"testAnchor\" anchor=\"testAnchor\" class=\"testClass\" text=\"Test description\" />";

            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldLinkMapper/SetField");
            var field = item.Fields[FieldName];

            var value = new Link()
                            {
                                Anchor = "testAnchor",
                                Class = "testClass",
                                Query = "",
                                Target = "",
                                TargetId = Guid.Empty,
                                Text = "Test description",
                                Title = "test alternative",
                                Type = LinkType.Anchor,
                                Url = "testAnchor"
                            };


            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetField(field, value, null, null);
            }

            //Assert
            Assert.AreEqual(expected, field.Value);
        }

        [Test]
        public void SetField_ExternalLink_ExternalLinkSetOnField()
        {
            //Assign
            var mapper = new SitecoreFieldLinkMapper();
            var expected =
                "<link url=\"http://www.google.com\" text=\"Test description\" class=\"testClass\" title=\"test alternative\" linktype=\"external\" />";

            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldLinkMapper/SetField");
            var field = item.Fields[FieldName];

            var value = new Link()
            {
                Anchor = "",
                Class = "testClass",
                Query = "",
                Target = "",
                TargetId = Guid.Empty,
                Text = "Test description",
                Title = "test alternative",
                Type = LinkType.External,
                Url = "http://www.google.com"
            };


            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetField(field, value, null, null);
            }

            //Assert
            Assert.AreEqual(expected, field.Value);
        }

        [Test]
        public void SetField_JavaScriptLink_JavaScriptLinkSetOnField()
        {
            //Assign
            var mapper = new SitecoreFieldLinkMapper();
            var expected =
                "<link url=\"javascript:alert('hello world');\" text=\"Test description\" class=\"testClass\" title=\"test alternative\" linktype=\"javascript\" />";

            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldLinkMapper/SetField");
            var field = item.Fields[FieldName];

            var value = new Link()
            {
                Anchor = "",
                Class = "testClass",
                Query = "",
                Target = "",
                TargetId = Guid.Empty,
                Text = "Test description",
                Title = "test alternative",
                Type = LinkType.JavaScript,
                Url = "javascript:alert('hello world');"
            };


            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetField(field, value, null, null);
            }

            //Assert
            Assert.AreEqual(expected, field.Value);
        }

        [Test]
        public void SetField_MailToLink_MailToLinkSetOnField()
        {
            //Assign
            var mapper = new SitecoreFieldLinkMapper();
            var expected =
                "<link url=\"mailto:test@test.com\" text=\"Test description\" class=\"testClass\" title=\"test alternative\" linktype=\"mailto\" />";

            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldLinkMapper/SetField");
            var field = item.Fields[FieldName];

            var value = new Link()
            {
                Anchor = "",
                Class = "testClass",
                Query = "",
                Target = "",
                TargetId = Guid.Empty,
                Text = "Test description",
                Title = "test alternative",
                Type = LinkType.MailTo,
                Url = "mailto:test@test.com"
            };


            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetField(field, value, null, null);
            }

            //Assert
            Assert.AreEqual(expected, field.Value);
        }

        [Test]
        public void SetField_InternalLink_InternalLinkSetOnField()
        {
            //Assign
            var mapper = new SitecoreFieldLinkMapper();
            var expected =
                "<link target=\"testTarget\" title=\"test alternative\" querystring=\"q=s\" linktype=\"internal\" id=\"{1AE37F34-3B6F-4F5F-A5C2-FD2B9D5A47A0}\" anchor=\"testAnchor\" url=\"/en/sitecore/content/Tests/DataMappers/SitecoreFieldLinkMapper/Target.aspx\" class=\"testClass\" text=\"Test description\" />";
            
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldLinkMapper/SetField");
            var field = item.Fields[FieldName];

            var value = new Link()
            {
                Anchor = "testAnchor",
                Class = "testClass",
                Query = "q=s",
                Target = "testTarget",
                TargetId = new Guid("{1AE37F34-3B6F-4F5F-A5C2-FD2B9D5A47A0}"),
                Text = "Test description",
                Title = "test alternative",
                Type = LinkType.Internal,
                Url = ""
            };


            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetField(field, value, null, null);
            }

            //Assert
            Assert.AreEqual(expected, field.Value);
        }

        [Test]
        [ExpectedException(typeof(MapperException))]
        public void SetField_InternalLinkMissingTarget_ThorwsException()
        {
            //Assign
            var mapper = new SitecoreFieldLinkMapper();

            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldLinkMapper/SetField");
            var field = item.Fields[FieldName];

            var value = new Link()
            {
                Anchor = "testAnchor",
                Class = "testClass",
                Query = "q=s",
                Target = "testTarget",
                TargetId = new Guid("{11111111-3B6F-4F5F-A5C2-FD2B9D5A47A0}"),
                Text = "Test description",
                Title = "test alternative",
                Type = LinkType.Internal,
                Url = ""
            };


            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetField(field, value, null, null);
            }

            //Assert
        }

        [Test]
        public void SetField_MedialLink_MediaLinkSetOnField()
        {
            //Assign
            var mapper = new SitecoreFieldLinkMapper();
            var expected =
                  "<link target=\"_blank\" title=\"test alternative\" linktype=\"media\" id=\"{8E1C32F3-CF15-4067-A3F4-85148606F9CD}\" url=\"/~/media/8E1C32F3CF154067A3F485148606F9CD.ashx\" class=\"testClass\" text=\"Test description\" />";


            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldLinkMapper/SetField");
            var field = item.Fields[FieldName];

            var value = new Link()
            {
                Anchor = "",
                Class = "testClass",
                Query = "",
                Target = "_blank",
                TargetId = new Guid("{8E1C32F3-CF15-4067-A3F4-85148606F9CD}"),
                Text = "Test description",
                Title = "test alternative",
                Type = LinkType.Media,
                Url = ""
            };


            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetField(field, value, null, null);
            }

            //Assert
            Assert.AreEqual(expected, field.Value);
        }

        [Test]
        [ExpectedException(typeof(MapperException))]
        public void SetField_MedialLinkMissingItem_ThrowsException()
        {
            //Assign
            var mapper = new SitecoreFieldLinkMapper();


            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldLinkMapper/SetField");
            var field = item.Fields[FieldName];

            var value = new Link()
            {
                Anchor = "",
                Class = "testClass",
                Query = "",
                Target = "_blank",
                TargetId = new Guid("{11111111-CF15-4067-A3F4-85148606F9CD}"),
                Text = "Test description",
                Title = "test alternative",
                Type = LinkType.Media,
                Url = ""
            };


            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetField(field, value, null, null);
            }

            //Assert
        }
        #endregion


    }
}
