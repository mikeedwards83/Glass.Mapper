using System;
using System.Collections.Specialized;
using System.Web;
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
            testHarness.RenderingContextWrapper.HasDataSource.Returns(true);
            testHarness.RenderingContextWrapper.GetDataSource().Returns(expectedId.ToString());
            testHarness.SitecoreContext.GetItem<StubClass>(expectedId.ToString()).Returns(classToReturn);

            // Act
            var result1 = testHarness.GlassController.DataSource;
            var result2 = testHarness.GlassController.DataSource;

            // Assert
            result1.Should().Be(classToReturn);
            result2.Should().BeSameAs(result1);
            testHarness.SitecoreContext.Received(1).GetItem<StubClass>(expectedId.ToString());
        }

        [Test]
        public void GlassController_has_no_datasource()
        {
            // Arrange
            var testHarness = new SingleTypedGlassControllerTestHarness();
            testHarness.RenderingContextWrapper.HasDataSource.Returns(true);
            testHarness.RenderingContextWrapper.GetDataSource().Returns(String.Empty);

            // Act
            var result = testHarness.GlassController.DataSource;

            // Assert
            result.Should().BeNull();
            testHarness.SitecoreContext.Received(0).GetItem<StubClass>(Arg.Any<string>());
        }

        [Test]
        public void GlassController_no_datasource()
        {
            // Arrange
            var testHarness = new SingleTypedGlassControllerTestHarness();
            testHarness.RenderingContextWrapper.GetDataSource().Returns(String.Empty);

            // Act
            var result = testHarness.GlassController.DataSource;

            // Assert
            result.Should().BeNull();
            testHarness.SitecoreContext.Received(0).GetItem<StubClass>(Arg.Any<string>());
        }

        [Test]
        public void GlassController_null_datasource()
        {
            // Arrange
            var testHarness = new SingleTypedGlassControllerTestHarness();
            testHarness.RenderingContextWrapper.GetDataSource().Returns(null as string);

            // Act
            var result = testHarness.GlassController.DataSource;

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
            var testHarness = new SingleTypedGlassControllerTestHarness();
            testHarness.SitecoreContext.GetCurrentItem<StubClass>().Returns(classToReturn);

            // Act
            var result1 = testHarness.GlassController.Context;
            var result2 = testHarness.GlassController.Context;

            // Assert
            result1.Should().Be(classToReturn);
            result2.Should().BeSameAs(result1);
            testHarness.SitecoreContext.Received(1).GetCurrentItem<StubClass>();
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
            testHarness.RenderingContextWrapper.HasDataSource.Returns(true);
            testHarness.RenderingContextWrapper.GetDataSource().Returns(expectedId.ToString());
            testHarness.SitecoreContext.GetItem<StubClass>(expectedId.ToString()).Returns(classToReturn);

            // Act
            var result1 = testHarness.GlassController.Layout;
            var result2 = testHarness.GlassController.Layout;

            // Assert
            result1.Should().Be(classToReturn);
            result2.Should().BeSameAs(result1);
            testHarness.SitecoreContext.Received(1).GetItem<StubClass>(expectedId.ToString());
        }

        [Test]
        public void GlassController_can_set_and_get_layout_item_from_context()
        {
            // Arrange
            StubClass classToReturn = new StubClass();
            var testHarness = new SingleTypedGlassControllerTestHarness();
            testHarness.RenderingContextWrapper.HasDataSource.Returns(false);
            testHarness.SitecoreContext.GetCurrentItem<StubClass>().Returns(classToReturn);

            // Act
            var result1 = testHarness.GlassController.Layout;
            var result2 = testHarness.GlassController.Layout;

            // Assert
            result1.Should().Be(classToReturn);
            result2.Should().BeSameAs(result1);
            testHarness.SitecoreContext.Received(1).GetCurrentItem<StubClass>();
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
                SitecoreContext = Substitute.For<ISitecoreContext>();
                GlassHtml = Substitute.For<IGlassHtml>();
                RenderingContextWrapper = Substitute.For<IRenderingContext>();
                HttpContext = Substitute.For<HttpContextBase>();
                GlassController = new GlassController<StubClass>(SitecoreContext, GlassHtml, RenderingContextWrapper, HttpContext);
            }

            public HttpContextBase HttpContext { get; private set; }

            public IRenderingContext RenderingContextWrapper { get; private set; }

            public IGlassHtml GlassHtml { get; private set; }

            public ISitecoreContext SitecoreContext { get; private set; }

            public GlassController<StubClass> GlassController { get; private set; }
        }
    }
}
