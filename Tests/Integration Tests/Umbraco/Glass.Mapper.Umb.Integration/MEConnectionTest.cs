using System;
using System.Collections.Generic;
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
            var path =
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) +
                @"\MEUmbracoPetaPocoTests.sdf;";
            Console.WriteLine(path);

            var connstring = "Data Source={0}Persist Security Info=False;".Formatted(path);
            var providerName = "System.Data.SqlServerCe.4.0";

            new SqlCeEngine(connstring).CreateDatabase();

            UmbracoDatabase umbracoDatabase = new UmbracoDatabase(connstring, providerName);

            SetProvider();

            PetaPocoExtensions.CreateDatabaseSchema((Database) umbracoDatabase);


            var unitOfWork = new PetaPocoUnitOfWorkProvider(connstring, providerName);
            
            var reader =
               new SqlCEHelper(connstring).ExecuteReader("select table_name from information_schema.tables where TABLE_TYPE <> 'VIEW'");
            while (reader.Read())
            {
                var tableName = reader.GetString("table_name");
                Console.WriteLine(tableName);
                var reader1 =
                    new SqlCEHelper(connstring).ExecuteScalar<int>("select count(*) from {0}".Formatted(tableName));
                Console.WriteLine(reader1);
            }

            var service = new ContentService(unitOfWork, new RepositoryFactory());
            
            var contents = service.GetRootContent();


        }


        public void SetProvider()
        {
            var syntaxConfig =
                typeof (ContentService).Assembly.GetType("Umbraco.Core.Persistence.SqlSyntax.SyntaxConfig");

            Assert.IsNotNull(syntaxConfig);

            var provider =
                typeof (ContentService).Assembly.GetType("Umbraco.Core.Persistence.SqlSyntax.SqlCeSyntaxProvider");

            Assert.IsNotNull(provider);


            var providerInstance =
                provider.Assembly.CreateInstance("Umbraco.Core.Persistence.SqlSyntax.SqlCeSyntaxProvider", false,
                                                 BindingFlags.NonPublic | BindingFlags.Static, null, null, null, null);

            Assert.IsNotNull(providerInstance);


            var method = syntaxConfig.GetProperty("SqlSyntaxProvider", BindingFlags.Public | BindingFlags.Static);
            Assert.IsNotNull(method);


            method.SetValue(null, providerInstance, null);
        }
    
}
}

