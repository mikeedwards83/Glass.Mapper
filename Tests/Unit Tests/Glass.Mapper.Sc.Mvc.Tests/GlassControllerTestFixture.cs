using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using FluentAssertions;
using Glass.Mapper.Sc.Web;
using Glass.Mapper.Sc.Web.Mvc;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Mvc.Tests
{
    [TestFixture]
    public class GlassControllerTestFixture
    {


        #region [ Constructors ]

        [Test]
        public void Constructor_Default_CreatsWithoutException()
        {
            //Act 
            var controller = new GlassController();
        }

        [Test]
        public void Constructor_NullSitecoreContext_CreatsWithoutException()
        {
            //Act 
            var controller = new StubController((ISitecoreContext)null);
        }

        #endregion

        #region [ Data Source Tests ]

        [Test]
        public void GlassController_can_set_and_get_datasource()
        {
            // Arrange
            ID expectedId = new ID(Guid.NewGuid());
            StubClass classToReturn = new StubClass();
            var testHarness = new GlassControllerTestHarness();
            testHarness.RenderingContextWrapper.HasDataSource.Returns(true);
            testHarness.RenderingContextWrapper.GetDataSource().Returns(expectedId.ToString());
            testHarness.SitecoreContext.GetItem<StubClass>(expectedId.ToString()).Returns(classToReturn);

            // Act
            var result = testHarness.GlassController.GetDataSourceItem<StubClass>();

            // Assert
            result.Should().Be(classToReturn);
        }

        [Test]
        public void GlassController_has_no_datasource()
        {
            // Arrange
            var testHarness = new GlassControllerTestHarness();
            testHarness.RenderingContextWrapper.HasDataSource.Returns(true);
            testHarness.RenderingContextWrapper.GetDataSource().Returns(String.Empty);

            // Act
            var result = testHarness.GlassController.GetDataSourceItem<StubClass>();

            // Assert
            result.Should().BeNull();
            testHarness.SitecoreContext.Received(0).GetItem<StubClass>(Arg.Any<string>());
        }

        [Test]
        public void GlassController_no_datasource()
        {
            // Arrange
            var testHarness = new GlassControllerTestHarness();
            testHarness.RenderingContextWrapper.GetDataSource().Returns(String.Empty);

            // Act
            var result = testHarness.GlassController.GetDataSourceItem<StubClass>();

            // Assert
            result.Should().BeNull();
            testHarness.SitecoreContext.Received(0).GetItem<StubClass>(Arg.Any<string>());
        }

        [Test]
        public void GlassController_null_datasource()
        {
            // Arrange
            var testHarness = new GlassControllerTestHarness();
            testHarness.RenderingContextWrapper.GetDataSource().Returns(null as string);

            // Act
            var result = testHarness.GlassController.GetDataSourceItem<StubClass>();

            // Assert
            result.Should().BeNull();
            testHarness.SitecoreContext.Received(0).GetItem<StubClass>(Arg.Any<string>());
        }

        #endregion

        #region [ Context Tests ]

        [Test]
        public void GlassController_can_set_and_get_context()
        {
            // Arrange
            StubClass classToReturn = new StubClass();
            var testHarness = new GlassControllerTestHarness();
            testHarness.SitecoreContext.GetCurrentItem<StubClass>().Returns(classToReturn);

            // Act
            var result = testHarness.GlassController.GetContextItem<StubClass>();

            // Assert
            result.Should().Be(classToReturn);
        }

        #endregion

        #region [ Layout Item Tests ]

        [Test]
        public void GlassController_can_set_and_get_layout_item_from_datasource()
        {
            // Arrange
            ID expectedId = new ID(Guid.NewGuid());
            StubClass classToReturn = new StubClass();
            var testHarness = new GlassControllerTestHarness();
            testHarness.RenderingContextWrapper.HasDataSource.Returns(true);
            testHarness.RenderingContextWrapper.GetDataSource().Returns(expectedId.ToString());
            testHarness.SitecoreContext.GetItem<StubClass>(expectedId.ToString()).Returns(classToReturn);

            // Act
            var result = testHarness.GlassController.GetLayoutItem<StubClass>();

            // Assert
            result.Should().Be(classToReturn);
        }

        [Test]
        public void GlassController_can_set_and_get_layout_item_from_context()
        {
            // Arrange
            StubClass classToReturn = new StubClass();
            var testHarness = new GlassControllerTestHarness();
            testHarness.RenderingContextWrapper.HasDataSource.Returns(false);
            testHarness.SitecoreContext.GetCurrentItem<StubClass>().Returns(classToReturn);

            // Act
            var result = testHarness.GlassController.GetLayoutItem<StubClass>();

            // Assert
            result.Should().Be(classToReturn);
        }

        #endregion

        #region [ View Tests ]

        [Test]
        public void GlassController_can_get_view_action_result()
        {
            // Arrange
            var testHarness = new GlassControllerTestHarness();

            // Act
            var result = testHarness.GlassController.GetView();

            // Assert
        }

        #endregion

        #region [ Rendering Parameters Tests ]

        [Test]
        public void GlassController_can_set_and_get_rendering_parameters()
        {
            // Arrange
            StubClass classToReturn = new StubClass();
            var testHarness = new GlassControllerTestHarness();
            const string renderingParameters = "p=1&r=2";
            testHarness.RenderingContextWrapper.GetRenderingParameters().Returns(renderingParameters);
            testHarness.GlassHtml.GetRenderingParameters<StubClass>(renderingParameters).Returns(classToReturn);

            // Act
            var result = testHarness.GlassController.GetRenderingParameters<StubClass>();

            // Assert
            result.Should().Be(classToReturn);
        }

        [Test]
        public void GlassController_context_inactive_returns_null_rendering_parameters()
        {
            // Arrange
            var testHarness = new GlassControllerTestHarness();

            // Act
            var result = testHarness.GlassController.GetRenderingParameters<StubClass>();

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void GlassController_empty_rendering_parameters()
        {
            // todo: review this behaviour

            // Arrange
            var testHarness = new GlassControllerTestHarness();
            testHarness.RenderingContextWrapper.GetRenderingParameters().Returns(String.Empty);

            // Act
            var result = testHarness.GlassController.GetRenderingParameters<StubClass>();

            // Assert
            result.Should().BeNull();

            testHarness.GlassHtml.Received(0).GetRenderingParameters<StubClass>(Arg.Any<string>());
        }

        [Test]
        public void GlassController_null_rendering_parameters()
        {
            // todo: review this behaviour

            // Arrange
            var testHarness = new GlassControllerTestHarness();
            testHarness.RenderingContextWrapper.GetRenderingParameters().Returns(null as string);

            // Act
            var result = testHarness.GlassController.GetRenderingParameters<StubClass>();

            // Assert
            result.Should().BeNull();

            testHarness.GlassHtml.Received(0).GetRenderingParameters<StubClass>(Arg.Any<string>());
        }

        #endregion

        #region [ Request Context Tests ]

        [Test]
        public void GlassController_can_get_query_string_from_http_context_mock()
        {
            // Arrange
            var testHarness = new GlassControllerTestHarness();
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("fred", "flintstone");
            testHarness.HttpContext.Request.QueryString.Returns(nvc);

            // Act
            var result = testHarness.GlassController.HttpContext.Request.QueryString["fred"];

            // Assert
            result.Should().Be("flintstone");
        }

        #endregion

        public class StubClass
        {
            
        }

        public class GlassControllerTestHarness
        {
            public GlassControllerTestHarness()
            {
                SitecoreContext = Substitute.For<ISitecoreContext>();
                GlassHtml = Substitute.For<IGlassHtml>();
                RenderingContextWrapper = Substitute.For<IRenderingContext>();
                HttpContext = Substitute.For<HttpContextBase>();
                GlassController = new StubController(SitecoreContext, GlassHtml, RenderingContextWrapper, HttpContext);
            }

            public HttpContextBase HttpContext { get; private set; }

            public IRenderingContext RenderingContextWrapper { get; private set; }

            public IGlassHtml GlassHtml { get; private set; }

            public ISitecoreContext SitecoreContext { get; private set; }

            public StubController GlassController { get; private set; }
        }

        public class StubController : GlassController
        {

            public StubController(ISitecoreContext sitecoreContext):base(sitecoreContext)
            {
                
            }
            public StubController(
                ISitecoreContext sitecoreContext,
                IGlassHtml glassHtml,
                IRenderingContext renderingContextWrapper,
                HttpContextBase httpContext) : base(sitecoreContext, glassHtml, renderingContextWrapper, httpContext)
            {

            }

            public ActionResult GetView()
            {
                return View("/asd");
            }

            public new T GetContextItem<T>(bool isLazy = false, bool inferType = false) where T : class
            {
                return base.GetContextItem<T>(isLazy, inferType);
            }

            public new T GetRenderingParameters<T>() where T : class
            {
                return base.GetRenderingParameters<T>();
            }
            public new T GetDataSourceItem<T>(bool isLazy = false, bool inferType = false) where T : class
            {
                return base.GetDataSourceItem<T>(isLazy, inferType);
            }
            public new T GetLayoutItem<T>(bool isLazy = false, bool inferType = false) where T : class
            {
                return base.GetLayoutItem<T>(isLazy, inferType);
            }
        }

        
    }
}
