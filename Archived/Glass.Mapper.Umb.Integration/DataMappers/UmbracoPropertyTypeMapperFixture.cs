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


//using System;
//using System.Linq;
//using Glass.Mapper.Umb.CastleWindsor;
//using Glass.Mapper.Umb.Configuration;
//using Glass.Mapper.Umb.Configuration.Attributes;
//using Glass.Mapper.Umb.DataMappers;
//using NUnit.Framework;
//using Umbraco.Core.Models;
//using Umbraco.Core.Persistence;
//using Umbraco.Core.Services;

//namespace Glass.Mapper.Umb.Integration.DataMappers
//{
//    [TestFixture]
//    public class UmbracoPropertyTypeMapperFixture
//    {
//        private ContentService _contentService;

//        #region Method - CanHandle

//        [Test]
//        public void CanHandle_TypeHasBeenLoadedByGlass_ReturnsTrue()
//        {
//            //Assign
//            var mapper = new UmbracoPropertyTypeMapper();
//            var config = new UmbracoPropertyConfiguration();
//            config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyTrue");
//            var context = Context.Create(DependencyResolver.CreateStandardResolver());
//            context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration"));

//            //Act
//            var result = mapper.CanHandle(config, context);

//            //Assert
//            Assert.IsTrue(result);
//        }

//        [Test]
//        public void CanHandle_TypeHasNotBeenLoadedByGlass_ReturnsTrueOnDemand()
//        {
//            //Assign
//            var mapper = new UmbracoPropertyTypeMapper();
//            var config = new UmbracoPropertyConfiguration();
//            config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyFalse");
//            var context = Context.Create(DependencyResolver.CreateStandardResolver());
//            context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Umb.Integration"));

//            //Act
//            var result = mapper.CanHandle(config, context);

//            //Assert
//            Assert.IsTrue(result);
//        }

//        #endregion

//        #region Method - GetProperty
        
//        [Test]
//        public void GetProperty_PropertyContainsId_ReturnsConcreteType()
//        {
//            //Assign
//            var item = Database.GetItem("/Umbraco/content/Tests/DataMappers/UmbracoPropertyTypeMapper/GetProperty");
//            var targetId = Guid.Parse("{BB01B0A5-A3F0-410E-8A6D-07FF3A1E78C3}");
//            var mapper = new UmbracoPropertyTypeMapper();
//            var Property = item.Propertys[PropertyName];
//            var config = new UmbracoPropertyConfiguration();
//            config.PropertyInfo = typeof (StubContaining).GetProperty("PropertyTrue");

//            var context = Context.Create(DependencyResolver.CreateStandardResolver());
//            context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
//            var service = new UmbracoService(Database, context);
            
//            var scContext = new UmbracoDataMappingContext(null, item, service);

//            using (new ItemEditing(item, true))
//            {
//                Property.Value = targetId.ToString();
//            }

//            //Act
//            var result = mapper.GetProperty(Property, config, scContext) as Stub;

//            //Assert
//            Assert.AreEqual(targetId, result.Id);
//        }

//        [Test]
//        public void GetProperty_PropertyEmpty_ReturnsNull()
//        {
//            //Assign
//            var item = Database.GetItem("/Umbraco/content/Tests/DataMappers/UmbracoPropertyTypeMapper/GetProperty");
//            var targetId = string.Empty;
//            var mapper = new UmbracoPropertyTypeMapper();
//            var Property = item.Propertys[PropertyName];
//            var config = new UmbracoPropertyConfiguration();
//            config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyTrue");

//            var context = Context.Create(DependencyResolver.CreateStandardResolver());
//            context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
//            var service = new UmbracoService(Database, context);
            
//            var scContext = new UmbracoDataMappingContext(null, item, service);

//            using (new ItemEditing(item, true))
//            {
//                Property.Value = targetId.ToString();
//            }

//            //Act
//            var result = mapper.GetProperty(Property, config, scContext) as Stub;

//            //Assert
//            Assert.IsNull(result);
//        }

//        [Test]
//        public void GetProperty_PropertyRandomText_ReturnsNull()
//        {
//            //Assign
//            var item = Database.GetItem("/Umbraco/content/Tests/DataMappers/UmbracoPropertyTypeMapper/GetProperty");
//            var targetId = "some random text";
//            var mapper = new UmbracoPropertyTypeMapper();
//            var Property = item.Propertys[PropertyName];
//            var config = new UmbracoPropertyConfiguration();
//            config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyTrue");

//            var context = Context.Create(DependencyResolver.CreateStandardResolver());
//            context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
//            var service = new UmbracoService(Database, context);

//            var scContext = new UmbracoDataMappingContext(null, item, service);

//            using (new ItemEditing(item, true))
//            {
//                Property.Value = targetId.ToString();
//            }

//            //Act
//            var result = mapper.GetProperty(Property, config, scContext) as Stub;

//            //Assert
//            Assert.IsNull(result);
//        }

//        #endregion
        
//        #region Method - SetProperty

//        [Test]
//        public void SetProperty_ClassContainsId_IdSetInProperty()
//        {
//            var item = Database.GetItem("/Umbraco/content/Tests/DataMappers/UmbracoPropertyTypeMapper/SetProperty");
//            var targetId = Guid.Parse("{BB01B0A5-A3F0-410E-8A6D-07FF3A1E78C3}");
//            var mapper = new UmbracoPropertyTypeMapper();
//            var Property = item.Propertys[PropertyName];

//            var config = new UmbracoPropertyConfiguration();
//            config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyTrue");

//            var context = Context.Create(DependencyResolver.CreateStandardResolver());
//            context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
//            var service = new UmbracoService(Database, context);

//            var propertyValue = new Stub();
//            propertyValue.Id = targetId;

//            var scContext = new UmbracoDataMappingContext(null, item, service);

//            using (new ItemEditing(item, true))
//            {
//                Property.Value = string.Empty;
//            }

//            //Act
//            using (new ItemEditing(item, true))
//            {
//                mapper.SetProperty(Property, propertyValue, config, scContext);
//            }
//            //Assert
//            Assert.AreEqual(targetId, Guid.Parse(item[PropertyName]));   
//        }

//        [Test]
//        [ExpectedException(typeof(NotSupportedException))]
//        public void SetProperty_ClassContainsNoIdProperty_ThrowsException()
//        {
//            //Assign
//            var item = Database.GetItem("/Umbraco/content/Tests/DataMappers/UmbracoPropertyTypeMapper/SetProperty");
//            var targetId = Guid.Parse("{BB01B0A5-A3F0-410E-8A6D-07FF3A1E78C3}");
//            var mapper = new UmbracoPropertyTypeMapper();
//            var Property = item.Propertys[PropertyName];

//            var config = new UmbracoPropertyConfiguration();
//            config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyNoId");

//            var context = Context.Create(DependencyResolver.CreateStandardResolver());
//            context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
//            var service = new UmbracoService(Database, context);

//            var propertyValue = new StubNoId();

//            var scContext = new UmbracoDataMappingContext(null, item, service);

//            using (new ItemEditing(item, true))
//            {
//                Property.Value = string.Empty;
//            }

//            //Act
//            using (new ItemEditing(item, true))
//            {
//                mapper.SetProperty(Property, propertyValue, config, scContext);
//            }

//            //Assert
//            Assert.AreEqual(string.Empty, item[PropertyName]);
//        }
        
//        [Test]
//        [ExpectedException(typeof(NullReferenceException))]
//        public void SetProperty_ClassContainsIdButItemMissing_ThrowsException()
//        {
//            //Assign
//            var item = Database.GetItem("/Umbraco/content/Tests/DataMappers/UmbracoPropertyTypeMapper/SetProperty");
//            var targetId = Guid.Parse("{11111111-A3F0-410E-8A6D-07FF3A1E78C3}");
//            var mapper = new UmbracoPropertyTypeMapper();
//            var Property = item.Propertys[PropertyName];

//            var config = new UmbracoPropertyConfiguration();
//            config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyTrue");

//            var context = Context.Create(DependencyResolver.CreateStandardResolver());
//            context.Load(new UmbracoAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
//            var service = new UmbracoService(Database, context);

//            var propertyValue = new Stub();
//            propertyValue.Id = targetId;

//            var scContext = new UmbracoDataMappingContext(null, item, service);

//            using (new ItemEditing(item, true))
//            {
//                Property.Value = string.Empty;
//            }

//            //Act
//            using (new ItemEditing(item, true))
//            {
//                mapper.SetProperty(Property, propertyValue, config, scContext);
//            }
//        }

//        #endregion

//        #region Stubs

//        [TestFixtureSetUp]
//        public void CreateStub()
//        {
//            string fieldValue = "test field value";
//            string name = "Target";
//            string contentTypeAlias = "TestType";
//            string contentTypeName = "Test Type";

//            var unitOfWork = Global.CreateUnitOfWork();
//            var repoFactory = new RepositoryFactory();
//            _contentService = new ContentService(unitOfWork, repoFactory);
//            var contentTypeService = new ContentTypeService(unitOfWork, repoFactory,
//                                                            new ContentService(unitOfWork),
//                                                            new MediaService(unitOfWork, repoFactory));
//            var dataTypeService = new DataTypeService(unitOfWork, repoFactory);

//            var contentType = new ContentType(-1);
//            contentType.Name = contentTypeName;
//            contentType.Alias = contentTypeAlias;
//            contentType.Thumbnail = string.Empty;
//            contentTypeService.Save(contentType);
//            Assert.Greater(contentType.Id, 0);

//            var definitions = dataTypeService.GetDataTypeDefinitionByControlId(new Guid("ec15c1e5-9d90-422a-aa52-4f7622c63bea"));
//            dataTypeService.Save(definitions.FirstOrDefault());
//            var propertyType = new PropertyType(definitions.FirstOrDefault());
//            propertyType.Alias = "TestProperty";
//            contentType.AddPropertyType(propertyType);
//            contentTypeService.Save(contentType);
//            Assert.Greater(contentType.Id, 0);

//            var content = new Content(name, -1, contentType);
//            content.Key = new Guid("{652E7922-ADB0-4853-BAF2-28449EB2C1A4}");
//            content.SetPropertyValue("TestProperty", fieldValue);
//            _contentService.Save(content);
//        }

//        [UmbracoType]
//        public class Stub
//        {
//            [UmbracoId]
//            public virtual Guid Id { get; set; }
//        }

//        public class StubContaining
//        {
//            public Stub PropertyTrue { get; set; }
//            public StubContaining PropertyFalse { get; set; }
//            public StubNoId PropertyNoId { get; set; }
//        }

//        [UmbracoType]
//        public class StubNoId
//        {
//        }

//        #endregion
//    }
//}




