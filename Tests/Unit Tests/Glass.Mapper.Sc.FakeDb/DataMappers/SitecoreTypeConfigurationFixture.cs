using System;
using Glass.Mapper.Sc.Configuration;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;

namespace Glass.Mapper.Sc.FakeDb.DataMappers
{
    [TestFixture]
    public class SitecoreTypeConfigurationFixture
    {
        #region ResolveItem

        [Test]
        public void ResolveItem()
        {
            //Arrange 

            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("TestItem")
            })
            {
                var config = new SitecoreTypeConfiguration();
                config.ItemUriConfig = new SitecoreInfoConfiguration();
                config.ItemUriConfig.PropertyInfo = typeof(StubClass).GetProperty("ItemUri");

                string path = "/sitecore/content/TestItem";
                var expected = database.GetItem(path);

                var instance = new StubClass();
                instance.ItemUri = new ItemUri(expected.ID, expected.Language, expected.Version, expected.Database);

                //Act
                var result = config.ResolveItem(instance, database.Database);

                //Assert
                Assert.AreEqual(expected.ID, result.ID);
                Assert.AreEqual(expected.Version, result.Version);
                Assert.AreEqual(expected.Language, result.Language);
            }

        }

        [Test]
        public void ResolveItem_NoId_ThrowsException()
        {
            //Arrange 
            using (Db database = new Db
            {
                new Sitecore.FakeDb.DbItem("TestItem")
            })
            {
                var config = new SitecoreTypeConfiguration();
                config.Type = typeof(StubClassNoId);

                config.AddProperty(
                    new SitecoreFieldConfiguration()
                    {
                        PropertyInfo = typeof(StubClassNoId).GetProperty("Field")
                    });

                var instance = new StubClassNoId();


                //Act
                Assert.Throws<NotSupportedException>(() => config.ResolveItem(instance, database.Database));

                //Assert
            }

        }

        #endregion


        #region Stubs

        public class StubClassNoId : StubInterface
        {
            public virtual string Field { get; set; }
        }

        public interface StubInterface
        {
            
        }
        public class StubClass
        {
            
            public virtual ItemUri ItemUri { get; set; }

        }


        #endregion

    }
}
