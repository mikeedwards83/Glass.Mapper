using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Glass.Mapper.Sc.Web.Ui;
using NUnit.Framework;
using Sitecore.Web.UI.WebControls;

namespace Glass.Mapper.Sc.Tests.Web.Ui
{
    [TestFixture]
    public class RenderingContextUserControlWrapperFixture
    {
        [Test]
        public void GetRenderingParameters_SingleLevelSubLayout_ReturnsParameters()
        {
            //Arrange
            var parameters = "afeafaefaef";
            var sublayout = new Sublayout();
            sublayout.Parameters = parameters;
            var renderingContext = new RenderingContextUserControlWrapper(sublayout);

            //Act
            var result = renderingContext.GetRenderingParameters();

            //Assert
            Assert.AreEqual(parameters, result);

        }

        [Test]
        public void GetRenderingParameters_MultiLevelSubLayout_ReturnsParameters()
        {
            //Arrange
            var parameters = "afeafaefaef";
            var sublayout = new Sublayout();
            sublayout.Parameters = parameters;
            var control = new HtmlGenericControl();
            sublayout.Controls.Add(control);

            var renderingContext = new RenderingContextUserControlWrapper(control);


            //Act
            var result = renderingContext.GetRenderingParameters();

            //Assert
            Assert.AreEqual(parameters, result);

        }

    }
}
