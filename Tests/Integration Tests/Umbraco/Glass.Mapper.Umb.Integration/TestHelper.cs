using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using SqlCE4Umbraco;
using log4net.Config;
using umbraco;
using umbraco.DataLayer;

namespace Glass.Mapper.Umb.Integration
{
    /// <summary>
    /// Common helper properties and methods useful to testing
    /// </summary>
    public static class TestHelper
    {

        /// <summary>
        /// Clears an initialized database
        /// </summary>
        public static void ClearDatabase()
        {
            var dataHelper = DataLayerHelper.CreateSqlHelper(GlobalSettings.DbDSN) as SqlCEHelper;
            if (dataHelper == null)
                throw new InvalidOperationException(
                    "The sql helper for unit tests must be of type SqlCEHelper, check the ensure the connection string used for this test is set to use SQLCE");
            
            MethodInfo methodInfo = typeof(SqlCEHelper).GetMethod("ClearDatabase", BindingFlags.Instance | BindingFlags.NonPublic);
            methodInfo.Invoke(dataHelper, null);
        }

        /// <summary>
        /// Initializes a new database
        /// </summary>
        public static void InitializeDatabase()
        {
            ConfigurationManager.AppSettings.Set("umbracoDbDSN",
                                                 @"datalayer=SQLCE4Umbraco.SqlCEHelper,SQLCE4Umbraco;data source=|DataDirectory|\Umbraco.sdf");

            ClearDatabase();

            var dataHelper = DataLayerHelper.CreateSqlHelper(GlobalSettings.DbDSN);
            var installer = dataHelper.Utility.CreateInstaller();
            if (installer.CanConnect)
            {
                installer.Install();
            }
        }

        /// <summary>
        /// Gets the current assembly directory.
        /// </summary>
        /// <value>The assembly directory.</value>
        public static string CurrentAssemblyDirectory
        {
            get
            {
                var codeBase = typeof (TestHelper).Assembly.CodeBase;
                var uri = new Uri(codeBase);
                var path = uri.LocalPath;
                return Path.GetDirectoryName(path);
            }
        }

        /// <summary>
        /// Maps the given <paramref name="relativePath"/> making it rooted on <see cref="CurrentAssemblyDirectory"/>. <paramref name="relativePath"/> must start with <code>~/</code>
        /// </summary>
        /// <param name="relativePath">The relative path.</param>
        /// <returns></returns>
        public static string MapPathForTest(string relativePath)
        {
            if (!relativePath.StartsWith("~/"))
                throw new ArgumentException("relativePath must start with '~/'", "relativePath");

            return relativePath.Replace("~/", CurrentAssemblyDirectory + "/");
        }

        public static void SetupLog4NetForTests()
        {
            XmlConfigurator.Configure(new FileInfo(MapPathForTest("~/unit-test-log4net.config")));
        }
    }
}
