using System.Linq;
using Glass.Mapper.Caching;
using Glass.Mapper.IoC;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectSaving;
using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;
using Glass.Mapper.Sc.IoC;
using NSubstitute;
using NUnit.Framework;

namespace Glass.Mapper.Sc.Tests.Configuration.Management
{
    [TestFixture]
    public class ConfigManagementTests
    {
        [Test]
        public void GetQueryParameters()
        {
            // Assign
            IConfigFactory<ISitecoreQueryParameter> queryParameterFactory = new QueryParameterConfigFactory();

            // Act
            var result = queryParameterFactory.GetItems();

            // Assert
            Assert.AreEqual(5, result.Count());
        }

        [Test]
        public void GetConfigurationResolvers()
        {
            // Assign
            IConfigFactory<IConfigurationResolverTask> queryParameterFactory = new ConfigurationResolverConfigFactory();

            // Act
            var result = queryParameterFactory.GetItems();

            // Assert
            Assert.AreEqual(4, result.Count());
        }

        [Test]
        public void GetDataMappers()
        {
            // Assign
            IConfigFactory<ISitecoreQueryParameter> queryParameterFactory = new QueryParameterConfigFactory();
            IConfigFactory<AbstractDataMapper> configFactory = new DataMapperConfigFactory(queryParameterFactory);

            // Act
            var result = configFactory.GetItems();

            // Assert
            Assert.AreEqual(37, result.Count());
        }

        [Test]
        public void GetDataMapperResolverTasks()
        {
            // Assign
            IConfigFactory<IDataMapperResolverTask> dataMapperResolverConfigFactory = new DataMapperTaskConfigFactory();

            // Act
            var result = dataMapperResolverConfigFactory.GetItems();

            // Assert
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void GetObjectConstructionTasks()
        {
            // Assign
            
            IConfigFactory<IObjectConstructionTask> dataMapperResolverConfigFactory = new ObjectConstructionTaskConfigFactory(()=>new HttpCache());

            // Act
            var result = dataMapperResolverConfigFactory.GetItems();

            // Assert
            Assert.AreEqual(8, result.Count());
        }

        [Test]
        public void GetObjectSavingTasks()
        {
            // Assign
            IConfigFactory<IObjectSavingTask> objectSavingTaskConfigFactory = new ObjectSavingTaskConfigFactory();

            // Act
            var result = objectSavingTaskConfigFactory.GetItems();

            // Assert
            Assert.AreEqual(1, result.Count());
        }
    }



}
