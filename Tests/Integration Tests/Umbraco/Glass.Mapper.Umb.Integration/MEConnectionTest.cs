using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlServerCe;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using SqlCE4Umbraco;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.UnitOfWork;
using Umbraco.Core.Services;
using Umbraco.Web;
using umbraco.DataLayer;
using umbraco.NodeFactory;
using umbraco.cms.businesslogic.web;

namespace Glass.Mapper.Umb.Integration
{
    [TestFixture]
    public class MEConnectionTest
    {



        [Test]
        public void ConnectionTest()
        {

            var providerName = "System.Data.SqlServerCe.4.0";
            var connstring = ConfigureConnectionString();

            //we have to boot umbraco
            //this can only be done once per test 
            //need to move this so that this is done on the application start
            var result = new CoreBootManager(new UmbracoApplication());
            result.Initialize();

            ConfigureDatabase(connstring, providerName);

            var unitOfWork = new PetaPocoUnitOfWorkProvider(connstring, providerName);

            //not required 
            PrintTables(connstring);

            var repoFactory = new RepositoryFactory();

            var service = new ContentService(unitOfWork, repoFactory);
            ContentTypeService cTypeService = new ContentTypeService(unitOfWork, repoFactory,
                                                                     new ContentService(unitOfWork),
                                                                     new MediaService(unitOfWork, repoFactory));


            var contents = service.GetRootContent();

            ContentType cType = new ContentType(-1);
            cType.Name = "TestType";
            cType.Alias = "TestType";
            cType.Thumbnail = string.Empty;

            cTypeService.Save(cType);

            Console.WriteLine("Ctype " + cType.Id);
            Assert.Greater(cType.Id, 0);

            Content content = new Content("METest", -1, cType);
            service.Save(content);

            Assert.Greater(content.Id, 0);

            var content2 = service.GetById(content.Id);
            Assert.AreEqual(content.Name, content2.Name);


        }

        /// <summary>
        /// Sets up the connection string from the web.config
        /// </summary>
        /// <returns></returns>
        public static string ConfigureConnectionString()
        {
            var path =
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) +
                @"\MEUmbracoPetaPocoTests.sdf;";
            Console.WriteLine(path);


            var connstring = "Data Source={0}Persist Security Info=False;".Formatted(path);

            //to set the connection string from the web.config we have to override the readonly
            var settings = ConfigurationManager.ConnectionStrings["umbracoDbDSN"];
            var fi = typeof (ConfigurationElement).GetField("_bReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            fi.SetValue(settings, false);

            ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ConnectionString = connstring;
            return connstring;
        }

        public static void ConfigureDatabase(string connString, string provider)
        {
            new SqlCeEngine(connString).CreateDatabase();

            UmbracoDatabase umbracoDatabase = new UmbracoDatabase(connString, provider);


            PetaPocoExtensions.CreateDatabaseSchema((Database)umbracoDatabase);
        }
    

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
    
}
}

