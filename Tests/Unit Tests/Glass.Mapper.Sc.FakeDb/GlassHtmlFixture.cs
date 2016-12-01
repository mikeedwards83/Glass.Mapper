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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;
using Sitecore.SecurityModel;
using Sitecore.Sites;
using Sitecore.Web;
using Site = System.Security.Policy.Site;

namespace Glass.Mapper.Sc.FakeDb
{
    [TestFixture]
    public class GlassHtmlFixture
    {

        #region Method - RenderLink

        /// <summary>
        /// https://github.com/mikeedwards83/Glass.Mapper/issues/155
        /// </summary>
        [Test]
        public void RenderLink_LinkContainsAnchor_Issue155()
        {
            //Assign
            string path = "/sitecore/content/target";

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
                {
                    {"StringField","" }
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);

                var html = GetGlassHtml(service);


                string fieldValue = "<link text='text' linktype='anchor' anchor='footer' title='' class='' />";
                string expected = "<a href='#footer' >text</a>";

                var item = database.GetItem(path);
                var field = item.Fields["StringField"];

                using (new ItemEditing(item, true))
                {
                    field.Value = fieldValue;
                }

                var model = service.GetItem<IStubLinkClass>(path);

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                var siteContext = new SiteContextStub(
                    new SiteInfo(
                        doc.FirstChild
                        )
                    );

                siteContext.SetDisplayMode(DisplayMode.Normal);

                Sitecore.Context.Site = siteContext;

                StringBuilder sb = new StringBuilder();
                //Act
                var result = html.RenderLink(model, x => x.Link);

                //Assert
                Assert.AreEqual(expected, result);
            }
        }

        /// <summary>
        /// https://github.com/mikeedwards83/Glass.Mapper/issues/116
        /// </summary>
        [Test]
        public void RenderLink_LinkContainsQueryString_Issue116()
        {
            //Assign
            string targetPath = "/sitecore/content/target";

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
                {
                    {"StringField", ""}
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);
                var html = GetGlassHtml(service);

                var item = database.GetItem(targetPath);

                string fieldValue =
                    "<link text='' linktype='internal' class='' title=''  querystring='foo=bar' id='{0}' />".Formatted(
                        item.ID.Guid.ToString("B"));
                string expected =
                    "<a href='/en/sitecore/content/Target.aspx?foo=bar' >Target</a>";

                var field = item.Fields["StringField"];

                using (new ItemEditing(item, true))
                {
                    field.Value = fieldValue;
                }

                var model = service.GetItem<IStubLinkClass>(targetPath);

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                var siteContext = new SiteContextStub(
                    new SiteInfo(
                        doc.FirstChild
                        )
                    );

                siteContext.SetDisplayMode(DisplayMode.Normal);

                Sitecore.Context.Site = siteContext;

                StringBuilder sb = new StringBuilder();
                //Act
                var result = html.RenderLink(model, x => x.Link);

                //Assert
                Assert.AreEqual(expected, result);
            }
        }

        #endregion



        #region Method - RenderImage


        /// <summary>
        /// https://github.com/mikeedwards83/Glass.Mapper/issues/133
        /// </summary>
        [Test]
        [Ignore("This test has to be run manually and compare manually because the order of parameters is different")]
        public void RenderImage_MatchesSitecoreOutput_Issue133()
        {
            //Assign
            string targetPath = "/sitecore/content/target";
            var mediaID = new ID("{D897833C-1F53-4FAE-B54B-BB5B11B8F851}");

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate("MediaTemplate", templateId)
                {
                    new DbField("Image")
                    {
                        Type =  "image"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target",new ID(), templateId)
                {
                    {"Image", ""}
                },
                new Sitecore.FakeDb.DbItem("media", mediaID)
                {
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);

                var html = GetGlassHtml(service);

                Sitecore.Resources.Media.MediaProvider mediaProvider =
                    NSubstitute.Substitute.For<Sitecore.Resources.Media.MediaProvider>();

                mediaProvider
                    .GetMediaUrl(Arg.Is<Sitecore.Data.Items.MediaItem>(i => i.ID == mediaID))
                    .Returns("/myimage");

                using (new Sitecore.FakeDb.Resources.Media.MediaProviderSwitcher(mediaProvider))
                {

                    string fieldValue =
                        "<image mediaid=\"{D897833C-1F53-4FAE-B54B-BB5B11B8F851}\" mediapath=\"/Files/20121222_001405\" src=\"~/media/D897833C1F534FAEB54BBB5B11B8F851.ashx\" hspace=\"15\" vspace=\"20\" />";

                    var item = database.GetItem(targetPath);
                    var field = item.Fields["Image"];

                    using (new ItemEditing(item, true))
                    {
                        field.Value = fieldValue;
                    }

                    var model = service.GetItem<StubClassWithImage>(targetPath);


                    var scControl = new Sitecore.Web.UI.WebControls.Image();
                    scControl.Item = item;
                    scControl.Field = "Image";
                    scControl.Parameters = "mw=200";

                    var doc = new XmlDocument();
                    doc.LoadXml(
                        "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                    var siteContext = new SiteContextStub(
                        new SiteInfo(
                            doc.FirstChild
                            )
                        );

                    siteContext.SetDisplayMode(DisplayMode.Normal);

                    Sitecore.Context.Site = siteContext;

                    StringBuilder sb = new StringBuilder();
                    HtmlTextWriter writer = new HtmlTextWriter(new StringWriter(sb));
                    //Act
                    scControl.RenderControl(writer);
                    var scResult = sb.ToString();
                    var result = html.RenderImage(model, x => x.Image, new { mw = 200 });

                    //Assert
                    Assert.AreEqual(result, scResult);
                }
            }

        }

        [Test]
        [Ignore("This test has to be run manually and compare manually because the order of parameters is different")]
        public void RenderImage_MatchesSitecoreOutput_Issue133_Test2()
        {
            //Assign

            string targetPath = "/sitecore/content/target";
            var mediaID = new ID("{D897833C-1F53-4FAE-B54B-BB5B11B8F851}");

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate("MediaTemplate", templateId)
                {
                    new DbField("Image")
                    {
                        Type =  "image"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target",new ID(), templateId)
                {
                    {"Image", ""}
                },
                new Sitecore.FakeDb.DbItem("media", mediaID)
                {
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);
                var html = GetGlassHtml(service);

                Sitecore.Resources.Media.MediaProvider mediaProvider =
              NSubstitute.Substitute.For<Sitecore.Resources.Media.MediaProvider>();

                mediaProvider
                    .GetMediaUrl(Arg.Is<Sitecore.Data.Items.MediaItem>(i => i.ID == mediaID))
                    .Returns("/myimage");

                using (new Sitecore.FakeDb.Resources.Media.MediaProviderSwitcher(mediaProvider))
                {
                    string fieldValue =
                    "<image mediaid=\"{D897833C-1F53-4FAE-B54B-BB5B11B8F851}\" mediapath=\"/Files/20121222_001405\" src=\"~/media/D897833C1F534FAEB54BBB5B11B8F851.ashx\" hspace=\"15\" vspace=\"20\" />";

                    var item = database.GetItem(targetPath);
                    var field = item.Fields["Image"];

                    using (new ItemEditing(item, true))
                    {
                        field.Value = fieldValue;
                    }

                    var model = service.GetItem<StubClassWithImage>(targetPath);


                    var scControl = new Sitecore.Web.UI.WebControls.Image();
                    scControl.Item = item;
                    scControl.Field = "Image";
                    scControl.Parameters = "w=200&as=true";

                    var doc = new XmlDocument();
                    doc.LoadXml(
                        "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                    var siteContext = new SiteContextStub(
                        new SiteInfo(
                            doc.FirstChild
                            )
                        );

                    siteContext.SetDisplayMode(DisplayMode.Normal);

                    Sitecore.Context.Site = siteContext;

                    StringBuilder sb = new StringBuilder();
                    HtmlTextWriter writer = new HtmlTextWriter(new StringWriter(sb));
                    //Act
                    scControl.RenderControl(writer);
                    var scResult = sb.ToString();
                    var result = html.RenderImage(model, x => x.Image, new { mw = 200, As = true });

                    //Assert
                    Assert.AreEqual(result, scResult);
                }

            }
        }

        [Test]
        [Ignore("This test has to be run manually and compare manually because the order of parameters is different")]
        public void RenderImage_MatchesSitecoreOutput_Issue133_Test3()
        {
            //Assign
            string targetPath = "/sitecore/content/target";
            var mediaID = new ID("{D897833C-1F53-4FAE-B54B-BB5B11B8F851}");

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate("MediaTemplate", templateId)
                {
                    new DbField("Image")
                    {
                        Type = "image"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", new ID(), templateId)
                {
                    {"Image", ""}
                },
                new Sitecore.FakeDb.DbItem("media", mediaID)
                {
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);
                var html = GetGlassHtml(service);

                Sitecore.Resources.Media.MediaProvider mediaProvider =
                    NSubstitute.Substitute.For<Sitecore.Resources.Media.MediaProvider>();

                mediaProvider
                    .GetMediaUrl(Arg.Is<Sitecore.Data.Items.MediaItem>(i => i.ID == mediaID))
                    .Returns("/myimage");

                using (new Sitecore.FakeDb.Resources.Media.MediaProviderSwitcher(mediaProvider))
                {


                    string fieldValue =
                        "<image mediaid=\"{D897833C-1F53-4FAE-B54B-BB5B11B8F851}\" mediapath=\"/Files/20121222_001405\" src=\"~/media/D897833C1F534FAEB54BBB5B11B8F851.ashx\" hspace=\"15\" vspace=\"20\" />";

                    var item = database.GetItem(targetPath);
                    var field = item.Fields["Image"];

                    using (new ItemEditing(item, true))
                    {
                        field.Value = fieldValue;
                    }

                    var model = service.GetItem<StubClassWithImage>(targetPath);


                    var scControl = new Sitecore.Web.UI.WebControls.Image();
                    scControl.Item = item;
                    scControl.Field = "Image";
                    scControl.Parameters = "width=200&as=true";

                    var doc = new XmlDocument();
                    doc.LoadXml(
                        "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                    var siteContext = new SiteContextStub(
                        new SiteInfo(
                            doc.FirstChild
                            )
                        );

                    siteContext.SetDisplayMode(DisplayMode.Normal);

                    Sitecore.Context.Site = siteContext;

                    StringBuilder sb = new StringBuilder();
                    HtmlTextWriter writer = new HtmlTextWriter(new StringWriter(sb));
                    //Act
                    scControl.RenderControl(writer);
                    var scResult = sb.ToString();
                    var result = html.RenderImage(model, x => x.Image, new { width = 200 });

                    //Assert
                    Assert.AreEqual(result, scResult);
                }

            }
        }

        #endregion
        #region Method - Editable

        [Test]
        public void Editable_InEditMode_StringFieldWithEditReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/target";

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    new DbField("StringField")
                    {
                        Type = "text"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    {"StringField", ""}
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<StubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.Save(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                var siteContext = new SiteContextStub(
                    new SiteInfo(
                        doc.FirstChild
                        )
                    );

                using (new EnableWebEditMode())
                {
                    using (new SiteContextSwitcher(siteContext))
                    {
                        //Act
                        string result;

                        using (new SecurityDisabler())
                        {
                            result = html.Editable(model, x => x.StringField);
                        }

                        //Assert
                        Assert.IsTrue(result.Contains(fieldValue));
                        //this is the webedit class
                        Console.WriteLine("result " + result);

                        Assert.IsTrue(result.Contains("scWebEditInput"));
                    }
                }
            }

        }


        [Test]
        public void Editable_NotInEditMode_StringFieldReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/target";

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    new DbField("StringField")
                    {
                        Type = "text"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    {"StringField", ""}
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<StubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.Save(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                var siteContext = new SiteContextStub(
                    new SiteInfo(
                        doc.FirstChild
                        )
                    );

                siteContext.SetDisplayMode(DisplayMode.Normal);

                Sitecore.Context.Site = siteContext;


                //Act
                var result = html.Editable(model, x => x.StringField);

                //Assert
                Assert.IsTrue(result.Contains(fieldValue));
                //this is the webedit class
                Assert.IsFalse(result.Contains("scWebEditInput"));
                Console.WriteLine("result " + result);
            }
        }



        [Test]
        public void Editable_SimpleLambdaInEditMode_StringFieldWithEditReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/target";

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    new DbField("StringField")
                    {
                        Type = "text"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    {"StringField", ""}
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<StubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.Save(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                var siteContext = new SiteContextStub(
                    new SiteInfo(
                        doc.FirstChild
                        )
                    );

                using (new EnableWebEditMode())
                {
                    using (new SiteContextSwitcher(siteContext))
                    {

                        //Act
                        string result;


                        using (new SecurityDisabler())
                        {
                            result = html.Editable(model, x => x.SubStub.StringField);
                        }

                        //Assert
                        Assert.IsTrue(result.Contains(fieldValue));
                        //this is the webedit class
                        Assert.IsTrue(result.Contains("scWebEditInput"));
                        Console.WriteLine("result " + result);
                    }
                }
            }

        }

        [Test]
        public void Editable_SimpleLambdaInEditModeInterface_StringFieldWithEditReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/target";

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    new DbField("StringField")
                    {
                        Type = "text"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    {"StringField", ""}
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);


                var html = new GlassHtml(service);

                var model = service.GetItem<IStubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.Save(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                var siteContext = new SiteContextStub(
                    new SiteInfo(
                        doc.FirstChild
                        )
                    );

                using (new EnableWebEditMode())
                {
                    using (new SiteContextSwitcher(siteContext))
                    {

                        //Act
                        string result;

                        using (new SecurityDisabler())
                        {
                            result = html.Editable(model, x => x.SubStub.StringField);
                        }
                        //Assert
                        Assert.IsTrue(result.Contains(fieldValue));
                        //this is the webedit class
                        Assert.IsTrue(result.Contains("scWebEditInput"));
                        Console.WriteLine("result " + result);
                    }
                }
            }
        }

        [Test]
        public void Editable_SimpleLambdaInEditModeInterfaceClearingContext_StringFieldWithEditReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/target";

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    new DbField("StringField")
                    {
                        Type = "text"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    {"StringField", ""}
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);

                var html = new GlassHtml(service);

                var model = service.GetItem<IStubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;


                using (new SecurityDisabler())
                {
                    service.Save(model);
                }


                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                var siteContext = new SiteContextStub(
                    new SiteInfo(
                        doc.FirstChild
                        )
                    );

                siteContext.SetDisplayMode(DisplayMode.Edit);

                Sitecore.Context.Site = siteContext;

                //Act
                string result;


                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(System.Object)));
                using (new EnableWebEditMode())
                {
                    using (new SiteContextSwitcher(siteContext))
                    {
                        // model = null;
                        using (new SecurityDisabler())
                        {
                            result = html.Editable(model, x => x.StringField);
                        }
                        //Assert
                        Assert.IsTrue(result.Contains(fieldValue));
                        //this is the webedit class
                        Assert.IsTrue(result.Contains("scWebEditInput"));
                        Console.WriteLine("result " + result);
                    }
                }
            }

        }

        [Test]
        public void Editable_SimpleLambdaInEditModeInterfaceWithInheritance_StringFieldWithEditReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/target";

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    new DbField("StringField")
                    {
                        Type = "text"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    {"StringField", ""}
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);

                var html = new GlassHtml(service);

                var model = service.GetItem<IStubClassInherits>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.Save(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                var siteContext = new SiteContextStub(
                    new SiteInfo(
                        doc.FirstChild
                        )
                    );

                using (new EnableWebEditMode())
                {
                    using (new SiteContextSwitcher(siteContext))
                    {

                        //Act
                        string result;

                        using (new SecurityDisabler())
                        {
                            result = html.Editable(model, x => x.SubStub.StringField);
                        }
                        //Assert
                        Assert.IsTrue(result.Contains(fieldValue));
                        //this is the webedit class
                        Assert.IsTrue(result.Contains("scWebEditInput"));
                        Console.WriteLine("result " + result);
                    }
                }
            }
        }

        [Test]
        public void Editable_SimpleLambdaNotInEditMode_StringFieldReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/target";

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    new DbField("StringField")
                    {
                        Type = "text"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    {"StringField", ""}
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);
                var html = GetGlassHtml(service);

                var model = service.GetItem<StubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.Save(model);
                }

                model = service.GetItem<StubClass>(targetPath);


                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                var siteContext = new SiteContextStub(
                    new SiteInfo(
                        doc.FirstChild
                        )
                    );

                using (new SiteContextSwitcher(siteContext))
                {


                    //Act
                    var result = html.Editable(model, x => x.SubStub.StringField);

                    //Assert
                    Assert.IsTrue(result.Contains(fieldValue));
                    //this is the webedit class
                    Assert.IsFalse(result.Contains("scWebEditInput"));
                    Console.WriteLine("result " + result);
                }
            }
        }

        [
            Test]
        public void Editable_ComplexLambdaInEditMode_StringFieldWithEditReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/target";

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    new DbField("StringField")
                    {
                        Type = "text"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    {"StringField", ""}
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<StubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.Save(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                var siteContext = new SiteContextStub(
                    new SiteInfo(
                        doc.FirstChild
                        )
                    );

                using (new EnableWebEditMode())
                {
                    using (new SiteContextSwitcher(siteContext))
                    {

                        //Act
                        string result;

                        using (new SecurityDisabler())
                        {
                            result = html.Editable(model, x => x.EnumerableSubStub.First().StringField);
                        }
                        //Assert
                        Assert.IsTrue(result.Contains(fieldValue));

                        Console.WriteLine("result " + result);

                        //this is the webedit class
                        Assert.IsTrue(result.Contains("scWebEditInput"));
                    }
                }
            }
        }

        [Test]
        public void Editable_ComplexLambdaInEditModeUsingInterface_StringFieldWithEditReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/target";

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    new DbField("StringField")
                    {
                        Type = "text"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    {"StringField", ""}
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);

                var html = new GlassHtml(service);

                var model = service.GetItem<IStubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.Save(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                var siteContext = new SiteContextStub(
                    new SiteInfo(
                        doc.FirstChild
                        )
                    );

                using (new EnableWebEditMode())
                {
                    using (new SiteContextSwitcher(siteContext))
                    {

                        //Act
                        string result;

                        using (new SecurityDisabler())
                        {
                            result = html.Editable(model, x => x.EnumerableSubStub.First().StringField);
                        }
                        //Assert
                        Assert.IsTrue(result.Contains(fieldValue));
                        //this is the webedit class
                        Assert.IsTrue(result.Contains("scWebEditInput"));
                        Console.WriteLine("result " + result);
                    }
                }
            }
        }

        [Test]
        public void Editable_ComplexLambdaInEditModeUsingInterfaceInheritance_StringFieldWithEditReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/target";

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    new DbField("StringField")
                    {
                        Type = "text"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    {"StringField", ""}
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);

                var html = new GlassHtml(service);

                var model = service.GetItem<IStubClassInherits>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.Save(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                var siteContext = new SiteContextStub(
                    new SiteInfo(
                        doc.FirstChild
                        )
                    );

                using (new EnableWebEditMode())
                {
                    using (new SiteContextSwitcher(siteContext))
                    {

                        //Act
                        string result;

                        using (new SecurityDisabler())
                        {
                            result = html.Editable(model, x => x.EnumerableSubStub.First().StringField);
                        }
                        //Assert
                        Assert.IsTrue(result.Contains(fieldValue));
                        //this is the webedit class
                        Assert.IsTrue(result.Contains("scWebEditInput"));
                        Console.WriteLine("result " + result);
                    }
                }
            }
        }


        [Test]
        public void Editable_InterfaceComplexLambdaNotInEditMode_StringFieldReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/target";

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    new DbField("StringField")
                    {
                        Type = "text"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    {"StringField", ""}
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<StubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.Save(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                var siteContext = new SiteContextStub(
                    new SiteInfo(
                        doc.FirstChild
                        )
                    );

                using (new SiteContextSwitcher(siteContext))
                {


                    //Act
                    var result = html.Editable(model, x => x.EnumerableSubStub.First().StringField);

                    //Assert
                    Assert.IsTrue(result.Contains(fieldValue));
                    //this is the webedit class
                    Assert.IsFalse(result.Contains("scWebEditInput"));
                    Console.WriteLine("result " + result);
                }
            }
        }

        [Test]
        public void Editable_InterfaceInEditMode_StringFieldWithEditReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/target";

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    new DbField("StringField")
                    {
                        Type = "text"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    {"StringField", ""}
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<IStubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.Save(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                var siteContext = new SiteContextStub(
                    new SiteInfo(
                        doc.FirstChild
                        )
                    );
                using (new EnableWebEditMode())
                {
                    using (new SiteContextSwitcher(siteContext))
                    {

                        //Act
                        string result;

                        using (new SecurityDisabler())
                        {
                            result = html.Editable(model, x => x.StringField);
                        }
                        //Assert
                        Assert.IsTrue(result.Contains(fieldValue));
                        //this is the webedit class
                        Assert.IsTrue(result.Contains("scWebEditInput"));
                        Console.WriteLine("result " + result);
                    }
                }
            }
        }

        [Test]
        public void Editable_InterfaceNotInEditMode_StringFieldReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/target";

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    new DbField("StringField")
                    {
                        Type = "text"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    {"StringField", ""}
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<IStubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.Save(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                var siteContext = new SiteContextStub(
                    new SiteInfo(
                        doc.FirstChild
                        )
                    );

                using (new SiteContextSwitcher(siteContext))
                {


                    //Act
                    var result = html.Editable(model, x => x.StringField);

                    //Assert
                    Assert.IsTrue(result.Contains(fieldValue));
                    //this is the webedit class
                    Assert.IsFalse(result.Contains("scWebEditInput"));
                    Console.WriteLine("result " + result);
                }
            }
        }

        [Test]
        public void Editable_InterfaceSimpleLambdaInEditMode_StringFieldWithEditReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/target";

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    new DbField("StringField")
                    {
                        Type = "text"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    {"StringField", ""}
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<IStubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.Save(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                var siteContext = new SiteContextStub(
                    new SiteInfo(
                        doc.FirstChild
                        )
                    );

                using (new EnableWebEditMode())
                {
                    using (new SiteContextSwitcher(siteContext))
                    {

                        //Act
                        string result;

                        using (new SecurityDisabler())
                        {
                            result = html.Editable(model, x => x.SubStub.StringField);
                        }
                        //Assert
                        Assert.IsTrue(result.Contains(fieldValue));
                        //this is the webedit class
                        Assert.IsTrue(result.Contains("scWebEditInput"), "result " + result);
                    }
                }
            }
        }

        [Test]
        public void Editable_InterfaceSimpleLambdaNotInEditMode_StringFieldReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/target";

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    new DbField("StringField")
                    {
                        Type = "text"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    {"StringField", ""}
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);
                var html = GetGlassHtml(service);

                var model = service.GetItem<IStubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.Save(model);
                }
                model = service.GetItem<IStubClass>(targetPath);

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                var siteContext = new SiteContextStub(
                    new SiteInfo(
                        doc.FirstChild
                        )
                    );

                using (new SiteContextSwitcher(siteContext))
                {

                    //Act
                    var result = html.Editable(model, x => x.SubStub.StringField);

                    //Assert
                    Assert.IsTrue(result.Contains(fieldValue));
                    //this is the webedit class
                    Assert.IsFalse(result.Contains("scWebEditInput"), "result " + result);
                }
            }
        }

        [Test]
        public void Editable_InterfaceComplexLambdaInEditMode_StringFieldWithEditReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/target";

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    new DbField("StringField")
                    {
                        Type = "text"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    {"StringField", ""}
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<IStubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.Save(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                var siteContext = new SiteContextStub(
                    new SiteInfo(
                        doc.FirstChild
                        )
                    );

                using (new EnableWebEditMode())
                {
                    using (new SiteContextSwitcher(siteContext))
                    {

                        //Act
                        string result;

                        using (new SecurityDisabler())
                        {
                            result = html.Editable(model, x => x.EnumerableSubStub.First().StringField);
                        }
                        //Assert
                        Assert.Equals("dd", "result " + result);
                        Assert.IsTrue(result.Contains(fieldValue));
                        //this is the webedit class
                        Assert.IsTrue(result.Contains("scWebEditInput"), "result " + result);
                    }
                }
            }
        }

        [Test]
        public void Editable_ComplexLambdaNotInEditMode_StringFieldReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/target";

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    new DbField("StringField")
                    {
                        Type = "text"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    {"StringField", ""}
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<IStubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.Save(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                var siteContext = new SiteContextStub(
                    new SiteInfo(
                        doc.FirstChild
                        )
                    );

                using (new SiteContextSwitcher(siteContext))
                {

                    //Act
                    string result;

                    using (new SecurityDisabler())
                    {
                        result = html.Editable(model, x => x.EnumerableSubStub.First().StringField);
                    }
                    //Assert
                    Assert.IsTrue(result.Contains(fieldValue));
                    //this is the webedit class
                    Assert.IsFalse(result.Contains("scWebEditInput"));
                    Console.WriteLine("result " + result);
                }
            }
        }

        [Test]
        public void Editable_InEditModeWithStandardOutput_StringFieldWithEditReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/target";

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    new DbField("StringField")
                    {
                        Type = "text"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    {"StringField", ""}
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<StubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.Save(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                var siteContext = new SiteContextStub(
                    new SiteInfo(
                        doc.FirstChild
                        )
                    );

                using (new EnableWebEditMode())
                {
                    using (new SiteContextSwitcher(siteContext))
                    {

                        //Act

                        string result;

                        using (new SecurityDisabler())
                        {
                            result = html.Editable(model, x => x.StringField, x => x.StringField);
                        }
                        //Assert
                        Assert.IsTrue(result.Contains(fieldValue));
                        //this is the webedit class

                        Assert.IsTrue(result.Contains("scWebEditInput"), "result " + result);
                    }
                }
            }
        }

        [Test]
        public void Editable_NotInEditModeWithStandardOutput_StringFieldWithEditReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/target";

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    new DbField("StringField")
                    {
                        Type = "text"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    {"StringField", ""}
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<StubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.Save(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                var siteContext = new SiteContextStub(
                    new SiteInfo(
                        doc.FirstChild
                        )
                    );

                model = service.GetItem<StubClass>(targetPath);


                using (new SiteContextSwitcher(siteContext))
                {
                    //Act
                    string result;

                    using (new SecurityDisabler())
                    {
                        result = html.Editable(model, x => x.StringField, x => x.StringField);
                    }
                    //Assert
                    Assert.AreEqual(fieldValue, result);
                    //this is the webedit class
                    Assert.IsFalse(result.Contains("scWebEditInput"), "result " + result);
                }    
            }
        }


        [Test]
        public void Editable_InEditModeWithStandardOutputAndFieldId_StringFieldWithEditReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/target";

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    new DbField(new ID(StubClass.StringFieldWithIdId))
                    {
                        Type = "text"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    { new ID(StubClass.StringFieldWithIdId), ""}
                }
            })
            { 
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<StubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringFieldWithId = fieldValue;

                using (new SecurityDisabler())
                {
                    service.Save(model);
                }


                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                var siteContext = new SiteContextStub(
                    new SiteInfo(
                        doc.FirstChild
                        )
                    );

                using (new EnableWebEditMode())
                {
                    using (new SiteContextSwitcher(siteContext))
                    {

                        //Act
                        string result;

                        using (new SecurityDisabler())
                        {
                            result = html.Editable(model, x => x.StringFieldWithId, x => x.StringFieldWithId);
                        } 
                        //Assert
                        Assert.IsTrue(result.Contains(fieldValue));
                        //this is the webedit class
                        Assert.IsTrue(result.Contains("scWebEditInput"), "result " + result);
                    }
                }
            }
        }

        [
            Test]
        public void Editable_NotInEditModeWithStandardOutputAndFieldId_StringFieldWithEditReturned()
        {
            //Assign
            string targetPath = "/sitecore/content/target";

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    new DbField("StringField")
                    {
                        Type = "text"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    {"StringField", ""}
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<StubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringFieldWithId = fieldValue;

                using (new SecurityDisabler())
                {
                    service.Save(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                var siteContext = new SiteContextStub(
                    new SiteInfo(
                        doc.FirstChild
                        )
                    );

                using (new SiteContextSwitcher(siteContext))
                {

                    //Act
                    var result = html.Editable(model, x => x.StringFieldWithId, x => x.StringFieldWithId);

                    //Assert
                    Assert.AreEqual(fieldValue, result);
                    //this is the webedit class
                    Assert.IsFalse(result.Contains("scWebEditInput"));
                    Console.WriteLine("result " + result);
                }
            }
        }

        [Test]
        public void Editable_DifferentLambdaExpressions_NoExceptionThrown()
        {
            //Assign
            string targetPath = "/sitecore/content/target";

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    new DbField("StringField")
                    {
                        Type = "text"
                    }
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    {"StringField", ""}
                }
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);

                var html = GetGlassHtml(service);

                var model1 = service.GetItem<StubClass>(targetPath);

                var fieldValue = "test content field";

                model1.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.Save(model1);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                var siteContext = new SiteContextStub(
                    new SiteInfo(
                        doc.FirstChild
                        )
                    );



                using (new SiteContextSwitcher(siteContext))
                {
                    //Act
                    string result;

                    using (new SecurityDisabler())
                    {
                        var temp = service.GetItem<StubClass>(targetPath);
                        result = html.Editable(temp, model => model.StringField);
                    }

                    using (new SecurityDisabler())
                    {
                        var model = service.GetItem<StubClass>(targetPath);
                        result = html.Editable(model, x=> model.StringField);
                    }

                    //Assert
                    Assert.AreEqual(fieldValue, result);
                    //this is the webedit class
                    Assert.IsFalse(result.Contains("scWebEditInput"));
                    Console.WriteLine("result " + result);
                }
            }
        }

        #endregion


        #region RenderingParameters

        [Test]
        public void RenderingParameters_StringPassedInWithParameters_ReturnsModelWithValues()
        {
            //Arrange
            var expectedNumber = 234;
            var expectedId1 = new Guid("{032B690F-5113-44C4-AEC7-A16B44382D4C}");
            var expectedId2 = new Guid("{6CF01319-0234-42C8-AEC1-FE757169F7A0}");
            var expectedFieldValue = "hello world";

            var parameters = "StringField={0}&Number={1}&Items={2}"
                .Formatted(
                    WebUtil.UrlEncode(expectedFieldValue),
                    WebUtil.UrlEncode(expectedNumber.ToString()),
                    WebUtil.UrlEncode("{0}|{1}".Formatted(expectedId1, expectedId2)));

            string targetPath = "/sitecore/content/target";

            var templateId = ID.NewID;
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {"StringField",""},
                    {"Number",""},
                    {"Items",""}
                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    {"StringField", ""}
                },
                new Sitecore.FakeDb.DbItem("Target",new ID(expectedId1), templateId),
                new Sitecore.FakeDb.DbItem("Target",new ID(expectedId2), templateId)

            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);

                var html = GetGlassHtml(service);

                //Act
                var result = html.GetRenderingParameters<RenderingTest>(parameters, templateId);

                //Assert
                Assert.AreEqual(expectedNumber, result.Number);
                Assert.AreEqual(expectedFieldValue, result.StringField);
                Assert.IsTrue(result.Items.Any(x => x.Id == expectedId1));
                Assert.IsTrue(result.Items.Any(x => x.Id == expectedId2));
                Assert.AreEqual(2, result.Items.Count());
            }
        }

        [Test]
        public void RenderingParameters_StringPassedInWithParametersUsingIdOnType_ReturnsModelWithValues()
        {
            //Arrange
            var expectedNumber = 234;
            var expectedId1 = new Guid("{032B690F-5113-44C4-AEC7-A16B44382D4C}");
            var expectedId2 = new Guid("{6CF01319-0234-42C8-AEC1-FE757169F7A0}");
            var expectedFieldValue = "hello world";

            var parameters = "StringField={0}&Number={1}&Items={2}"
                .Formatted(
                    WebUtil.UrlEncode(expectedFieldValue),
                    WebUtil.UrlEncode(expectedNumber.ToString()),
                    WebUtil.UrlEncode("{0}|{1}".Formatted(expectedId1, expectedId2)));

            string targetPath = "/sitecore/content/target";

            var templateId = new ID(RenderingTestWithAttribute.TemplateId);
            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                                       {"StringField", ""},
                    {"Number", ""},
                    {"Items", ""},

                },
                new Sitecore.FakeDb.DbItem("Target", ID.NewID, templateId)
                {
                    {"StringField", ""}
                },
                  new Sitecore.FakeDb.DbItem("Target",new ID(expectedId1), templateId),
                new Sitecore.FakeDb.DbItem("Target",new ID(expectedId2), templateId)
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreContext(database.Database);

                var html = GetGlassHtml(service);

                //Act
                var result = html.GetRenderingParameters<RenderingTestWithAttribute>(parameters);

                //Assert
                Assert.AreEqual(expectedNumber, result.Number);
                Assert.AreEqual(expectedFieldValue, result.StringField);
                Assert.IsTrue(result.Items.Any(x => x.Id == expectedId1));
                Assert.IsTrue(result.Items.Any(x => x.Id == expectedId2));
                Assert.AreEqual(2, result.Items.Count());
            }
        }

        #endregion


        #region Stubs

        public class RenderingTest
        {
            public virtual int Number { get; set; }
            public virtual string StringField { get; set; }
            public virtual IEnumerable<QuickInfo> Items { get; set; }
        }
        //The template ID is the ID of the rendering parameters template
        [SitecoreType(TemplateId = TemplateId, AutoMap = true)]
        public class RenderingTestWithAttribute
        {
            public const string TemplateId = "{6C815B38-4D88-4F01-916D-8D7C6548005E}";
            public virtual int Number { get; set; }
            public virtual string StringField { get; set; }
            public virtual IEnumerable<QuickInfo> Items { get; set; }
        }
        public class QuickInfo
        {
            public virtual string Name { get; set; }
            public virtual Guid Id { get; set; }
        }



        [SitecoreType]
        public class StubClass
        {
            [SitecoreId]
            public virtual Guid Id { get; set; }

            [SitecoreField]
            public virtual string StringField { get; set; }

            /// <summary>
            /// Real name StringFieldId
            /// </summary>
            [SitecoreField(FieldId = StringFieldWithIdId)]
            public virtual string StringFieldWithId { get; set; }

            public const string StringFieldWithIdId = "{296D9461-09AA-48EE-ADF3-9E3812B570D2}";


            [SitecoreField]
            public virtual string DateField { get; set; }

            [SitecoreQuery("./../*", IsRelative = true)]
            public virtual StubClass SubStub { get; set; }

            [SitecoreQuery("./../*", IsRelative = true)]
            public virtual IEnumerable<StubLambdaClass> EnumerableSubStub { get; set; }
        }

        [SitecoreType]
        public class StubClassWithImage
        {
            [SitecoreId]
            public virtual Guid Id { get; set; }

            [SitecoreField]
            public virtual Image Image { get; set; }


        }


        [SitecoreType]
        public interface IStubLambdaClass : IStubClass
        {

        }


        [SitecoreType]
        public interface IStubClassInherits : IStubClass
        {
        }

        [SitecoreType]
        public interface IStubClass
        {
            [SitecoreId]
            Guid Id { get; set; }

            [SitecoreField]
            string StringField { get; set; }

            [SitecoreField]
            string DateField { get; set; }

            [SitecoreQuery("./../*", IsRelative = true)]
            StubClass SubStub { get; set; }

            [SitecoreQuery("./../*", IsRelative = true)]
            IEnumerable<StubLambdaClass> EnumerableSubStub { get; set; }
        }

        [SitecoreType]
        public class StubLambdaClass : StubClass
        {

        }


        public interface IStubLinkClass
        {
            [SitecoreField("StringField")]
            Link Link { get; set; }
        }



        public class SiteContextStub : SiteContext
        {
            public SiteContextStub(SiteInfo info) : base(info)
            {

            }

            public void SetDisplayMode(DisplayMode mode)
            {
                this.SetDisplayMode(mode, DisplayModeDuration.Temporary);
            }
        }

        #endregion

        private IGlassHtml GetGlassHtml(ISitecoreContext sitecoreContext)
        {
            return sitecoreContext.GlassHtml;
        }
    }
}




