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
using System.Linq;
using Glass.Mapper.Umb.CastleWindsor;
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
        public void General_RetrieveContentAndPropertiesFromUmbraco_ReturnPopulatedClass()
        {
            //Assign
            string fieldValue = "test field value";
            string name = "Target";
            string contentTypeAlias = "TestType";
            string contentTypeName = "Test Type";
            string contentTypeProperty = "TestProperty";

            var unitOfWork = Global.CreateUnitOfWork();
            var repoFactory = new RepositoryFactory();
            var contentService = new ContentService(unitOfWork, repoFactory);
            var contentTypeService = new ContentTypeService(unitOfWork, repoFactory,
                                                                     new ContentService(unitOfWork),
                                                                     new MediaService(unitOfWork, repoFactory));
            var dataTypeService = new DataTypeService(unitOfWork, repoFactory);
            var context = WindsorContainer.GetContext();

            var loader = new UmbracoFluentConfigurationLoader();
            var stubConfig = loader.Add<Stub>();
            stubConfig.Configure(x =>
            {
                x.Id(y => y.Id);
                x.Id(y => y.Key);
                x.Property(y => y.TestProperty);
                x.Info(y => y.Name).InfoType(UmbracoInfoType.Name);
                x.Info(y => y.ContentTypeName).InfoType(UmbracoInfoType.ContentTypeName);
                x.Info(y => y.ContentTypeAlias).InfoType(UmbracoInfoType.ContentTypeAlias);
                x.Info(y => y.Path).InfoType(UmbracoInfoType.Path);
                x.Info(y => y.Version).InfoType(UmbracoInfoType.Version);
                x.Info(y => y.CreateDate).InfoType(UmbracoInfoType.CreateDate);
                x.Info(y => y.UpdateDate).InfoType(UmbracoInfoType.UpdateDate);
                x.Info(y => y.Creator).InfoType(UmbracoInfoType.Creator);
            });

            context.Load(loader);

            IContentType contentType = new ContentType(-1);
            contentType.Name = contentTypeName;
            contentType.Alias = contentTypeAlias;
            contentType.Thumbnail = string.Empty;
            contentTypeService.Save(contentType);
            Assert.Greater(contentType.Id, 0);

			var definitions = dataTypeService.GetDataTypeDefinitionByControlId(new Guid("ec15c1e5-9d90-422a-aa52-4f7622c63bea"));
            dataTypeService.Save(definitions.FirstOrDefault());
            var propertyType = new PropertyType(definitions.FirstOrDefault());
            propertyType.Alias = contentTypeProperty;
            contentType.AddPropertyType(propertyType);
            contentTypeService.Save(contentType);
            Assert.Greater(contentType.Id, 0);

            var content = new Content(name, -1, contentType);
            content.SetPropertyValue(contentTypeProperty, fieldValue);
            contentService.Save(content);

            var umbracoService = new UmbracoService(contentService, context);

            //Act
            var result = umbracoService.GetItem<Stub>(content.Id);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(fieldValue, result.TestProperty);
            Assert.AreEqual(content.Id, result.Id);
            Assert.AreEqual(content.Key, result.Key);
            Assert.AreEqual(name, result.Name);
            Assert.AreEqual(contentTypeName, result.ContentTypeName);
            Assert.AreEqual(content.ParentId + "," + content.Id, result.Path);
            Assert.AreEqual(contentTypeAlias, result.ContentTypeAlias);
            Assert.AreEqual(content.Version, result.Version);
            Assert.AreEqual(content.CreateDate, result.CreateDate);
            Assert.AreEqual(content.UpdateDate, result.UpdateDate);
            Assert.AreEqual("admin", result.Creator);
        }

        #region Stub

        public class Stub
        {
            public virtual int Id { get; set; }
            public virtual Guid Key { get; set; }
            public virtual string TestProperty { get; set; }
            public virtual string Name { get; set; }
            public virtual string ContentTypeName { get; set; }
            public virtual string ContentTypeAlias { get; set; }
            public virtual string Path { get; set; }
            public virtual Guid Version { get; set; }
            public virtual DateTime CreateDate { get; set; }
            public virtual DateTime UpdateDate { get; set; }
            public virtual string Creator { get; set; }
        }

        #endregion

    }
}




