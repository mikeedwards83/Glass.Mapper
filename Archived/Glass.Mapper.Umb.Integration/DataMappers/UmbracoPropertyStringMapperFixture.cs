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
using Glass.Mapper.Umb.Configuration;
using Glass.Mapper.Umb.DataMappers;
using NUnit.Framework;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;

namespace Glass.Mapper.Umb.Integration.DataMappers
{
    [TestFixture]
    public class UmbracoPropertyStringMapperFixture
    {
        private ContentService _contentService;
        
        #region Method - GetProperty

        [Test]
        public void GetProperty_PropertyContainsData_StringIsReturned()
        {
            //Assign
            var fieldValue = "hello world";
            var contentTypeProperty = "TestProperty";
            
            var content = _contentService.GetById(new Guid("{5F6D851E-46C0-40C7-A93A-EC3F6D7EBA3E}"));
            var property = content.Properties[contentTypeProperty];
            property.Value = fieldValue;

            var mapper = new UmbracoPropertyStringMapper();
            var config = new UmbracoPropertyConfiguration();
            
            //Act
            var result = mapper.GetProperty(property, config, null) as string;

            //Assert
            Assert.AreEqual(fieldValue, result);
        }
        
        #endregion

        #region Method - SetField

        [Test]
        public void SetProperty_PropertyString_ValueWrittenToProperty()
        {
            //Assign
            var contentTypeProperty = "TestProperty";
            var expected = "Test data";
            var content = _contentService.GetById(new Guid("{5F6D851E-46C0-40C7-A93A-EC3F6D7EBA3E}"));
            var property = content.Properties[contentTypeProperty];

            var mapper = new UmbracoPropertyStringMapper();
            var config = new UmbracoPropertyConfiguration();

            //Act
            mapper.SetProperty(property, expected, config, null);

            //Assert
            Assert.AreEqual(expected, property.Value);
        }

        #endregion

        #region Method - CanHandle

        [Test]
        public void CanHandle_StringType_ReturnsTrue()
        {
            //Assign
            var mapper = new UmbracoPropertyStringMapper();
            var config = new UmbracoPropertyConfiguration();
	        config.PropertyInfo = typeof (Stub).GetProperty("TestProperty");

            //Act
            var result = mapper.CanHandle(config, null);

            //Assert
            Assert.IsTrue(result);
        }

        #endregion

        #region Stub

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
            propertyType.Alias = "TestProperty";
            contentType.AddPropertyType(propertyType);
            contentTypeService.Save(contentType);
            Assert.Greater(contentType.Id, 0);

            var content = new Content(name, -1, contentType);
            content.Key = new Guid("{5F6D851E-46C0-40C7-A93A-EC3F6D7EBA3E}");
            content.SetPropertyValue("TestProperty", fieldValue);
            _contentService.Save(content);
        }

        public class Stub
        {
            public virtual string TestProperty { get; set; }
        }

        #endregion
    }
}
