using System;
using System.Linq.Expressions;
using Glass.Mapper.Sc.Fields;
using Glass.Mapper.Sc.Web;
using Glass.Mapper.Sc.Web.Ui;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Sc.FakeDb.Web.Ui
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
            Assert.AreEqual(expected, result);
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
            Assert.AreEqual(expected, result);

           
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
            Assert.AreEqual(expected, result);

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
            Assert.AreEqual(expected, result);

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
            Assert.AreEqual(expected, result);

        }

        [Test]
        public void DataSource_returns_from_rendering_context_successfully()
        {
            // Arrange
            var testHarness = new GlassUserControlTestHarness();
            const string expected = "data source";
            testHarness.RenderingContext.GetDataSource().Returns(expected);

            // Act
            var result = testHarness.GlassUserControl.DataSource;

            // Assert
                        Assert.AreEqual(expected, result);
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
                        Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetRenderingParameters_returns_from_glass_html_successfully()
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
                        Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetRenderingParameters_returns_if_null_successfully()
        {
            // Arrange
            var testHarness = new GlassUserControlTestHarness();
            testHarness.RenderingContext.GetRenderingParameters().Returns(null as string);

            // Act
            var result = testHarness.GlassUserControl.GetRenderingParameters<StubClass>();

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetRenderingParameters_returns_if_empty_string_successfully()
        {
            // Arrange
            var testHarness = new GlassUserControlTestHarness();
            testHarness.RenderingContext.GetRenderingParameters().Returns(String.Empty);

            // Act
            var result = testHarness.GlassUserControl.GetRenderingParameters<StubClass>();

            // Assert
            Assert.IsNull(result);
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
                        Assert.AreEqual(expected, result);
        }

        [Test] // Most DI frameworks use property injection since the construction is done via the webforms framework
        public void Can_set_values_using_property_injection_successfully()
        {
            // Arrange
            ISitecoreContext sitecoreContext = Substitute.For<ISitecoreContext>();
            IRenderingContext renderingContext = Substitute.For<IRenderingContext>();
            IGlassHtml glassHtml = Substitute.For<IGlassHtml>();
            var glassUserControl = new GlassUserControl<StubClass>();
            glassUserControl.SitecoreContext = sitecoreContext;
            glassUserControl.RenderingContext = renderingContext;
            glassUserControl.GlassHtml = glassHtml;

            // Act - no actions to perform

            // Assert
            Assert.AreEqual(sitecoreContext, glassUserControl.SitecoreContext);
            Assert.AreEqual(renderingContext, glassUserControl.RenderingContext);
            Assert.AreEqual(glassHtml, glassUserControl.GlassHtml);
        }

        [Test]
        [Ignore("Fails not sure why")]
        public void Not_setting_sitecore_context_results_in_original_behaviour()
        {
            Assert.Throws<NotSupportedException>(() =>
            {
                // Arrange
                var glassUserControl = new GlassUserControl<StubClass>();

                // Act
                var result = glassUserControl.SitecoreContext;

                // Assert - expected NotSupportedException from SitecoreContext cache
            });

        }

        [Test]
        [Ignore("Fails not sure why")]
        public void Not_setting_glass_html_results_in_original_behaviour()
        {
            Assert.Throws<NotSupportedException>(() =>
            {
                // Arrange
                var glassUserControl = new GlassUserControl<StubClass>();

            // Act
            var result = glassUserControl.GlassHtml;

                // Assert - expected NotSupportedException from SitecoreContext cache
            });
        }

        [Test]
        public void Not_setting_rendering_context_results_in_original_behaviour()
        {
            // Arrange
            var glassUserControl = new GlassUserControl<StubClass>();

            // Act
            IRenderingContext result = glassUserControl.RenderingContext;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(typeof(RenderingContextUserControlWrapper), result.GetType());
        }

        [Test]
        public void GetContextItem_returns_successfully()
        {
            // Arrange
            var testHarness = new GlassUserControlTestHarness();
            StubClass expected = new StubClass();
            testHarness.SitecoreContext.GetCurrentItem<StubClass>().Returns(expected);

            // Act
            var result = testHarness.GlassUserControl.GetContextItem<StubClass>();

            // Assert
                        Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetDataSourceItem_returns_successfully()
        {
            // Arrange
            var testHarness = new GlassUserControlTestHarness();
            testHarness.RenderingContext.GetDataSource().Returns("fred");
            StubClass expected = new StubClass();
            testHarness.SitecoreContext.GetItem<StubClass>("fred").Returns(expected);

            // Act
            var result = testHarness.GlassUserControl.GetDataSourceItem<StubClass>();

            // Assert
                        Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetDataSourceItem_no_datasource_set_returns_null()
        {
            // Arrange
            var testHarness = new GlassUserControlTestHarness();

            // Act
            var result = testHarness.GlassUserControl.GetDataSourceItem<StubClass>();

            // Assert
            Assert.IsNull(result);
            testHarness.SitecoreContext.Received(0).GetItem<StubClass>(Arg.Any<string>());
        }

        [Test]
        public void GetLayoutItem_returns_data_source_successfully()
        {
            // Arrange
            var testHarness = new GlassUserControlTestHarness();
            testHarness.RenderingContext.GetDataSource().Returns("fred");
            StubClass expected = new StubClass();
            testHarness.SitecoreContext.GetItem<StubClass>("fred").Returns(expected);

            // Act
            var result = testHarness.GlassUserControl.GetLayoutItem<StubClass>();

            // Assert
                        Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetLayoutItem_returns_context_successfully()
        {
            // Arrange
            var testHarness = new GlassUserControlTestHarness();
            StubClass expected = new StubClass();
            testHarness.SitecoreContext.GetCurrentItem<StubClass>().Returns(expected);

            // Act
            var result = testHarness.GlassUserControl.GetLayoutItem<StubClass>();

            // Assert
                        Assert.AreEqual(expected, result);
        }

        [Test]
        public void Model_can_be_set_successfully()
        {
            // Arrange
            var testHarness = new GlassUserControlTestHarness();
            StubClass expected = new StubClass();
            testHarness.GlassUserControl.Model = expected;

            // Act
            var result = testHarness.GlassUserControl.Model;

            // Assert
                        Assert.AreEqual(expected, result);
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
