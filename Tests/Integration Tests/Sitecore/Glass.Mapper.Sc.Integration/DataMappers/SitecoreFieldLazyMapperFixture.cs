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
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;
using Sitecore.Configuration;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    [TestFixture]
    public class SitecoreFieldStringMapperFixture : AbstractMapperFixture
    {

        #region Method - GetField

        [Test]
        public void GetField_FieldContainsData_StringIsReturned()
        {
            //Assign
            var fieldValue = "hello world";

            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldStringMapper/GetField");
            var field = item.Fields[FieldName];
            var lazyMapper = new SitecoreFieldLazyStringMapper();
            var mapper = new SitecoreFieldStringMapper();
            lazyMapper.Mapper = mapper;
            var config = new SitecoreFieldConfiguration();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            //Act
            Lazy<string> result = lazyMapper.GetField(field, config, null) as Lazy<string>;

            //Assert
            Assert.AreEqual(fieldValue, result.Value);
        }

        #endregion

        #region Method - SetField

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetField_RichText_ThrowsException()
        {
            //Assign
            var expected = "<p>Test with <a href=\"~/link.aspx?_id=BFD7975DF42F41E19DDA9A38E971555F&amp;_z=z\">link</a></p>";
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldStringMapper/SetFieldRichText");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldStringMapper();
            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof(StubClass).GetProperty("String");


            Sitecore.Context.Site = Factory.GetSite("website");

            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetField(field, expected, config, null);
            }

            Sitecore.Context.Site = null;

            //Assert
            Assert.AreEqual(expected, field.Value);
        }

        [Test]
        public void SetField_FielNonRichText_ValueWrittenToField()
        {
            //Assign
            var expected = "<p>Test with <a href=\"~/link.aspx?_id=BFD7975DF42F41E19DDA9A38E971555F&amp;_z=z\">link</a></p>";
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldStringMapper/SetField");
            var field = item.Fields[FieldName];

            var mapper = new SitecoreFieldStringMapper();
            var config = new SitecoreFieldConfiguration();
            config.Setting = SitecoreFieldSettings.RichTextRaw;
            
            Sitecore.Context.Site = Factory.GetSite("website");

            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetField(field, expected, config, null);
            }

            Sitecore.Context.Site = null;

            //Assert
            Assert.AreEqual(expected, field.Value);
        }

        #endregion

        #region Method - CanHandle

        [Test]
        public void CanHandle_LazyType_ReturnsTrue()
        {
            //Assign
            var mapper = new SitecoreFieldLazyStringMapper();
            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof (StubClass).GetProperty("LazyString");

            //Act
            var result = mapper.CanHandle(config, null);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanHandle_NonLazyType_ReturnsFalse()
        {
            //Assign
            var mapper = new SitecoreFieldLazyStringMapper();
            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof(StubClass).GetProperty("String");

            //Act
            var result = mapper.CanHandle(config, null);

            //Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region Stub

        public class StubClass
        {
            public Lazy<string> LazyString { get; set; }

            public string String { get; set; }
        }
        #endregion
    }
}




