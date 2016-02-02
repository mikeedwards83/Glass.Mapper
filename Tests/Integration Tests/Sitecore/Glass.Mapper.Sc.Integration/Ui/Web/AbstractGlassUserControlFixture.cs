using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Web.Ui;
using NUnit.Framework;
using Sitecore.Web.UI.WebControls;

namespace Glass.Mapper.Sc.Integration.Ui.Web
{
    [TestFixture]
    public class AbstractGlassUserControlFixture
    {

        [Test]
        public void GetRenderingParameters_UsingClassWithValue_ReturnsFieldWithValue()
        {
            //Arrange
            var control = new StubAbstractGlassUserControl();
            var resolver = Utilities.CreateStandardResolver();
            var context = Context.Create(resolver);
            var db = Sitecore.Configuration.Factory.GetDatabase("master");

            Sitecore.Context.Database = db;
            control.SitecoreContext = new SitecoreContext(context);
            var sublayout = new Sublayout();
            sublayout.Parameters = "StringField=efefefef";

            sublayout.Controls.Add(control);

            //Act
            var result = control.GetRenderingParameters<GlassTestRenderingParameters>();

            //Act
            Assert.AreEqual("efefefef", result.StringField);
        }


        [Test]
        public void GetRenderingParameters_UsingInterfaceWithValue_ReturnsFieldWithValue()
        {
            //Arrange
            var control = new StubAbstractGlassUserControl();
            var resolver = Utilities.CreateStandardResolver();
            var context = Context.Create(resolver);
            var db = Sitecore.Configuration.Factory.GetDatabase("master");

            Sitecore.Context.Database = db;
            control.SitecoreContext = new SitecoreContext(context);
            var sublayout = new Sublayout();
            sublayout.Parameters = "StringField=efefefef";

            sublayout.Controls.Add(control);

            //Act
            var result = control.GetRenderingParameters<IGlassTestRenderingParameters>();

            //Act
            Assert.AreEqual("efefefef", result.StringField);
        }
        [Test]
        public void GetRenderingParameters_UsingClassNoValue_ReturnsFieldWithValue()
        {
            //Arrange
            var control = new StubAbstractGlassUserControl();
            var resolver = Utilities.CreateStandardResolver();
            var context = Context.Create(resolver);
            var db = Sitecore.Configuration.Factory.GetDatabase("master");

            Sitecore.Context.Database = db;
            control.SitecoreContext = new SitecoreContext(context);
            var sublayout = new Sublayout();
            sublayout.Parameters = string.Empty;

            sublayout.Controls.Add(control);

            //Act
            var result = control.GetRenderingParameters<GlassTestRenderingParameters>();

            //Act
            Assert.AreEqual(null, result);

        }


        [Test]
        public void GetRenderingParameters_UsingInterfaceNoValue_ReturnsFieldWithValue()
        {
            //Arrange
            var control = new StubAbstractGlassUserControl();
            var resolver = Utilities.CreateStandardResolver();
            var context = Context.Create(resolver);
            var db = Sitecore.Configuration.Factory.GetDatabase("master");

            Sitecore.Context.Database = db;
            control.SitecoreContext = new SitecoreContext(context);
            var sublayout = new Sublayout();
            sublayout.Parameters = string.Empty;

            sublayout.Controls.Add(control);

            //Act
            var result = control.GetRenderingParameters<IGlassTestRenderingParameters>();

            //Act
            Assert.AreEqual(null, result);
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
        }


    }
}
