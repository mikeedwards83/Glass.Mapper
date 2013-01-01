using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Integration.DataMappers.SitecoreQueryParameters
{
    [TestFixture]
    public class ItemIdParameterFixture : AbstractMapperFixture
    {
        #region Method - GetValue

        [Test]
        public void GetValue_ReturnsItemFullPath()
        {
            //Assign
            var param = new ItemIdParameter();
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreQueryParameters/ItemIdParameter");

            //Act
            var result = param.GetValue(item);

            //Assert
            Assert.AreEqual("{DF486E14-EE4D-472A-BD8C-8BBD4C468A2D}", result);
        }

        #endregion
    }
}
