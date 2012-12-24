using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    [TestFixture]
    public  class SitecoreFieldFloatMapperFixture : AbstractMapperFixture
    {
        #region Method - GetFieldValue

        [Test]
        public void GetFieldValue_FieldContainsValidFloat_ReturnsFloat()
        {
            //Assign
            string fieldValue = "3.141592";
            float expected = 3.141592F;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldFloatMapper/GetFieldValue");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldFloatMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = (float)mapper.GetFieldValue(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetFieldValue_FieldContainsEmptyString_ReturnsFloatZero()
        {
            //Assign
            string fieldValue = string.Empty;
            float expected = 0f;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldFloatMapper/GetFieldValue");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldFloatMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = (float)mapper.GetFieldValue(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        [ExpectedException(typeof(MapperException))]
        public void GetFieldValue_FieldContainsInvalidValidFloat_ReturnsFloat()
        {
            //Assign
            string fieldValue = "hello world";
            float expected = 3.141592f;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldFloatMapper/GetFieldValue");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldFloatMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = (float)mapper.GetFieldValue(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        #endregion


        #region Method - GetFieldValue

        [Test]
        public void SetFieldValue_ObjectisValidFloat_SetsFieldValue()
        {
            //Assign
            string expected = "3.141592";
            float objectValue = 3.141592f;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldFloatMapper/SetFieldValue");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldFloatMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetFieldValue(field, objectValue, null, null);
            }


            //Assert
            Assert.AreEqual(expected, field.Value);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetFieldValue_ObjectIsInt_ThrowsException()
        {
            //Assign
            int objectValue = 3;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldFloatMapper/SetFieldValue");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldFloatMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetFieldValue(field, objectValue, null, null);
            }


            //Assert
        }

        #endregion
    }
}
