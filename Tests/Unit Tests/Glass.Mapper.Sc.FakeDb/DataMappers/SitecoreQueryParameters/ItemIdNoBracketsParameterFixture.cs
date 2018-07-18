using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Glass.Mapper.Sc.FakeDb.DataMappers.SitecoreQueryParameters
{
    [TestFixture]
    public class ItemIdNoBracketsParameterFixture 
    {
        #region Method - GetValue

        [Test]
        public void GetValue_ReturnsItemFullPath()
        {
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                //Assign
                var param = new ItemIdNoBracketsParameter();
                var item =
                    database.GetItem(
                        "/sitecore/content/Target");

                //Act
                var result = param.GetValue(item);

                //Assert
                Assert.AreEqual(item.ID.Guid.ToString("N").ToLower(), result);
            }
        }

        #endregion
    }
}




