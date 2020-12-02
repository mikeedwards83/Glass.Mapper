using System;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.LayoutService;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.FakeDb;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;

namespace Glass.Mapper.Sc.Headless.Tests
{
    [TestFixture]
    public class GlassRenderingContentsResolverFixture
    {
        #region Stub
        [SitecoreType]
        public class StubClass
        {
            [SitecoreField]
            public virtual string StringField { get; set; }

            [SitecoreId]
            public virtual ID Id { get; set; }
        }

        public class StubContentResolver : GlassRenderingContentsResolver<StubClass>
        {
            public StubContentResolver(ISitecoreService sitecoreService) : base(sitecoreService)
            {
                
            }

            public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
            {
                return GetContextItem<StubClass>(rendering, renderingConfig);
            }
        }
        #endregion

        [Test]
        public void GetStubComponent_ReturnsStubComponentSerializedObject()
        {
            // Arrange 

            //Assign
            Guid id = Guid.NewGuid();

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target", new ID(id))
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));


                //Act
                var db = database.Database;
                var item = db.GetItem("/sitecore/content/target");

                var service = new SitecoreService(database.Database);

                var renderingConfig = Substitute.For<IRenderingConfiguration>();
                var rendering = new Rendering
                {
                    RenderingItem = new RenderingItem(item),
                    DataSource = "/sitecore/content/target"
                };

                var contentResolver = new StubContentResolver(service);
                var result = contentResolver.ResolveContents(rendering, renderingConfig);


                //Assert
                Assert.That(result, Is.Not.Null);
                //Assert.AreEqual(id, result.Id);
            }
        }
    }
}