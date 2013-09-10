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
            var expected = "<img src='~/media/Images/Carousel/carousel-example.ashx?w=380' alt='someAlt' width='380' />";
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
            var expected = "<img src='~/media/Images/Carousel/carousel-example.ashx?h=450' alt='someAlt' height='450' />";
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
            var expected = "<img src='~/media/Images/Carousel/carousel-example.ashx' class='someClass' alt='someAlt' />";
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

        [Test]
        public void RenderImage_ValidImageWithWidthAndStretcj_RendersCorrectHtml()
        {
            //Arrange
            var expected = "<img src='~/media/Images/Carousel/carousel-example.ashx?w=900&amp;as=True' alt='someAlt' width='900' />";
            var scContext = Substitute.For<ISitecoreContext>();
            var html = new GlassHtml(scContext);
            var image = new Fields.Image();
            image.Alt = "someAlt";
            image.Width = 200;
            image.Height = 105;
            image.Src = "~/media/Images/Carousel/carousel-example.ashx";
            var parameters = new ImageParameters { Width = 900, AllowStretch = true};
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
            var expected = "<img src='~/media/Images/Carousel/carousel-example.ashx' class='AClass' alt='someAlt' />";
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
            var expected = "<img src='~/media/Images/Carousel/carousel-example.ashx' alt='someAlt' border='9' hspace='10' vspace='15' />";
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
            var expected = "<a href='/somewhere.aspx?temp=fred#aAnchor' class='myclass' target='_blank' title='mytitle' >hello world</a>";
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
            var expected = "<a href='/somewhere.aspx?temp=fred#anOther' class='myclass' target='_blank' title='mytitle' >hello world</a>";
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

        #endregion

    }
}
