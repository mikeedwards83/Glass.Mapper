using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;
using Sitecore.Data.Items;
using Sitecore.FakeDb;

namespace Glass.Mapper.Sc.FakeDb.DataMappers
{
    [TestFixture]
    public class SitecoreItemMapperFixture
    {

        #region MapToProperty

        [Test]
        public void MapToProperty_MapsItemToProperty()
        {
            //Arrange

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("TestItem")
            })
            {
                var item = database.GetItem("/sitecore/content/TestItem");
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
