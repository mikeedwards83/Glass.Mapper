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
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Umb.Configuration;
using Glass.Mapper.Umb.DataMappers;
using NUnit.Framework;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.UnitOfWork;
using Umbraco.Core.Services;

namespace Glass.Mapper.Umb.Integration.DataMappers
{
    [TestFixture]
    public class UmbracoIdMapperFixture
    {
        private PetaPocoUnitOfWorkProvider _unitOfWork;
        private RepositoryFactory _repoFactory;

        public UmbracoIdMapperFixture()
        {
            CreateStub();
        }

        #region Property - ReadOnly

        [Test]
        public void ReadOnly_ReturnsTrue()
        {
            //Assign
            var mapper = new UmbracoIdMapper();
            bool expected = true;

            //Act
            bool value = mapper.ReadOnly;

            //Assert
            Assert.AreEqual(expected, value);
        }

        #endregion

        #region Method - MapToProperty

        [Test]
        public void MapToProperty_ItemIdAsInt_ReturnsIdAsInt()
        {
            var contentService = new ContentService(_unitOfWork, _repoFactory);
            var content = contentService.GetById(new Guid("{C382AE57-D325-4357-A32A-0A959BBD4101}"));

            //Assign
            var mapper = new UmbracoIdMapper();
            var config = new UmbracoIdConfiguration();
            var property = typeof (Stub).GetProperty("Id");

            Assert.IsNotNull(content, "Item is null, check in Umbraco that item exists");

            config.PropertyInfo = property;
            
            mapper.Setup(new DataMapperResolverArgs(null, config));

            var dataContext = new UmbracoDataMappingContext(null, content, null, contentService);
            var expected = content.Id;

            //Act
            var value = mapper.MapToProperty(dataContext);

            //Assert
            Assert.AreEqual(expected, value);
        }

        [Test]
        public void MapToProperty_ItemIdAsGuid_ReturnsIdAsGuid()
        {
            var contentService = new ContentService(_unitOfWork, _repoFactory);
            var content = contentService.GetById(new Guid("{C382AE57-D325-4357-A32A-0A959BBD4101}"));

            //Assign
            var mapper = new UmbracoIdMapper();
            var config = new UmbracoIdConfiguration();
            var property = typeof(Stub).GetProperty("Key");

            Assert.IsNotNull(content, "Item is null, check in Umbraco that item exists");

            config.PropertyInfo = property;

            mapper.Setup(new DataMapperResolverArgs(null, config));

            var dataContext = new UmbracoDataMappingContext(null, content, null, contentService);
            var expected = content.Key;

            //Act
            var value = mapper.MapToProperty(dataContext);

            //Assert
            Assert.AreEqual(expected, value);
        }

        #endregion

        #region Stub

        private void CreateStub()
        {
            string name = "Target";
            string contentTypeAlias = "TestType";
            string contentTypeName = "Test Type";

            _unitOfWork = Global.CreateUnitOfWork();
            _repoFactory = new RepositoryFactory();
            var contentService = new ContentService(_unitOfWork, _repoFactory);
            var contentTypeService = new ContentTypeService(_unitOfWork, _repoFactory,
                                                            new ContentService(_unitOfWork),
                                                            new MediaService(_unitOfWork, _repoFactory));
            
            var contentType = new ContentType(-1);
            contentType.Name = contentTypeName;
            contentType.Alias = contentTypeAlias;
            contentType.Thumbnail = string.Empty;
            contentTypeService.Save(contentType);
            Assert.Greater(contentType.Id, 0);

            var content = new Content(name, -1, contentType);
            content.Key = new Guid("{C382AE57-D325-4357-A32A-0A959BBD4101}");
            contentService.Save(content);
        }

        public class Stub
        {
            public virtual int Id { get; set; }
            public virtual Guid Key { get; set; }
        }

        #endregion
    }
}
