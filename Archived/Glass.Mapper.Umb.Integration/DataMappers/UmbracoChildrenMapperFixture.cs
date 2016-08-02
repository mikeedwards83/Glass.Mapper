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
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Umb.Configuration;
using Glass.Mapper.Umb.Configuration.Attributes;
using Glass.Mapper.Umb.DataMappers;
using NSubstitute;
using NUnit.Framework;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;

namespace Glass.Mapper.Umb.Integration.DataMappers
{
    [TestFixture]
    public class UmbracoChildrenMapperFixture
    {
        private ContentService _contentService;

        #region Method - MapToProperty

        [Test]
        public void MapToProperty_ContentHasChildren_ChildrenObjectsAreCreated()
        {
            //Assign
            var content = _contentService.GetById(new Guid("{373D1D1C-BD0D-4A8C-9A67-1D513815FE10}"));
            var children = _contentService.GetChildren(content.Id);
            var service = Substitute.For<IUmbracoService>();
            var predicate = Arg.Is<IContent>(x => children.Any(y => x.Id == y.Id));

            var config = new UmbracoChildrenConfiguration();
            config.InferType = false;
            config.IsLazy = false;
            config.PropertyInfo = typeof(Stub).GetProperty("Children");

            var mapper = new UmbracoChildrenMapper();

            //ME - Although this looks correct I am not sure it is
            service.ContentService.Returns(_contentService);
            service.CreateType(typeof(StubChild), predicate, false, false).ReturnsForAnyArgs(info => new StubChild
                                                                                  {
                                                                                      Id =  info.Arg<IContent>().Id
                                                                                  });

            var context = new UmbracoDataMappingContext(null, content, service, false);
            mapper.Setup(new DataMapperResolverArgs(null, config));

            //Act
            var result = mapper.MapToProperty(context) as IEnumerable<StubChild>;

            //Assert
            Assert.AreEqual(1, children.Count());
            Assert.AreEqual(children.Count(), result.Count());

            foreach (IContent child in children)
            {
                Assert.IsTrue(result.Any(x => x.Id == child.Id));
            }
        }

        [Test]
        public void MapToProperty_ContentHasNoChildren_NoObjectsCreated()
        {
            //Assign
            var content = _contentService.GetById(new Guid("{3F34475B-D744-40E9-BC30-5D33249FA9FE}"));
            var children = _contentService.GetChildren(content.Id);
            var service = Substitute.For<IUmbracoService>();
            var predicate = Arg.Is<IContent>(x => children.Any(y => x.Id == y.Id));

            var config = new UmbracoChildrenConfiguration();
            config.InferType = false;
            config.IsLazy = false;
            config.PropertyInfo = typeof(Stub).GetProperty("Children");

            var mapper = new UmbracoChildrenMapper();

            //ME - Although this looks correct I am not sure it is
            service.CreateType(typeof(StubChild), predicate, false, false).ReturnsForAnyArgs(info => new StubChild
            {
                Id = info.Arg<IContent>().Id
            });

            var context = new UmbracoDataMappingContext(null, content, service, false);
            mapper.Setup(new DataMapperResolverArgs(null, config));

            //Act
            var result = mapper.MapToProperty(context) as IEnumerable<StubChild>;

            //Assert
            Assert.AreEqual(0, result.Count());
        }

        #endregion

        #region Property - ReadOnly

        [Test]
        public void ReadOnly_ReturnsTrue()
        {
            //Assign
            var mapper = new UmbracoChildrenMapper();

            //Act
            var result = mapper.ReadOnly;

            //Assert
            Assert.IsTrue(result);
        }

        #endregion
  
        #region CanHandle

        [Test]
        public void CanHandle_ConfigIsChildren_ReturnsTrue()
        {
            //Assign
            var mapper = new UmbracoChildrenMapper();
            var config = new UmbracoChildrenConfiguration();

            //Act
            var result = mapper.CanHandle(config, null);

            //Assert
            Assert.IsTrue(result);
        }
        
        #endregion

        #region Stubs

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

            var parentContent = new Content(name, -1, contentType);
            parentContent.Key = new Guid("{373D1D1C-BD0D-4A8C-9A67-1D513815FE10}");
            _contentService.Save(parentContent);

            var content = new Content(name, parentContent.Id, contentType);
            content.Key = new Guid("{3F34475B-D744-40E9-BC30-5D33249FA9FE}");
            _contentService.Save(content);
        }

        [UmbracoType]
        public class StubChild
        {
            [UmbracoId]
            public virtual int Id { get; set; }
        }

        public class Stub
        {
            public IEnumerable<StubChild> Children { get; set; } 
        }
        
        #endregion
    }
}




