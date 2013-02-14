using System;
using Glass.Mapper.Umb.Integration.Helpers;
using NUnit.Framework;
using Umbraco.Core;
using Umbraco.Core.Services;
using umbraco;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.cache;

namespace Glass.Mapper.Umb.Integration
{
    [TestFixture, RequiresSTA]
    public class ConnectionFixture : BaseDatabaseFactoryTest
    {
       /* [SetUp]
        public virtual void Initialize()
        {
            TestHelper.SetupLog4NetForTests();
            TestHelper.InitializeContentDirectories();

            AppDomain.CurrentDomain.SetData("DataDirectory", TestHelper.CurrentAssemblyDirectory);

            //if (RequiresDbSetup)
                TestHelper.InitializeDatabase();

            //// Resolution.Freeze();

            ////NOTE: We are not constructing with the service context here because it is not required for these tests (currently)
            //// if we do, this means that we have to initialized the RepositoryResolver too.
            //ApplicationContext.Current = new ApplicationContext
            //    {
            //        IsReady = true,
            //        //assign the db context
            //        DatabaseContext = new DatabaseContext(new DefaultDatabaseFactory())
            //    };

        }

        [TearDown]
        public virtual void TearDown()
        {
            TestHelper.CleanContentDirectories();

            ////reset the app context
            //DatabaseContext.Database.Dispose();
            //ApplicationContext.ApplicationCache.ClearAllCache();
            //ApplicationContext.Current = null;
            //// Resolution.IsFrozen = false;

            //if (RequiresDbSetup)
            //{
                TestHelper.ClearDatabase();
            //    SqlCeContextGuardian.CloseBackgroundConnection();
            //}

            AppDomain.CurrentDomain.SetData("DataDirectory", null);

            Cache.ClearAllCache();

            //UmbracoSettings.ResetSetters();
        }*/

        [Test] 
        public void ConnectionTest_ReadsDictionaryItem()
        {
            var value1 = Dictionary.DictionaryItem.hasKey("KEY");
            Assert.IsFalse(value1);

            Dictionary.DictionaryItem.addKey("KEY", "VALUE");

            var value2 = Dictionary.DictionaryItem.hasKey("KEY");
            Assert.IsTrue(value2);
        }
        [Test]
        public void Get_Item_User_Property()
        {
            ContentService c= new ContentService();
            var sd =c.GetById(0);
            Assert.AreEqual(sd, null);
        }
    }
}
