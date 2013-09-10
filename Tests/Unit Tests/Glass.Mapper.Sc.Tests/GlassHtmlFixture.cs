using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.RenderField;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Tests
{
    [TestFixture]
    public class GlassHtmlFixture
    {

        #region Method - RenderImage

        [Test]
        public void RenderImage_ValidImageWithParametersWidth_RendersCorrectHtml()
        {
            //Arrange
            var expected = "<img src='~/media/Images/Carousel/carousel-example.ashx?w=380' alt='someAlt' />";
            var scContext = Substitute.For<ISitecoreContext>();
            var html = new GlassHtml(scContext);
            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var parameters = new ImageParameters {Width = 380};
            var model = new {Image = image};

            //Act
            var result = html.RenderImage(model, x => x.Image, parameters, true);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RenderImage_ValidImageWithParametersHeight_RendersCorrectHtml()
        {
            //Arrange
            var expected = "<img src='~/media/Images/Carousel/carousel-example.ashx?h=450' alt='someAlt' />";
            var scContext = Substitute.For<ISitecoreContext>();
            var html = new GlassHtml(scContext);
            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var parameters = new ImageParameters { Height = 450};
            var model = new { Image = image };

            //Act
            var result = html.RenderImage(model, x => x.Image, parameters, true);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RenderImage_ValidImageWithParametersClass_RendersCorrectHtml()
        {
            //Arrange
            var expected = "<img src='~/media/Images/Carousel/carousel-example.ashx' alt='someAlt' class='someClass' />";
            var scContext = Substitute.For<ISitecoreContext>();
            var html = new GlassHtml(scContext);
            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var parameters = new ImageParameters {Class = "someClass"};
            var model = new { Image = image };

            //Act
            var result = html.RenderImage(model, x => x.Image, parameters, true);

            //Assert
            Assert.AreEqual(expected, result);
        }

        #endregion

    }
}
