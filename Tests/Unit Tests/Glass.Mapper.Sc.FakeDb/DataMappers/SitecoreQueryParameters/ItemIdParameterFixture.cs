using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Glass.Mapper.Sc.FakeDb.DataMappers.SitecoreQueryParameters
{
    [TestFixture]
    public class ItemIdParameterFixture 
    {
        #region Method - GetValue

        [Test]
        public void GetValue_ReturnsItemFullPath()
        {
            //Assign
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var param = new ItemIdParameter();
                var item =
                    database.GetItem("/sitecore/content/Target");

                //Act
                var result = param.GetValue(item);

                //Assert
                Assert.AreEqual(item.ID.Guid.ToString("B").ToUpper(), result);
            }
        }

        #endregion
    }
}




