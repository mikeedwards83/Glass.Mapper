using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Integration.DataMappers.SitecoreQueryParameters
{
    [TestFixture]
    public class ItemIdNoBracketsParameterFixture : AbstractMapperFixture
    {
        #region Method - GetValue

        [Test]
        public void GetValue_ReturnsItemFullPath()
        {
            //Assign
            var param = new ItemIdNoBracketsParameter();
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreQueryParameters/ItemIdNoBracketsParameter");

            //Act
            var result = param.GetValue(item);

            //Assert
            Assert.AreEqual("3E61E999FED643E09D128B19D60D8A13".ToLower(), result);
        }

        #endregion
    }
}
