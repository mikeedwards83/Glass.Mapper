using System;
using System.IO;
using System.Reflection;
using log4net.Config;
using SqlCE4Umbraco;
using umbraco.DataLayer;
using Umbraco.Core.IO;

namespace Glass.Mapper.Umb.Integration.Helpers
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
            var dataHelper = DataLayerHelper.CreateSqlHelper(@"datalayer=SQLCE4Umbraco.SqlCEHelper,SQLCE4Umbraco;data source=|DataDirectory|\UmbracoPetaPocoTests.sdf", false) as SqlCEHelper;

            if (dataHelper == null)
                throw new InvalidOperationException("The sql helper for unit tests must be of type SqlCEHelper, check the ensure the connection string used for this test is set to use SQLCE");

            MethodInfo methodInfo = typeof(SqlCEHelper).GetMethod("ClearDatabase", BindingFlags.Instance | BindingFlags.NonPublic);
            methodInfo.Invoke(dataHelper, null);
        }

        /// <summary>
        /// Initializes a new database
        /// </summary>
        public static void InitializeDatabase()
        {
            ClearDatabase();

            var dataHelper = DataLayerHelper.CreateSqlHelper(@"datalayer=SQLCE4Umbraco.SqlCEHelper,SQLCE4Umbraco;data source=|DataDirectory|\UmbracoPetaPocoTests.sdf", false);

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
        static public string CurrentAssemblyDirectory
        {
            get
            {
                var codeBase = typeof(TestHelper).Assembly.CodeBase;
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

        public static void InitializeContentDirectories()
        {
            CreateDirectories(new[] { SystemDirectories.Masterpages, SystemDirectories.MvcViews, SystemDirectories.Media });
        }

        public static void CleanContentDirectories()
        {
            CleanDirectories(new[] { SystemDirectories.Masterpages, SystemDirectories.MvcViews, SystemDirectories.Media });
        }

        public static void CreateDirectories(string[] directories)
        {
            foreach (var directory in directories)
            {
                var directoryInfo = new DirectoryInfo(IOHelper.MapPath(directory));
                if (directoryInfo.Exists == false)
                    Directory.CreateDirectory(IOHelper.MapPath(directory));
            }
        }

        public static void CleanDirectories(string[] directories)
        {
            foreach (var directory in directories)
            {
                var directoryInfo = new DirectoryInfo(IOHelper.MapPath(directory));
                if (directoryInfo.Exists)
                    directoryInfo.GetFiles().ForEach(x => x.Delete());
            }
        }
    }
}
