using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    [TestFixture]
    public  class SitecoreFieldDecimalMapperFixture : AbstractMapperFixture
    {
        #region Method - GetField

        [Test]
        public void GetField_FieldContainsValidDecimal_ReturnsDecimal()
        {
            //Assign
            string fieldValue = "3.141592";
            decimal expected = 3.141592M;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldDecimalMapper/GetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldDecimalMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = (decimal)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        public void GetField_FieldContainsEmptyString_ReturnsDecimalZero()
        {
            //Assign
            string fieldValue = string.Empty;
            decimal expected = 0;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldDecimalMapper/GetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldDecimalMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = (decimal)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        [ExpectedException(typeof(MapperException))]
        public void GetField_FieldContainsInvalidValidDecimal_ReturnsDecimal()
        {
            //Assign
            string fieldValue = "hello world";
            decimal expected = 3.141592M;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldDecimalMapper/GetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldDecimalMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = (decimal)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        #endregion


        #region Method - GetField

        [Test]
        public void SetField_ObjectisValidDecimal_SetsFieldValue()
        {
            //Assign
            string expected = "3.141592";
            decimal objectValue = 3.141592M;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldDecimalMapper/SetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldDecimalMapper();

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
        public void SetField_ObjectIsInt_ThrowsException()
        {
            //Assign
            int objectValue = 3;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldDecimalMapper/SetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldDecimalMapper();

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
