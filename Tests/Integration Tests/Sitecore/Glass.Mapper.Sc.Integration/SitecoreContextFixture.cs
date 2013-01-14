using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Integration
{
    [TestFixture]
    public class SitecoreContextFixture
    {
        #region GetCurrentItem

        [Test]
        public void GetCurrentItem_NoParameters()
        {
            //Assign
            var db = Sitecore.Configuration.Factory.GetDatabase("master");
            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var path = "/sitecore/content/Tests/SitecoreContext/GetCurrentItem/Target";
            var scContext = new SitecoreContext();

            Sitecore.Context.Item = db.GetItem(path);

            //Act
            var result = scContext.GetCurrentItem<StubClass>();

            //Assert
            Assert.AreEqual(Sitecore.Context.Item.ID, result.Id);

        }

        [Test]
        public void GetCurrentItem_OneParameters()
        {

        }

        #endregion

        #region Stub

        [SitecoreType]
        public class StubClass
        {
            public StubClass()
            {

            }

            [SitecoreId]
            public ID Id { get; set; }
        }

        #endregion
    }
}
