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
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.DataMappers;
using Glass.Mapper.Sc.Fields;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;
using Sitecore.Links;

namespace Glass.Mapper.Sc.FakeDb.DataMappers
{
    [TestFixture]
    public class SitecoreFieldLinkMapperFixture
    {

        #region Method - GetField

        [Test]
        public void GetField_FieldContainsAnchor_ReturnsAnchorLink()
        {
            //Assign
            var templateId = ID.NewID;
            var targetId = ID.NewID;
            var fieldName = "Field";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {

                var mapper = new SitecoreFieldLinkMapper();
                var fieldValue =
                    "<link text=\"Test description\" linktype=\"anchor\" url=\"testAnchor\" anchor=\"testAnchor\" title=\"test alternate\" class=\"testClass\" />";

                var item = database.GetItem("/sitecore/content/Target");
                var field = item.Fields[fieldName];

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
        }

        [Test]
        public void GetField_FieldContainsExternal_ReturnsExternalLink()
        {
            //Assign
            var templateId = ID.NewID;
            var targetId = ID.NewID;
            var fieldName = "Field";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                var mapper = new SitecoreFieldLinkMapper();
                var fieldValue =
                    "<link text=\"Test description\" linktype=\"external\" url=\"http://www.google.com\" anchor=\"\" title=\"test alternative\" class=\"testClass\" target=\"_blank\" />";

                var item = database.GetItem("/sitecore/content/Target");
                var field = item.Fields[fieldName];

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
        }

        [Test]
        public void GetField_FieldContainsJavaScript_ReturnsJavascriptLink()
        {
            //Assign
            var templateId = ID.NewID;
            var targetId = ID.NewID;
            var fieldName = "Field";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                var mapper = new SitecoreFieldLinkMapper();
                var fieldValue =
                    "<link text=\"Test description\" linktype=\"javascript\" url=\"javascript:alert('hello world');\" anchor=\"\" title=\"test alternate\" class=\"testClass\" />";

                var item = database.GetItem("/sitecore/content/Target");
                var field = item.Fields[fieldName];

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
        }

        [Test]
        public void GetField_FieldContainsMailto_ReturnsMailtoLink()
        {
            //Assign
            var templateId = ID.NewID;
            var targetId = ID.NewID;
            var fieldName = "Field";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                var mapper = new SitecoreFieldLinkMapper();
                var fieldValue =
                    "<link text=\"Test description\" linktype=\"mailto\" url=\"mailto:test@test.com\" anchor=\"\" title=\"test alternate\" class=\"testClass\" />";

                var item = database.GetItem("/sitecore/content/Target");
                var field = item.Fields[fieldName];

                using (new ItemEditing(item, true))
                {
                    field.Value = fieldValue;
                }

                //Act
                var result = mapper.GetField(field, new SitecoreFieldConfiguration(), null) as Link;

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
        }

        [Test]
        public void GetField_FieldContainsInternal_ReturnsInternalLink()
        {
            //Assign
            var templateId = ID.NewID;
            var targetId = ID.NewID;
            var fieldName = "Field";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                var mapper = new SitecoreFieldLinkMapper(new FakeUrlOptionsResolver());
                var fieldValue =
                    "<link text=\"Test description\" linktype=\"internal\" url=\"/Target.aspx\" anchor=\"testAnchor\" querystring=\"q=s\" title=\"test alternative\" class=\"testClass\" target=\"testTarget\" id=\"{0}\" />"
                        .Formatted(targetId);
                    
                Sitecore.Context.Site = null;


                var item = database.GetItem("/sitecore/content/Target");
                var field = item.Fields[fieldName];

                using (new ItemEditing(item, true))
                {
                    field.Value = fieldValue;
                }

                Sitecore.Links.LinkProvider provider =
                     Substitute.For<Sitecore.Links.LinkProvider>();
                provider
                  .GetItemUrl(item, Arg.Any<Sitecore.Links.UrlOptions>())
                  .Returns("/target.aspx");


                using (new Sitecore.FakeDb.Links.LinkProviderSwitcher(provider))
                {
                    //Act
                    var result = mapper.GetField(field, new SitecoreFieldConfiguration(), null) as Link;

                    //Assert
                    Assert.AreEqual("testAnchor", result.Anchor);
                    Assert.AreEqual("testClass", result.Class);
                    Assert.AreEqual("q=s", result.Query);
                    Assert.AreEqual("testTarget", result.Target);
                    Assert.AreEqual(targetId.Guid, result.TargetId);
                    Assert.AreEqual("Test description", result.Text);
                    Assert.AreEqual("test alternative", result.Title);
                    Assert.AreEqual(LinkType.Internal, result.Type);
                    Assert.AreEqual("/target.aspx", result.Url);
                }
            }
        }

        [Test]
        public void GetField_FieldContainsInternalButItemMissing_ReturnsEmptyUrl()
        {
            //Assign
            var templateId = ID.NewID;
            var targetId = ID.NewID;
            var fieldName = "Field";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                //Assign
                var mapper = new SitecoreFieldLinkMapper();
                var fieldValue =
                    "<link text=\"Test description\" linktype=\"internal\" url=\"/Tests/DataMappers/SitecoreFieldLinkMapper/Target.aspx\" anchor=\"testAnchor\" querystring=\"q=s\" title=\"test alternative\" class=\"testClass\" target=\"testTarget\" id=\"{AAAAAAAA-3B6F-4F5F-A5C2-FD2B9D5A47A0}\" />";

                Sitecore.Context.Site = null;


                var item = database.GetItem("/sitecore/content/Target");
                var field = item.Fields[fieldName];

                using (new ItemEditing(item, true))
                {
                    field.Value = fieldValue;
                }

                //Act
                var result = mapper.GetField(field, new SitecoreFieldConfiguration(), null) as Link;

                //Assert
                Assert.AreEqual("testAnchor", result.Anchor);
                Assert.AreEqual("testClass", result.Class);
                Assert.AreEqual("q=s", result.Query);
                Assert.AreEqual("testTarget", result.Target);
                Assert.AreEqual(new Guid("{AAAAAAAA-3B6F-4F5F-A5C2-FD2B9D5A47A0}"), result.TargetId);
                Assert.AreEqual("Test description", result.Text);
                Assert.AreEqual("test alternative", result.Title);
                Assert.AreEqual(LinkType.Internal, result.Type);
                Assert.AreEqual(string.Empty, result.Url);
            }
        }

        [Test]
        public void GetField_FieldContainsInternalAbsoluteUrl_ReturnsInternalLink()
        {
            //Assign
            var templateId = ID.NewID;
            var targetId = ID.NewID;
            var fieldName = "Field";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                var mapper = new SitecoreFieldLinkMapper(new FakeUrlOptionsResolver());
                var fieldValue =
                    "<link text=\"Test description\" linktype=\"internal\" url=\"/Tests/DataMappers/SitecoreFieldLinkMapper/Target.aspx\" anchor=\"testAnchor\" querystring=\"q=s\" title=\"test alternative\" class=\"testClass\" target=\"testTarget\" id=\"{0}\" />"
                    .Formatted(targetId);

                Sitecore.Context.Site = null;


                var item = database.GetItem("/sitecore/content/Target");
                var field = item.Fields[fieldName];
                var config = new SitecoreFieldConfiguration();
                config.UrlOptions = SitecoreInfoUrlOptions.LanguageEmbeddingNever;

                using (new ItemEditing(item, true))
                {
                    field.Value = fieldValue;
                }

                Sitecore.Links.LinkProvider provider =
                   Substitute.For<Sitecore.Links.LinkProvider>();
                provider
                  .GetItemUrl(item, Arg.Is<Sitecore.Links.UrlOptions>(x=>x.LanguageEmbedding == LanguageEmbedding.Never))
                  .Returns("/target.aspx");


                using (new Sitecore.FakeDb.Links.LinkProviderSwitcher(provider))
                {

                    //Act
                    var result = mapper.GetField(field, config, null) as Link;

                    //Assert
                    Assert.AreEqual("testAnchor", result.Anchor);
                    Assert.AreEqual("testClass", result.Class);
                    Assert.AreEqual("q=s", result.Query);
                    Assert.AreEqual("testTarget", result.Target);
                    Assert.AreEqual(targetId.Guid, result.TargetId);
                    Assert.AreEqual("Test description", result.Text);
                    Assert.AreEqual("test alternative", result.Title);
                    Assert.AreEqual(LinkType.Internal, result.Type);
                    Assert.AreEqual("/target.aspx",
                        result.Url);
                }
            }
        }

        [Test]
        public void GetField_FieldContainsInternalMissingItem_ReturnsNotSetLink()
        {
            //Assign
            var templateId = ID.NewID;
            var targetId = ID.NewID;
            var fieldName = "Field";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                var mapper = new SitecoreFieldLinkMapper(new FakeUrlOptionsResolver());
                var fieldValue =
                    "<link text=\"Test description\" linktype=\"internal\" url=\"/Target.aspx\" anchor=\"testAnchor\" querystring=\"q=s\" title=\"test alternative\" class=\"testClass\" target=\"testTarget\" id=\"{11111111-3B6F-4F5F-A5C2-FD2B9D5A47A0}\" />";

                var item = database.GetItem("/sitecore/content/Target");
                var field = item.Fields[fieldName];

                using (new ItemEditing(item, true))
                {
                    field.Value = fieldValue;
                }

                //Act
                var result = mapper.GetField(field, new SitecoreFieldConfiguration(), null) as Link;

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
        }

        [Test]
        public void GetField_FieldContainsMediaLink_ReturnsMediaLink()
        {
            //Assign
            var templateId = ID.NewID;
            var targetId = ID.NewID;
            var fieldName = "Field";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                var mapper = new SitecoreFieldLinkMapper(new FakeUrlOptionsResolver());
                var fieldValue =
                    "<link text=\"Test description\" linktype=\"media\" url=\"/Files/20121222_001405\" title=\"test alternative\" class=\"testClass\" target=\"_blank\" id=\"{0}\" />"
                    .Formatted(targetId);

                var item = database.GetItem("/sitecore/content/Target");
                var field = item.Fields[fieldName];

                using (new ItemEditing(item, true))
                {
                    field.Value = fieldValue;
                }
                Sitecore.Resources.Media.MediaProvider mediaProvider =
                    NSubstitute.Substitute.For<Sitecore.Resources.Media.MediaProvider>();

                mediaProvider
                    .GetMediaUrl(Arg.Is<Sitecore.Data.Items.MediaItem>(i => i.ID == targetId))
                    .Returns("Media.Aspx");

                // substitute the original provider with the mocked one
                using (new Sitecore.FakeDb.Resources.Media.MediaProviderSwitcher(mediaProvider))
                {
                    //Act
                    var result = mapper.GetField(field, null, null) as Link;

                    //Assert
                    Assert.AreEqual("", result.Anchor);
                    Assert.AreEqual("testClass", result.Class);
                    Assert.AreEqual("", result.Query);
                    Assert.AreEqual("_blank", result.Target);
                    Assert.AreEqual(targetId.Guid, result.TargetId);
                    Assert.AreEqual("Test description", result.Text);
                    Assert.AreEqual("test alternative", result.Title);
                    Assert.AreEqual(LinkType.Media, result.Type);
                    Assert.AreEqual("Media.Aspx", result.Url);
                }
            }
        }

        [Test]
        public void GetField_FieldContainsMediaLinkMissingItem_ReturnsNotSetLink()
        {
            //Assign
            var templateId = ID.NewID;
            var targetId = ID.NewID;
            var fieldName = "Field";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                var mapper = new SitecoreFieldLinkMapper(new FakeUrlOptionsResolver());
                var fieldValue =
                    "<link text=\"Test description\" linktype=\"media\" url=\"/Files/20121222_001405\" title=\"test alternative\" class=\"testClass\" target=\"_blank\" id=\"{11111111-CF15-4067-A3F4-85148606F9CD}\" />";

                var item = database.GetItem("/sitecore/content/Target");
                var field = item.Fields[fieldName];

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
        }

        #endregion
        //#region Method - SetField


        [Test]
        public void SetField_AnchorLink_AnchorLinkSetOnField()
        {
            //Assign
            var templateId = ID.NewID;
            var targetId = ID.NewID;
            var fieldName = "Field";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                var mapper = new SitecoreFieldLinkMapper(new FakeUrlOptionsResolver());
                var expected =
                    "<link title=\"test alternative\" linktype=\"anchor\" url=\"testAnchor\" anchor=\"testAnchor\" class=\"testClass\" text=\"Test description\" />";

                var item = database.GetItem("/sitecore/content/Target");
                var field = item.Fields[fieldName];

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
                AssertHtml.AreHtmlElementsEqual(expected, field.Value, "link");
            }
        }

        [Test]
        public void SetField_ExternalLink_ExternalLinkSetOnField()
        {
            //Assign
            var templateId = ID.NewID;
            var targetId = ID.NewID;
            var fieldName = "Field";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                var mapper = new SitecoreFieldLinkMapper(new FakeUrlOptionsResolver());
                var expected =
                    "<link url=\"http://www.google.com\" text=\"Test description\" class=\"testClass\" title=\"test alternative\" linktype=\"external\" />";

                var item = database.GetItem("/sitecore/content/Target");
                var field = item.Fields[fieldName];

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
                AssertHtml.AreHtmlElementsEqual(expected, field.Value,"link");
            }
        }

        [Test]
        public void SetField_JavaScriptLink_JavaScriptLinkSetOnField()
        {
            //Assign
            var templateId = ID.NewID;
            var targetId = ID.NewID;
            var fieldName = "Field";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                var mapper = new SitecoreFieldLinkMapper(new FakeUrlOptionsResolver());
                var expected =
                    "<link url=\"javascript:alert('hello world');\" text=\"Test description\" class=\"testClass\" title=\"test alternative\" linktype=\"javascript\" />";

                var item =database.GetItem("/sitecore/content/Target");
                var field = item.Fields[fieldName];

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
                AssertHtml.AreHtmlElementsEqual(expected, field.Value, "link");
            }
        }

        [Test]
        public void SetField_MailToLink_MailToLinkSetOnField()
        {
            //Assign
            var templateId = ID.NewID;
            var targetId = ID.NewID;
            var fieldName = "Field";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                var mapper = new SitecoreFieldLinkMapper(new FakeUrlOptionsResolver());
                var expected =
                    "<link url=\"mailto:test@test.com\" text=\"Test description\" class=\"testClass\" title=\"test alternative\" linktype=\"mailto\" />";

                var item = database.GetItem("/sitecore/content/Target");
                var field = item.Fields[fieldName];

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
                AssertHtml.AreHtmlElementsEqual(expected, field.Value, "link");
            }
        }

        [Test]
        public void SetField_InternalLink_InternalLinkSetOnField()
        {
            //Assign
            var templateId = ID.NewID;
            var targetId = ID.NewID;
            var fieldName = "Field";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                var mapper = new SitecoreFieldLinkMapper(new FakeUrlOptionsResolver());
                var expected =
                    "<link target=\"testTarget\" title=\"test alternative\" querystring=\"q=s\" linktype=\"internal\" id=\"{0}\" anchor=\"testAnchor\" url=\"/en/sitecore/content/Target.aspx\" class=\"testClass\" text=\"Test description\" />"
                        .Formatted(targetId.Guid.ToString("B").ToUpperInvariant());

                var item = database.GetItem("/sitecore/content/Target");
                var field = item.Fields[fieldName];

                var value = new Link()
                {
                    Anchor = "testAnchor",
                    Class = "testClass",
                    Query = "q=s",
                    Target = "testTarget",
                    TargetId = targetId.Guid,
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
                AssertHtml.AreHtmlElementsEqual(expected, field.Value, "link");
            }
        }

        [Test]
        public void SetField_InternalLinkMissingTarget_ThorwsException()
        {
            //Assign
            var templateId = ID.NewID;
            var targetId = ID.NewID;
            var fieldName = "Field";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                var mapper = new SitecoreFieldLinkMapper(new FakeUrlOptionsResolver());

                var item = database.GetItem("/sitecore/content/Target");
                var field = item.Fields[fieldName];

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
                    Assert.Throws<MapperException>(() =>
                    {
                        mapper.SetField(field, value, null, null);

                    });
                }

                //Assert
            }
        }

        [Test]
        public void SetField_MedialLink_MediaLinkSetOnField()
        {
            //Assign
            var templateId = ID.NewID;
            var targetId = ID.NewID;
            var fieldName = "Field";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                var mapper = new SitecoreFieldLinkMapper(new FakeUrlOptionsResolver());
                var expected =
                    "<link target=\"_blank\" title=\"test alternative\" linktype=\"media\" id=\"{0}\" url=\"Media.Aspx\" class=\"testClass\" text=\"Test description\" />"
                                            .Formatted(targetId.Guid.ToString("B").ToUpperInvariant());



                var item = database.GetItem("/sitecore/content/Target");
                var field = item.Fields[fieldName];

                var value = new Link()
                {
                    Anchor = "",
                    Class = "testClass",
                    Query = "",
                    Target = "_blank",
                    TargetId = targetId.Guid,
                    Text = "Test description",
                    Title = "test alternative",
                    Type = LinkType.Media,
                    Url = ""
                };


                using (new ItemEditing(item, true))
                {
                    field.Value = string.Empty;
                }

                Sitecore.Resources.Media.MediaProvider mediaProvider =
                  NSubstitute.Substitute.For<Sitecore.Resources.Media.MediaProvider>();

                mediaProvider
                    .GetMediaUrl(Arg.Is<Sitecore.Data.Items.MediaItem>(i => i.ID == targetId))
                    .Returns("Media.Aspx");

                // substitute the original provider with the mocked one
                using (new Sitecore.FakeDb.Resources.Media.MediaProviderSwitcher(mediaProvider))
                {
                    //Act
                    using (new ItemEditing(item, true))
                    {
                        mapper.SetField(field, value, null, null);
                    }

                    //Assert
                    AssertHtml.AreHtmlElementsEqual(expected, field.Value, "link");
                }
            }
        }

        [Test]
        public void SetField_MedialLinkMissingItem_ThrowsException()
        {
            //Assign
            var templateId = ID.NewID;
            var targetId = ID.NewID;
            var fieldName = "Field";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                var mapper = new SitecoreFieldLinkMapper(new FakeUrlOptionsResolver());


                var item = database.GetItem("/sitecore/content/Target");
                var field = item.Fields[fieldName];

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
                    Assert.Throws<MapperException>(() => {
                                                             mapper.SetField(field, value, null, null);
                    });
                }

                //Assert
            }
        }

        //#endregion


        public class FakeUrlOptionsResolver : UrlOptionsResolver
        {
            public override UrlOptions CreateUrlOptions(SitecoreInfoUrlOptions urlOptions)
            {
                return base.CreateUrlOptions(urlOptions, new UrlOptions());
            }
        }
    }
}




