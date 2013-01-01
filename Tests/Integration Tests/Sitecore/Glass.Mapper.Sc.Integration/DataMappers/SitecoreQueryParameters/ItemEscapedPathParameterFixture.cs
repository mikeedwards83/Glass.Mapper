using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Integration.DataMappers.SitecoreQueryParameters
{
    [TestFixture]
    public class ItemEscapedPathParameterFixture : AbstractMapperFixture
    {
        #region Method - GetValue

        [Test]
        public void GetValue_ReturnsItemFullPath()
        {
            //Assign
            var param = new ItemEscapedPathParameter();
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreQueryParameters/ItemEscapedPathParameter");

            //Act
            var result = param.GetValue(item);

            //Assert
            Assert.AreEqual("/#sitecore#/#content#/#Tests#/#DataMappers#/#SitecoreQueryParameters#/#ItemEscapedPathParameter#", result);
        }

        #endregion
    }
}
