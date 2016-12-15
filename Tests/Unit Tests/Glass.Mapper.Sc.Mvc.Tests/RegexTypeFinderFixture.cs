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

        public class StubFinder : RegexTypeFinder
        {
            public override string GetContents(string path)
            {
                return path;
            }
        }
    }
}
