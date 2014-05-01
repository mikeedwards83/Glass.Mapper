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
using Glass.Mapper.Umb.CastleWindsor;
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
    public class UmbracoInfoMapperFixture
    {
        private PetaPocoUnitOfWorkProvider _unitOfWork;
        private RepositoryFactory _repoFactory;

        #region Method - MapToProperty

        [Test]
        [Sequential]
        public void MapToProperty_UmbracoInfoType_GetsExpectedValueFromUmbraco(
            [Values(
                //UmbracoInfoType.Url,
                UmbracoInfoType.ContentTypeAlias,
                UmbracoInfoType.ContentTypeName,
                UmbracoInfoType.Name,
                UmbracoInfoType.Creator
                )] UmbracoInfoType type,
            [Values(
                //"target", //Url
                "TestType", //ContentTypeAlias
                "Test Type", //ContentTypeName
                "Target", //Name
                "admin" //Creator
                )] object expected
            )
        {
            //Assign
            var mapper = new UmbracoInfoMapper();
            var config = new UmbracoInfoConfiguration();
            config.Type = type;
            mapper.Setup(new DataMapperResolverArgs(null, config));

            var contentService = new ContentService(_unitOfWork, _repoFactory);
            var content = contentService.GetById(new Guid("{FB6A8073-48B4-4B85-B80C-09CBDECC27C9}"));

            Assert.IsNotNull(content, "Content is null, check in Umbraco that item exists");
            var dataContext = new UmbracoDataMappingContext(null, content, null, false);

            //Act
            var value = mapper.MapToProperty(dataContext);
            Console.WriteLine(type);

            //Assert
            Assert.AreEqual(expected, value);
        }

        [Test]
        [ExpectedException(typeof(MapperException))]
        public void MapToProperty_UmbracoInfoTypeNotSet_ThrowsException()
        {
            //Assign
            UmbracoInfoType type = UmbracoInfoType.NotSet;

            var mapper = new UmbracoInfoMapper();
            var config = new UmbracoInfoConfiguration();
            config.Type = type;
            mapper.Setup(new DataMapperResolverArgs(null, config));

            var contentService = new ContentService(_unitOfWork, _repoFactory);
            var content = contentService.GetById(new Guid("{FB6A8073-48B4-4B85-B80C-09CBDECC27C9}"));

            Assert.IsNotNull(content, "Content is null, check in Umbraco that item exists");
            var dataContext = new UmbracoDataMappingContext(null, content, null, false);

            //Act
            var value = mapper.MapToProperty(dataContext);

            //Assert
            //No asserts expect exception
        }

        [Test]
        public void MapToProperty_UmbracoInfoTypeVersion_ReturnsVersionIdAsGuid()
        {
            //Assign
            var type = UmbracoInfoType.Version;

            var mapper = new UmbracoInfoMapper();
            var config = new UmbracoInfoConfiguration();
            config.Type = type;
            mapper.Setup(new DataMapperResolverArgs(null, config));

            var contentService = new ContentService(_unitOfWork, _repoFactory);
            var content = contentService.GetById(new Guid("{FB6A8073-48B4-4B85-B80C-09CBDECC27C9}"));
            var expected = content.Version;

            Assert.IsNotNull(content, "Content is null, check in Umbraco that item exists");
            var dataContext = new UmbracoDataMappingContext(null, content, null, false);

            //Act
            var value = mapper.MapToProperty(dataContext);

            //Assert
            Assert.AreEqual(expected, value);
        }

        [Test]
        public void MapToProperty_UmbracoInfoTypePath_ReturnsPathAsString()
        {
            //Assign
            var type = UmbracoInfoType.Path;

            var mapper = new UmbracoInfoMapper();
            var config = new UmbracoInfoConfiguration();
            config.Type = type;
            mapper.Setup(new DataMapperResolverArgs(null, config));

            var contentService = new ContentService(_unitOfWork, _repoFactory);
            var content = contentService.GetById(new Guid("{FB6A8073-48B4-4B85-B80C-09CBDECC27C9}"));
            var expected = content.Path;

            Assert.IsNotNull(content, "Content is null, check in Umbraco that item exists");
            var dataContext = new UmbracoDataMappingContext(null, content, null, false);

            //Act
            var value = mapper.MapToProperty(dataContext);

            //Assert
            Assert.AreEqual(expected, value);
        }

        [Test]
        public void MapToProperty_UmbracoInfoTypeCreateDate_ReturnsCreateDateAsDateTime()
        {
            //Assign
            var type = UmbracoInfoType.CreateDate;

            var mapper = new UmbracoInfoMapper();
            var config = new UmbracoInfoConfiguration();
            config.Type = type;
            mapper.Setup(new DataMapperResolverArgs(null, config));

            var contentService = new ContentService(_unitOfWork, _repoFactory);
            var content = contentService.GetById(new Guid("{FB6A8073-48B4-4B85-B80C-09CBDECC27C9}"));
            var expected = content.CreateDate;

            Assert.IsNotNull(content, "Content is null, check in Umbraco that item exists");
            var dataContext = new UmbracoDataMappingContext(null, content, null, false);

            //Act
            var value = mapper.MapToProperty(dataContext);

            //Assert
            Assert.AreEqual(expected, value);
        }

        [Test]
        public void MapToProperty_UmbracoInfoTypeUpdateDate_ReturnsUpdateDateAsDateTime()
        {
            //Assign
            var type = UmbracoInfoType.UpdateDate;

            var mapper = new UmbracoInfoMapper();
            var config = new UmbracoInfoConfiguration();
            config.Type = type;
            mapper.Setup(new DataMapperResolverArgs(null, config));

            var contentService = new ContentService(_unitOfWork, _repoFactory);
            var content = contentService.GetById(new Guid("{FB6A8073-48B4-4B85-B80C-09CBDECC27C9}"));
            var expected = content.UpdateDate;

            Assert.IsNotNull(content, "Content is null, check in Umbraco that item exists");
            var dataContext = new UmbracoDataMappingContext(null, content, null, false);

            //Act
            var value = mapper.MapToProperty(dataContext);

            //Assert
            Assert.AreEqual(expected, value);
        }

        #endregion

        #region Method - MapToCms

        [Test]
        public void MapToCms_SavingName_UpdatesTheItemName()
        {
            //Assign
            var type = UmbracoInfoType.Name;
            var expected = "new name";

            var mapper = new UmbracoInfoMapper();
            var config = new UmbracoInfoConfiguration();
            config.Type = type;
            mapper.Setup(new DataMapperResolverArgs(null, config));

            var contentService = new ContentService(_unitOfWork, _repoFactory);
            var content = contentService.GetById(new Guid("{FB6A8073-48B4-4B85-B80C-09CBDECC27C9}"));
            var oldName = content.Name;

            Assert.IsNotNull(content, "Content is null, check in Umbraco that item exists");

            var context = Context.Create(DependencyResolver.CreateStandardResolver());
            var dataContext = new UmbracoDataMappingContext(null, content, new UmbracoService(contentService, context), false);
            dataContext.PropertyValue = expected;

            string actual = string.Empty;

            //Act
            mapper.MapToCms(dataContext);
            content = contentService.GetById(new Guid("{FB6A8073-48B4-4B85-B80C-09CBDECC27C9}"));
            actual = content.Name;

            //Assert
            Assert.AreEqual(expected, actual);
            
            content.Name = oldName;
            contentService.Save(content);
        }

        #endregion

        #region Stubs

        [TestFixtureSetUp]
        public void CreateStub()
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
            content.Key = new Guid("{FB6A8073-48B4-4B85-B80C-09CBDECC27C9}");
            contentService.Save(content);
            contentService.Publish(content);
        }
        
        public class Stub
        {
            public Guid TemplateId { get; set; }
        }

        #endregion
    }
}




