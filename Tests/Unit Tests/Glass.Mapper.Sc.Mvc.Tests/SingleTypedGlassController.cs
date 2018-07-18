using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FluentAssertions;
using Glass.Mapper.Sc.Web;
using Glass.Mapper.Sc.Web.Mvc;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Mvc.Tests
{
    [TestFixture]
    public class SingleTypedGlassControllerTestFixture
    {
        #region [ Data Source Tests ]

        [Test]
        public void GlassController_can_set_and_get_datasource()
        {
            // Arrange
            ID expectedId = new ID(Guid.NewGuid());
            StubClass classToReturn = new StubClass();
            var testHarness = new SingleTypedGlassControllerTestHarness();
            testHarness.MvcContext.HasDataSource.Returns(true);
            testHarness.MvcContext.GetDataSourceItem<StubClass>(Arg.Any<GetKnownOptions>()).Returns(classToReturn);


            // Act
            var result1 = testHarness.GlassController.GetDataSource<StubClass>();
            var result2 = testHarness.GlassController.GetDataSource<StubClass>();

            // Assert
            result1.Should().Be(classToReturn);
            result2.Should().BeSameAs(result1);
            testHarness.MvcContext.Received(2).GetDataSourceItem<StubClass>(Arg.Any<GetKnownOptions>());
        }

        [Test]
        public void GlassController_has_no_datasource()
        {
            // Arrange
            var testHarness = new SingleTypedGlassControllerTestHarness();
            testHarness.MvcContext.HasDataSource.Returns(true);
            testHarness.MvcContext.GetDataSourceItem<StubClass>(Arg.Any<GetKnownOptions>()).Returns((StubClass) null);


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
            var testHarness = new SingleTypedGlassControllerTestHarness();
            testHarness.MvcContext.GetContextItem<StubClass>(Arg.Any<GetKnownOptions>()).Returns(classToReturn);


            // Act
            var result1 = testHarness.GlassController.GetContext<StubClass>();
            var result2 = testHarness.GlassController.GetContext<StubClass>();

            // Assert
            result1.Should().Be(classToReturn);
            result2.Should().BeSameAs(result1);
            testHarness.MvcContext.Received(2).GetContextItem<StubClass>(Arg.Any<GetKnownOptions>());
        }

        #endregion

        #region [ Layout Item Tests ]

        [Test]
        public void GlassController_can_set_and_get_layout_item_from_datasource()
        {
            // Arrange
            ID expectedId = new ID(Guid.NewGuid());
            StubClass classToReturn = new StubClass();
            var testHarness = new SingleTypedGlassControllerTestHarness();
            testHarness.MvcContext.HasDataSource.Returns(true);
            testHarness.MvcContext.GetDataSourceItem<StubClass>(Arg.Any<GetKnownOptions>()).Returns(classToReturn);

            // Act
            var result1 = testHarness.GlassController.GetLayout<StubClass>();
            var result2 = testHarness.GlassController.GetLayout<StubClass>();

            // Assert
            result1.Should().Be(classToReturn);
            result2.Should().BeSameAs(result1);
            testHarness.MvcContext.Received(2).GetDataSourceItem<StubClass>(Arg.Any<GetKnownOptions>());
        }

        [Test]
        public void GlassController_can_set_and_get_layout_item_from_context()
        {
            // Arrange
            StubClass classToReturn = new StubClass();
            var testHarness = new SingleTypedGlassControllerTestHarness();

            testHarness.MvcContext.HasDataSource.Returns(false);
            testHarness.MvcContext.GetContextItem<StubClass>(Arg.Any<GetKnownOptions>()).Returns(classToReturn);

            // Act
            var result1 = testHarness.GlassController.GetLayout<StubClass>();
            var result2 = testHarness.GlassController.GetLayout<StubClass>();

            // Assert
            result1.Should().Be(classToReturn);
            result2.Should().BeSameAs(result1);
            testHarness.MvcContext.Received(2).GetContextItem<StubClass>(Arg.Any<GetKnownOptions>());

        }

        #endregion

        #region [ Request Context Tests ]

        [Test]
        public void GlassController_can_get_query_string_from_http_context_mock()
        {
            // Arrange
            var testHarness = new SingleTypedGlassControllerTestHarness();
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

        public class SingleTypedGlassControllerTestHarness
        {
            public SingleTypedGlassControllerTestHarness()
            {
                SitecoreService = Substitute.For<ISitecoreService>();
                MvcContext = Substitute.For<IMvcContext>();
                HttpContext = Substitute.For<HttpContextBase>();
                GlassController = new StubGlassController(MvcContext);
                GlassController.ControllerContext = new ControllerContext(HttpContext, new RouteData(), GlassController);
            }

            public HttpContextBase HttpContext { get; private set; }

            public IMvcContext MvcContext { get; private set; }

            public ISitecoreService SitecoreService { get; private set; }

            public StubGlassController GlassController { get; private set; }
        }

        public class StubGlassController : GlassController
        {
            public StubGlassController(IMvcContext mvcContext) :base(mvcContext, null)
            {
            }

            public T GetLayout<T>(GetKnownOptions options = null) where T:class
            {
                return base.GetLayout<T>(options);
            }

            public  T GetContext<T>(GetKnownOptions options = null) where T : class
            {
                return base.GetContext<T>(options);
            }

            public T GetDataSource<T>(GetKnownOptions options = null) where T : class
            {
                return base.GetDataSource<T>(options);
            }
        }
    }
}
