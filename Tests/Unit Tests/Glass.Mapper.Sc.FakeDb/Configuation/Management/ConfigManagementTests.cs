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
            QueryParameterConfigFactory queryParameterFactory = new QueryParameterConfigFactory();
            queryParameterFactory.Finalise();


            // Act
            var result = queryParameterFactory.GetItems();

            // Assert
            Assert.AreEqual(5, result.Count());
        }

        [Test]
        public void GetConfigurationResolvers()
        {
            // Assign
            ConfigurationResolverConfigFactory queryParameterFactory = new ConfigurationResolverConfigFactory();
            queryParameterFactory.Finalise();

            // Act
            var result = queryParameterFactory.GetItems();

            // Assert
            Assert.AreEqual(5, result.Count());
        }

        [Test]
        public void GetDataMappers()
        {
            // Assign
            QueryParameterConfigFactory queryParameterFactory = new QueryParameterConfigFactory();
            DataMapperConfigFactory configFactory = new DataMapperConfigFactory(queryParameterFactory);
            configFactory.Finalise();
            queryParameterFactory.Finalise();

            // Act
            var result = configFactory.GetItems();

            // Assert
            Assert.AreEqual(42, result.Count());
        }

        [Test]
        public void GetDataMapperResolverTasks()
        {
            // Assign
            DataMapperTaskConfigFactory dataMapperResolverConfigFactory = new DataMapperTaskConfigFactory();

            dataMapperResolverConfigFactory.Finalise();

            // Act
            var result = dataMapperResolverConfigFactory.GetItems();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void GetObjectConstructionTasks()
        {
            // Assign
            DependencyResolver dependencyResolver = new DependencyResolver(new Config());

            ObjectConstructionTaskConfigFactory dataMapperResolverConfigFactory = new ObjectConstructionTaskConfigFactory(dependencyResolver);
            dataMapperResolverConfigFactory.Finalise();

            // Act
            var result = dataMapperResolverConfigFactory.GetItems();

            // Assert
            Assert.AreEqual(8, result.Count());
        }

        [Test]
        public void GetObjectSavingTasks()
        {
            // Assign
            ObjectSavingTaskConfigFactory objectSavingTaskConfigFactory = new ObjectSavingTaskConfigFactory();
            objectSavingTaskConfigFactory.Finalise();

            // Act
            var result = objectSavingTaskConfigFactory.GetItems();

            // Assert
            Assert.AreEqual(1, result.Count());
        }
    }



}
