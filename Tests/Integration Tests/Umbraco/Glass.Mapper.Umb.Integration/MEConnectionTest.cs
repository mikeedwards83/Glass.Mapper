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

            var unitOfWork = Global.CreateUnitOfWork();

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

       

    

    
    
}
}

