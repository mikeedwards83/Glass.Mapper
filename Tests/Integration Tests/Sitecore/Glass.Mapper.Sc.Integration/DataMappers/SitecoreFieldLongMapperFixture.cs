using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    [TestFixture]
    public  class SitecoreFieldLongMapperFixture : AbstractMapperFixture
    {
        #region Method - GetField

        [Test]
        public void GetField_FieldContainsValidLong_ReturnsLong()
        {
            //Assign
            string fieldValue = "3";
            long expected = 3;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldLongMapper/GetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldLongMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = (long)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetField_FieldContainsEmptyString_ReturnsLongZero()
        {
            //Assign
            string fieldValue = string.Empty;
            long expected = 0;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldLongMapper/GetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldLongMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = (long)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        [ExpectedException(typeof(MapperException))]
        public void GetField_FieldContainsInvalidValidLong_ReturnsLong()
        {
            //Assign
            string fieldValue = "hello world";
            long expected = 3;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldLongMapper/GetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldLongMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = (long)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        #endregion


        #region Method - SetField

        [Test]
        public void SetField_ObjectisValidLong_SetsFieldValue()
        {
            //Assign
            string expected = "3";
            long objectValue = 3;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldLongMapper/SetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldLongMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetField(field, objectValue, null, null);
            }


            //Assert
            Assert.AreEqual(expected, field.Value);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetField_ObjectIsDouble_ThrowsException()
        {
            //Assign
            double objectValue = 3;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldLongMapper/SetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldLongMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetField(field, objectValue, null, null);
            }


            //Assert
        }

        #endregion
    }
}
