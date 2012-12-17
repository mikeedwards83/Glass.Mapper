using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    [TestFixture]
    public  class SitecoreFieldDoubleMapperFixture : AbstractMapperFixture
    {
        #region Method - GetFieldValue

        [Test]
        public void GetFieldValue_FieldContainsValidDouble_ReturnsDouble()
        {
            //Assign
            string fieldValue = "3.141592";
            double expected = 3.141592D;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldDoubleMapper/GetFieldValue");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldDoubleMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = (Double)mapper.GetFieldValue(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        public void GetFieldValue_FieldContainsEmptyString_ReturnsDoubleZero()
        {
            //Assign
            string fieldValue = string.Empty;
            Double expected = 0;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldDoubleMapper/GetFieldValue");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldDoubleMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = (double)mapper.GetFieldValue(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        [ExpectedException(typeof(MapperException))]
        public void GetFieldValue_FieldContainsInvalidValidDouble_ReturnsDouble()
        {
            //Assign
            string fieldValue = "hello world";
            double expected = 3.141592D;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldDoubleMapper/GetFieldValue");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldDoubleMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = (double)mapper.GetFieldValue(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        #endregion


        #region Method - GetFieldValue

        [Test]
        public void SetFieldValue_ObjectisValidDouble_SetsFieldValue()
        {
            //Assign
            string expected = "3.141592";
            double objectValue = 3.141592D;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldDoubleMapper/SetFieldValue");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldDoubleMapper();

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
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldDoubleMapper/SetFieldValue");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldDoubleMapper();

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
