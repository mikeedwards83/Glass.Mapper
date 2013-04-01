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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    [TestFixture]
    public  class SitecoreFieldIntegerMapperFixture : AbstractMapperFixture
    {
        #region Method - GetField

        [Test]
        public void GetField_FieldContainsValidInteger_ReturnsInteger()
        {
            //Assign
            string fieldValue = "3";
            int expected = 3;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldIntegerMapper/GetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldIntegerMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = (int)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetField_FieldContainsEmptyString_ReturnsIntegerZero()
        {
            //Assign
            string fieldValue = string.Empty;
            int expected = 0;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldIntegerMapper/GetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldIntegerMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = (int)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        [ExpectedException(typeof(MapperException))]
        public void GetField_FieldContainsInvalidValidInteger_ReturnsInteger()
        {
            //Assign
            string fieldValue = "hello world";
            int expected = 3;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldIntegerMapper/GetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldIntegerMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            var result = (int)mapper.GetField(field, null, null);

            //Assert
            Assert.AreEqual(expected, result);
        }

        #endregion


        #region Method - SetField

        [Test]
        public void SetField_ObjectisValidInteger_SetsFieldValue()
        {
            //Assign
            string expected = "3";
            int objectValue = 3;
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldIntegerMapper/SetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldIntegerMapper();

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
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldIntegerMapper/SetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldIntegerMapper();

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



