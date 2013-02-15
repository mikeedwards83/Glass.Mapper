using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlServerCe;
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


        /// <summary>
        /// Sets up the connection string from the web.config
        /// </summary>
        /// <returns></returns>
        public static string ConfigureConnectionString()
        {
            if (!_connStringSetup)
            {

                var path =
                    System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) +
                    @"\MEUmbracoPetaPocoTests.sdf;";
                Console.WriteLine(path);


                var connstring = "Data Source={0}Persist Security Info=False;".Formatted(path);

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
                _manager = new CoreBootManager(new UmbracoApplication());
                _manager.Initialize();
        }
    

    /// <summary>
        /// Setup blank Umbraco DB
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="provider"></param>
        public static void ConfigureDatabase()
        {
            new SqlCeEngine(ConnectionString).CreateDatabase();

            UmbracoDatabase umbracoDatabase = new UmbracoDatabase(ConnectionString, ProviderName);


            PetaPocoExtensions.CreateDatabaseSchema((Database)umbracoDatabase);
        }

        public static PetaPocoUnitOfWorkProvider CreateUnitOfWork()
        {
            return  new PetaPocoUnitOfWorkProvider(ConnectionString, ProviderName);
        }

    }
}
