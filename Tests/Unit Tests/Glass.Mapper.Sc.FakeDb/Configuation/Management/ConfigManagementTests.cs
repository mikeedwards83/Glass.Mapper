using System.Linq;
using Glass.Mapper.IoC;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectSaving;
using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;
using Glass.Mapper.Sc.IoC;
using NSubstitute;
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
            IConfigFactory<ISitecoreQueryParameter> queryParameterFactory = new QueryParameterConfigFactory(null);

            // Act
            var result = queryParameterFactory.GetItems();

            // Assert
            Assert.AreEqual(5, result.Count());
        }

        [Test]
        public void GetConfigurationResolvers()
        {
            // Assign
            IConfigFactory<AbstractConfigurationResolverTask> queryParameterFactory = new ConfigurationResolverConfigFactory(null);

            // Act
            var result = queryParameterFactory.GetItems();

            // Assert
            Assert.AreEqual(5, result.Count());
        }

        [Test]
        public void GetDataMappers()
        {
            // Assign
            IDependencyResolver dependencyResolver = Substitute.For<IDependencyResolver>();
            IConfigFactory<ISitecoreQueryParameter> queryParameterFactory = new QueryParameterConfigFactory(null);
            dependencyResolver.QueryParameterFactory.Returns(queryParameterFactory);

            IConfigFactory<AbstractDataMapper> configFactory = new DataMapperConfigFactory(dependencyResolver);

            // Act
            var result = configFactory.GetItems();

            // Assert
            Assert.AreEqual(42, result.Count());
        }

        [Test]
        public void GetDataMapperResolverTasks()
        {
            // Assign
            IConfigFactory<AbstractDataMapperResolverTask> dataMapperResolverConfigFactory = new DataMapperTaskConfigFactory(null);

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
            IConfigFactory<AbstractObjectSavingTask> objectSavingTaskConfigFactory = new ObjectSavingTaskConfigFactory(null);

            // Act
            var result = objectSavingTaskConfigFactory.GetItems();

            // Assert
            Assert.AreEqual(1, result.Count());
        }
    }



}
