using System;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;
using Sitecore.Data;

namespace Glass.Mapper.Sc.FakeDb.DataMappers
{
    [TestFixture]
    public class SitecoreFieldGuidMapperFixture : AbstractMapperFixture 
    {
        #region Method - GetField

        [Test]
        public void GetField_ContainsGuid_GuidObjectReturned()
        {
            //Assign
            var fieldValue = "{FC1D0AFD-71CC-47e2-84B3-7F1A2973248B}";
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var expected = new Guid(fieldValue);

            var mapper = new SitecoreFieldGuidMapper();

           

            //Act
            var result = (Guid)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetField_FieldEmpty_EmptyGuidReturned()
        {
            //Assign
            var fieldValue = string.Empty;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var expected = Guid.Empty;

            var mapper = new SitecoreFieldGuidMapper();

          

            //Act
            var result = (Guid)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        #endregion

        #region Method - SetField

        [Test]
        public void SetField_Guidpassed_ValueSetOnField()
        {
            //Assign
            var expected = "{FC1D0AFD-71CC-47E2-84B3-7F1A2973248B}";
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, string.Empty);
            var field = item.Fields[new ID(fieldId)];

            var value = new Guid(expected);

            var mapper = new SitecoreFieldGuidMapper();

          item.Editing.BeginEdit();

            //Act
         
                mapper.SetField(field, value, null, null);
            
            //Assert
            Assert.AreEqual(expected, field.Value);
        }

        [Test]
        public void SetField_EmptyGuidPassed_ValueSetOnField()
        {
            //Assign
            var expected = "{00000000-0000-0000-0000-000000000000}";
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, string.Empty);
            var field = item.Fields[new ID(fieldId)];

            var value = new Guid(expected);

            var mapper = new SitecoreFieldGuidMapper();
            
            item.Editing.BeginEdit();

            //Act
            
                mapper.SetField(field, value, null, null);
            
            //Assert
            Assert.AreEqual(expected, field.Value);
        }

        [Test]
        public void SetField_IntegerPassed_ValueSetOnField()
        {
            //Assign
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, string.Empty);
            var field = item.Fields[new ID(fieldId)];

            var value = 1;

            var mapper = new SitecoreFieldGuidMapper();

            item.Editing.BeginEdit();

            //Act

            Assert.Throws<MapperException>(() =>
            {
                mapper.SetField(field, value, null, null);
            });
            //Assert
        }

        #endregion
    }

   
}




