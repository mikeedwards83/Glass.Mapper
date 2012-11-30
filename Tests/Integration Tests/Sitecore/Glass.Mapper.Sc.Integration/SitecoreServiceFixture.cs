using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Integration
{
    [TestFixture]
    public class SitecoreServiceFixture
    {
        #region Method - GetItem

        [Test]
        public void GetItem_UsingItemId_ReturnsItem()
        {
             //Assign
            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var service = new SitecoreService(db);
            Guid id = new Guid("{6DE18DBD-0AF3-404A-8018-02B8A19515C1}");

            //Act
            var result =(StubClass) null;// service.GetItem<StubClass>(id);

            //Assert
            Assert.IsNotNull(result);
        }

        #endregion

        #region Stubs

        [SitecoreType]
        public class StubClass
        {
           
        }

        #endregion
    }
}
