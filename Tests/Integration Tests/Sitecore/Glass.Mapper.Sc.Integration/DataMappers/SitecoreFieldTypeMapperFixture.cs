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
using Glass.Mapper.Sc.CastleWindsor;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;
using Glass.Mapper.Sc.Configuration;
using Sitecore.FakeDb;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    [TestFixture]
    public class SitecoreFieldTypeMapperFixture : AbstractMapperFixture
    {

        #region Method - CanHandle

        [Test]
        public void CanHandle_TypeHasBeenLoadedByGlass_ReturnsTrue()
        {
            //Assign
            var mapper = new SitecoreFieldTypeMapper();
            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyTrue");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanHandle_TypeHasNotBeenLoadedByGlass_ReturnsTrueOnDemand()
        {
            //Assign
            var mapper = new SitecoreFieldTypeMapper();
            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyFalse");
            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsTrue(result);
        }

        #endregion

        #region Method - GetField


        [Test]
        public void GetField_FieldContainsId_ReturnsConcreteType()
        {
             //Assign
            string itemName = "SitecoreFieldTypeMapper";
            var targetId = Sitecore.Data.ID.NewID;

            using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
            {
                new DbItem("some name", targetId ),
                new Sitecore.FakeDb.DbItem(itemName)
                {
                    new DbField(FieldName)
                    {
                        Value = targetId.ToString()
                    }
                }
            })
            {
                var database = Sitecore.Configuration.Factory.GetDatabase("master");
                var item = database.GetItem("/sitecore/content/{0}".Formatted(itemName));

                var mapper = new SitecoreFieldTypeMapper();
                var field = item.Fields[FieldName];
                var config = new SitecoreFieldConfiguration();
                config.PropertyInfo = typeof (StubContaining).GetProperty("PropertyTrue");

                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
                var service = new SitecoreService(Database, context);

                var scContext = new SitecoreDataMappingContext(null, item, service);

                //Act
                var result = mapper.GetField(field, config, scContext) as Stub;

                //Assert
                Assert.AreEqual(targetId.Guid, result.Id);
            }

        }

        [Test]
        public void GetField_FieldEmpty_ReturnsNull()
        {
            //Assign
            string itemName = "SitecoreFieldTypeMapper";

            using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
            {
                new Sitecore.FakeDb.DbItem(itemName)
                {
                    new DbField(FieldName)
                }
            })
            {
                var database = Sitecore.Configuration.Factory.GetDatabase("master");
                var item = database.GetItem("/sitecore/content/{0}".Formatted(itemName));

                var mapper = new SitecoreFieldTypeMapper();
                var field = item.Fields[FieldName];
                var config = new SitecoreFieldConfiguration();
                config.PropertyInfo = typeof (StubContaining).GetProperty("PropertyTrue");

                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
                var service = new SitecoreService(Database, context);

                var scContext = new SitecoreDataMappingContext(null, item, service);

                //Act
                var result = mapper.GetField(field, config, scContext) as Stub;

                //Assert
                Assert.IsNull(result);
            }

        }

        [Test]
        public void GetField_FieldRandomText_ReturnsNull()
        {
            //Assign

            string itemName = "SitecoreFieldTypeMapper";

            using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
            {
                new Sitecore.FakeDb.DbItem(itemName)
                {
                    new DbField(FieldName)
                    {
                        Value = "some random text"
                    }
                }
            })
            {
                var database = Sitecore.Configuration.Factory.GetDatabase("master");
                var item = database.GetItem("/sitecore/content/{0}".Formatted(itemName));

                var mapper = new SitecoreFieldTypeMapper();
                var field = item.Fields[FieldName];
                var config = new SitecoreFieldConfiguration();
                config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyTrue");

                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
                var service = new SitecoreService(Database, context);

                var scContext = new SitecoreDataMappingContext(null, item, service);

                //Act
                var result = mapper.GetField(field, config, scContext) as Stub;

                //Assert
                Assert.IsNull(result);
            }

        }

        #endregion


        #region Method - SetField

        [Test]
        public void SetField_ClassContainsId_IdSetInField()
        {

            //Arrange
            string itemName = "SitecoreFieldTypeMapper";
            var targetId = Sitecore.Data.ID.NewID;


            using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
            {
                new Sitecore.FakeDb.DbItem(itemName)
                {
                    new DbField(FieldName)
                },
                new DbItem("target item", targetId)
            })
            {

                var database = Sitecore.Configuration.Factory.GetDatabase("master");
                var item = database.GetItem("/sitecore/content/{0}".Formatted(itemName));

                var mapper = new SitecoreFieldTypeMapper();
                var field = item.Fields[FieldName];

                var config = new SitecoreFieldConfiguration();
                config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyTrue");

                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
                var service = new SitecoreService(Database, context);

                var propertyValue = new Stub();
                propertyValue.Id = targetId.Guid;

                var scContext = new SitecoreDataMappingContext(null, item, service);

                //Act
                using (new ItemEditing(item, true))
                {
                    mapper.SetField(field, propertyValue, config, scContext);
                }
                //Assert
                Assert.AreEqual(targetId.Guid, Guid.Parse(item[FieldName]));
            }
        }


        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetField_ClassContainsNoIdProperty_ThrowsException()
        {
            //Assign

            string itemName = "SitecoreFieldTypeMapper";

            using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
            {
               new Sitecore.FakeDb.DbItem(itemName)
                {
                    new DbField(FieldName)
                }
            })
            {
                var database = Sitecore.Configuration.Factory.GetDatabase("master");
                var item = database.GetItem("/sitecore/content/{0}".Formatted(itemName));

                var mapper = new SitecoreFieldTypeMapper();
                var field = item.Fields[FieldName];

                var config = new SitecoreFieldConfiguration();
                config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyNoId");

                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
                var service = new SitecoreService(Database, context);

                var propertyValue = new StubNoId();

                var scContext = new SitecoreDataMappingContext(null, item, service);

                //Act
                using (new ItemEditing(item, true))
                {
                    mapper.SetField(field, propertyValue, config, scContext);
                }

                //Assert
                Assert.AreEqual(string.Empty, item[FieldName]);
            }
        }


        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SetField_ClassContainsIdButItemMissing_ThrowsException()
        {
            //Assign

            string itemName = "SitecoreFieldTypeMapper";

            using (Sitecore.FakeDb.Db db = new Sitecore.FakeDb.Db
            {
                new Sitecore.FakeDb.DbItem(itemName)
                {
                    new DbField(FieldName)
                }
            })
            {
                var database = Sitecore.Configuration.Factory.GetDatabase("master");
                var item = database.GetItem("/sitecore/content/{0}".Formatted(itemName));

                var targetId = Guid.Parse("{11111111-A3F0-410E-8A6D-07FF3A1E78C3}");
                var mapper = new SitecoreFieldTypeMapper();
                var field = item.Fields[FieldName];

                var config = new SitecoreFieldConfiguration();
                config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyTrue");

                var context = Context.Create(Utilities.CreateStandardResolver());
                context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
                var service = new SitecoreService(Database, context);

                var propertyValue = new Stub();
                propertyValue.Id = targetId;

                var scContext = new SitecoreDataMappingContext(null, item, service);

                //Act
                using (new ItemEditing(item, true))
                {
                    mapper.SetField(field, propertyValue, config, scContext);
                }
            }

        }
        #endregion

        #region Stubs

        [SitecoreType]
        public class Stub
        {
            [SitecoreId]
            public virtual Guid Id { get; set; }
        }

        public class StubContaining
        {
            public Stub PropertyTrue { get; set; }
            public StubContaining PropertyFalse { get; set; }
            public StubNoId PropertyNoId { get; set; }
        }

        [SitecoreType]
        public class StubNoId
        {
        }

        #endregion



    }
}




