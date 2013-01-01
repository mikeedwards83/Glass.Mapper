using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Integration.DataMappers.SitecoreQueryParameters
{
    [TestFixture]
    public class ItemPathParameterFixture : AbstractMapperFixture
    {
        #region Method - GetValue

        [Test]
        public void GetValue_ReturnsItemFullPath()
        {
            //Assign
            var param = new ItemPathParameter();
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreQueryParameters/ItemPathParameter");

            //Act
            var result = param.GetValue(item);

            //Assert
            Assert.AreEqual(item.Paths.FullPath, result);
        }

        #endregion
    }
}
