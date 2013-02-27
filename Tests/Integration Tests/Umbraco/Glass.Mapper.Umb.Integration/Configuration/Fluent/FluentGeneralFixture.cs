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
using Glass.Mapper.Umb.Configuration;
using Glass.Mapper.Umb.Configuration.Fluent;
using NUnit.Framework;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;

namespace Glass.Mapper.Umb.Integration.Configuration.Fluent
{
    [TestFixture]
    public class FluentGeneralFixture
    {

        [Test]
        public void General_RetrieveItemAndFieldsFromSitecore_ReturnPopulatedClass()
        {
            //Assign
            string fieldValue = "test field value";
          //  int id = 2000;
            string name = "Target";

            var unitOfWork = Global.CreateUnitOfWork();

            var repoFactory = new RepositoryFactory();

            var service = new ContentService(unitOfWork, repoFactory);
            ContentTypeService cTypeService = new ContentTypeService(unitOfWork, repoFactory,
                                                                     new ContentService(unitOfWork),
                                                                     new MediaService(unitOfWork, repoFactory));

            var context = Context.Create(new Umb.GlassConfig());

            var loader = new UmbracoFluentConfigurationLoader();

            var stubConfig = loader.Add<Stub>();
            stubConfig.Configure(x =>
                                     {
                                         //x.Id(y => y.Id);
                                        // x.Property(y => y.Property);
                                         x.Info(y => y.Name).InfoType(UmbracoInfoType.Name);
                                     });

            context.Load(loader);

            ContentType cType = new ContentType(-1);
            cType.Name = "TestType";
            cType.Alias = "TestType";
            cType.Thumbnail = string.Empty;
            cTypeService.Save(cType);

            Console.WriteLine("Ctype " + cType.Id);
            Assert.Greater(cType.Id, 0);

            Content content = new Content(name, -1, cType);
            service.Save(content);

          
            var us = new UmbracoService();

            //Act
            var result = us.GetItem<Stub>(content.Id);

            //Assert
            Assert.IsNotNull(result);
          //  Assert.AreEqual(fieldValue, result.Property);
            //Assert.AreEqual(content.Id, result.Id);
            Assert.AreEqual(name, result.Name);

        }


        #region Stub

        public class Stub
        {
           // public virtual int Id { get; set; }
           // public virtual string Property { get; set; }
            public virtual string Name { get; set; }

        }

        #endregion

    }
}



