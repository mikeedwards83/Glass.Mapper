using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;
using Sitecore.Data.Items;
using Sitecore.FakeDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.Sc.FakeDb.Issues.Issue376
{
    [TestFixture]
    public class Issue376Fixture
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

                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));


                var service = new SitecoreService(database.Database);
                var item = database.GetItem("/sitecore/content/TestItem");

                //Act
                var result = service.GetItem<StubClass>("/sitecore/content/TestItem");

                //Assign
                Assert.AreEqual(item, result.Item);
            }

        }


        #endregion

        #region Stubs

        public class StubClass
        {
            [SitecoreItem]
            public virtual Item Item { get; set; }

            public virtual string Field1 { get; set; }
        }

        #endregion
    }
}
