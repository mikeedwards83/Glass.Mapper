using System;
using System.Linq.Expressions;
using FluentAssertions;
using Glass.Mapper.Sc.Fields;
using Glass.Mapper.Sc.Web;
using Glass.Mapper.Sc.Web.Ui;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Tests.Web.Ui
{
    [TestFixture]
    public class GlassUserControlTestFixture
    {
        [Test]
        public void Model_returns_from_context_item_successfully()
        {
            // Arrange
            var testHarness = new GlassUserControlTestHarness();
            StubClass expected = new StubClass();
            testHarness.SitecoreContext.GetCurrentItem<StubClass>().Returns(expected);

            // Act
            var result = testHarness.GlassUserControl.Model;

            // Assert
            result.Should().Be(expected);
        }

        [Test]
        public void Model_returns_from_data_source_successfully()
        {
            // Arrange
            var testHarness = new GlassUserControlTestHarness();
            StubClass expected = new StubClass();
            string dataSourceId = Guid.NewGuid().ToString();

            testHarness.RenderingContext.GetDataSource().Returns(dataSourceId);
            testHarness.SitecoreContext.GetItem<StubClass>(dataSourceId, testHarness.GlassUserControl.IsLazy, testHarness.GlassUserControl.InferType).Returns(expected);

            // Act
            var result = testHarness.GlassUserControl.Model;

            // Assert
            result.Should().Be(expected);
        }

        [Test]
        public void Editable_returns_from_glass_html_successfully()
        {
            // Arrange
            var testHarness = new GlassUserControlTestHarness();
            const string expected = "field value";
            StubClass expectedStub = new StubClass();
            testHarness.SitecoreContext.GetCurrentItem<StubClass>().Returns(expectedStub);

            Expression<Func<StubClass, object>> fieldExpression = x => x.Field;
            testHarness.GlassHtml.Editable(testHarness.GlassUserControl.Model, fieldExpression, null as object).Returns(expected);

            // Act
            var result = testHarness.GlassUserControl.Editable(fieldExpression);

            // Assert
            result.Should().Be(expected);
        }

        [Test]
        public void Editable_returns_from_glass_html_with_standard_output_successfully()
        {
            // Arrange
            var testHarness = new GlassUserControlTestHarness();
            const string expected = "field value";
            StubClass expectedStub = new StubClass();
            testHarness.SitecoreContext.GetCurrentItem<StubClass>().Returns(expectedStub);

            Expression<Func<StubClass, object>> fieldExpression = x => x.Field;
            Expression<Func<StubClass, string>> defaultExpression = x => "fred";

            testHarness.GlassHtml.Editable(testHarness.GlassUserControl.Model, fieldExpression, defaultExpression, null).Returns(expected);

            // Act
            var result = testHarness.GlassUserControl.Editable(fieldExpression, defaultExpression);

            // Assert
            result.Should().Be(expected);
        }

        [Test]
        public void RenderImage_returns_from_glass_html_successfully()
        {
            // Arrange
            var testHarness = new GlassUserControlTestHarness();
            const string expected = "superimage";
            StubClass expectedStub = new StubClass();
            testHarness.SitecoreContext.GetCurrentItem<StubClass>().Returns(expectedStub);

            Expression<Func<StubClass, object>> fieldExpression = x => x.GlassImage;
            testHarness.GlassHtml.RenderImage(testHarness.GlassUserControl.Model, fieldExpression, null, false, false).Returns(expected);

            // Act
            var result = testHarness.GlassUserControl.RenderImage(fieldExpression);

            // Assert
            result.Should().Be(expected);
        }

        [Test]
        public void RenderingParameters_returns_from_rendering_context_successfully()
        {
            // Arrange
            var testHarness = new GlassUserControlTestHarness();
            const string expected = "rendering";
            testHarness.RenderingContext.GetRenderingParameters().Returns(expected);

            // Act
            var result = testHarness.GlassUserControl.RenderingParameters;

            // Assert
            result.Should().Be(expected);
        }

        [Test]
        public void RenderingParameters_returns_from_glass_html_successfully()
        {
            // Arrange
            var testHarness = new GlassUserControlTestHarness();
            const string renderingParameters = "rendering";
            testHarness.RenderingContext.GetRenderingParameters().Returns(renderingParameters);
            StubClass expected = new StubClass();
            testHarness.GlassHtml.GetRenderingParameters<StubClass>(renderingParameters).Returns(expected);

            // Act
            var result = testHarness.GlassUserControl.GetRenderingParameters<StubClass>();

            // Assert
            result.Should().Be(expected);
        }

        [Test]
        public void RenderLink_returns_from_glass_html_successfully()
        {
            // Arrange
            var testHarness = new GlassUserControlTestHarness();
            const string expected = "superimage";
            StubClass expectedStub = new StubClass();
            testHarness.SitecoreContext.GetCurrentItem<StubClass>().Returns(expectedStub);

            Expression<Func<StubClass, object>> fieldExpression = x => x.GlassLink;
            testHarness.GlassHtml.RenderLink(testHarness.GlassUserControl.Model, fieldExpression, null, false, null).Returns(expected);

            // Act
            var result = testHarness.GlassUserControl.RenderLink(fieldExpression);

            // Assert
            result.Should().Be(expected);
        }

        public class StubClass
        {
            public Image GlassImage { get; set; }

            public Link GlassLink { get; set; }

            public string Field { get; set; }
        }

        public class GlassUserControlTestHarness
        {
            public GlassUserControlTestHarness()
            {
                SitecoreContext = Substitute.For<ISitecoreContext>();
                GlassHtml = Substitute.For<IGlassHtml>();
                RenderingContext = Substitute.For<IRenderingContext>();
                GlassUserControl = new GlassUserControl<StubClass>(SitecoreContext, GlassHtml, RenderingContext);
            }

            public IRenderingContext RenderingContext { get; private set; }

            public IGlassHtml GlassHtml { get; private set; }

            public ISitecoreContext SitecoreContext { get; private set; }

            public GlassUserControl<StubClass> GlassUserControl { get; private set; } 
        }
    }
}
