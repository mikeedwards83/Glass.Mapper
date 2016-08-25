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
    public class AbstractUmbracoPropertyMapperFixture
    {
        private ContentService _contentService;

        #region Constructors

        [Test]
        public void Constructor_TypesPassed_TypesHandledSet()
        {
            //Assign
            var type1 = typeof(int);
            var type2 = typeof(string);

            //Act
            var mapper = new StubMapper(type1, type2);

            //Assert
            Assert.IsTrue(mapper.TypesHandled.Any(x => x == type1));
            Assert.IsTrue(mapper.TypesHandled.Any(x => x == type2));
        }

        #endregion

        #region Method - CanHandle

        [Test]
        public void CanHandle_TypeIsHandledWithConfig_ReturnsTrue()
        {
            //Assign
            var config = new UmbracoPropertyConfiguration();
            var type1 = typeof (string);
            var mapper = new StubMapper(type1);

            config.PropertyInfo = typeof (Stub).GetProperty("Property");

            //Act
            var result = mapper.CanHandle(config, null);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanHandle_TwoTypesAreHandledWithConfig_ReturnsTrue()
        {
            //Assign
            var config = new UmbracoPropertyConfiguration();
            var type2 = typeof(string);
            var type1 = typeof(int);
            var mapper = new StubMapper(type1, type2);

            config.PropertyInfo = typeof(Stub).GetProperty("Property");

            //Act
            var result = mapper.CanHandle(config, null);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanHandle_IncorrectConfigType_ReturnsFalse()
        {
            //Assign
            var config = new UmbracoIdConfiguration();
            var type2 = typeof(string);
            var type1 = typeof(int);
            var mapper = new StubMapper(type1, type2);

            config.PropertyInfo = typeof(Stub).GetProperty("Property");

            //Act
            var result = mapper.CanHandle(config, null);

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void CanHandle_IncorrectPropertType_ReturnsFalse()
        {
            //Assign
            var config = new UmbracoIdConfiguration();
            var type1 = typeof(int);
            var mapper = new StubMapper(type1);

            config.PropertyInfo = typeof(Stub).GetProperty("Property");

            //Act
            var result = mapper.CanHandle(config, null);

            //Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region Method - MapToProperty

        [Test]
        public void MapToProperty_GetsValueByPropertyAlias_ReturnsPropertyValue()
        {
            //Assign
            var propertyValue = "test value set";
            var propertyAlias = "Property";
            var content = _contentService.GetById(new Guid("{D2517065-2818-4AF7-B851-493E46EA79D5}"));
           
            var config = new UmbracoPropertyConfiguration();
            config.PropertyAlias = propertyAlias;

            var mapper = new StubMapper(null);
            mapper.Setup(new DataMapperResolverArgs(null,config));
            mapper.Value = propertyValue;

            var context = new UmbracoDataMappingContext(null, content, null, false);

            content.Properties[propertyAlias].Value = propertyValue;

            //Act
            var result = mapper.MapToProperty(context);
            
            //Assert
            Assert.AreEqual(propertyValue, result);
        }

        #endregion

        #region Method - MapToCms

        [Test]
        public void MapToCms_SetsValueByPropertyAlias_PropertyValueUpdated()
        {
            //Assign
            var propertyValue = "test value set";
            var propertyAlias = "Property";
            var content = _contentService.GetById(new Guid("{D2517065-2818-4AF7-B851-493E46EA79D5}"));

            var config = new UmbracoPropertyConfiguration();
            config.PropertyAlias = propertyAlias;
            config.PropertyInfo = typeof(Stub).GetProperty("Property");

            var mapper = new StubMapper(null);
            mapper.Setup(new DataMapperResolverArgs(null, config));
            mapper.Value = propertyValue;

            var context = new UmbracoDataMappingContext(new Stub(), content, null, false);

            content.Properties[propertyAlias].Value = string.Empty;

            //Act
            mapper.MapToCms(context);

            //Assert
            Assert.AreEqual(propertyValue, mapper.Value);
        }

        #endregion

        #region Stubs

        [TestFixtureSetUp]
        public void CreateStub()
        {
            string fieldValue = "test field value";
            string name = "Target";
            string contentTypeAlias = "TestType";
            string contentTypeName = "Test Type";

            var unitOfWork = Global.CreateUnitOfWork();
            var repoFactory = new RepositoryFactory();
            _contentService = new ContentService(unitOfWork, repoFactory);
            var contentTypeService = new ContentTypeService(unitOfWork, repoFactory,
                                                            new ContentService(unitOfWork),
                                                            new MediaService(unitOfWork, repoFactory));
            var dataTypeService = new DataTypeService(unitOfWork, repoFactory);

            var contentType = new ContentType(-1);
            contentType.Name = contentTypeName;
            contentType.Alias = contentTypeAlias;
            contentType.Thumbnail = string.Empty;
            contentTypeService.Save(contentType);
            Assert.Greater(contentType.Id, 0);

            var definitions = dataTypeService.GetDataTypeDefinitionByControlId(new Guid("ec15c1e5-9d90-422a-aa52-4f7622c63bea"));
            dataTypeService.Save(definitions.FirstOrDefault());
            var propertyType = new PropertyType(definitions.FirstOrDefault());
            propertyType.Alias = "Property";
            contentType.AddPropertyType(propertyType);
            contentTypeService.Save(contentType);
            Assert.Greater(contentType.Id, 0);

            var content = new Content(name, -1, contentType);
            content.Key = new Guid("{D2517065-2818-4AF7-B851-493E46EA79D5}");
            content.SetPropertyValue("Property", fieldValue);
            _contentService.Save(content);
        }

        public class StubMapper : AbstractUmbracoPropertyMapper
        {
            public string Value { get; set; }

            public StubMapper(params Type[] typeHandlers)
                : base(typeHandlers)
            {
            }
            
            public override void SetProperty(Property property, object value, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context)
            {
                property.Value = Value;
            }

            public override object SetPropertyValue(object value, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context)
            {
                throw new NotImplementedException();
            }

            public override object GetPropertyValue(object propertyValue, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context)
            {
                return Value;
            }
        }

        public class Stub
        {
            public string Property { get; set; }
        }

        #endregion
    }
}

