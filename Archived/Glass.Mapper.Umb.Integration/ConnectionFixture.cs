/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-
using System;
using NUnit.Framework;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;

namespace Glass.Mapper.Umb.Integration
{
    [TestFixture]
    public class ConnectionFixture
    {
        [Test]
        public void ConnectionTest()
        {
            var unitOfWork = Global.CreateUnitOfWork();

            var repoFactory = new RepositoryFactory();

            var service = new ContentService(unitOfWork, repoFactory);
            var cTypeService = new ContentTypeService(unitOfWork, repoFactory,
                                                                     new ContentService(unitOfWork),
                                                                     new MediaService(unitOfWork, repoFactory));
            
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

