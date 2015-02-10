using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
            var expected = "<img src='~/media/Images/Carousel/carousel-example.ashx?h=105&amp;w=240' alt='someAlt' height='105' width='380' />";
            var scContext = Substitute.For<ISitecoreContext>();
            var html = new GlassHtml(scContext);
            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var parameters = new  {Width = 380, W=240};
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
            var expected = "<img src='~/media/Images/Carousel/carousel-example.ashx?h=450&amp;w=200' height='450' alt='someAlt' width='200' />";
            var scContext = Substitute.For<ISitecoreContext>();
            var html = new GlassHtml(scContext);
            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var parameters = new  { Height = 450, H=450};
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
            var expected = "<img src='~/media/Images/Carousel/carousel-example.ashx?h=105&amp;w=200' alt='someAlt' height='105' class='someClass' width='200' />";
            var scContext = Substitute.For<ISitecoreContext>();
            var html = new GlassHtml(scContext);
            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var parameters = new  {Class = "someClass"};
            var model = new { Image = image };

            //Act
            var result = html.RenderImage(model, x => x.Image, parameters, true);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RenderImage_ValidImageWithWidthAndStretcj_RendersCorrectHtml()
        {
            //Arrange
            var expected = "<img src='~/media/Images/Carousel/carousel-example.ashx?h=105&amp;as=True&amp;w=900' alt='someAlt' height='105' width='900' />";
            var scContext = Substitute.For<ISitecoreContext>();
            var html = new GlassHtml(scContext);
            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var parameters = new  { Width = 900, As = true};
            var model = new { Image = image };

            //Act
            var result = html.RenderImage(model, x => x.Image, parameters, true);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RenderImage_ValidImageWithClass_RendersCorrectHtml()
        {
            //Arrange
            var expected = "<img src='~/media/Images/Carousel/carousel-example.ashx?h=105&amp;w=200' alt='someAlt' height='105' class='AClass' width='200' />";
            var scContext = Substitute.For<ISitecoreContext>();
            var html = new GlassHtml(scContext);
            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.Class = "AClass";
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var model = new { Image = image };

            //Act
            var result = html.RenderImage(model, x => x.Image, null, true);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RenderImage_ValidImageWithBorderHSpaceVSpace_RendersCorrectHtml()
        {
            //Arrange
            var expected =
                "<img src='~/media/Images/Carousel/carousel-example.ashx?h=105&amp;w=200' width='200' vspace='15' height='105' hspace='10' border='9' alt='someAlt' />";
            var scContext = Substitute.For<ISitecoreContext>();
            var html = new GlassHtml(scContext);
            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.HSpace = 10;
            image.VSpace = 15;
            image.Border = "9";
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var model = new { Image = image };

            //Act
            var result = html.RenderImage(model, x => x.Image, null, true);

            //Assert
            Assert.AreEqual(expected, result);
        }
        [Test]
        public void RenderImage_ValidImageWithBorderHSpaceVSpaceW_RendersCorrectHtml()
        {
            //Arrange
            var expected =
                "<img src='~/media/Images/Carousel/carousel-example.ashx?h=105&amp;w=400' width='200' vspace='15' height='105' hspace='10' border='9' alt='someAlt' />";
            var scContext = Substitute.For<ISitecoreContext>();
            var html = new GlassHtml(scContext);
            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.HSpace = 10;
            image.VSpace = 15;
            image.Border = "9";
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var model = new { Image = image };
            var parameters = new {w = 400};
            //Act
            var result = html.RenderImage(model, x => x.Image, parameters, true);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RenderImage_RemoveHeightWidthAttributes_RendersCorrectHtml()
        {
            //Arrange
            var expected =
                "<img src='~/media/Images/Carousel/carousel-example.ashx?h=105&amp;w=400' vspace='15' hspace='10' border='9' alt='someAlt' />";
            var scContext = Substitute.For<ISitecoreContext>();
            var html = new GlassHtml(scContext);
            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.HSpace = 10;
            image.VSpace = 15;
            image.Border = "9";
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var model = new { Image = image };
            var parameters = new { w = 400 };
            //Act
            var result = html.RenderImage(model, x => x.Image, parameters, true, false);

            //Assert
            Assert.AreEqual(expected, result);
        }


        #endregion

        #region Method - RenderLink

        [Test]
        public void RenderLink_LinkWithNoAttributes()
        {
            //Arrange
            var expected = "<a href='/somewhere.aspx' >hello world</a>";
            var scContext = Substitute.For<ISitecoreContext>();
            var html = new GlassHtml(scContext);
            var link = new Fields.Link();
            link.Text = "hello world";
            link.Url = "/somewhere.aspx";

            var model = new {Link = link};

            //Act
            var result = html.RenderLink(model, x => x.Link);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RenderLink_LinkWithAllSetProperties()
        {
            //Arrange
            var expected = "<a href='/somewhere.aspx?temp=fred#aAnchor' target='_blank' class='myclass' title='mytitle' >hello world</a>";
            var scContext = Substitute.For<ISitecoreContext>();
            var html = new GlassHtml(scContext);
            var link = new Fields.Link();
            link.Text = "hello world";
            link.Url = "/somewhere.aspx";
            link.Anchor = "aAnchor";
            link.Class = "myclass";
            link.Query = "temp=fred";
            link.Target = "_blank";
            link.Title = "mytitle";

            var model = new { Link = link };

            //Act
            var result = html.RenderLink(model, x => x.Link);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RenderLink_LinkWithMixedPropertiesAndParameters()
        {
            //Arrange
            var expected = "<a href='/somewhere.aspx?temp=fred#anOther' target='_blank' class='myclass' title='mytitle' >hello world</a>";
            var scContext = Substitute.For<ISitecoreContext>();
            var html = new GlassHtml(scContext);
            var link = new Fields.Link();
            link.Text = "hello world";
            link.Url = "/somewhere.aspx";
            link.Anchor = "aAnchor";
            link.Class = "myclass";
            link.Query = "temp=fred";
            link.Target = "_blank";
            link.Title = "mytitle";

            var model = new { Link = link };
            var parameters = new NameValueCollection {{"anchor", "anOther"}};

            //Act
            var result = html.RenderLink(model, x => x.Link, parameters);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RenderLink_LinkWithClassAndQuery()
        {
            //Arrange
            var expected = "<a href='/somewhere.aspx?temp=fred' class='myclass' >hello world</a>";
            var scContext = Substitute.For<ISitecoreContext>();
            var html = new GlassHtml(scContext);
            var link = new Fields.Link();
            link.Text = "hello world";
            link.Url = "/somewhere.aspx";
            link.Query = "temp=fred";

            var model = new { Link = link };
            var parameters = new NameValueCollection { { "class", "myclass" } };

            //Act
            var result = html.RenderLink(model, x => x.Link, parameters);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void RenderLink_LinkWithCustomContent()
        {
            //Arrange
            var expected = "<a href='/somewhere.aspx?temp=fred' class='myclass' >my other content</a>";
            var scContext = Substitute.For<ISitecoreContext>();
            var html = new GlassHtml(scContext);
            var link = new Fields.Link();
            link.Text = "hello world";
            link.Url = "/somewhere.aspx";
            link.Query = "temp=fred";

            var model = new { Link = link };
            var parameters = new NameValueCollection { { "class", "myclass" } };
            var content = "my other content";

            //Act
            var result = html.RenderLink(model, x => x.Link, parameters, contents: content);

            //Assert
            Assert.AreEqual(expected, result);
        }


        [Test]
        public void RenderLink_WithMultiParametersRepeated_ReturnsAllParamters()
        {
            //Arrange
            var expected =
                "<a href='/somewhere.aspx?temp=fred&amp;temp=fred2&amp;temp=fred3&amp;temp1=jane' class='myclass' >my other content</a>";
            var scContext = Substitute.For<ISitecoreContext>();
            var html = new GlassHtml(scContext);
            var link = new Fields.Link();
            link.Text = "hello world";
            link.Url = "/somewhere.aspx";
            link.Query = "temp=fred&temp=fred2&temp=fred3&temp1=jane";

            var model = new {Link = link};
            var parameters = new NameValueCollection {{"class", "myclass"}};
            var content = "my other content";

            //Act
            var result = html.RenderLink(model, x => x.Link, parameters, contents: content);

            //Assert
            Assert.AreEqual(expected, result);
        }

        #endregion

    }
}
