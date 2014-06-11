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
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sc.Tests.DataMappers
{
    [TestFixture]
    public class SitecoreFieldBooleanMapperFixture : AbstractMapperFixture
    {

        #region Method - GetField

        [Test]
        public void GetField_FieldValueZero_ReturnsFalse()
        {
            //Assign
            
            var fieldName = "Field";
            var fieldValue = "0";

            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];


            var mapper = new SitecoreFieldBooleanMapper();

            //Act
            var result = mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(false, result);

        }

        [Test]
        public void GetField_FieldValueStringEmpty_ReturnsFalse()
        {
            //Assign
           
            var fieldName = "Field";
            var fieldValue = string.Empty;

            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];


            var mapper = new SitecoreFieldBooleanMapper();

            //Act
            var result = mapper.GetField(field, null, null);


            //Assert
            Assert.AreEqual(false, result);

        }

        [Test]
        public void GetField_FieldValueOne_ReturnsTrue()
        {
            //Assign
           
            var fieldName = "Field";
            var fieldValue = "1";
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldBooleanMapper();

            //Act
            var result = mapper.GetField(field, null, null);


            //Assert
            Assert.AreEqual(true, result);

        }

        [Test]
        public void GetField_FieldValueRandom_ReturnsFalse()
        {
            //Assign
       
            var fieldName = "Field";
            var fieldValue = "afaegaeg";
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldBooleanMapper();
            
            //Act
            var result = mapper.GetField(field, null, null);


            //Assert
            Assert.AreEqual(false, result);

        }
        #endregion

        #region Method - SetField

        [Test]
        public void SetField_ValueFalse_FieldSetToZero()
        {
            //Assign
           
            var fieldName = "Field";
            var expected = "0";
            var mapper = new SitecoreFieldBooleanMapper();
            var objectValue = false;

            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, string.Empty);
            var field = item.Fields[new ID(fieldId)];


            item.Editing.BeginEdit();

            //Act
           
            mapper.SetField(field, objectValue, null, null);
             
            //Assert
            Assert.AreEqual(expected, field.Value);

        }

        [Test]
        public void SetField_ValueTrue_FieldSetToOne()
        {
            //Assign
           
            var fieldName = "Field";
            var expected = "1";
            var mapper = new SitecoreFieldBooleanMapper();
            var objectValue = true;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, string.Empty);
            var field = item.Fields[new ID(fieldId)];
            
            item.Editing.BeginEdit();

            //Act
          
            mapper.SetField(field, objectValue, null, null);
          

            //Assert
            Assert.AreEqual(expected, field.Value);

        }
        #endregion

    }
}




