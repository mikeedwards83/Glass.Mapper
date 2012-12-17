using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;
using Glass.Mapper.Sc.Configuration;
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
            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanHandle_TypeHasNotBeenLoadedByGlass_ReturnsFalse()
        {
            //Assign
            var mapper = new SitecoreFieldTypeMapper();
            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyFalse");
            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region Method - GetFieldValue


        [Test]
        public void GetFieldValue_FieldContainsId_ReturnsConcreteType()
        {
            //Assign
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldTypeMapper/GetFieldValue");
            var targetId = Guid.Parse("{BB01B0A5-A3F0-410E-8A6D-07FF3A1E78C3}");
            var mapper = new SitecoreFieldTypeMapper();
            var field = item.Fields[FieldName];
            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof (StubContaining).GetProperty("PropertyTrue");

            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreService(Database, context);


            var scContext = new SitecoreDataMappingContext(null, item, service);

            using (new ItemEditing(item, true))
            {
                field.Value = targetId.ToString();
            }

            //Act
            var result = mapper.GetFieldValue(field, config, scContext) as Stub;

            //Assert
            Assert.AreEqual(targetId, result.Id);

        }

        [Test]
        public void GetFieldValue_FieldEmpty_ReturnsNull()
        {
            //Assign
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldTypeMapper/GetFieldValue");
            var targetId = string.Empty;
            var mapper = new SitecoreFieldTypeMapper();
            var field = item.Fields[FieldName];
            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyTrue");

            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreService(Database, context);


            var scContext = new SitecoreDataMappingContext(null, item, service);

            using (new ItemEditing(item, true))
            {
                field.Value = targetId.ToString();
            }

            //Act
            var result = mapper.GetFieldValue(field, config, scContext) as Stub;

            //Assert
            Assert.IsNull(result);

        }

        [Test]
        public void GetFieldValue_FieldRandomText_ReturnsNull()
        {
            //Assign
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldTypeMapper/GetFieldValue");
            var targetId = "some random text";
            var mapper = new SitecoreFieldTypeMapper();
            var field = item.Fields[FieldName];
            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyTrue");

            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreService(Database, context);


            var scContext = new SitecoreDataMappingContext(null, item, service);

            using (new ItemEditing(item, true))
            {
                field.Value = targetId.ToString();
            }

            //Act
            var result = mapper.GetFieldValue(field, config, scContext) as Stub;

            //Assert
            Assert.IsNull(result);

        }

        #endregion


        #region Method - SetFieldValue

        [Test]
        public void SetFieldValue_ClassContainsId_IdSetInField()
        {
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldTypeMapper/SetFieldValue");
            var targetId = Guid.Parse("{BB01B0A5-A3F0-410E-8A6D-07FF3A1E78C3}");
            var mapper = new SitecoreFieldTypeMapper();
            var field = item.Fields[FieldName];

            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyTrue");

            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreService(Database, context);

            var propertyValue = new Stub();
            propertyValue.Id = targetId;

            var scContext = new SitecoreDataMappingContext(null, item, service);

            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetFieldValue(field, propertyValue, config, scContext);
            }
            //Assert
            Assert.AreEqual(targetId, Guid.Parse(item[FieldName]));   
        }


        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetFieldValue_ClassContainsNoIdProperty_ThrowsException()
        {
            //Assign
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldTypeMapper/SetFieldValue");
            var targetId = Guid.Parse("{BB01B0A5-A3F0-410E-8A6D-07FF3A1E78C3}");
            var mapper = new SitecoreFieldTypeMapper();
            var field = item.Fields[FieldName];

            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyNoId");

            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreService(Database, context);

            var propertyValue = new StubNoId();

            var scContext = new SitecoreDataMappingContext(null, item, service);

            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetFieldValue(field, propertyValue, config, scContext);
            }

            //Assert
            Assert.AreEqual(string.Empty, item[FieldName]);
        }


        [Test]
        [ExpectedException(typeof(NullReferenceException))]
        public void SetFieldValue_ClassContainsIdButItemMissing_ThrowsException()
        {
            //Assign
            var item = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreFieldTypeMapper/SetFieldValue");
            var targetId = Guid.Parse("{11111111-A3F0-410E-8A6D-07FF3A1E78C3}");
            var mapper = new SitecoreFieldTypeMapper();
            var field = item.Fields[FieldName];

            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = typeof(StubContaining).GetProperty("PropertyTrue");

            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));
            var service = new SitecoreService(Database, context);

            var propertyValue = new Stub();
            propertyValue.Id = targetId;

            var scContext = new SitecoreDataMappingContext(null, item, service);

            using (new ItemEditing(item, true))
            {
                field.Value = string.Empty;
            }

            //Act
            using (new ItemEditing(item, true))
            {
                mapper.SetFieldValue(field, propertyValue, config, scContext);
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
