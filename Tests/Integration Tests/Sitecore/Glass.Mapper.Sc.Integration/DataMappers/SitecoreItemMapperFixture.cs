using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    [TestFixture]
    public class SitecoreItemMapperFixture
    {

        #region MapToProperty

        [Test]
        public void MapToProperty_MapsItemToProperty()
        {
            //Arrange
            string itemName = Guid.NewGuid().ToString();

            using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db    {
                new Sitecore.FakeDb.DbItem(itemName) { }
            })
            {

                var database = Sitecore.Configuration.Factory.GetDatabase("master");

                var item = database.GetItem("/sitecore/content/{0}".Formatted(itemName));
                var mapper = new SitecoreItemMapper();
                var obj = new StubClass();

                var mappingContext = new SitecoreDataMappingContext(obj, item, null);

                //Act
                var result = mapper.MapToProperty(mappingContext);

                //Assign
                Assert.AreEqual(item, result);
            }
        }


        #endregion

        #region Stubs

        public class StubClass
        {
            public virtual Item Item { get; set; }

            public virtual string Field1 { get; set; }
        }

        #endregion
    }
}
