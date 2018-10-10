using System;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;
using Sitecore.Data;

namespace Glass.Mapper.Sc.FakeDb.DataMappers
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
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldDecimalMapper();


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
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldDecimalMapper();
            
            //Act
            var result = (decimal)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
       
        public void GetField_FieldContainsInvalidValidDecimal_ReturnsDecimal()
        {
            //Assign
            string fieldValue = "hello world";
            decimal expected = 3.141592M;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldDecimalMapper();

            //Act
            Assert.Throws<MapperException>(() =>
            {
                var result = (decimal) mapper.GetField(field, null, null);
            });


        }

        #endregion


        #region Method - GetField

        [Test]
        public void SetField_ObjectisValidDecimal_SetsFieldValue()
        {
            //Assign
            string expected = "3.141592";
            decimal objectValue = 3.141592M;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, string.Empty);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldDecimalMapper();
            
            item.Editing.BeginEdit();

            //Act
          
            mapper.SetField(field, objectValue, null, null);
            


            //Assert
            Assert.AreEqual(expected, field.Value);
        }

        [Test]
        public void SetField_ObjectIsInt_ThrowsException()
        {
            //Assign
            int objectValue = 3;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, string.Empty);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldDecimalMapper();

            item.Editing.BeginEdit();

            //Act

            Assert.Throws<NotSupportedException>(() =>
            {
                mapper.SetField(field, objectValue, null, null);
            });
            //Assert
        }

        #endregion
    }
}




