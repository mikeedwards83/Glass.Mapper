using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Sc.Configuration.Attributes;
using NUnit.Framework;
using Sitecore.FakeDb;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Globalization;

namespace Glass.Mapper.Sc.FakeDb.Caching
{
    [TestFixture]
    public class CacheFixtures
    {
        [Test]
        public void Cache_CachableItemRequestedInTwoLanguagesThatDontHaveVersions_ReturnsToSeparateInstances()
        {
            //Arrange
            string path = "/sitecore/content/target";

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreService(database.Database);
                var lang1 = Language.Parse("fr-Fr");
                var lang2 = Language.Parse("nl-BE");
                var lang3 = Language.Parse("en");

                //Act
                    var langItem1 = service.GetItem<StubClass>(path, x=>x.Language(lang1).CacheEnabled().VersionCountDisable());
                    var langItem2 = service.GetItem<StubClass>(path, x=>x.Language(lang2).CacheEnabled().VersionCountDisable());
                    var langItem3 = service.GetItem<StubClass>(path, x=>x.Language(lang3).CacheEnabled().VersionCountDisable());
                

                //Assert
                Assert.NotNull(langItem1);
                Assert.NotNull(langItem2);
                Assert.NotNull(langItem3);
                Assert.AreNotEqual(langItem2, langItem3);
                Assert.AreNotEqual(langItem2, langItem1);
            }
        }

        [Test]
        public void Cache_CachableItemRequestedTwiceReturnsTheSameInstance()
        {
            //Arrange
            string path = "/sitecore/content/target";

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("Target")
            })
            {
                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new OnDemandLoader<SitecoreTypeConfiguration>(typeof(StubClass)));

                var service = new SitecoreService(database.Database);
                var lang1 = Language.Parse("en");


                //Act
                var item1 = service.GetItem<StubClass>(path, x=>x.Language(lang1).CacheEnabled());
                var item2 = service.GetItem<StubClass>(path, x=>x.Language(lang1).CacheEnabled());

                //Assert
                Assert.AreEqual(item1, item2);

            }
        }

        [SitecoreType]
        public class StubClass
        {

        }

    }
}
