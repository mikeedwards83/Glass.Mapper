using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Integration.DataMappers
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
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldEnumMapper/GetField");
            var field = item.Fields[FieldName];
            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof (Stub).GetProperty("Property");

            var mapper = new SitecoreFieldEnumMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

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
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldEnumMapper/GetField");
            var field = item.Fields[FieldName];
            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof(Stub).GetProperty("Property");

            var mapper = new SitecoreFieldEnumMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = (StubEnum)mapper.GetField(field, config, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        [ExpectedException(typeof(MapperException))]
        public void GetField_FieldContainsEmptyString_ThowsMapperException()
        {
            //Assign
            string fieldValue = string.Empty;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldEnumMapper/GetField");
            var field = item.Fields[FieldName];
            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof(Stub).GetProperty("Property");

            var mapper = new SitecoreFieldEnumMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = (StubEnum)mapper.GetField(field, config, null);

            //Assert
        }

        [Test]
        [ExpectedException(typeof (MapperException))]
        public void GetField_FieldContainsInvalidValidEnum_ThrowsException()
        {
            //Assign
            string fieldValue = "hello world";
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldEnumMapper/GetField");
            var field = item.Fields[FieldName];
            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof(Stub).GetProperty("Property");

            var mapper = new SitecoreFieldEnumMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = (StubEnum)mapper.GetField(field, config, null);

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
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldEnumMapper/SetField");
            var field = item.Fields[FieldName];

            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof(Stub).GetProperty("Property");

            var mapper = new SitecoreFieldEnumMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetField(field, objectValue, config, null);
            }


            //Assert
            Assert.AreEqual(expected, field.Value);
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void SetField_ObjectIsInt_ThrowsException()
        {
            //Assign
            string objectValue = "hello world";
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldEnumMapper/SetField");
            var field = item.Fields[FieldName];

            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof(Stub).GetProperty("Property");

            var mapper = new SitecoreFieldEnumMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetField(field, objectValue, config, null);
            }


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
