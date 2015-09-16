using System;
using System.Collections.Specialized;
using System.Web;
using FluentAssertions;
using Glass.Mapper.Sc.Web.Mvc;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Mvc.Tests
{
    [TestFixture]
    public class DifferentTypedGlassControllerTestFixture
    {
        #region [ Data Source Tests ]

        [Test]
        public void GlassController_can_set_and_get_datasource()
        {
            // Arrange
            ID expectedId = new ID(Guid.NewGuid());
            DataSourceStubClass classToReturn = new DataSourceStubClass();
            var testHarness = new DifferentTypedGlassControllerTestHarness();
            testHarness.RenderingContextWrapper.HasDataSource.Returns(true);
            testHarness.RenderingContextWrapper.GetDataSource().Returns(expectedId.ToString());
            testHarness.SitecoreContext.GetItem<DataSourceStubClass>(expectedId.ToString()).Returns(classToReturn);

            // Act
            var result1 = testHarness.GlassController.DataSource;
            var result2 = testHarness.GlassController.DataSource;

            // Assert
            result1.Should().Be(classToReturn);
            result2.Should().BeSameAs(result1);
            testHarness.SitecoreContext.Received(1).GetItem<DataSourceStubClass>(expectedId.ToString());
        }

        [Test]
        public void GlassController_has_no_datasource()
        {
            // Arrange
            var testHarness = new DifferentTypedGlassControllerTestHarness();
            testHarness.RenderingContextWrapper.HasDataSource.Returns(true);
            testHarness.RenderingContextWrapper.GetDataSource().Returns(String.Empty);

            // Act
            var result = testHarness.GlassController.DataSource;

            // Assert
            result.Should().BeNull();
            testHarness.SitecoreContext.Received(0).GetItem<DataSourceStubClass>(Arg.Any<string>());
        }

        [Test]
        public void GlassController_no_datasource()
        {
            // Arrange
            var testHarness = new DifferentTypedGlassControllerTestHarness();
            testHarness.RenderingContextWrapper.GetDataSource().Returns(String.Empty);

            // Act
            var result = testHarness.GlassController.DataSource;

            // Assert
            result.Should().BeNull();
            testHarness.SitecoreContext.Received(0).GetItem<DataSourceStubClass>(Arg.Any<string>());
        }

        [Test]
        public void GlassController_null_datasource()
        {
            // Arrange
            var testHarness = new DifferentTypedGlassControllerTestHarness();
            testHarness.RenderingContextWrapper.GetDataSource().Returns(null as string);

            // Act
            var result = testHarness.GlassController.DataSource;

            // Assert
            result.Should().BeNull();
            testHarness.SitecoreContext.Received(0).GetItem<DataSourceStubClass>(Arg.Any<string>());
        }

        #endregion

        #region [ Context Tests ]

        [Test]
        public void GlassController_can_set_and_get_context()
        {
            // Arrange
            ContextStubClass classToReturn = new ContextStubClass();
            var testHarness = new DifferentTypedGlassControllerTestHarness();
            testHarness.SitecoreContext.GetCurrentItem<ContextStubClass>().Returns(classToReturn);

            // Act
            var result1 = testHarness.GlassController.Context;
            var result2 = testHarness.GlassController.Context;

            // Assert
            result1.Should().Be(classToReturn);
            result2.Should().BeSameAs(result1);
            testHarness.SitecoreContext.Received(1).GetCurrentItem<ContextStubClass>();
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
                SitecoreContext = Substitute.For<ISitecoreContext>();
                GlassHtml = Substitute.For<IGlassHtml>();
                RenderingContextWrapper = Substitute.For<IRenderingContextWrapper>();
                HttpContext = Substitute.For<HttpContextBase>();
                GlassController = new GlassController<ContextStubClass, DataSourceStubClass>(SitecoreContext, GlassHtml, RenderingContextWrapper, HttpContext);
            }

            public HttpContextBase HttpContext { get; private set; }

            public IRenderingContextWrapper RenderingContextWrapper { get; private set; }

            public IGlassHtml GlassHtml { get; private set; }

            public ISitecoreContext SitecoreContext { get; private set; }

            public GlassController<ContextStubClass, DataSourceStubClass> GlassController { get; private set; }
        }
    }
}
