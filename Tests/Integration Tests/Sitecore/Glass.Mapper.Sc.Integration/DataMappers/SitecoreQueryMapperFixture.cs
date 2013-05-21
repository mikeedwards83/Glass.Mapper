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
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.CastleWindsor;
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
            config.PropertyInfo = typeof (StubClass).GetProperty("StubMappeds");


            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));


            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanHandle_CorrectConfigIEnumerableNotMappedClass_ReturnsTrueOnDemand()
        {
            //Assign
            var mapper = new SitecoreQueryMapper(null);
            var config = new SitecoreQueryConfiguration();
            config.PropertyInfo = typeof (StubClass).GetProperty("StubNotMappeds");

            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));


            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanHandle_CorrectConfigNotMappedClass_ReturnsTrueOnDemand()
        {
            //Assign
            var mapper = new SitecoreQueryMapper(null);
            var config = new SitecoreQueryConfiguration();
            config.PropertyInfo = typeof (StubClass).GetProperty("StubNotMapped");

            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));


            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CanHandle_CorrectConfigMappedClass_ReturnsFalse()
        {
            //Assign
            var mapper = new SitecoreQueryMapper(null);
            var config = new SitecoreQueryConfiguration();
            config.PropertyInfo = typeof (StubClass).GetProperty("StubMapped");

            var context = Context.Create(Utilities.CreateStandardResolver());
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
            config.PropertyInfo = typeof (StubClass).GetProperty("StubMapped");

            var context = Context.Create(Utilities.CreateStandardResolver());
            context.Load(new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Integration"));


            //Act
            var result = mapper.CanHandle(config, context);

            //Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region Method - MapToProperty

        [Test]
        public void MapToProperty_RelativeQuery_ReturnsNoResults()
        {
            //Assign
            //Assign
            var config = new SitecoreQueryConfiguration();
            config.PropertyInfo = typeof (StubClass).GetProperty("StubMappeds");

            config.Query = "../Results/DoesNotExist/*";
            config.IsRelative = true;

            var context = Context.Create(Utilities.CreateStandardResolver());
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
            Assert.AreEqual(0, results.Count());
        }

        [Test]
        public void MapToProperty_RelativeQuery_ReturnsResults()
        {
            //Assign
            //Assign
            var config = new SitecoreQueryConfiguration();
            config.PropertyInfo = typeof (StubClass).GetProperty("StubMappeds");
            config.Query = "../Results/*";
            config.IsRelative = true;

            var context = Context.Create(Utilities.CreateStandardResolver());
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
            config.PropertyInfo = typeof (StubClass).GetProperty("StubMappeds");

            config.Query = "/sitecore/content/Tests/DataMappers/SitecoreQueryMapper/Results/*";
            config.IsRelative = false;

            var context = Context.Create(Utilities.CreateStandardResolver());
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
            config.PropertyInfo = typeof (StubClass).GetProperty("StubMapped");
            config.Query = "../Results/Result1";
            config.IsRelative = true;

            var context = Context.Create(Utilities.CreateStandardResolver());
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
            config.PropertyInfo = typeof (StubClass).GetProperty("StubMapped");
            config.Query = "/sitecore/content/Tests/DataMappers/SitecoreQueryMapper/Results/Result1";
            config.IsRelative = false;

            var context = Context.Create(Utilities.CreateStandardResolver());
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
            config.PropertyInfo = typeof(StubClass).GetProperty("StubMappeds");
            config.Query = "../Results/*";
            config.IsRelative = true;
            config.UseQueryContext = true;

            var context = Context.Create(Utilities.CreateStandardResolver());
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
            config.PropertyInfo = typeof (StubClass).GetProperty("StubMappeds");

            config.Query = "/sitecore/content/Tests/DataMappers/SitecoreQueryMapper/Results/*";
            config.IsRelative = false;
            config.UseQueryContext = true;

            var context = Context.Create(Utilities.CreateStandardResolver());
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
            config.PropertyInfo = typeof (StubClass).GetProperty("StubMappeds");
            config.Query = "{path}/../Results/*";
            config.IsRelative = false;
            config.UseQueryContext = true;

            var context = Context.Create(Utilities.CreateStandardResolver());
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

        public class StubClass
        {
            public IEnumerable<StubMapped> StubMappeds { get; set; }

            public IEnumerable<StubNotMapped> StubNotMappeds { get; set; }

            public StubMapped StubMapped { get; set; }
            public StubNotMapped StubNotMapped { get; set; }
        }
        
        #endregion
    }
}




