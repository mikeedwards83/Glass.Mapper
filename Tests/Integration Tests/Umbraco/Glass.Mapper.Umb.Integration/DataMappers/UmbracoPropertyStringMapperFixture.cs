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
using Umbraco.Core.Persistence.UnitOfWork;
using Umbraco.Core.Services;

namespace Glass.Mapper.Umb.Integration.DataMappers
{
    [TestFixture]
    public class UmbracoPropertyStringMapperFixture
    {
        private static bool _isSetup;
        private PetaPocoUnitOfWorkProvider _unitOfWork;
        private RepositoryFactory _repoFactory;
        private const string ContentTypeProperty = "TestProperty";

        public UmbracoPropertyStringMapperFixture()
        {
            CreateStub();
        }

        #region Method - GetProperty

        [Test]
        public void GetProperty_PropertyContainsData_StringIsReturned()
        {
            //Assign
            var fieldValue = "hello world";
            
            var contentService = new ContentService(_unitOfWork, _repoFactory);
            var content = contentService.GetById(new Guid("{5F6D851E-46C0-40C7-A93A-EC3F6D7EBA3E}"));
            var property = content.Properties[ContentTypeProperty];
            property.Value = fieldValue;

            var mapper = new UmbracoPropertyStringMapper();
            var config = new UmbracoPropertyConfiguration();
            
            //Act
            var result = mapper.GetPropertyValue(property, config, null) as string;

            //Assert
            Assert.AreEqual(fieldValue, result);
        }


        [Test]
        public void GetField_RichText_StringIsReturnedWithScapedUrl()
        {
            //Assign
            var fieldValue = "<p>Test with <a href=\"~/link.aspx?_id=BFD7975DF42F41E19DDA9A38E971555F&amp;_z=z\">link</a></p>";
            var expected = "<p>Test with <a href=\"/en/Tests/DataMappers/SitecoreFieldStringMapper/GetField.aspx\">link</a></p>";
            var contentService = new ContentService(_unitOfWork, _repoFactory);
            var content = contentService.GetById(new Guid("{5F6D851E-46C0-40C7-A93A-EC3F6D7EBA3E}"));
            var property = content.Properties[ContentTypeProperty];

            property.Value = fieldValue;

            var mapper = new UmbracoPropertyStringMapper();
            var config = new UmbracoPropertyConfiguration();
            
            //Act
            var result = mapper.GetPropertyValue(property, config, null) as string;

            //Assert
            Assert.AreEqual(expected, result);
        }
        
        #endregion

        #region Method - SetField

        //[Test]
        //[ExpectedException(typeof(NotSupportedException))]
        //public void SetField_RichText_ThrowsException()
        //{
        //    //Assign
        //    var expected = "<p>Test with <a href=\"~/link.aspx?_id=BFD7975DF42F41E19DDA9A38E971555F&amp;_z=z\">link</a></p>";
        //    var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldStringMapper/SetFieldRichText");
        //    var field = item.Fields[FieldName];

        //    var mapper = new UmbracoPropertyStringMapper();
        //    var config = new UmbracoPropertyConfiguration();
        //    config.PropertyInfo = new FakePropertyInfo(typeof (string));

        //    Sitecore.Context.Site = Sitecore.Configuration.Factory.GetSite("website");

        //    using (new ItemEditing(item, true))
        //    {
        //        field.Value = string.Empty;
        //    }

        //    //Act
        //    using (new ItemEditing(item, true))
        //    {
        //        mapper.SetField(field, expected, config, null);
        //    }

        //    Sitecore.Context.Site = null;

        //    //Assert
        //    Assert.AreEqual(expected, field.Value);
        //}

        //[Test]
        //public void SetField_FielNonRichText_ValueWrittenToField()
        //{
        //    //Assign
        //    var expected = "<p>Test with <a href=\"~/link.aspx?_id=BFD7975DF42F41E19DDA9A38E971555F&amp;_z=z\">link</a></p>";
        //    var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldStringMapper/SetField");
        //    var field = item.Fields[FieldName];

        //    var mapper = new UmbracoPropertyStringMapper();
        //    var config = new UmbracoPropertyConfiguration();
        //    config.Setting = SitecoreFieldSettings.RichTextRaw;
            
        //    Sitecore.Context.Site = Sitecore.Configuration.Factory.GetSite("website");

        //    using (new ItemEditing(item, true))
        //    {
        //        field.Value = string.Empty;
        //    }

        //    //Act
        //    using (new ItemEditing(item, true))
        //    {
        //        mapper.SetField(field, expected, config, null);
        //    }

        //    Sitecore.Context.Site = null;

        //    //Assert
        //    Assert.AreEqual(expected, field.Value);
        //}

        #endregion

        #region Method - CanHandle

        [Test]
        public void CanHandle_StreamType_ReturnsTrue()
        {
            //Assign
            var mapper = new UmbracoPropertyStringMapper();
            var config = new UmbracoPropertyConfiguration();
            config.PropertyInfo = new FakePropertyInfo(typeof(String));

            //Act
            var result = mapper.CanHandle(config, null);

            //Assert
            Assert.IsTrue(result);
        }

        #endregion

        #region Stub

        private void CreateStub()
        {
            if (_isSetup)
                return;

            string fieldValue = "test field value";
            string name = "Target";
            string contentTypeAlias = "TestType";
            string contentTypeName = "Test Type";

            _unitOfWork = Global.CreateUnitOfWork();
            _repoFactory = new RepositoryFactory();
            var contentService = new ContentService(_unitOfWork, _repoFactory);
            var contentTypeService = new ContentTypeService(_unitOfWork, _repoFactory,
                                                            new ContentService(_unitOfWork),
                                                            new MediaService(_unitOfWork, _repoFactory));
            var dataTypeService = new DataTypeService(_unitOfWork, _repoFactory);

            var contentType = new ContentType(-1);
            contentType.Name = contentTypeName;
            contentType.Alias = contentTypeAlias;
            contentType.Thumbnail = string.Empty;
            contentTypeService.Save(contentType);
            Assert.Greater(contentType.Id, 0);

            var definitions = dataTypeService.GetDataTypeDefinitionByControlId(new Guid("ec15c1e5-9d90-422a-aa52-4f7622c63bea"));
            dataTypeService.Save(definitions.FirstOrDefault());
            var propertyType = new PropertyType(definitions.FirstOrDefault());
            propertyType.Alias = ContentTypeProperty;
            contentType.AddPropertyType(propertyType);
            contentTypeService.Save(contentType);
            Assert.Greater(contentType.Id, 0);

            var content = new Content(name, -1, contentType);
            content.Key = new Guid("{5F6D851E-46C0-40C7-A93A-EC3F6D7EBA3E}");
            content.SetPropertyValue(ContentTypeProperty, fieldValue);
            contentService.Save(content);

            _isSetup = true;
        }

        public class Stub
        {
            public virtual string TestProperty { get; set; }
        }

        #endregion
    }
}



