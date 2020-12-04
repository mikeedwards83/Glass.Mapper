using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Glass.Mapper.Sc.LayoutService;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.FakeDb;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Sitecore.Resources.Media;

#if SC90 || SC91 || SC92  || SC93 || SC100
using Sitecore.Abstractions;
using Sitecore.DependencyInjection;
#endif

namespace Glass.Mapper.Sc.Headless.Tests
{
    [TestFixture]
    public class GlassRenderingContentsResolverFixture
    {

        [SetUp]
        public void Init()
        {
            var mediaUrlProvider = Substitute.For<BaseMediaManager>();
            SitecoreVersionAbstractions.MediaManager = new LazyResetable<BaseMediaManager>(() => mediaUrlProvider);

            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubDataSourceClass)));

            mediaUrlProvider
                .GetMediaUrl(Arg.Is<MediaItem>(i => i.ID == MediaId), Arg.Any<MediaUrlOptions>())
                .Returns(MediaUrlEnd);
        }

        [TearDown]
        public void Cleanup()
        {
        }

        #region Constants 
        //Paths
        const string ItemPath = "/sitecore/content/target";

        //Fields
        private const string StringFieldName = "StringField";
        private const string StringFieldValue = "testing123";
        private const string ImageFieldName = "Image";
        private static readonly ID MediaId = new ID("{D897833C-1F53-4FAE-B54B-BB5B11B8F851}");
        private const string ImageFieldValue =
            "<image mediaid=\"{D897833C-1F53-4FAE-B54B-BB5B11B8F851}\" mediapath=\"/Files/20121222_001405\" src=\"~/media/D897833C1F534FAEB54BBB5B11B8F851.ashx\" hspace=\"15\" vspace=\"20\" />";
        private static readonly string MediaUrlEnd = "/~/media/Test.ashx";
        #endregion

        #region Stub
        [SitecoreType]
        public class StubDataSourceClass
        {
            [SitecoreId]
            public virtual ID Id { get; set; }

            [SitecoreField]
            public virtual string StringField { get; set; }

            [SitecoreField]
            public virtual Image Image { get; set; }
        }

        public class StubContextItemContentResolver : GlassRenderingContentsResolver<StubDataSourceClass>
        {
            public StubContextItemContentResolver(ISitecoreService sitecoreService) : base(sitecoreService)
            {
                
            }

            public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
            {
                return GetContextItem(rendering, renderingConfig);
            }
        }

        public class StubProcessedContextItemContentResolver : GlassRenderingContentsResolver<StubDataSourceClass>
        {
            public StubProcessedContextItemContentResolver(ISitecoreService sitecoreService) : base(sitecoreService)
            {

            }

            public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
            {
                var contextItem = GetContextItem(rendering, renderingConfig);
                var processedItem = ProcessItem(contextItem, rendering, renderingConfig);
                return processedItem;
            }
        }

        private void SetHttpContext()
        {
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://www.glass.lu", null), new HttpResponse(null));
        }
        #endregion

#if SC93 || SC100
        [Test]
        public void GetStubComponent_ReturnsStubComponentContextItem()
        {
            //Assign
            var itemId = Guid.NewGuid();

            //Fake DB
            using (var database = new Db
            {
                new DbItem("Target", new ID(itemId))
                {
                    new DbField(StringFieldName)
                    {
                        Value = StringFieldValue
                    },
                    new DbField(ImageFieldName)
                    {
                        Value = ImageFieldValue
                    },
                },
                new DbItem("MediaItem", MediaId)
                {
                    new DbField("alt") {Value = "test alt"},
                    new DbField("height") {Value = "480"},
                    new DbField("width") {Value = "640"},
                }
            })
            {
                // Arrange 
                var db = database.Database;
                var item = db.GetItem(ItemPath);

                var service = new SitecoreService(database.Database);

                //Rendering configuration
                var renderingConfig = Substitute.For<IRenderingConfiguration>();
                renderingConfig.IncludeServerUrlInContextItemMediaUrls.Returns(false);

                var rendering = new Rendering
                {
                    RenderingItem = new RenderingItem(item),
                    DataSource = ItemPath
                };

                //GlassContentsResolver
                var contentResolver = new StubContextItemContentResolver(service);

                //Act
                var result = contentResolver.ResolveContents(rendering, renderingConfig);
                //Cast result object to StubClass as this is the expected type
                var resultObject = (StubDataSourceClass)result;

                //Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(resultObject, Is.Not.Null);
                Assert.That(resultObject.Id, Is.EqualTo(item.ID));
                Assert.That(resultObject.StringField, Is.EqualTo(item.Fields[StringFieldName].Value));
                Assert.That(resultObject.Image.MediaExists, Is.Not.Null);
                Assert.That(resultObject.Image.MediaExists, Is.True);
                Assert.That(resultObject.Image.Src, Does.EndWith(MediaUrlEnd));
            }
        }

        [Test]
        public void GetStubComponent_ReturnsStubComponentSerializedObject()
        {
            //Assign
            var itemId = Guid.NewGuid();
           
            //Fake DB
            using (var database = new Db
            {
                new DbItem("Target", new ID(itemId))
                {
                    new DbField(StringFieldName)
                    {
                        Value = StringFieldValue
                    },
                    new DbField(ImageFieldName)
                    {
                        Value = ImageFieldValue
                    },
                },
                new DbItem("MediaItem", MediaId)
                {
                    new DbField("alt") {Value = "test alt"},
                    new DbField("height") {Value = "480"},
                    new DbField("width") {Value = "640"},
                }
            })
            {
                // Arrange 

                var db = database.Database;
                var item = db.GetItem(ItemPath);

                var service = new SitecoreService(database.Database);

                //Rendering configuration
                var renderingConfig = Substitute.For<IRenderingConfiguration>();
                renderingConfig.IncludeServerUrlInContextItemMediaUrls.Returns(false);

                var rendering = new Rendering
                {
                    RenderingItem = new RenderingItem(item),
                    DataSource = ItemPath
                };

                SetHttpContext();
                //GlassContentsResolver
                var contentResolver = new StubProcessedContextItemContentResolver(service);

                //Act
                var result = contentResolver.ResolveContents(rendering, renderingConfig);
                //Cast result object to StubClass as this is the expected type
                var resultObject = (StubDataSourceClass) result;

                //Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(resultObject, Is.Not.Null);
                Assert.That(resultObject.Id, Is.EqualTo(item.ID));
                Assert.That(resultObject.StringField, Is.EqualTo(item.Fields[StringFieldName].Value));
                Assert.That(resultObject.Image.MediaExists, Is.Not.Null);
                Assert.That(resultObject.Image.MediaExists, Is.True);
                Assert.That(resultObject.Image.Src, Does.EndWith(MediaUrlEnd));
            }
        }
    }
    #endif
}