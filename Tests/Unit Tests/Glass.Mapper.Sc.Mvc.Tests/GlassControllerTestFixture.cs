using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FluentAssertions;
using Glass.Mapper.Sc.Web.Mvc;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Mvc.Tests
{
    [TestFixture]
    public class GlassControllerTestFixture
    {


        #region Constructors

        [Test]
        public void Constructor_Default_CreatsWithoutException()
        {
            //Act 
            Context.Create(Substitute.For<Sc.IoC.IDependencyResolver>());
            var controller = new GlassController();
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
            testHarness.MvcContext.HasDataSource.Returns(true);
            testHarness.MvcContext.GetDataSourceItem<StubClass>(Arg.Any<GetKnownOptions>()).Returns(classToReturn);

            // Act
            var result = testHarness.GlassController.GetDataSource<StubClass>();

            // Assert
            result.Should().Be(classToReturn);
        }

        [Test]
        public void GlassController_has_no_datasource()
        {
            // Arrange
            var testHarness = new GlassControllerTestHarness();
            testHarness.MvcContext.HasDataSource.Returns(true);
            testHarness.MvcContext.GetDataSourceItem<StubClass>(Arg.Any<GetKnownOptions>()).Returns((StubClass)null);


            // Act
            var result = testHarness.GlassController.GetDataSource<StubClass>();

            // Assert
            result.Should().BeNull();
            testHarness.MvcContext.Received(1).GetDataSourceItem<StubClass>(Arg.Any<GetKnownOptions>());
        }


        #endregion

        #region [ Context Tests ]

        [Test]
        public void GlassController_can_set_and_get_context()
        {
            // Arrange
            StubClass classToReturn = new StubClass();
            var testHarness = new GlassControllerTestHarness();
            testHarness.MvcContext.GetContextItem<StubClass>(Arg.Any<GetKnownOptions>()).Returns(classToReturn);

            // Act
            var result = testHarness.GlassController.GetContext<StubClass>();

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
            testHarness.MvcContext.HasDataSource.Returns(true);
            testHarness.MvcContext.GetDataSourceItem<StubClass>(Arg.Any<GetKnownOptions>()).Returns(classToReturn);

            // Act
            var result = testHarness.GlassController.GetLayout<StubClass>();

            // Assert
            result.Should().Be(classToReturn);
        }

        [Test]
        public void GlassController_can_set_and_get_layout_item_from_context()
        {
            // Arrange
            StubClass classToReturn = new StubClass();
            var testHarness = new GlassControllerTestHarness();
            testHarness.MvcContext.HasDataSource.Returns(false);
            testHarness.MvcContext.GetContextItem<StubClass>(Arg.Any<GetKnownOptions>()).Returns(classToReturn);

            // Act
            var result = testHarness.GlassController.GetLayout<StubClass>();

            // Assert
            result.Should().Be(classToReturn);
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
            testHarness.MvcContext.RenderingParameters.Returns(renderingParameters);
            testHarness.MvcContext.GlassHtml.GetRenderingParameters<StubClass>(renderingParameters).Returns(classToReturn);

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
            testHarness.MvcContext.RenderingParameters.Returns(String.Empty);

            // Act
            var result = testHarness.GlassController.GetRenderingParameters<StubClass>();

            // Assert
            result.Should().BeNull();

            testHarness.MvcContext.GlassHtml.Received(0).GetRenderingParameters<StubClass>(Arg.Any<string>());
        }

        [Test]
        public void GlassController_null_rendering_parameters()
        {
            // todo: review this behaviour

            // Arrange
            var testHarness = new GlassControllerTestHarness();
            testHarness.MvcContext.RenderingParameters.Returns(null as string);

            // Act
            var result = testHarness.GlassController.GetRenderingParameters<StubClass>();

            // Assert
            result.Should().BeNull();

            testHarness.MvcContext.GlassHtml.Received(0).GetRenderingParameters<StubClass>(Arg.Any<string>());
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
                MvcContext = Substitute.For<IMvcContext>();
                MvcContext.GlassHtml = Substitute.For<IGlassHtml>();
                HttpContext = Substitute.For<HttpContextBase>();
                GlassController = new StubController(MvcContext);
                GlassController.ControllerContext = new ControllerContext(HttpContext, new RouteData(), GlassController);
            }

            public HttpContextBase HttpContext { get; private set; }
            public IMvcContext MvcContext { get; private set; }
            public StubController GlassController { get; private set; }
        }

        public class StubController : GlassController
        {

         
            public StubController(IMvcContext mvcContext) : base(mvcContext, null)
            {

            }

            public new T GetContext<T>(GetKnownOptions options = null) where T : class
            {
                return base.GetContext<T>(options);
            }

            public new T GetRenderingParameters<T>() where T : class
            {
                return base.GetRenderingParameters<T>();
            }
            public new T GetDataSource<T>(GetKnownOptions options = null) where T : class
            {
                return base.GetDataSource<T>(options);
            }
            public new T GetLayout<T>(GetKnownOptions options = null) where T : class
            {
                return base.GetLayout<T>(options);
            }
        }

        
    }
}
