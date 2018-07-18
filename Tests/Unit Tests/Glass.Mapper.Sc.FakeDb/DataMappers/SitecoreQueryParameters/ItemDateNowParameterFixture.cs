using System;
using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;
using NUnit.Framework;
using Sitecore.FakeDb;

namespace Glass.Mapper.Sc.FakeDb.DataMappers.SitecoreQueryParameters
{
    [TestFixture]
    public class ItemDateNowParameterFixture 
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
                var param = new ItemDateNowParameter();
                var item =
                    database.GetItem(
                        "/sitecore/content/Target");

                //Act
                var result = param.GetValue(item);

                //Assert
                Assert.AreEqual(Sitecore.DateUtil.ToIsoDate(DateTime.Now), result);
            }
        }

        #endregion
    }
}




