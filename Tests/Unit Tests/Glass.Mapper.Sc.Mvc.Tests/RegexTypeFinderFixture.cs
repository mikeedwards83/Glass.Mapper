using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Pipelines.Response;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Mvc.Tests
{
    public class RegexTypeFinderFixture
    {
        [Test]
        [TestCase("@inherits Glass.Mapper.Sc.Razor.Web.Ui.TypedTemplate<Glass.Website.Kernel.View.Common.Menu>", "Glass.Website.Kernel.View.Common.Menu")]
        public void InheritsRegex_ReturnsType(string contents, string expected)
        {
            //Act
            var result = RegexTypeFinder.InheritsRegex.Match(contents);

            //Assert
            Assert.AreEqual(expected, result.Groups["type"].Value);
        }

        [Test]
        [TestCase("@model Glass.Website.Kernel.View.Common.Menu", "Glass.Website.Kernel.View.Common.Menu")]
        public void ModelRegex_ReturnsType(string contents, string expected)
        {
            //Act
            var result = RegexTypeFinder.ModelRegex.Match(contents);

            //Assert
            Assert.AreEqual(expected, result.Groups["type"].Value);
        }
        [Test]
        public void InheritsRegex_MultiLine_ReturnsType()
        {
            //Arrange
            var contents = "@using  Glass.Mapper.Sc.Mvc.Tests\n\r" +
                    "@using Glass.Website.Kernel.Data.sitecore.templates.GlassWebsite.Components\n\r" +
                    "@inherits Glass.Mapper.Sc.Web.Mvc.GlassView<RegexTypeFinderFixture>\n\r";

            //Act
            var result = RegexTypeFinder.InheritsRegex.Match(contents);

            //Assert
            Assert.AreEqual("RegexTypeFinderFixture", result.Groups["type"].Value);
        }



   
        [Test]
        public void GetType_ReturnsType()
        {
            //Arrange
            string contents = "@model Glass.Mapper.Sc.Mvc.Tests.RegexTypeFinderFixture";
            var finder = new StubFinder();
            //Act
            var result = finder.GetType(contents);

            //Assert
            Assert.AreEqual(typeof(RegexTypeFinderFixture), result);
        }

        [Test]
        public void GetType_MultiLine_ReturnsType()
        {

            //Arrange
            var contents = "@using  Glass.Mapper.Sc.Mvc.Tests\n\r" +
                     "@using Glass.Website.Kernel.Data.sitecore.templates.GlassWebsite.Components\n\r" +
                     "@inherits Glass.Mapper.Sc.Web.Mvc.GlassView<RegexTypeFinderFixture>\n\r";

            var finder = new StubFinder();
            //Act
            var result = finder.GetType(contents);

            //Assert
            Assert.AreEqual(typeof(RegexTypeFinderFixture), result);
        }

        [Test]
        public void GetType_MultiLineDiffOrder_ReturnsType()
        {

            //Arrange
            var contents = 
                     "@using Glass.Website.Kernel.Data.sitecore.templates.GlassWebsite.Components\n\r" +
                     "@using  Glass.Mapper.Sc.Mvc.Tests\n\r" +
                     "@inherits Glass.Mapper.Sc.Web.Mvc.GlassView<RegexTypeFinderFixture>\n\r";

            var finder = new StubFinder();
            //Act
            var result = finder.GetType(contents);

            //Assert
            Assert.AreEqual(typeof(RegexTypeFinderFixture), result);
        }

        [Test]
        public void GetType_MultiLineNamespaceSpecialChar_ReturnsType()
        {

            //Arrange
            var contents =
                     "@using Glass.Website.Kernel.Data.sitecore.templates.GlassWebsite.Components\n\r" +
                     "@using  Glass.Mapper.Sc.Mvc.Tests.A_test\n\r" +
                     "@inherits Glass.Mapper.Sc.Web.Mvc.GlassView<Stub>\n\r";

            var finder = new StubFinder();
            //Act
            var result = finder.GetType(contents);

            //Assert
            Assert.AreEqual(typeof(A_test.Stub), result);
        }

     
        public class StubFinder : RegexTypeFinder
        {
            public override string GetContents(string path)
            {
                return path;
            }
        }
    }

    namespace A_test
    {
        public class Stub
        {
            
        }
    }
}
