using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Web;
using Glass.Mapper.Sc.Web.WebForms;
using Glass.Mapper.Sc.Web.WebForms.Ui;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;
using Sitecore.Web.UI.WebControls;

namespace Glass.Mapper.Sc.WebForms.FakeDb.Ui.Web
{
    [TestFixture]
    public class AbstractGlassUserControlFixture
    {

        [Test]
        public void GetRenderingParameters_UsingClassWithValue_ReturnsFieldWithValue()
        {
            //Arrange
            var templateId = new ID("{6C815B38-4D88-4F01-916D-8D7C6548005E}");
            var targetId = ID.NewID;
            var fieldName = "StringField";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                var db = database.Database;
                var resolver = Utilities.CreateStandardResolver();
                var context = Context.Create(resolver);
                var control = new StubAbstractGlassUserControl(new WebFormsContext(new SitecoreService(db, context)));

                Sitecore.Context.Database = db;
                var sublayout = new Sublayout();
                sublayout.Parameters = "StringField=efefefef";

                sublayout.Controls.Add(control);

                //Act
                var result = control.GetRenderingParameters<GlassTestRenderingParameters>();

                //Act
                Assert.AreEqual("efefefef", result.StringField);
            }
        }


        [Test]
        public void GetRenderingParameters_UsingInterfaceWithValue_ReturnsFieldWithValue()
        {
            //Arrange
            var templateId = new ID("{6C815B38-4D88-4F01-916D-8D7C6548005E}");
            var targetId = ID.NewID;
            var fieldName = "StringField";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                var resolver = Utilities.CreateStandardResolver();
                var context = Context.Create(resolver);
                var db = database.Database;
                var control = new StubAbstractGlassUserControl(new WebFormsContext(new SitecoreService(db, context)));

                Sitecore.Context.Database = db;
                var sublayout = new Sublayout();
                sublayout.Parameters = "StringField=efefefef";

                sublayout.Controls.Add(control);

                //Act
                var result = control.GetRenderingParameters<IGlassTestRenderingParameters>();

                //Act
                Assert.AreEqual("efefefef", result.StringField);
            }
        }
        [Test]
        public void GetRenderingParameters_UsingClassNoValue_ReturnsFieldWithValue()
        {
            //Arrange
            var templateId = ID.NewID;
            var targetId = ID.NewID;
            var fieldName = "StringField";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                var resolver = Utilities.CreateStandardResolver();

                var context = Context.Create(resolver);
                var db = database.Database;
                var control = new StubAbstractGlassUserControl(new WebFormsContext(new SitecoreService(db, context)));

                Sitecore.Context.Database = db;
                var sublayout = new Sublayout();
                sublayout.Parameters = string.Empty;

                sublayout.Controls.Add(control);

                //Act
                var result = control.GetRenderingParameters<GlassTestRenderingParameters>();

                //Act
                Assert.AreEqual(null, result);
            }
        }


        [Test]
        public void GetRenderingParameters_UsingInterfaceNoValue_ReturnsFieldWithValue()
        {
            //Arrange
            var templateId = ID.NewID;
            var targetId = ID.NewID;
            var fieldName = "StringField";

            using (Db database = new Db
            {
                new DbTemplate(templateId)
                {
                    {fieldName, ""}
                },
                new Sitecore.FakeDb.DbItem("Target", targetId, templateId),

            })
            {
                var resolver = Utilities.CreateStandardResolver();

                var context = Context.Create(resolver);
                var db = database.Database;
                var control = new StubAbstractGlassUserControl(new WebFormsContext(new SitecoreService(db, context)));

                Sitecore.Context.Database = db;
                var sublayout = new Sublayout();
                sublayout.Parameters = string.Empty;

                sublayout.Controls.Add(control);

                //Act
                var result = control.GetRenderingParameters<IGlassTestRenderingParameters>();

                //Act
                Assert.AreEqual(null, result);
            }
        }

        [SitecoreType(TemplateId = "{6C815B38-4D88-4F01-916D-8D7C6548005E}", AutoMap = true)]
        public interface IGlassTestRenderingParameters
        {
            [SitecoreField("StringField")]
            string StringField { get; set; }
        }

        [SitecoreType(TemplateId = "{6C815B38-4D88-4F01-916D-8D7C6548005E}", AutoMap = true)]
        public interface GlassTestRenderingParameters
        {
            [SitecoreField("StringField")]
            string StringField { get; set; }
        }


        public class StubAbstractGlassUserControl : AbstractGlassUserControl
        {
            public StubAbstractGlassUserControl(IWebFormsContext context) : base(context)
            {
            }
        }


    }
}
