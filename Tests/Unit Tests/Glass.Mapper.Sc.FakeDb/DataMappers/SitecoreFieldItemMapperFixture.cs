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
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.FakeDb;

namespace Glass.Mapper.Sc.FakeDb.DataMappers
{
    [TestFixture]
    public class SitecoreFieldItemMapperFixture
    {
        #region Method - GetFieldValue

        [Test]
        public void GetFieldValue_FieldContainsId_ReturnsItem()
        {
            //Assign
            var itemId = Guid.NewGuid();

            using (Db database = new Db
            {
                new DbItem("Target", new ID(itemId))
                {
                    { "Field", new ID(itemId).ToString() }
                }
            })
            {
                var item = database.GetItem("/sitecore/content/Target");
                var mapper = new SitecoreFieldItemMapper();
                var field = item.Fields["Field"];
                var config = new SitecoreFieldConfiguration();
                var context = Context.Create(Utilities.CreateStandardResolver());
                var service = new SitecoreService(database.Database, context);
                var scContext = new SitecoreDataMappingContext(null, item, service);

                //Act
                var result = mapper.GetFieldValue(field.Value, config, scContext) as Item;

                //Assert
                Assert.NotNull(result);
                Assert.AreEqual(item.ID, result.ID);
                Assert.AreEqual(item.Name, "Target");
            }
        }

        [Test]
        public void GetFieldValue_FieldContainsInvalidId_ReturnsNull()
        {
            //Assign
            var itemId = Guid.NewGuid();

            using (Db database = new Db
            {
                new DbItem("Target", new ID(itemId))
                {
                    { "Field", "invalid id" }
                }
            })
            {
                var item = database.GetItem("/sitecore/content/Target");
                var mapper = new SitecoreFieldItemMapper();
                var field = item.Fields["Field"];
                var config = new SitecoreFieldConfiguration();
                var context = Context.Create(Utilities.CreateStandardResolver());
                var service = new SitecoreService(database.Database, context);
                var scContext = new SitecoreDataMappingContext(null, item, service);

                //Act
                var result = mapper.GetFieldValue(field.Value, config, scContext) as Item;

                //Assert
                Assert.Null(result);
            }
        }

        #endregion

        #region Method - SetFieldValue

        [Test]
        public void SetFieldValue_NullPassed_ReturnsNull()
        {
            //Assign
            using (Db database = new Db
            {
                new DbItem("Target")
            })
            {
                var item = database.GetItem("/sitecore/content/Target");
                var mapper = new SitecoreFieldItemMapper();
                var config = new SitecoreFieldConfiguration();
                var context = Context.Create(Utilities.CreateStandardResolver());
                var service = new SitecoreService(database.Database, context);
                var scContext = new SitecoreDataMappingContext(null, item, service);

                //Act
                var result = mapper.SetFieldValue(null, config, scContext);

                //Assert
                Assert.Null(result);
            }
        }

        [Test]
        public void SetFieldValue_NotItemTypePassed_ThrowsMapperException()
        {
            //Assign
            var itemId = Guid.NewGuid();

            using (Db database = new Db
            {
                new DbItem("Target", new ID(itemId))
                {
                    { "Field", "" }
                }
            })
            {
                var item = database.GetItem("/sitecore/content/Target");
                var mapper = new SitecoreFieldItemMapper();
                var config = new SitecoreFieldConfiguration();
                var context = Context.Create(Utilities.CreateStandardResolver());
                var service = new SitecoreService(database.Database, context);
                var scContext = new SitecoreDataMappingContext(null, item, service);

                //Act
                Assert.Throws<MapperException>(() =>
                {
                    mapper.SetFieldValue("invalid value type", config, scContext);
                });

                //Assert
            }
        }

        [Test]
        public void SetFieldValue_ItemPassed_ReturnsId()
        {
            //Assign
            using (Db database = new Db
            {
                new DbItem("Target")
            })
            {
                var item = database.GetItem("/sitecore/content/Target");
                var mapper = new SitecoreFieldItemMapper();
                var config = new SitecoreFieldConfiguration();
                var context = Context.Create(Utilities.CreateStandardResolver());
                var service = new SitecoreService(database.Database, context);
                var scContext = new SitecoreDataMappingContext(null, item, service);

                //Act
                var result = mapper.SetFieldValue(item, config, scContext);

                //Assert
                Assert.NotNull(result);
                Assert.AreEqual(item.ID.ToString(), result);
            }
        }

        #endregion
    }
}
