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
using Umbraco.Core.Services;

namespace Glass.Mapper.Umb.Integration.DataMappers
{
    [TestFixture]
    public class UmbracoIdMapperFixture
    {
        private ContentService _contentService;
        
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
        public void MapToProperty_ContentIdAsInt_ReturnsIdAsInt()
        {
            var content = _contentService.GetById(new Guid("{263768E1-E958-4B00-BB00-191CC33A3F48}"));

            //Assign
            var mapper = new UmbracoIdMapper();
            var config = new UmbracoIdConfiguration();
            var property = typeof(Stub).GetProperty("Id");

            Assert.IsNotNull(content, "Content is null, check in Umbraco that item exists");

            config.PropertyInfo = property;
            
            mapper.Setup(new DataMapperResolverArgs(null, config));

            var dataContext = new UmbracoDataMappingContext(null, content, null, false);
            var expected = content.Id;

            //Act
            var value = mapper.MapToProperty(dataContext);

            //Assert
            Assert.AreEqual(expected, value);
        }

        [Test]
        public void MapToProperty_ContentIdAsGuid_ReturnsIdAsGuid()
        {
            var content = _contentService.GetById(new Guid("{263768E1-E958-4B00-BB00-191CC33A3F48}"));

            //Assign
            var mapper = new UmbracoIdMapper();
            var config = new UmbracoIdConfiguration();
            var property = typeof(Stub).GetProperty("Key");

            Assert.IsNotNull(content, "Content is null, check in Umbraco that item exists");

            config.PropertyInfo = property;

            mapper.Setup(new DataMapperResolverArgs(null, config));

            var dataContext = new UmbracoDataMappingContext(null, content, null, false);
            var expected = content.Key;

            //Act
            var value = mapper.MapToProperty(dataContext);

            //Assert
            Assert.AreEqual(expected, value);
        }

        #endregion

        #region Stub

        [TestFixtureSetUp]
        public void CreateStub()
        {
            string name = "Target";
            string contentTypeAlias = "TestType";
            string contentTypeName = "Test Type";

            var unitOfWork = Global.CreateUnitOfWork();
            var repoFactory = new RepositoryFactory();
            _contentService = new ContentService(unitOfWork, repoFactory);
            var contentTypeService = new ContentTypeService(unitOfWork, repoFactory,
                                                            new ContentService(unitOfWork),
                                                            new MediaService(unitOfWork, repoFactory));
            
            var contentType = new ContentType(-1);
            contentType.Name = contentTypeName;
            contentType.Alias = contentTypeAlias;
            contentType.Thumbnail = string.Empty;
            contentTypeService.Save(contentType);
            Assert.Greater(contentType.Id, 0);

            var content = new Content(name, -1, contentType);
            content.Key = new Guid("{263768E1-E958-4B00-BB00-191CC33A3F48}");
            _contentService.Save(content);
        }

        public class Stub
        {
            public virtual int Id { get; set; }
            public virtual Guid Key { get; set; }
        }

        #endregion
    }
}

