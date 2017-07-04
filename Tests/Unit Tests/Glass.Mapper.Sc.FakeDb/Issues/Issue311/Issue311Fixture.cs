using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Configuration.Fluent;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;
using Sitecore.Sites;
using Sitecore.Web;

namespace Glass.Mapper.Sc.FakeDb.Issues.Issue311
{
    [TestFixture]
    public class Issue311Fixture
    {
        [Test]
        public void ItemWithTwoNamedFieldsTheSame_UsingName_ReturnsFirstField()
        {
            //Arrange

            var templateId = ID.NewID;
            var fieldId11 = ID.NewID;
            var filedId12 = ID.NewID;
            var fieldName = "Field1";

            using (Db db = new Db()
            {
                new DbTemplate("Base",templateId)
                {
                    new DbField(fieldName, fieldId11)
                    {
                        Type = SitecoreFieldStringMapper.RichTextKey
                    },
                     new DbField(fieldName, filedId12)
                    {
                        Type = SitecoreFieldStringMapper.RichTextKey
                    }
                },

                new DbItem("Target", ID.NewID, templateId)
                {
                    {fieldId11, "Field 1 content" },
                    {filedId12, "Field 2 content" }
                }
            })
            {
                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                var siteContext = new GlassHtmlFixture.SiteContextStub(
                    new SiteInfo(
                        doc.FirstChild
                        )
                    );

                siteContext.SetDisplayMode(DisplayMode.Normal);

                Sitecore.Context.Site = siteContext;

                var loader = new SitecoreFluentConfigurationLoader();
                var stub =  loader.Add<StubClass>();
                stub.Field(x => x.Test1).FieldName(fieldName);
                stub.Field(x => x.Test2).FieldName(fieldName);


                var resolver = Utilities.CreateStandardResolver();
                resolver.DataMapperFactory.Insert(0, () => new Issue145.Issue145.StubDataMapper());

                var context = Context.Create(resolver);

                context.Load(loader);

                var service = new SitecoreService(db.Database, context);

                //Act
                var result = service.GetItem<StubClass>("/sitecore/content/Target");

                //Assert
                Assert.AreEqual("Field 1 content", result.Test1);
                Assert.AreEqual("Field 1 content", result.Test2);

            }
        }

        [Test]
        public void ItemWithTwoNamedFieldsTheSame_UsingIds_ReturnsFirstField()
        {
            //Arrange

            var templateId = ID.NewID;
            var fieldId11 = ID.NewID;
            var filedId12 = ID.NewID;
            var fieldName = "Field1";

            using (Db db = new Db()
            {
                new DbTemplate("Base",templateId)
                {
                    new DbField(fieldName, fieldId11)
                    {
                        Type = SitecoreFieldStringMapper.RichTextKey
                    },
                     new DbField(fieldName, filedId12)
                    {
                        Type = SitecoreFieldStringMapper.RichTextKey
                    }
                },

                new DbItem("Target", ID.NewID, templateId)
                {
                    {fieldId11, "Field 1 content" },
                    {filedId12, "Field 2 content" }
                }
            })
            {
                var doc = new XmlDocument();
                doc.LoadXml(
                    "<site name='GetHomeItem' virtualFolder='/' physicalFolder='/' rootPath='/sitecore/content/Tests/SitecoreContext/GetHomeItem' startItem='/Target1' database='master' domain='extranet' allowDebug='true' cacheHtml='true' htmlCacheSize='10MB' registryCacheSize='0' viewStateCacheSize='0' xslCacheSize='5MB' filteredItemsCacheSize='2MB' enablePreview='true' enableWebEdit='true' enableDebugger='true' disableClientData='false' />");

                var siteContext = new GlassHtmlFixture.SiteContextStub(
                    new SiteInfo(
                        doc.FirstChild
                        )
                    );

                siteContext.SetDisplayMode(DisplayMode.Normal);

                Sitecore.Context.Site = siteContext;

                var loader = new SitecoreFluentConfigurationLoader();
                var stub = loader.Add<StubClass>();
                stub.Field(x => x.Test1).FieldId(fieldId11);
                stub.Field(x => x.Test2).FieldId(filedId12);


                var resolver = Utilities.CreateStandardResolver();
                resolver.DataMapperFactory.Insert(0, () => new Issue145.Issue145.StubDataMapper());

                var context = Context.Create(resolver);

                context.Load(loader);

                var service = new SitecoreService(db.Database, context);

                //Act
                var result = service.GetItem<StubClass>("/sitecore/content/Target");

                //Assert
                Assert.AreEqual("Field 1 content", result.Test1);
                Assert.AreEqual("Field 2 content", result.Test2);

            }
        }
        public class StubClass
        {
            public virtual string Test1 { get; set; }
            public virtual string Test2 { get; set; }
        }
    }
}
