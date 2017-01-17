using System.Linq;
using Glass.Mapper.IoC;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectSaving;
using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;
using Glass.Mapper.Sc.IoC;
using NUnit.Framework;
using IDependencyResolver = Glass.Mapper.Sc.IoC.IDependencyResolver;

namespace Glass.Mapper.Sc.FakeDb.Configuation.Management
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
            IConfigFactory<AbstractConfigurationResolverTask> queryParameterFactory = new ConfigurationResolverConfigFactory();

            // Act
            var result = queryParameterFactory.GetItems();

            // Assert
            Assert.AreEqual(5, result.Count());
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
            Assert.AreEqual(41, result.Count());
        }

        [Test]
        public void GetDataMapperResolverTasks()
        {
            // Assign
            IConfigFactory<AbstractDataMapperResolverTask> dataMapperResolverConfigFactory = new DataMapperTaskConfigFactory();

            // Act
            var result = dataMapperResolverConfigFactory.GetItems();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void GetObjectConstructionTasks()
        {
            // Assign
            IDependencyResolver dependencyResolver = new DependencyResolver(new Config());
            IConfigFactory<AbstractObjectConstructionTask> dataMapperResolverConfigFactory = new ObjectConstructionTaskConfigFactory(dependencyResolver);

            // Act
            var result = dataMapperResolverConfigFactory.GetItems();

            // Assert
            Assert.AreEqual(8, result.Count());
        }

        [Test]
        public void GetObjectSavingTasks()
        {
            // Assign
            IConfigFactory<AbstractObjectSavingTask> objectSavingTaskConfigFactory = new ObjectSavingTaskConfigFactory();

            // Act
            var result = objectSavingTaskConfigFactory.GetItems();

            // Assert
            Assert.AreEqual(1, result.Count());
        }
    }



}
