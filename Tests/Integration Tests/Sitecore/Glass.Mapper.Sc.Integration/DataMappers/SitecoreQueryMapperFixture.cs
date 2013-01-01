using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.DataMappers;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Integration.DataMappers
{
    [TestFixture]
    public class SitecoreQueryMapperFixture : AbstractMapperFixture
    {

        #region Property - ReadOnly

        [Test]
        public void ReadOnly_ReturnsTrue()
        {
            //Assign
            var mapper = new SitecoreQueryMapper(null);

            //Act
            var result = mapper.ReadOnly;

            //Assert
            Assert.IsTrue(result);
        }

        #endregion
        
        #region Method - CanHandle

        [Test]
        public void CanHandle_CorrectConfigIEnumerableMappedClass_ReturnsTrue()
        {
            //Assign
            var mapper = new SitecoreQueryMapper(null);
            var config = new SitecoreQueryConfiguration();
            config.PropertyInfo = new FakePropertyInfo(typeof(IEnumerable<StubMapped>));
            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));


            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanHandle_CorrectConfigIEnumerableNotMappedClass_ReturnsFalse()
        {
            //Assign
            var mapper = new SitecoreQueryMapper(null);
            var config = new SitecoreQueryConfiguration();
            config.PropertyInfo = new FakePropertyInfo(typeof(IEnumerable<StubNotMapped>));
            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));


            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void CanHandle_CorrectConfigNotMappedClass_ReturnsFalse()
        {
            //Assign
            var mapper = new SitecoreQueryMapper(null);
            var config = new SitecoreQueryConfiguration();
            config.PropertyInfo = new FakePropertyInfo(typeof(StubNotMapped));
            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));


            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void CanHandle_CorrectConfigMappedClass_ReturnsFalse()
        {
            //Assign
            var mapper = new SitecoreQueryMapper(null);
            var config = new SitecoreQueryConfiguration();
            config.PropertyInfo = new FakePropertyInfo(typeof(StubMapped));
            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));


            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanHandle_IncorrectConfigMappedClass_ReturnsFalse()
        {
            //Assign
            var mapper = new SitecoreQueryMapper(null);
            var config = new SitecoreFieldConfiguration();
            config.PropertyInfo = new FakePropertyInfo(typeof(StubMapped));
            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));


            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region Method - MapToProperty

        [Test]
        public void MapToProperty_RelativeQuery_ReturnsResults()
        {
            //Assign
            //Assign
            var config = new SitecoreQueryConfiguration();
            config.PropertyInfo = new FakePropertyInfo(typeof(IEnumerable<StubMapped>));
            config.Query = "../Results/*";
            config.IsRelative = true;

            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var mapper = new SitecoreQueryMapper(null);
            mapper.Setup(new DataMapperResolverArgs(context, config));

            var source = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreQueryMapper/Source");
            var service = new SitecoreService(Database, context);

            var result1 = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreQueryMapper/Results/Result1");
            var result2 = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreQueryMapper/Results/Result2");

            //Act
            var results =
                mapper.MapToProperty(new SitecoreDataMappingContext(null, source, service)) as IEnumerable<StubMapped>;

            //Assert
            Assert.AreEqual(2, results.Count());
            Assert.IsTrue(results.Any(x => x.Id == result1.ID.Guid));
            Assert.IsTrue(results.Any(x => x.Id == result2.ID.Guid));

        }

        [Test]
        public void MapToProperty_AbsoluteQuery_ReturnsResults()
        {
            //Assign
            //Assign
            var config = new SitecoreQueryConfiguration();
            config.PropertyInfo = new FakePropertyInfo(typeof(IEnumerable<StubMapped>));
            config.Query = "/sitecore/content/Tests/DataMappers/SitecoreQueryMapper/Results/*";
            config.IsRelative = false;

            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var mapper = new SitecoreQueryMapper(null);
            mapper.Setup(new DataMapperResolverArgs(context, config));

            var source = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreQueryMapper/Source");
            var service = new SitecoreService(Database, context);

            var result1 = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreQueryMapper/Results/Result1");
            var result2 = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreQueryMapper/Results/Result2");

            //Act
            var results =
                mapper.MapToProperty(new SitecoreDataMappingContext(null, source, service)) as IEnumerable<StubMapped>;

            //Assert
            Assert.AreEqual(2, results.Count());
            Assert.IsTrue(results.Any(x => x.Id == result1.ID.Guid));
            Assert.IsTrue(results.Any(x => x.Id == result2.ID.Guid));

        }

        [Test]
        public void MapToProperty_RelativeQuery_ReturnsSingleResults()
        {
            //Assign
            //Assign
            var config = new SitecoreQueryConfiguration();
            config.PropertyInfo = new FakePropertyInfo(typeof(StubMapped));
            config.Query = "../Results/Result1";
            config.IsRelative = true;

            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var mapper = new SitecoreQueryMapper(null);
            mapper.Setup(new DataMapperResolverArgs(context, config));

            var source = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreQueryMapper/Source");
            var service = new SitecoreService(Database, context);

            var result1 = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreQueryMapper/Results/Result1");

            //Act
            var result =
                mapper.MapToProperty(new SitecoreDataMappingContext(null, source, service)) as StubMapped;

            //Assert
            Assert.AreEqual(result1.ID.Guid, result.Id);

        }

        [Test]
        public void MapToProperty_AbsoluteQuery_ReturnsSingleResults()
        {
            //Assign
            //Assign
            var config = new SitecoreQueryConfiguration();
            config.PropertyInfo = new FakePropertyInfo(typeof(StubMapped));
            config.Query = "/sitecore/content/Tests/DataMappers/SitecoreQueryMapper/Results/Result1";
            config.IsRelative = false;

            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var mapper = new SitecoreQueryMapper(null);
            mapper.Setup(new DataMapperResolverArgs(context, config));

            var source = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreQueryMapper/Source");
            var service = new SitecoreService(Database, context);

            var result1 = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreQueryMapper/Results/Result1");

            //Act
            var result =
                mapper.MapToProperty(new SitecoreDataMappingContext(null, source, service)) as StubMapped;

            //Assert
            Assert.AreEqual(result1.ID.Guid, result.Id);

        }

        [Test]
        public void MapToProperty_RelativeQueryWithQueryContext_ReturnsResults()
        {
            //Assign
            //Assign
            var config = new SitecoreQueryConfiguration();
            config.PropertyInfo = new FakePropertyInfo(typeof(IEnumerable<StubMapped>));
            config.Query = "../Results/*";
            config.IsRelative = true;
            config.UseQueryContext = true;

            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var mapper = new SitecoreQueryMapper(null);
            mapper.Setup(new DataMapperResolverArgs(context, config));

            var source = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreQueryMapper/Source");
            var service = new SitecoreService(Database, context);

            var result1 = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreQueryMapper/Results/Result1");
            var result2 = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreQueryMapper/Results/Result2");

            //Act
            var results =
                mapper.MapToProperty(new SitecoreDataMappingContext(null, source, service)) as IEnumerable<StubMapped>;

            //Assert
            Assert.AreEqual(2, results.Count());
            Assert.IsTrue(results.Any(x => x.Id == result1.ID.Guid));
            Assert.IsTrue(results.Any(x => x.Id == result2.ID.Guid));

        }

        [Test]
        public void MapToProperty_AbsoluteQueryWithQueryContext_ReturnsResults()
        {
            //Assign
            //Assign
            var config = new SitecoreQueryConfiguration();
            config.PropertyInfo = new FakePropertyInfo(typeof(IEnumerable<StubMapped>));
            config.Query = "/sitecore/content/Tests/DataMappers/SitecoreQueryMapper/Results/*";
            config.IsRelative = false;
            config.UseQueryContext = true;

            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var mapper = new SitecoreQueryMapper(null);
            mapper.Setup(new DataMapperResolverArgs(context, config));

            var source = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreQueryMapper/Source");
            var service = new SitecoreService(Database, context);

            var result1 = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreQueryMapper/Results/Result1");
            var result2 = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreQueryMapper/Results/Result2");

            //Act
            var results =
                mapper.MapToProperty(new SitecoreDataMappingContext(null, source, service)) as IEnumerable<StubMapped>;

            //Assert
            Assert.AreEqual(2, results.Count());
            Assert.IsTrue(results.Any(x => x.Id == result1.ID.Guid));
            Assert.IsTrue(results.Any(x => x.Id == result2.ID.Guid));

        }

        [Test]
        public void MapToProperty_AbsoluteQueryWithParameter_ReturnsResults()
        {
            //Assign
            //Assign
            var config = new SitecoreQueryConfiguration();
            config.PropertyInfo = new FakePropertyInfo(typeof(IEnumerable<StubMapped>));
            config.Query = "{path}/../Results/*";
            config.IsRelative = false;
            config.UseQueryContext = true;

            var context = Context.Create(new GlassConfig());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));

            var mapper = new SitecoreQueryMapper(null);
            mapper.Setup(new DataMapperResolverArgs(context, config));

            var source = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreQueryMapper/Source");
            var service = new SitecoreService(Database, context);

            var result1 = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreQueryMapper/Results/Result1");
            var result2 = Database.GetItem("/sitecore/content/Tests/DataMappers/SitecoreQueryMapper/Results/Result2");

            //Act
            var results =
                mapper.MapToProperty(new SitecoreDataMappingContext(null, source, service)) as IEnumerable<StubMapped>;

            //Assert
            Assert.AreEqual(2, results.Count());
            Assert.IsTrue(results.Any(x => x.Id == result1.ID.Guid));
            Assert.IsTrue(results.Any(x => x.Id == result2.ID.Guid));

        }

        #endregion

        #region Stubs

        [SitecoreType]
        public class StubMapped
        {
            [SitecoreId]
            public virtual Guid Id { get; set; }
        }

        public class StubNotMapped { }
        
        #endregion
    }
}
