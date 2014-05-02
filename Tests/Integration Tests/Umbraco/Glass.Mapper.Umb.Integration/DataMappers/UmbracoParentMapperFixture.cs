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
using NSubstitute;
using NUnit.Framework;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.UnitOfWork;
using Umbraco.Core.Services;

namespace Glass.Mapper.Umb.Integration.DataMappers
{
    [TestFixture]
    public class UmbracoParentMapperFixture
    {
        private PetaPocoUnitOfWorkProvider _unitOfWork;
        private RepositoryFactory _repoFactory;
        
        #region Property - ReadOnly

        [Test]
        public void ReadOnly_ReturnsTrue()
        {
            //Assign
            var mapper = new UmbracoParentMapper();

            //Act
            var result = mapper.ReadOnly;

            //Assert
            Assert.IsTrue(result);
        }

        #endregion

        #region Method - MapToProperty

        [Test]
        public void MapToProperty_ConfigurationSetupCorrectly_CallsCreateClassOnService()
        {
            //Assign
            var contentService = new ContentService(_unitOfWork, _repoFactory);
            var content = contentService.GetById(new Guid("{C382AE57-D325-4357-A32A-0A959BBD4101}"));
            var service = Substitute.For<IUmbracoService>();
            service.ContentService.Returns(contentService);
            var context = new UmbracoDataMappingContext(null, content, service, false);

            var config = new UmbracoParentConfiguration();
            config.PropertyInfo = typeof(Stub).GetProperty("Property");

            var mapper = new UmbracoParentMapper();
            mapper.Setup(new DataMapperResolverArgs(null, config));

            //Act
            mapper.MapToProperty(context);

            //Assert
            //ME - I am not sure why I have to use the Arg.Is but just using item.Parent as the argument fails.
            service.Received().CreateType(config.PropertyInfo.PropertyType, Arg.Is<IContent>(x => x.Id == content.ParentId), false, false);
        }

        [Test]
        public void MapToProperty_ConfigurationIsLazy_CallsCreateClassOnServiceWithIsLazy()
        {
            //Assign
            var contentService = new ContentService(_unitOfWork, _repoFactory);
            var content = contentService.GetById(new Guid("{C382AE57-D325-4357-A32A-0A959BBD4101}"));
            var service = Substitute.For<IUmbracoService>();
            service.ContentService.Returns(contentService);
            var context = new UmbracoDataMappingContext(null, content, service, false);

            var config = new UmbracoParentConfiguration();
            config.PropertyInfo = typeof(Stub).GetProperty("Property");
            config.IsLazy = true;

            var mapper = new UmbracoParentMapper();
            mapper.Setup(new DataMapperResolverArgs(null, config));

            //Act
            mapper.MapToProperty(context);

            //Assert
            //ME - I am not sure why I have to use the Arg.Is but just using item.Parent as the argument fails.
            service.Received().CreateType(config.PropertyInfo.PropertyType, Arg.Is<IContent>(x => x.Id == content.ParentId), true, false);
        }

        [Test]
        public void MapToProperty_ConfigurationInferType_CallsCreateClassOnServiceWithInferType()
        {
            //Assign
            var contentService = new ContentService(_unitOfWork, _repoFactory);
            var content = contentService.GetById(new Guid("{C382AE57-D325-4357-A32A-0A959BBD4101}"));
            var service = Substitute.For<IUmbracoService>();
            service.ContentService.Returns(contentService);
            var context = new UmbracoDataMappingContext(null, content, service, false);

            var config = new UmbracoParentConfiguration();
            config.PropertyInfo = typeof(Stub).GetProperty("Property");
            config.InferType = true;

            var mapper = new UmbracoParentMapper();
            mapper.Setup(new DataMapperResolverArgs(null, config));

            //Act
            mapper.MapToProperty(context);

            //Assert
            //ME - I am not sure why I have to use the Arg.Is but just using item.Parent as the argument fails.
            service.Received().CreateType(config.PropertyInfo.PropertyType, Arg.Is<IContent>(x => x.Id == content.ParentId), false, true);
        }

        #endregion

        #region Method - CanHandle

        [Test]
        public void CanHandle_ConfigurationIsUmbracoParent_ReturnsTrue()
        {
            //Assign
            var config = new UmbracoParentConfiguration();
            var mapper = new UmbracoParentMapper();

            //Act
            var result = mapper.CanHandle(config, null);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanHandle_ConfigurationIsUmbracoInfo_ReturnsFalse()
        {
            //Assign
            var config = new UmbracoInfoConfiguration();
            var mapper = new UmbracoParentMapper();

            //Act
            var result = mapper.CanHandle(config, null);

            //Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region Method - MapToCms

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void MapToCms_ThrowsException()
        {
            //Assign
            var mapper = new UmbracoParentMapper();

            //Act
            mapper.MapToCms(null);
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

            var parentContent = new Content(name, -1, contentType);
            parentContent.Key = new Guid("{4172C94F-52C1-4301-9CDA-FD2142496C95}");
            contentService.Save(parentContent);

            var content = new Content(name, parentContent.Id, contentType);
            content.Key = new Guid("{C382AE57-D325-4357-A32A-0A959BBD4101}");
            contentService.Save(content);
        }

        public class Stub
        {
            public string Property { get; set; }
        }

        #endregion
    }
}




