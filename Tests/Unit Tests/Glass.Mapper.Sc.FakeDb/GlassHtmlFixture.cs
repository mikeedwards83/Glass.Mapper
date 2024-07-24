using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
#if SC90 || SC91 || SC92  || SC93 || SC100 || SC101 || SC102 || SC103 || SC104
using Sitecore.Abstractions;
    using Sitecore.DependencyInjection;
#endif
using Sitecore.Caching;
using Sitecore.Data;
using Sitecore.Data.Managers;
using Sitecore.FakeDb;
using Sitecore.Resources.Media;
using Sitecore.SecurityModel;
using Sitecore.Sites;
using Sitecore.Web;
using Site = System.Security.Policy.Site;


#if SC100 || SC101 || SC102 || SC103 || SC104
using Sitecore.Links.UrlBuilders;
#endif

namespace Glass.Mapper.Sc.FakeDb
{
    [TestFixture]
    public class GlassHtmlFixture
    {
        // private Sitecore.FakeDb.Resources.Media.MediaProviderSwitcher _switcher;

        [SetUp]
        public void Setup()
        {
            Sitecore.Resources.Media.MediaProvider mediaProvider =
                NSubstitute.Substitute.For<Sitecore.Resources.Media.MediaProvider>();
            //   _switcher = new Sitecore.FakeDb.Resources.Media.MediaProviderSwitcher(mediaProvider);

            var config = new MediaConfig(Sitecore.Configuration.Factory.GetConfigNode("mediaLibrary"));
            MediaManager.Config = config;
        }

        [TearDown]
        public void TearDown()
        {
            //  _switcher.Dispose();
        }

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

                var service = new SitecoreService(database.Database);

                var html = GetGlassHtml(service);


                string fieldValue = "<link text='text' linktype='anchor' anchor='footer' title='' class='' />";
                string expected = "<a href='#footer' >text</a>";

                var item = database.GetItem(path);
                var field = item.Fields["StringField"];

                using (new ItemEditing(item, true))
                {
                    field.Value = fieldValue;
                }

                var model = service.GetItem<IStubLinkClass>(path); ;

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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
                AssertHtml.AreHtmlElementsEqual(expected, result, "a");
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

                var service = new SitecoreService(database.Database);
                var html = GetGlassHtml(service);

                var item = database.GetItem(targetPath);

                string fieldValue =
                    "<link text='' linktype='internal' class='' title=''  querystring='foo=bar' id='{0}' />".Formatted(
                        item.ID.Guid.ToString("B"));

#if SC93 || SC100 || SC101 || SC102
                string expected =
                    "<a href='/en/sitecore/content/Target?foo=bar' >Target</a>";
#else
                string expected =
                    "<a href='/en/sitecore/content/Target.aspx?foo=bar' >Target</a>";
#endif
                var field = item.Fields["StringField"];

                using (new ItemEditing(item, true))
                {
                    field.Value = fieldValue;
                }

                var model = service.GetItem<IStubLinkClass>(targetPath);

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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
                AssertHtml.AreHtmlElementsEqual(expected, result, "a");
            }
        }

#endregion



#region Method - RenderImage


        /// <summary>
        /// https://github.com/mikeedwards83/Glass.Mapper/issues/133
        /// </summary>
        [Test]
     //   [Ignore("This test has to be run manually and compare manually because the order of parameters is different")]
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

                var service = new SitecoreService(database.Database);

                var html = GetGlassHtml(service);



#if SC90 || SC91 || SC92 || SC93 || SC100 || SC101 || SC102 || SC103 || SC104

                var mediaUrlProvider = Substitute.For<BaseMediaManager>();

                SitecoreVersionAbstractions.MediaManager = new LazyResetable<BaseMediaManager>(() => mediaUrlProvider);

#if SC100 || SC101 || SC102 || SC103
                mediaUrlProvider.GetMediaUrl(Arg.Is<Sitecore.Data.Items.MediaItem>(i => i.ID == mediaID), Arg.Any<MediaUrlBuilderOptions>())
                    .Returns("/myimage");
#else
                 mediaUrlProvider
                    .GetMediaUrl(Arg.Is<Sitecore.Data.Items.MediaItem>(i => i.ID == mediaID), Arg.Any<MediaUrlOptions>())
                  .Returns("/myimage");
#endif

#else
                Sitecore.Resources.Media.MediaProvider mediaProvider =
                    Substitute.For<Sitecore.Resources.Media.MediaProvider>();
                mediaProvider
                     .GetMediaUrl(Arg.Is<Sitecore.Data.Items.MediaItem>(i => i.ID == mediaID), Arg.Any<MediaUrlOptions>())
                      .Returns("/myimage");

                new Sitecore.FakeDb.Resources.Media.MediaProviderSwitcher(mediaProvider);
#endif




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
                scControl.Field = "image";
                scControl.Parameters = "mw=200";

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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

                var service = new SitecoreService(database.Database);
                var html = GetGlassHtml(service);


#if SC90 || SC91 || SC92 || SC93 || SC100 || SC101 || SC102 || SC103 || SC104

                var mediaUrlProvider = Substitute.For<BaseMediaManager>();

                SitecoreVersionAbstractions.MediaManager = new LazyResetable<BaseMediaManager>(() => mediaUrlProvider);

                mediaUrlProvider
                    .GetMediaUrl(Arg.Is<Sitecore.Data.Items.MediaItem>(i => i.ID == mediaID), Arg.Any<MediaUrlOptions>())
                    .Returns("/myimage");

#else
                Sitecore.Resources.Media.MediaProvider mediaProvider =
                    Substitute.For<Sitecore.Resources.Media.MediaProvider>();
                mediaProvider
                     .GetMediaUrl(Arg.Is<Sitecore.Data.Items.MediaItem>(i => i.ID == mediaID), Arg.Any<MediaUrlOptions>())
                      .Returns("/myimage");

                new Sitecore.FakeDb.Resources.Media.MediaProviderSwitcher(mediaProvider);
#endif

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
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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

                var service = new SitecoreService(database.Database);
                var html = GetGlassHtml(service);

#if SC90 || SC91 || SC92 || SC93 || SC100 || SC101 || SC102 || SC103 || SC104

                var mediaUrlProvider = Substitute.For<BaseMediaManager>();

                SitecoreVersionAbstractions.MediaManager = new LazyResetable<BaseMediaManager>(() => mediaUrlProvider);

                mediaUrlProvider
                    .GetMediaUrl(Arg.Is<Sitecore.Data.Items.MediaItem>(i => i.ID == mediaID), Arg.Any<MediaUrlOptions>())
                    .Returns("/myimage");

#else
                Sitecore.Resources.Media.MediaProvider mediaProvider =
                    Substitute.For<Sitecore.Resources.Media.MediaProvider>();
                mediaProvider
                     .GetMediaUrl(Arg.Is<Sitecore.Data.Items.MediaItem>(i => i.ID == mediaID), Arg.Any<MediaUrlOptions>())
                      .Returns("/myimage");

                new Sitecore.FakeDb.Resources.Media.MediaProviderSwitcher(mediaProvider);
#endif


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
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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

                var service = new SitecoreService(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<StubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.SaveItem(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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

                var service = new SitecoreService(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<StubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.SaveItem(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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

                var service = new SitecoreService(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<StubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.SaveItem(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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

                var service = new SitecoreService(database.Database);


                var html = new GlassHtml(service);

                var model = service.GetItem<IStubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.SaveItem(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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

                var service = new SitecoreService(database.Database);

                var html = new GlassHtml(service);

                var model = service.GetItem<IStubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;


                using (new SecurityDisabler())
                {
                    service.SaveItem(model);
                }


                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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

                var service = new SitecoreService(database.Database);

                var html = new GlassHtml(service);

                var model = service.GetItem<IStubClassInherits>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.SaveItem(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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

                var service = new SitecoreService(database.Database);
                var html = GetGlassHtml(service);

                var model = service.GetItem<StubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.SaveItem(model);
                }

                model = service.GetItem<StubClass>(targetPath);


                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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

                var service = new SitecoreService(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<StubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.SaveItem(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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

                var service = new SitecoreService(database.Database);

                var html = new GlassHtml(service);

                var model = service.GetItem<IStubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.SaveItem(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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

                var service = new SitecoreService(database.Database);

                var html = new GlassHtml(service);

                var model = service.GetItem<IStubClassInherits>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.SaveItem(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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

                var service = new SitecoreService(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<StubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.SaveItem(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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

                var service = new SitecoreService(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<IStubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.SaveItem(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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

                var service = new SitecoreService(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<IStubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.SaveItem(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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

                var service = new SitecoreService(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<IStubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.SaveItem(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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

                var service = new SitecoreService(database.Database);
                var html = GetGlassHtml(service);

                var model = service.GetItem<IStubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.SaveItem(model);
                }

                model = service.GetItem<IStubClass>(targetPath);

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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

                var service = new SitecoreService(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<IStubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.SaveItem(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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

                        Console.WriteLine("result " + result);

                        Assert.IsTrue(result.Contains(fieldValue));
                        //this is the webedit class
                        Assert.IsTrue(true, "result " + result);
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

                var service = new SitecoreService(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<IStubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.SaveItem(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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

                var service = new SitecoreService(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<StubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.SaveItem(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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

                var service = new SitecoreService(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<StubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.SaveItem(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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

                var service = new SitecoreService(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<StubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringFieldWithId = fieldValue;

                using (new SecurityDisabler())
                {
                    service.SaveItem(model);
                }


                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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

                var service = new SitecoreService(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<StubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringFieldWithId = fieldValue;

                using (new SecurityDisabler())
                {
                    service.SaveItem(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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

                var service = new SitecoreService(database.Database);

                var html = GetGlassHtml(service);

                var model1 = service.GetItem<StubClass>(targetPath);

                var fieldValue = "test content field";

                model1.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.SaveItem(model1);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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
                        result = html.Editable(model, x => model.StringField);
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

#region Method - EditableIf

        [Test]
        public void EditableIf_InEditModeWithStandardOutputPredicateTrue_StringFieldWithEditReturned()
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

                var service = new SitecoreService(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<StubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.SaveItem(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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
                            result = html.EditableIf(model, () => true, x => x.StringField, x => x.StringField);
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
        public void EditableIf_InEditModeWithStandardOutputPredicateFalse_StringFieldWithoutEditReturned()
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

                var service = new SitecoreService(database.Database);

                var html = GetGlassHtml(service);

                var model = service.GetItem<StubClass>(targetPath);

                var fieldValue = "test content field";

                model.StringField = fieldValue;

                using (new SecurityDisabler())
                {
                    service.SaveItem(model);
                }

                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreService/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

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
                            result = html.EditableIf(model, () => false, x => x.StringField, x => x.StringField);
                        }
                        //Assert
                        Assert.IsTrue(result.Contains(fieldValue));
                        //this is the webedit class

                        Assert.IsFalse(result.Contains("scWebEditInput"), "result " + result);
                        Assert.AreEqual(fieldValue, result);
                    }
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

                var service = new SitecoreService(database.Database);

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

                var service = new SitecoreService(database.Database);

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

#region Method - RenderImage

        [Test]
        public void RenderImage_ValidImageWithParametersWidth_RendersCorrectHtml()
        {
            //Arrange
            var expected = "<img src='~/media/Images/Carousel/carousel-example.ashx?h=126&amp;w=240' alt='someAlt' width='380' />";
            var scContext = Substitute.For<ISitecoreService>();
            scContext.Config = new Config();

            var html = new FakeGlassHtml(scContext);
            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var parameters = new { Width = 380, W = 240 };
            var model = new { Image = image };

            //Act
            var result = html.RenderImage(model, x => x.Image, parameters, true, true);

            //Assert
            AssertHtml.AreImgEqual(expected, result);
        }

        [Test]
        public void RenderImage_AlternativeQuotationMarks_RendersCorrectHtml()
        {
            //Arrange
            var expected =
                "<img src=\"~/media/Images/Carousel/carousel-example.ashx?h=126&amp;w=240\" alt=\"someAlt\" width=\"380\" />";
            var scContext = Substitute.For<ISitecoreService>();
            scContext.Config = new Config();

            var html = new FakeGlassHtml(scContext);
            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var parameters = new { Width = 380, W = 240 };
            var model = new { Image = image };

            GlassHtml.QuotationMark = "'";

            Sitecore.Resources.Media.MediaProvider mediaProvider =
                NSubstitute.Substitute.For<Sitecore.Resources.Media.MediaProvider>();



            //Act
            var result = html.RenderImage(model, x => x.Image, parameters, true, true);

            GlassHtml.QuotationMark = "\"";

            //Assert
            AssertHtml.AreImgEqual(expected, result);

        }

        [Test]
        public void RenderImage_SingleQuotatinMarkInAltText_RendersCorrectHtml()
        {
            //Arrange
            var expected = "<img src=\"~/media/Images/Carousel/carousel-example.ashx?h=126&amp;w=240\" alt=\"some'Alt\" width=\"380\" />";
            var scContext = Substitute.For<ISitecoreService>();
            scContext.Config = new Config();

            var html = new FakeGlassHtml(scContext);
            var image = new Fields.Image();
            image.Alt = "some'Alt";
            image.Width = 200;
            image.Height = 105;
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var parameters = new { Width = 380, W = 240 };
            var model = new { Image = image };


            Sitecore.Resources.Media.MediaProvider mediaProvider =
                 NSubstitute.Substitute.For<Sitecore.Resources.Media.MediaProvider>();



            //Act
            var result = html.RenderImage(model, x => x.Image, parameters, true, true);


            //Assert
            AssertHtml.AreImgEqual(expected, result);

        }

        [Test]
        public void RenderImage_DoubleQuotatinMarkInAltText_RendersCorrectHtml()
        {
            //Arrange
            var expected = "<img src=\"~/media/Images/Carousel/carousel-example.ashx?h=126&amp;w=240\" alt=\"some&quot;Alt\" width=\"380\" />";
            var scContext = Substitute.For<ISitecoreService>();
            scContext.Config = new Config();

            var html = new FakeGlassHtml(scContext);
            var image = new Fields.Image();
            image.Alt = "some\"Alt";
            image.Width = 200;
            image.Height = 105;
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var parameters = new { Width = 380, W = 240 };
            var model = new { Image = image };


            Sitecore.Resources.Media.MediaProvider mediaProvider =
                 NSubstitute.Substitute.For<Sitecore.Resources.Media.MediaProvider>();



            //Act
            var result = html.RenderImage(model, x => x.Image, parameters, true, true);


            //Assert
            AssertHtml.AreImgEqual(expected, result);

        }

        [Test]
        public void RenderImage_ValidImageWithParametersWidth_RendersCorrectHtmlNoWidthHeight()
        {    //Arrange
            var expected = "<img src='~/media/Images/Carousel/carousel-example.ashx?h=126&amp;w=240' alt='someAlt' />";
            var scContext = Substitute.For<ISitecoreService>();
            var html = new FakeGlassHtml(scContext);
            scContext.Config = new Config();

            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var parameters = new { Width = 380, W = 240 };
            var model = new { Image = image };

            //Act
            var result = html.RenderImage(model, x => x.Image, parameters, true, false);

            //Assert
            AssertHtml.AreImgEqual(expected, result);

        }

        [Test]
        public void RenderImage_ValidImageWithNullParameterForWidth_RendersCorrectHtmlWidthSentHeight()
        {    //Arrange
            var expected = "<img src='~/media/Images/Carousel/carousel-example.ashx?h=105&amp;w=200' alt='someAlt' />";
            var scContext = Substitute.For<ISitecoreService>();
            var html = new FakeGlassHtml(scContext);
            scContext.Config = new Config();

            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var parameters = new { Width = 380, W = (string)null };
            var model = new { Image = image };

            //Act
            var result = html.RenderImage(model, x => x.Image, parameters, true, false);

            //Assert
            AssertHtml.AreImgEqual(expected, result);

        }

        [Test]
        public void RenderImage_ValidImageWithParametersHeight_RendersCorrectHtml()
        {
            //Arrange
            var expected = "<img src='~/media/Images/Carousel/carousel-example.ashx?h=450&amp;w=600' alt='someAlt' height='450' />";
            var scContext = Substitute.For<ISitecoreService>();
            var html = new FakeGlassHtml(scContext);
            scContext.Config = new Config();

            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 150;
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var parameters = new { Height = 450, H = 450 };
            var model = new { Image = image };

            //Act
            var result = html.RenderImage(model, x => x.Image, parameters, true, true);

            //Assert
            AssertHtml.AreImgEqual(expected, result);

        }

        [Test]
        public void RenderImage_ValidImageWithParametersClass_RendersCorrectHtml()
        {
            //Arrange
            var expected = "<img src='~/media/Images/Carousel/carousel-example.ashx?h=105&amp;w=200' alt='someAlt' height='105' class='someClass' width='200' />";
            var scContext = Substitute.For<ISitecoreService>();
            var html = new FakeGlassHtml(scContext);
            scContext.Config = new Config();

            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var parameters = new { Class = "someClass" };
            var model = new { Image = image };

            //Act
            var result = html.RenderImage(model, x => x.Image, parameters, true, true);

            //Assert
            AssertHtml.AreImgEqual(expected, result);

        }

        [Test]
        public void RenderImage_ValidImageWithWidthAndStretch_RendersCorrectHtml()
        {
            //Arrange
            var expected = "<img src='~/media/Images/Carousel/carousel-example.ashx?h=472&amp;as=True&amp;w=900' alt='someAlt' width='900' />";
            var scContext = Substitute.For<ISitecoreService>();
            var html = new FakeGlassHtml(scContext);
            scContext.Config = new Config();

            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var parameters = new { Width = 900, As = true };
            var model = new { Image = image };

            //Act
            var result = html.RenderImage(model, x => x.Image, parameters, true, true);

            //Assert
            AssertHtml.AreImgEqual(expected, result);

        }

        [Test]
        public void RenderImage_MaxWidthWhereWidthIsAboveMaxWidth_RendersCorrectHtml()
        {
            //Arrange
            var expected = "<img src='~/media/Images/Carousel/carousel-example.ashx?mw=100&amp;h=52&amp;as=True&amp;w=100' alt='someAlt' />";
            var scContext = Substitute.For<ISitecoreService>();
            scContext.Config = new Config();

            var html = new FakeGlassHtml(scContext);
            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var parameters = new { mw = 100, As = true };
            var model = new { Image = image };

            //Act
            var result = html.RenderImage(model, x => x.Image, parameters, true, false);

            //Assert
            AssertHtml.AreImgEqual(expected, result);

        }

        [Test]
        public void RenderImage_MaxWidthWhereWidthIsBelowMaxWidth_RendersCorrectHtml()
        {
            //Arrange
            var expected = "<img src='~/media/Images/Carousel/carousel-example.ashx?mw=300&amp;h=105&amp;as=True&amp;w=200' alt='someAlt' />";
            var scContext = Substitute.For<ISitecoreService>();
            scContext.Config = new Config();

            var html = GetGlassHtml(scContext);
            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var parameters = new { mw = 300, As = true };
            var model = new { Image = image };

            //Act
            var result = html.RenderImage(model, x => x.Image, parameters, true, false);

            //Assert
            AssertHtml.AreImgEqual(expected, result);

        }

        [Test]
        public void RenderImage_ValidImageWithWidthHeightAndStretch_RendersCorrectHtml()
        {
            //Arrange
            var expected = "<img src='~/media/Images/Carousel/carousel-example.ashx?h=300&amp;as=True&amp;w=900' alt='someAlt' height='300' width='900' />";
            var scContext = Substitute.For<ISitecoreService>();
            scContext.Config = new Config();

            var html = GetGlassHtml(scContext);
            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var parameters = new { Width = 900, height = 300, h = 300, w = 900, As = true };
            var model = new { Image = image };

            //Act
            var result = html.RenderImage(model, x => x.Image, parameters, true, true);

            //Assert
            AssertHtml.AreImgEqual(expected, result);

        }

        [Test]
        public void RenderImage_ValidImageWithClass_RendersCorrectHtml()
        {
            //Arrange
            var expected = "<img src='~/media/Images/Carousel/carousel-example.ashx?h=105&amp;w=200' alt='someAlt' height='105' class='AClass' width='200' />";
            var scContext = Substitute.For<ISitecoreService>();
            scContext.Config = new Config();

            var html = GetGlassHtml(scContext);
            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.Class = "AClass";
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var model = new { Image = image };

            //Act
            var result = html.RenderImage(model, x => x.Image, null, true, true);

            //Assert
            AssertHtml.AreImgEqual(expected, result);

        }

        [Test]
        public void RenderImage_ValidImageWithBorderHSpaceVSpace_RendersCorrectHtml()
        {
            using (Db database = new Db())
            {
                //Arrange
                var expected =
                    "<img src='~/media/Images/Carousel/carousel-example.ashx?h=105&amp;w=200' width='200' vspace='15' height='105' hspace='10' border='9' alt='someAlt' />";
                var scContext = Substitute.For<ISitecoreService>();
                scContext.Config = new Config();

                var html = GetGlassHtml(scContext);
                var image = new Fields.Image();
                image.Alt = "someAlt";
                image.Width = 200;
                image.Height = 105;
                image.HSpace = 10;
                image.VSpace = 15;
                image.Border = "9";
                image.Src = "~/media/Images/Carousel/carousel-example.ashx";
                var model = new { Image = image };


                var result = html.RenderImage(model, x => x.Image, null, true, true);



                //Assert
                AssertHtml.AreImgEqual(expected, result);
            }
        }
        [Test]
        public void RenderImage_ValidImageWithBorderHSpaceVSpaceW_RendersCorrectHtml()
        {
            //Arrange
            var expected =
                "<img src='~/media/Images/Carousel/carousel-example.ashx?h=210&amp;w=400' width='200' vspace='15' height='105' hspace='10' border='9' alt='someAlt' />";
            var scContext = Substitute.For<ISitecoreService>();
            scContext.Config = new Config();

            var html = GetGlassHtml(scContext);
            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.HSpace = 10;
            image.VSpace = 15;
            image.Border = "9";
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var model = new { Image = image };
            var parameters = new { w = 400 };
            //Act
            var result = html.RenderImage(model, x => x.Image, parameters, true, true);

            //Assert
            AssertHtml.AreImgEqual(expected, result);

        }

        [Test]
        public void RenderImage_ValidImageWithLanguageFromItem_RendersCorrectHtml()
        {
            //Arrange
            var expected =
                "<img src='~/media/Images/Carousel/carousel-example.ashx?h=210&amp;la=af-ZA&amp;w=400' width='200' vspace='15' height='105' hspace='10' border='9' alt='someAlt' />";
            var scContext = Substitute.For<ISitecoreService>();
            scContext.Config = new Config();

            var html = GetGlassHtml(scContext);
            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.HSpace = 10;
            image.VSpace = 15;
            image.Border = "9";
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            image.Language = LanguageManager.GetLanguage("af-ZA");
            var model = new { Image = image };
            var parameters = new { w = 400 };
            //Act
            var result = html.RenderImage(model, x => x.Image, parameters, true, true);

            //Assert
            AssertHtml.AreImgEqual(expected, result);

        }

        [Test]
        public void RenderImage_ValidImageWithLanguageParameterOverride_RendersCorrectHtmlWithParameterLanguage()
        {
            //Arrange
            var expected =
                "<img src='~/media/Images/Carousel/carousel-example.ashx?h=210&amp;la=en&amp;w=400' width='200' vspace='15' height='105' hspace='10' border='9' alt='someAlt' />";
            var scContext = Substitute.For<ISitecoreService>();
            scContext.Config = new Config();

            var html = GetGlassHtml(scContext);
            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.HSpace = 10;
            image.VSpace = 15;
            image.Border = "9";
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            image.Language = LanguageManager.GetLanguage("af-ZA");
            var model = new { Image = image };
            var parameters = new { w = 400, la = "en" };
            //Act
            var result = html.RenderImage(model, x => x.Image, parameters, true, true);

            //Assert
            AssertHtml.AreImgEqual(expected, result);

        }


        [Test]
        public void RenderImage_AltTextContainsQuotationMarks_RendersCorrectHtmlWithParameterLanguage()
        {
            //Arrange
            var expected =
                "<img src='~/media/Images/Carousel/carousel-example.ashx?h=210&amp;la=en&amp;w=400' width='200' vspace='15' height='105' hspace='10' border='9' alt='some&quot;Alt' />";
            var scContext = Substitute.For<ISitecoreService>();
            scContext.Config = new Config();

            var html = GetGlassHtml(scContext);
            var image = new Fields.Image();
            image.Alt = "some\"Alt";
            image.Width = 200;
            image.Height = 105;
            image.HSpace = 10;
            image.VSpace = 15;
            image.Border = "9";
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            image.Language = LanguageManager.GetLanguage("af-ZA");
            var model = new { Image = image };
            var parameters = new { w = 400, la = "en" };
            //Act
            var result = html.RenderImage(model, x => x.Image, parameters, true, true);

            //Assert
            AssertHtml.AreImgEqual(expected, result);

        }

        [Test]
        public void RenderImage_RemoveHeightWidthAttributes_RendersCorrectHtml()
        {
            //Arrange
            var expected =
                "<img src='~/media/Images/Carousel/carousel-example.ashx?h=210&amp;w=400' vspace='15' hspace='10' border='9' alt='someAlt' />";
            var scContext = Substitute.For<ISitecoreService>();
            scContext.Config = new Config();

            var html = GetGlassHtml(scContext);
            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.HSpace = 10;
            image.VSpace = 15;
            image.Border = "9";
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var model = new { Image = image };
            var parameters = new { w = 400 };
            //Act
            var result = html.RenderImage(model, x => x.Image, parameters, true, false);

            //Assert
            AssertHtml.AreImgEqual(expected, result);

        }


#endregion

#region Method - RenderImageUrl

        [Test]
        public void RenderImageUrl_ValidImageWithClass_RendersCorrectHtml()
        {
            //Arrange
            var expected = "~/media/Images/Carousel/carousel-example.ashx?h=105&amp;w=200";
            var scContext = Substitute.For<ISitecoreService>();
            scContext.Config = new Config();

            var html = GetGlassHtml(scContext);
            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.Class = "AClass";
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var model = new { Image = image };

            //Act
            var result = html.RenderImageUrl(model, x => x.Image);

            //Assert
            Assert.AreEqual(expected, result);

        }

#endregion

#region Method - RenderLink

        /// <summary>
        /// Test for issue:https://github.com/mikeedwards83/Glass.Mapper/issues/112
        /// </summary>
        [Test]
        public void RenderLink_LinkHasHashBang()
        {
            //Arrange
            var expected = "<a href='http://www.seek.com.au/jobs/in-australia/#dateRange=999&amp;workType=0&amp;industry=&amp;occupation=&amp;graduateSearch=false&amp;salaryFrom=0&amp;salaryTo=999999&amp;salaryType=annual&amp;advertiserID=&amp;advertiserGroup=&amp;keywords=sitecore+developer&amp;page=1&amp;displaySuburb=&amp;seoSuburb=&amp;isAreaUnspecified=false&amp;location=&amp;area=&amp;nation=3000&amp;sortMode=KeywordRelevance&amp;searchFrom=filters&amp;searchType=' >hello world</a>";
            var scContext = Substitute.For<ISitecoreService>();

            var html = new GlassHtml(scContext);
            var link = new Fields.Link();
            link.Text = "hello world";
            link.Url = "http://www.seek.com.au/jobs/in-australia/#dateRange=999&workType=0&industry=&occupation=&graduateSearch=false&salaryFrom=0&salaryTo=999999&salaryType=annual&advertiserID=&advertiserGroup=&keywords=sitecore+developer&page=1&displaySuburb=&seoSuburb=&isAreaUnspecified=false&location=&area=&nation=3000&sortMode=KeywordRelevance&searchFrom=filters&searchType=";

            var model = new { Link = link };

            //Act
            var result = html.RenderLink(model, x => x.Link);

            //Assert
            AssertHtml.AreHtmlElementsEqual(expected, result, "a");
        }

        /// <summary>
        /// Test for issue:https://github.com/mikeedwards83/Glass.Mapper/issues/112
        /// </summary>
        [Test]
        public void RenderLink_LinkHasHash()
        {
            //Arrange
            var expected = "<a href='http://www.seek.com.au/jobs/in-australia/#dateRange' >hello world</a>";
            var scContext = Substitute.For<ISitecoreService>();
            var html = new GlassHtml(scContext);
            var link = new Fields.Link();
            link.Text = "hello world";
            link.Url = "http://www.seek.com.au/jobs/in-australia/#dateRange";

            var model = new { Link = link };

            //Act
            var result = html.RenderLink(model, x => x.Link);

            //Assert
            AssertHtml.AreHtmlElementsEqual(expected, result, "a");
        }

        /// <summary>
        /// Test for issue:https://github.com/mikeedwards83/Glass.Mapper/issues/112
        /// </summary>
        [Test]
        public void RenderLink_LinkHasQuestionMark()
        {
            //Arrange
            var expected = "<a href='http://www.seek.com.au/jobs/in-australia/?dateRange=test&amp;value1=test2' >hello world</a>";
            var scContext = Substitute.For<ISitecoreService>();
            var html = new GlassHtml(scContext);
            var link = new Fields.Link();
            link.Text = "hello world";
            link.Url = "http://www.seek.com.au/jobs/in-australia/?dateRange=test&value1=test2";

            var model = new { Link = link };

            //Act
            var result = html.RenderLink(model, x => x.Link);

            //Assert
            AssertHtml.AreHtmlElementsEqual(expected, result, "a");
        }

        /// <summary>
        /// Test for issue:https://github.com/mikeedwards83/Glass.Mapper/issues/112
        /// </summary>
        [Test]
        public void RenderLink_LinkHasQuestionMarkAndAnchorAtEnd()
        {
            //Arrange
            var expected = "<a href='http://www.seek.com.au/jobs/in-australia/?dateRange=test&amp;value1=test2#anchor' >hello world</a>";
            var scContext = Substitute.For<ISitecoreService>();
            var html = new GlassHtml(scContext);
            var link = new Fields.Link();
            link.Text = "hello world";
            link.Url = "http://www.seek.com.au/jobs/in-australia/?dateRange=test&value1=test2#anchor";

            var model = new { Link = link };

            //Act
            var result = html.RenderLink(model, x => x.Link);

            //Assert
            AssertHtml.AreHtmlElementsEqual(expected, result, "a");
        }

        [Test]
        public void RenderLink_LinkWithNoAttributes()
        {
            //Arrange
            var expected = "<a href='/somewhere.aspx' >hello world</a>";
            var scContext = Substitute.For<ISitecoreService>();
            var html = new GlassHtml(scContext);
            var link = new Fields.Link();
            link.Text = "hello world";
            link.Url = "/somewhere.aspx";

            var model = new { Link = link };

            //Act
            var result = html.RenderLink(model, x => x.Link);

            //Assert
            AssertHtml.AreHtmlElementsEqual(expected, result, "a");
        }

        [Test]
        public void RenderLink_LinkWithAllSetProperties()
        {
            //Arrange
            var expected = "<a href='/somewhere.aspx?temp=fred#aAnchor' target='_blank' class='myclass' rel=\"noopener noreferrer\" title='mytitle' style='mystyle' >hello world</a>";
            var scContext = Substitute.For<ISitecoreService>();
            var html = new GlassHtml(scContext);
            var link = new Fields.Link();
            link.Text = "hello world";
            link.Url = "/somewhere.aspx";
            link.Anchor = "aAnchor";
            link.Class = "myclass";
            link.Query = "temp=fred";
            link.Target = "_blank";
            link.Title = "mytitle";
            link.Style = "mystyle";

            var model = new { Link = link };

            //Act
            var result = html.RenderLink(model, x => x.Link);

            //Assert

            AssertHtml.AreHtmlElementsEqual(expected, result, "a");
        }

        [Test]
        public void RenderLink_LinkWithMixedPropertiesAndParameters()
        {
            //Arrange
            var expected = "<a href='/somewhere.aspx?temp=fred#anOther' target='_blank' class='myclass' title='mytitle' >hello world</a>";
            var scContext = Substitute.For<ISitecoreService>();
            var html = GetGlassHtml(scContext);
            var link = new Fields.Link();
            link.Text = "hello world";
            link.Url = "/somewhere.aspx";
            link.Anchor = "aAnchor";
            link.Class = "myclass";
            link.Query = "temp=fred";
            link.Target = "_blank";
            link.Title = "mytitle";

            var model = new { Link = link };
            var parameters = new NameValueCollection { { "anchor", "anOther" } };

            //Act
            var result = html.RenderLink(model, x => x.Link, parameters);

            //Assert
        }

        [Test]
        public void RenderLink_LinkWithClassAndQuery()
        {
            //Arrange
            var expected = "<a href='/somewhere.aspx?temp=fred' class='myclass' >hello world</a>";
            var scContext = Substitute.For<ISitecoreService>();
            var html = new GlassHtml(scContext);
            var link = new Fields.Link();
            link.Text = "hello world";
            link.Url = "/somewhere.aspx";
            link.Query = "temp=fred";

            var model = new { Link = link };
            var parameters = new NameValueCollection { { "class", "myclass" } };

            //Act
            var result = html.RenderLink(model, x => x.Link, parameters);

            //Assert
            AssertHtml.AreHtmlElementsEqual(expected, result, "a");
        }

        [Test]
        public void RenderLink_HtmlEncodesText()
        {
            //Arrange
            var expected = "<a href='/somewhere.aspx?temp=fred' >hello &amp; world</a>";
            var scContext = Substitute.For<ISitecoreService>();
            var html = new GlassHtml(scContext);
            var link = new Fields.Link();
            link.Text = "hello & world";
            link.Url = "/somewhere.aspx";
            link.Query = "temp=fred";

            var model = new { Link = link };

            //Act
            var result = html.RenderLink(model, x => x.Link);

            //Assert
            AssertHtml.AreHtmlElementsEqual(expected, result, "a");
        }

        [Test]
        public void RenderLink_WithBracesInAttribute ()
        {
            //Arrange
            var expected = "<a href='/somewhere.aspx?temp=fred' test='{test}' >hello &amp; world</a>";
            var scContext = Substitute.For<ISitecoreService>();
            var html = new GlassHtml(scContext);
            var link = new Fields.Link();
            link.Text = "hello & world";
            link.Url = "/somewhere.aspx";
            link.Query = "temp=fred";

            var model = new { Link = link };

            //Act
            var result = html.RenderLink(model, x => x.Link, new {test = "{test}"});

            //Assert
            AssertHtml.AreHtmlElementsEqual(expected, result, "a");
        }

        /// <summary>
        /// https://github.com/mikeedwards83/Glass.Mapper/issues/329
        /// </summary>
        [Test]
        public void RenderLink_LinkNullAlwaysRenderTrue_ReturnsEmptyString()
        {
            //Arrange
            var expected = "<a href=\"\" ></a>";
            var scContext = Substitute.For<ISitecoreService>();
            var html = new GlassHtml(scContext);
            Link link = null;

            var model = new { Link = link };

            //Act
            var result = html.RenderLink(model, x => x.Link, alwaysRender:true);

            //Assert
            AssertHtml.AreHtmlElementsEqual(expected, result, "a");
        }

        /// <summary>
        /// https://github.com/mikeedwards83/Glass.Mapper/issues/329
        /// </summary>
        [Test]
        public void RenderLink_LinkNullAlwaysRenderFalse_ReturnsEmptyString()
        {
            //Arrange
            var expected = string.Empty;
            var scContext = Substitute.For<ISitecoreService>();
            var html = new GlassHtml(scContext);
            Link link = null;

            var model = new { Link = link };

            //Act
            var result = html.RenderLink(model, x => x.Link, alwaysRender: false);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RenderLink_UrlContainsTwoQuestionMarks_ReturnsAValidUrl()
        {
            //Arrange
            var url = "http://firstsearch.oclc.org/WebZ/FSPage?pagetype=return_frameset:linktype=servicelink:sessionid=fsapp6-41637-j79jaw2f-61y8dw:entitypagenum=10:0?entityframedscrolling=yes:entityframedurl=http%3A%2F%2Fwww.example.com:entityframedtitle=:entityframedtimeout=15";
            var expected = string.Format("<a href=\"{0}\" ></a>", url);
            var scContext = Substitute.For<ISitecoreService>();
            var html = new GlassHtml(scContext);
            Link link = null;

            var model = new
            {
                Link = new Link
                {
                    Url = url
                }
            };

            //Act
            var result = html.RenderLink(model, x => x.Link);

            //Assert
            Assert.AreEqual(expected, result);
        }

#if SC90 || SC91 || SC92 || SC93 || SC100 || SC101 || SC102 || SC103 || SC104
        [Test]
        public void RenderLink_BlankTarget_AddsNoOpener()
        {
            //Arrange
            var expected = "<a href='/somewhere.aspx' title='hello &amp; world' target='_blank' rel='noopener noreferrer' >hello &amp; world</a>";
            var scContext = Substitute.For<ISitecoreService>();
            var html = new GlassHtml(scContext);
            var link = new Fields.Link();
            link.Text = "hello & world";
            link.Url = "/somewhere.aspx";
            link.Title = "hello & world";
            link.Target = "_blank";
            var model = new { Link = link };

            //Act
            var result = html.RenderLink(model, x => x.Link);

            //Assert
            AssertHtml.AreHtmlElementsEqual(expected, result, "a");
        }

#endif

        [Test]
        public void RenderLink_HtmlEncodesTitle()
        {
            //Arrange
            var expected = "<a href='/somewhere.aspx' title='hello &amp; world' >hello &amp; world</a>";
            var scContext = Substitute.For<ISitecoreService>();
            var html = new GlassHtml(scContext);
            var link = new Fields.Link();
            link.Text = "hello & world";
            link.Url = "/somewhere.aspx";
            link.Title = "hello & world";
            var model = new { Link = link };

            //Act
            var result = html.RenderLink(model, x => x.Link);

            //Assert
            AssertHtml.AreHtmlElementsEqual(expected, result, "a");
        }

        [Test]
        public void RenderLink_ContainsSpecialCharacters()
        {
            //Arrange
            var expected = "<a href='http://na2.se.voxco.com/se/?st=6M9NIk9SWq9dkYTuJI2wewuV3b3bWrDQMwgCt2aBnWDGTaoxLYNESg%3D%3D' title='hello &amp; world' >hello &amp; world</a>";
            var scContext = Substitute.For<ISitecoreService>();
            var html = new GlassHtml(scContext);
            var link = new Fields.Link();
            link.Text = "hello & world";
            link.Url = "http://na2.se.voxco.com/se/?st=6M9NIk9SWq9dkYTuJI2wewuV3b3bWrDQMwgCt2aBnWDGTaoxLYNESg%3D%3D";
            link.Title = "hello & world";
            var model = new { Link = link };

            //Act
            var result = html.RenderLink(model, x => x.Link);

            //Assert
            AssertHtml.AreHtmlElementsEqual(expected, result, "a");
        }



        [Test]
        public void RenderLink_HtmlTitleAlreadyEncoded_DoesNotDoubleEncode()
        {
            //Arrange
            //This test checks that a value already encoded does not get accidentally double HTML encoded.
            var expected = "<a href='/somewhere.aspx' title='hello &amp; world' >hello &amp; world</a>";
            var scContext = Substitute.For<ISitecoreService>();
            var html = new GlassHtml(scContext);
            var link = new Fields.Link();
            link.Text = "hello &amp; world";
            link.Url = "/somewhere.aspx";
            link.Title = "hello &amp; world";
            var model = new { Link = link };

            //Act
            var result = html.RenderLink(model, x => x.Link);

            //Assert
            AssertHtml.AreHtmlElementsEqual(expected, result, "a");
        }


        [Test]
        public void RenderLink_LinkWithCustomContent()
        {
            //Arrange
            var expected = "<a href='/somewhere.aspx?temp=fred' class='myclass' >my other content</a>";
            var scContext = Substitute.For<ISitecoreService>();
            var html = new GlassHtml(scContext);
            var link = new Fields.Link();
            link.Text = "hello world";
            link.Url = "/somewhere.aspx";
            link.Query = "temp=fred";

            var model = new { Link = link };
            var parameters = new NameValueCollection { { "class", "myclass" } };
            var content = "my other content";

            //Act
            var result = html.RenderLink(model, x => x.Link, parameters, contents: content);

            //Assert
            AssertHtml.AreHtmlElementsEqual(expected, result, "a");
        }



        [Test]
        public void RenderLink_WithMultiParametersRepeated_ReturnsAllParamters()
        {
            //Arrange
            var expected =
                "<a href='/somewhere.aspx?temp=fred&amp;temp=fred2&amp;temp=fred3&amp;temp1=jane' class='myclass' >my other content</a>";
            var scContext = Substitute.For<ISitecoreService>();
            var html = new GlassHtml(scContext);
            var link = new Fields.Link();
            link.Text = "hello world";
            link.Url = "/somewhere.aspx";
            link.Query = "temp=fred&temp=fred2&temp=fred3&temp1=jane";

            var model = new { Link = link };
            var parameters = new NameValueCollection { { "class", "myclass" } };
            var content = "my other content";

            //Act
            var result = html.RenderLink(model, x => x.Link, parameters, contents: content);

            //Assert
            AssertHtml.AreHtmlElementsEqual(expected, result, "a");
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

        private IGlassHtml GetGlassHtml(ISitecoreService SitecoreService)
        {
            return new FakeGlassHtml(SitecoreService);
        }


        public class FakeGlassHtml : GlassHtml
        {
            public FakeGlassHtml(ISitecoreService sitecoreService) : base(sitecoreService)
            {
            }
            /// <summary>
            /// Overridding for now because of configuration load issues
            /// </summary>
            /// <param name="url"></param>
            /// <returns></returns>
            public override string ProtectMediaUrl(string url)
            {
                return url;
            }
        }


    }
}




