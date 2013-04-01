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
using System.Text;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    [TestFixture]
    public class AbstractSitecoreFieldMapperFixture : AbstractMapperFixture
    {
        #region Constructors

        [Test]
        public void Constructor_TypesPassed_TypesHandledSet()
        {
            //Assign
            var type1 = typeof (int);
            var type2 = typeof (string);

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
            var config = new SitecoreFieldConfiguration();
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
            var config = new SitecoreFieldConfiguration();
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
            var config = new SitecoreIdConfiguration();
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
            var config = new SitecoreIdConfiguration();
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
        public void MapToProperty_GetsValueByFieldName_ReturnsFieldValue()
        {
            //Assign
            var fieldValue = "test value";
            var fieldName = "Field";
            var database = Sitecore.Configuration.Factory.GetDatabase("master");
            var item = database.GetItem("/sitecore/content/Tests/DataMappers/AbstractSitecoreFieldMapper/MapToProperty");
           
            var config = new SitecoreFieldConfiguration();
            config.FieldName = fieldName;

            var mapper = new StubMapper(null);
            mapper.Setup(new DataMapperResolverArgs(null,config));
            mapper.Value = fieldValue;

            var context = new SitecoreDataMappingContext(null, item, null);

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                item[fieldName] = fieldValue;
                item.Editing.EndEdit();
            }

            //Act
            var result = mapper.MapToProperty(context);
            
            //Assert
            Assert.AreEqual(fieldValue, result);

        }

        [Test]
        public void MapToProperty_GetsValueByFieldId_ReturnsFieldValue()
        {
            //Assign
            var fieldValue = "test value";
            var fieldId = new ID("{6B43481F-F129-4F53-BEEE-EA84F9B1A6D4}");
            var database = Sitecore.Configuration.Factory.GetDatabase("master");
            var item = database.GetItem("/sitecore/content/Tests/DataMappers/AbstractSitecoreFieldMapper/MapToProperty");

            var config = new SitecoreFieldConfiguration();
            config.FieldId = fieldId;

            var mapper = new StubMapper(null);
            mapper.Setup(new DataMapperResolverArgs(null,config));
            mapper.Value = fieldValue;

            var context = new SitecoreDataMappingContext(null, item, null);

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                item[fieldId] = fieldValue;
                item.Editing.EndEdit();
            }

            //Act
            var result = mapper.MapToProperty(context);

            //Assert
            Assert.AreEqual(fieldValue, result);

        }

        #endregion

        #region Method - MapToCms

        [Test]
        public void MapToCms_SetsValueByFieldId_FieldValueUpdated()
        {
            //Assign
            var fieldValue = "test value set";
            var fieldId = new ID("{6B43481F-F129-4F53-BEEE-EA84F9B1A6D4}");
            var database = Sitecore.Configuration.Factory.GetDatabase("master");
            var item = database.GetItem("/sitecore/content/Tests/DataMappers/AbstractSitecoreFieldMapper/MapToCms");

            var config = new SitecoreFieldConfiguration();
            config.FieldId = fieldId;
            config.PropertyInfo = typeof(Stub).GetProperty("Property");

            var mapper = new StubMapper(null);
            mapper.Setup(new DataMapperResolverArgs(null,config));
            mapper.Value = fieldValue;

            var context = new SitecoreDataMappingContext(new Stub(), item, null);

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                item[fieldId] = string.Empty;
                item.Editing.EndEdit();
            }

            //Act
            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                mapper.MapToCms(context);
                item.Editing.EndEdit();

            }
            //Assert
            Assert.AreEqual(fieldValue, mapper.Value);

        }

        [Test]
        public void MapToCms_SetsValueByFieldName_FieldValueUpdated()
        {
            //Assign
            var fieldValue = "test value set";
            var fieldName = "Field";
            var database = Sitecore.Configuration.Factory.GetDatabase("master");
            var item = database.GetItem("/sitecore/content/Tests/DataMappers/AbstractSitecoreFieldMapper/MapToCms");

            var config = new SitecoreFieldConfiguration();
            config.FieldName = fieldName;
            config.PropertyInfo = typeof(Stub).GetProperty("Property");

            var mapper = new StubMapper(null);
            mapper.Setup(new DataMapperResolverArgs(null,config));
            mapper.Value = fieldValue;

            var context = new SitecoreDataMappingContext(new Stub(), item, null);

            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                item[fieldName] = string.Empty;
                item.Editing.EndEdit();
            }

            //Act
            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                mapper.MapToCms(context);
                item.Editing.EndEdit();

            }
            //Assert
            Assert.AreEqual(fieldValue, mapper.Value);

        }

        #endregion

        #region Stubs

        public class StubMapper : AbstractSitecoreFieldMapper
        {
            public string Value { get; set; }
            public StubMapper(params Type[] typeHandlers)
                : base(typeHandlers)
            {
            }

            public override object GetFieldValue(string fieldValue, Configuration.SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
            {
                return Value;
            }

            public override void SetField(Sitecore.Data.Fields.Field field, object value, Configuration.SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
            {
                field.Value = Value;
            }

            public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
            {
                throw new NotImplementedException();
            }
        }

        public class Stub
        {
            public string Property { get; set; }
        }

        #endregion

    }
}



