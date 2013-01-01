using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using SqlCE4Umbraco;
using umbraco.DataLayer;
using umbraco.cms.businesslogic;
using GlobalSettings = umbraco.GlobalSettings;

namespace Glass.Mapper.Umb.Integration
{
    [TestFixture, RequiresSTA]
    public class ConnectionFixture
    {
        public string CurrentAssemblyDirectory
        {
            get
            {
                var codeBase = typeof(ConnectionFixture).Assembly.CodeBase;
                var uri = new Uri(codeBase);
                var path = uri.LocalPath;
                return Path.GetDirectoryName(path);
            }
        }

        [Test]
        public void ConnectionTest_ReadsDictionaryItem()
        {
            var value1 = Dictionary.DictionaryItem.hasKey("KEY");
            Assert.IsFalse(value1);

            Dictionary.DictionaryItem.addKey("KEY", "VALUE");

            var value2 = Dictionary.DictionaryItem.hasKey("KEY");
            Assert.IsTrue(value2);
        }

        [TearDown]
        public void Dispose()
        {
            ClearDatabase();
            ConfigurationManager.AppSettings.Set("umbracoDbDSN", "");
        }

        [SetUp]
        public void Initialize()
        {
            InitializeDatabase();
        }

        private void ClearDatabase()
        {
            var dataHelper = DataLayerHelper.CreateSqlHelper(GlobalSettings.DbDSN) as SqlCEHelper;
            if (dataHelper == null)
                throw new InvalidOperationException("The sql helper for unit tests must be of type SqlCEHelper, check the ensure the connection string used for this test is set to use SQLCE");

            MethodInfo methodInfo = typeof(SqlCEHelper).GetMethod("ClearDatabase", BindingFlags.Instance | BindingFlags.NonPublic);
            methodInfo.Invoke(dataHelper, null);
            AppDomain.CurrentDomain.SetData("DataDirectory", null);
        }

        private void InitializeDatabase()
        {
            ConfigurationManager.AppSettings.Set("umbracoDbDSN", @"datalayer=SQLCE4Umbraco.SqlCEHelper,SQLCE4Umbraco;data source=|DataDirectory|\Umbraco.sdf");

            ClearDatabase();

            AppDomain.CurrentDomain.SetData("DataDirectory", CurrentAssemblyDirectory);
            var dataHelper = DataLayerHelper.CreateSqlHelper(GlobalSettings.DbDSN);
            var installer = dataHelper.Utility.CreateInstaller();

            if (installer.CanConnect)
                installer.Install();
        }
    }
}
