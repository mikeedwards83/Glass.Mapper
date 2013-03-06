using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using SqlCE4Umbraco;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.UnitOfWork;
using Umbraco.Web;

namespace Glass.Mapper.Umb.Integration
{
    public class Global
    {
        private static bool _connStringSetup = false;

        public const string ProviderName = "System.Data.SqlServerCe.4.0";

        private static CoreBootManager _manager = null;

        /// <summary>
        /// Prints a list of tables in the umbraco DB
        /// </summary>
        /// <param name="connString"></param>
        public static void PrintTables(string connString)
        {
            var reader =
                new SqlCEHelper(connString).ExecuteReader(
                    "select table_name from information_schema.tables where TABLE_TYPE <> 'VIEW'");
            while (reader.Read())
            {
                var tableName = reader.GetString("table_name");
                Console.WriteLine(tableName);
                var reader1 =
                    new SqlCEHelper(connString).ExecuteScalar<int>("select count(*) from {0}".Formatted(tableName));
                Console.WriteLine(reader1);
            }
        }

        public static string ConnectionString
        {
            get { return ConfigureConnectionString(); }
        }

        public static void CleanPreviousRun()
        {
            if (ApplicationContext.Current != null && ApplicationContext.Current.DatabaseContext != null)
                ApplicationContext.Current.DatabaseContext.Database.Dispose();
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                @"\UmbracoPetaPocoTests.sdf";
            File.Delete(path);
        }

        /// <summary>
        /// Sets up the connection string from the web.config
        /// </summary>
        /// <returns></returns>
        public static string ConfigureConnectionString()
        {
            if (!_connStringSetup)
            {

                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                    @"\UmbracoPetaPocoTests.sdf";

                var connstring = "Data Source={0};Persist Security Info=False;".Formatted(path);

                //to set the connection string from the web.config we have to override the readonly
                var settings = ConfigurationManager.ConnectionStrings["umbracoDbDSN"];
                var fi = typeof (ConfigurationElement).GetField("_bReadOnly",
                                                                BindingFlags.Instance | BindingFlags.NonPublic);
                fi.SetValue(settings, false);

                ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ConnectionString = connstring;
            }

            return ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ConnectionString;
        }

        public static void InitializeUmbraco()
        {
            //You can only boot the manager once per thread
            //if you run tests twice without this check you get an exception
            var umbracoApplication = new UmbracoApplication();
            _manager = new WebBootManager(umbracoApplication);
            _manager.Initialize();
            _manager.Startup(appContext => {});
            _manager.Complete(appContext => {});
        }
    
        public static void ConfigureDatabase()
        {
            var installer = new SqlCEInstaller(new SqlCEHelper(ConnectionString));
            if (installer.CanConnect)
            {
                //  installer.Install();
                UmbracoDatabase umbracoDatabase = new UmbracoDatabase(ConnectionString, ProviderName);
                umbracoDatabase.CreateDatabaseSchema();
            }
        }

        public static PetaPocoUnitOfWorkProvider CreateUnitOfWork()
        {
            return new PetaPocoUnitOfWorkProvider(ConnectionString, ProviderName);
        }
    }
}
