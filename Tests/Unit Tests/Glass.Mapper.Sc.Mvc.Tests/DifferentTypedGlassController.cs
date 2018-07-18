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
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.Mvc.Tests
{
    [TestFixture]
    public class DifferentTypedGlassControllerTestFixture
    {
        #region [ Data Source Tests ]

       

        [Test]
        public void GlassController_has_no_datasource()
        {
            // Arrange
            var testHarness = new DifferentTypedGlassControllerTestHarness();
            testHarness.MvcContext.HasDataSource.Returns(true);
            testHarness.MvcContext.GetDataSourceItem<DataSourceStubClass>(Arg.Any<GetKnownOptions>()).Returns((DataSourceStubClass) null);

            // Act
            var result = testHarness.GlassController.GetDataSource<DataSourceStubClass>();
            // Assert
            result.Should().BeNull();
            testHarness.MvcContext.Received(1).GetDataSourceItem<DataSourceStubClass>(Arg.Any<GetKnownOptions>());
        }

        [Test]
        public void GlassController_no_datasource()
        {
            // Arrange
            var testHarness = new DifferentTypedGlassControllerTestHarness();
            testHarness.MvcContext.DataSourceItem.Returns((Item) null);

            // Act
            var result = testHarness.GlassController.GetDataSource<DataSourceStubClass>();

            // Assert
            result.Should().BeNull();
            testHarness.MvcContext.Received(0).GetDataSourceItem<DataSourceStubClass>(Arg.Any<GetKnownOptions>());
        }

        [Test]
        public void GlassController_null_datasource()
        {
            // Arrange
            var testHarness = new DifferentTypedGlassControllerTestHarness();
            testHarness.MvcContext.DataSourceItem.Returns((Item)null);

            // Act
            var result = testHarness.GlassController.GetDataSource<DataSourceStubClass>();

            // Assert
            result.Should().BeNull();
            testHarness.MvcContext.Received(0).GetDataSourceItem<DataSourceStubClass>(Arg.Any<GetKnownOptions>());
        }

        #endregion

        #region [ Context Tests ]

        [Test]
        public void GlassController_can_set_and_get_context()
        {
            // Arrange
            ContextStubClass classToReturn = new ContextStubClass();
            var testHarness = new DifferentTypedGlassControllerTestHarness();
            testHarness.MvcContext.GetContextItem<ContextStubClass>(Arg.Any<GetKnownOptions>()).Returns(classToReturn);

            // Act
            var result1 = testHarness.GlassController.GetContext<ContextStubClass>();
            var result2 = testHarness.GlassController.GetContext<ContextStubClass>();

            // Assert
            result1.Should().Be(classToReturn);
            result2.Should().BeSameAs(result1);
            testHarness.MvcContext.Received(2).GetContextItem<ContextStubClass>(Arg.Any<GetKnownOptions>());
        }

        #endregion

        #region [ Request Context Tests ]

        [Test]
        public void GlassController_can_get_query_string_from_http_context_mock()
        {
            // Arrange
            var testHarness = new DifferentTypedGlassControllerTestHarness();
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("fred", "flintstone");
            testHarness.HttpContext.Request.QueryString.Returns(nvc);

            // Act
            var result = testHarness.GlassController.HttpContext.Request.QueryString["fred"];

            // Assert
            result.Should().Be("flintstone");
        }

        #endregion

        public class DataSourceStubClass
        {
            
        }

        public class ContextStubClass
        {
            
        }

        public class DifferentTypedGlassControllerTestHarness
        {
            public DifferentTypedGlassControllerTestHarness()
            {
                MvcContext = Substitute.For<IMvcContext>();
                SitecoreService = Substitute.For<ISitecoreService>();
                HttpContext = Substitute.For<HttpContextBase>();
                GlassController = new StubGlassController(MvcContext);
                GlassController.ControllerContext = new ControllerContext(HttpContext, new RouteData(), GlassController);
            }

            public IMvcContext MvcContext { get; set; }
            public ISitecoreService SitecoreService { get; set; }
            public HttpContextBase HttpContext { get; private set; }

            public StubGlassController GlassController { get; private set; }

            public class StubGlassController : GlassController
            {
                public StubGlassController(
                    IMvcContext mvcContext):base(mvcContext, null)
                {
                }

                public new T GetContext<T>(GetKnownOptions options = null) where T : class
                {
                    return base.GetContext<T>(options);
                }
                public new T GetDataSource<T>(GetKnownOptions options = null) where T : class
                {
                    return base.GetDataSource<T>(options);
                }
            }
        }
    }
}
