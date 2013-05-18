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
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    [TestFixture]
    public class SitecoreIgnoreMapperFixture : AbstractMapperFixture
    {

        #region Method - MapCmsToProperty

        [Test]
        public void MapCmsToProperty_DoesNotAlterObject()
        {
            //Assign
            var fieldValue = "hello world";
            var propertyValue = "goodbye world";

            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreIgnoreMapper/Target");
            var field = item.Fields["Field"];

            var mapper = new SitecoreIgnoreMapper();
            var config = new SitecoreIgnoreMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            var stub = new StubClass();
            stub.Field = propertyValue;
            var context = new SitecoreDataMappingContext(stub, item, null);

            //Act
            mapper.MapCmsToProperty(context);


            //Assert
            Assert.AreEqual(stub.Field, propertyValue);
        }

        #endregion

        #region Method - MapPropertyToCms

        [Test]
        public void MapPropertyToCms_DoesNotAlterObject()
        {
            //Assign
            var fieldValue = "hello world";
            var propertyValue = "goodbye world";

            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreIgnoreMapper/Target");
            var field = item.Fields["Field"];

            var mapper = new SitecoreIgnoreMapper();
            var config = new SitecoreIgnoreMapper();

            using (new ItemEditing(item, true))
            {
                field.Value = fieldValue;
            }

            var stub = new StubClass();
            stub.Field = propertyValue;
            var context = new SitecoreDataMappingContext(stub, item, null);

            //Act
            mapper.MapPropertyToCms(context);


            //Assert
            Assert.AreEqual(fieldValue, item.Fields["Field"].Value);
        }

        #endregion


        #region Stub

        public class StubClass
        {
            public string Field { get; set; }
        }
        #endregion
    }
}
