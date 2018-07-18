using System;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;
using Sitecore.Data;

namespace Glass.Mapper.Sc.FakeDb.DataMappers
{
    [TestFixture]
    public class SitecoreFieldEnumMapperFixture : AbstractMapperFixture
    {
        #region Method - GetField

        [Test]
        public void GetField_FieldContainsValidEnum_ReturnsEnum()
        {
            //Assign
            string fieldValue = "Value1";
            StubEnum expected = StubEnum.Value1;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof (Stub).GetProperty("Property");

            var mapper = new SitecoreFieldEnumMapper();

      //Act
            var result = (StubEnum)mapper.GetField(field, config, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetField_FieldContainsValidEnumInLowercase_ReturnsEnum()
        {
            //Assign
            string fieldValue = "value1";
            StubEnum expected = StubEnum.Value1;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof(Stub).GetProperty("Property");

            var mapper = new SitecoreFieldEnumMapper();

            //Act
            var result = (StubEnum)mapper.GetField(field, config, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetField_FieldContainsValidEnumInteger_ReturnsEnum()
        {
            //Assign
            string fieldValue = "2";
            StubEnum expected = StubEnum.Value2;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof(Stub).GetProperty("Property");

            var mapper = new SitecoreFieldEnumMapper();
            
            //Act
            var result = (StubEnum)mapper.GetField(field, config, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetField_FieldContainsEmptyString_ReturnsDefaultEnum()
        {
            //Assign
            string fieldValue = string.Empty;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof(Stub).GetProperty("Property");

            var mapper = new SitecoreFieldEnumMapper();

            //Act
            var result = (StubEnum)mapper.GetField(field, config, null);

            //Assert
            Assert.AreEqual(StubEnum.Value1, result);
        }

        [Test]
        public void GetField_FieldContainsInvalidValidEnum_ThrowsException()
        {
            //Assign
            string fieldValue = "hello world";
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof(Stub).GetProperty("Property");

            var mapper = new SitecoreFieldEnumMapper();

            //Act

            Assert.Throws<MapperException>(() =>
            {
                var result = (StubEnum) mapper.GetField(field, config, null);
            });
            //Assert
        }

        #endregion


        #region Method - SetField

        [Test]
        public void SetField_ObjectisValidEnum_SetsFieldValue()
        {
            //Assign
            string expected = "Value2";
            StubEnum objectValue = StubEnum.Value2;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, string.Empty);
            var field = item.Fields[new ID(fieldId)];

            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof(Stub).GetProperty("Property");

            var mapper = new SitecoreFieldEnumMapper();

            item.Editing.BeginEdit();

            //Act
           
                mapper.SetField(field, objectValue, config, null);
            


            //Assert
            Assert.AreEqual(expected, field.Value);
        }

        [Test]
        public void SetField_ObjectIsInt_ThrowsException()
        {
            //Assign
            string objectValue = "hello world";
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, string.Empty);
            var field = item.Fields[new ID(fieldId)];

            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof(Stub).GetProperty("Property");

            var mapper = new SitecoreFieldEnumMapper();

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                mapper.SetField(field, objectValue, config, null);

            });

            //Assert
        }

        #endregion

        #region Method - CanHandle

        [Test]
        public void CanHandle_PropertyIsEnum_ReturnsTrue()
        {
            //Assign
            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof (Stub).GetProperty("Property");

            var mapper = new SitecoreFieldEnumMapper();

            //Act
            var result = mapper.CanHandle(config, null);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanHandle_PropertyIsNotEnum_ReturnsTrue()
        {
            //Assign
            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof(Stub).GetProperty("PropertyNotEnum");

            var mapper = new SitecoreFieldEnumMapper();

            //Act
            var result = mapper.CanHandle(config, null);

            //Assert
            Assert.IsFalse(result);
        }


        #endregion

        #region Stub

        public enum StubEnum
        {
            Value1 =1,
            Value2 = 2
        }

        public class Stub
        {
            public StubEnum Property { get; set; }
            public string PropertyNotEnum { get; set; }
        }
        
    


    #endregion
    }
}




