using System;
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
        private Context _context;
        private BaseMediaManager _mediaUrlProvider;

        [SetUp]
        public void Init()
        {
            _context = Context.Create(Utilities.CreateStandardResolver());
            _mediaUrlProvider = Substitute.For<BaseMediaManager>();
            SitecoreVersionAbstractions.MediaManager = new LazyResetable<BaseMediaManager>(() => _mediaUrlProvider);

        }

        [TearDown]
        public void Cleanup()
        {
            _context = null;
            _mediaUrlProvider = null;
        }

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

        public class StubContentResolver : GlassRenderingContentsResolver<StubDataSourceClass>
        {
            public StubContentResolver(ISitecoreService sitecoreService) : base(sitecoreService)
            {
                
            }

            public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
            {
                return GetContextItem<StubDataSourceClass>(rendering, renderingConfig);
            }
        }
        #endregion

        #if SC93 || SC100
        [Test]
        public void GetStubComponent_ReturnsStubComponentSerializedObject()
        {
            //Assign
            var itemId = Guid.NewGuid();
            const string itemPath = "/sitecore/content/target";

            //Fields
            const string stringFieldName = "StringField";
            const string stringFieldValue = "testing123";
            const string imageFieldName = "Image";
            const string imageFieldValue =
                "<image mediaid=\"{D897833C-1F53-4FAE-B54B-BB5B11B8F851}\" mediapath=\"/Files/20121222_001405\" src=\"~/media/D897833C1F534FAEB54BBB5B11B8F851.ashx\" hspace=\"15\" vspace=\"20\" />";
            var mediaId = new ID("{D897833C-1F53-4FAE-B54B-BB5B11B8F851}");
            var mediaUrlEnd = "/~/media/Test.ashx";

            //Fake DB
            using (var database = new Db
            {
                new DbItem("Target", new ID(itemId))
                {
                    new DbField(stringFieldName)
                    {
                        Value = stringFieldValue
                    },
                    new DbField(imageFieldName)
                    {
                        Value = imageFieldValue
                    },
                },
                new DbItem("MediaItem", mediaId)
                {
                    new DbField("alt") {Value = "test alt"},
                    new DbField("height") {Value = "480"},
                    new DbField("width") {Value = "640"},
                }
            })
            {
                // Arrange 
                _context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubDataSourceClass)));

                _mediaUrlProvider
                    .GetMediaUrl(Arg.Is<MediaItem>(i => i.ID == mediaId), Arg.Any<MediaUrlOptions>())
                    .Returns(mediaUrlEnd);

                var db = database.Database;
                var item = db.GetItem(itemPath);

                var service = new SitecoreService(database.Database);

                //Rendering configuration
                var renderingConfig = Substitute.For<IRenderingConfiguration>();
                renderingConfig.IncludeServerUrlInContextItemMediaUrls.Returns(true);

                var rendering = new Rendering
                {
                    RenderingItem = new RenderingItem(item),
                    DataSource = itemPath
                };

                var contentResolver = new StubContentResolver(service);

                //Act
                var result = contentResolver.ResolveContents(rendering, renderingConfig);
                //Cast result object to StubClass as this is the expected type
                var resultObject = (StubDataSourceClass) result;

                //Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(resultObject, Is.Not.Null);
                Assert.That(resultObject.Id, Is.EqualTo(item.ID));
                Assert.That(resultObject.StringField, Is.EqualTo(item.Fields[stringFieldName].Value));
                Assert.That(resultObject.Image.MediaExists, Is.Not.Null);
                Assert.That(resultObject.Image.MediaExists, Is.True);
                Assert.That(resultObject.Image.Src, Does.EndWith(mediaUrlEnd));
            }
        }
    }
    #endif
}