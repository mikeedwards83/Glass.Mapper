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

namespace Glass.Mapper.Sc.Tests.DataMappers
{
    [TestFixture]
    public class SitecoreFieldDateTimeMapperFixture : AbstractMapperFixture
    {
        #region Method - GetField

        [Test]
        public void GetField_FieldContainsValidDate_ReturnsDateTime()
        {
            //Assign
            string fieldValue = "20120101T010101";
            DateTime expected = new DateTime(2012,01,01,01,01,01);
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, fieldValue);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldDateTimeMapper();

          
            //Act
            var result = (DateTime) mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        #endregion

        #region Method - SetField

        [Test]
        public void SetField_DateTimePassed_SetsFieldValue()
        {
            //Assign
            string expected = "20120101T010101";
            DateTime objectValue = new DateTime(2012, 01, 01, 01, 01, 01);
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, string.Empty);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldDateTimeMapper();

            item.Editing.BeginEdit();
            //Act
          
                mapper.SetField(field, objectValue, null, null);
           
        

            //Assert
            Assert.AreEqual(expected, field.Value);
        }

        [Test]
        [ExpectedException(typeof (NotSupportedException))]
        public void SetField_NonDateTimePassed_ExceptionThrown()
        {
            //Assign
            string expected = "20120101T010101";
            int objectValue = 4;
            var fieldId = Guid.NewGuid();

            var item = Helpers.CreateFakeItem(fieldId, string.Empty);
            var field = item.Fields[new ID(fieldId)];

            var mapper = new SitecoreFieldDateTimeMapper();

            item.Editing.BeginEdit();

            //Act

            mapper.SetField(field, objectValue, null, null);



            //Assert
        }

        #endregion
    }
}




