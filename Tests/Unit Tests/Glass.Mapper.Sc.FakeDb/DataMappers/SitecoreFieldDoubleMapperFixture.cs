using System;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;
using Sitecore.Data;

namespace Glass.Mapper.Sc.FakeDb.DataMappers
{
    [TestFixture]
    public  class SitecoreFieldDoubleMapperFixture : AbstractMapperFixture
    {
        #region Method - GetField

        [Test]
        public void GetField_FieldContainsValidDouble_ReturnsDouble()
        {
            //Assign
            string fieldValue = "3.141592";
            double expected = 3.141592D;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldDoubleMapper();

          
            //Act
            var result = (Double)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetField_FieldContainsEmptyString_ReturnsDoubleZero()
        {
            //Assign
            string fieldValue = string.Empty;
            Double expected = 0;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldDoubleMapper();

            //Act
            var result = (double)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
       
        public void GetField_FieldContainsInvalidValidDouble_ReturnsDouble()
        {
            //Assign
            string fieldValue = "hello world";
            double expected = 3.141592D;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldDoubleMapper();

            //Act
            Assert.Throws<MapperException>(() =>
            {
                var result = (double) mapper.GetField(field, null, null);

                //Assert
                Assert.AreEqual(expected, result);
            });
        }

        #endregion


        #region Method - GetField

        [Test]
        public void SetField_ObjectisValidDouble_SetsFieldValue()
        {
            //Assign
            string expected = "3.141592";
            double objectValue = 3.141592D;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, string.Empty);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldDoubleMapper();

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

            var mapper = new SitecoreFieldDoubleMapper();

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




