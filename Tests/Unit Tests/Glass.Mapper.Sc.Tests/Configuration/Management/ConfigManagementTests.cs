using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Caching;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.MultiInterfaceResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.OnDemandResolver;
using Glass.Mapper.Pipelines.ConfigurationResolver.Tasks.StandardResolver;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.DataMapperResolver.Tasks;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CacheAdd;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CacheCheck;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateConcrete;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateInterface;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CreateMultiInterface;
using Glass.Mapper.Pipelines.ObjectSaving;
using Glass.Mapper.Pipelines.ObjectSaving.Tasks;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.DataMappers;
using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;
using Glass.Mapper.Sc.Pipelines.ConfigurationResolver;
using Glass.Mapper.Sc.Pipelines.ObjectConstruction;
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
            ICacheManager cacheManager = Substitute.For<ICacheManager>();
            IConfigFactory<IObjectConstructionTask> dataMapperResolverConfigFactory = new ObjectConstructionTaskConfigFactory(cacheManager);

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

    public class DataMapperTaskConfigFactory : AbstractConfigFactory<IDataMapperResolverTask>
    {
        protected override void AddTypes()
        {
            TypeGenerators.Add(() => new DataMapperStandardResolverTask());
        }
    }

    public class ObjectSavingTaskConfigFactory : AbstractConfigFactory<IObjectSavingTask>
    {
        protected override void AddTypes()
        {
            TypeGenerators.Add(() => new StandardSavingTask());
        }
    }

    public class ObjectConstructionTaskConfigFactory : AbstractConfigFactory<IObjectConstructionTask>
    {
        private readonly ICacheManager cacheManager;

        public ObjectConstructionTaskConfigFactory(ICacheManager cacheManager)
        {
            this.cacheManager = cacheManager;
        }

        protected override void AddTypes()
        {
            TypeGenerators.Add(() => new CreateDynamicTask());
            TypeGenerators.Add(() => new SitecoreItemTask());
            TypeGenerators.Add(() => new CacheCheckTask(cacheManager));
            TypeGenerators.Add(() => new EnforcedTemplateCheck());
            TypeGenerators.Add(() => new CreateMultiInferaceTask());
            TypeGenerators.Add(() => new CreateConcreteTask());
            TypeGenerators.Add(() => new CreateInterfaceTask());
            TypeGenerators.Add(() => new CacheAddTask(cacheManager));
        }
    }

    public class QueryParameterConfigFactory : AbstractConfigFactory<ISitecoreQueryParameter>
    {
        protected override void AddTypes()
        {
            TypeGenerators.Add(() => new ItemDateNowParameter());
            TypeGenerators.Add(() => new ItemEscapedPathParameter());
            TypeGenerators.Add(() => new ItemIdNoBracketsParameter());
            TypeGenerators.Add(() => new ItemIdParameter());
            TypeGenerators.Add(() => new ItemPathParameter());
        }
    }

    public class ConfigurationResolverConfigFactory : AbstractConfigFactory<IConfigurationResolverTask>
    {
        protected override void AddTypes()
        {
            TypeGenerators.Add(() => new SitecoreItemResolverTask());
            TypeGenerators.Add(() => new MultiInterfaceResolverTask());
            TypeGenerators.Add(() => new ConfigurationStandardResolverTask());
            TypeGenerators.Add(() => new ConfigurationOnDemandResolverTask<SitecoreTypeConfiguration>());
        }
    }

    public class DataMapperConfigFactory : AbstractConfigFactory<AbstractDataMapper>
    {
        private readonly IConfigFactory<ISitecoreQueryParameter> queryParameterFactory;

        public DataMapperConfigFactory(IConfigFactory<ISitecoreQueryParameter> queryParameterFactory)
        {
            this.queryParameterFactory = queryParameterFactory;
        }

        protected override void AddTypes()
        {
            TypeGenerators.Add(() => new SitecoreIgnoreMapper());
            TypeGenerators.Add(() => new SitecoreChildrenCastMapper());
            TypeGenerators.Add(() => new SitecoreChildrenMapper());
            TypeGenerators.Add(() => new SitecoreFieldBooleanMapper());
            TypeGenerators.Add(() => new SitecoreFieldDateTimeMapper());
            TypeGenerators.Add(() => new SitecoreFieldDecimalMapper());
            TypeGenerators.Add(() => new SitecoreFieldDoubleMapper());
            TypeGenerators.Add(() => new SitecoreFieldEnumMapper());
            TypeGenerators.Add(() => new SitecoreFieldFileMapper());
            TypeGenerators.Add(() => new SitecoreFieldFloatMapper());
            TypeGenerators.Add(() => new SitecoreFieldGuidMapper());
            TypeGenerators.Add(() => new SitecoreFieldHtmlEncodingMapper());
            TypeGenerators.Add(() => new SitecoreFieldIEnumerableMapper());
            TypeGenerators.Add(() => new SitecoreFieldImageMapper());
            TypeGenerators.Add(() => new SitecoreFieldIntegerMapper());
            TypeGenerators.Add(() => new SitecoreFieldLinkMapper());
            TypeGenerators.Add(() => new SitecoreFieldLongMapper());
            TypeGenerators.Add(() => new SitecoreFieldNameValueCollectionMapper());
            TypeGenerators.Add(() => new SitecoreFieldDictionaryMapper());
            TypeGenerators.Add(() => new SitecoreFieldNullableDateTimeMapper());
            TypeGenerators.Add(() => new SitecoreFieldNullableDoubleMapper());
            TypeGenerators.Add(() => new SitecoreFieldNullableDecimalMapper());
            TypeGenerators.Add(() => new SitecoreFieldNullableFloatMapper());
            TypeGenerators.Add(() => new SitecoreFieldNullableGuidMapper());
            TypeGenerators.Add(() => new SitecoreFieldNullableIntMapper());
            TypeGenerators.Add(() => new SitecoreFieldRulesMapper());
            TypeGenerators.Add(() => new SitecoreFieldStreamMapper());
            TypeGenerators.Add(() => new SitecoreFieldStringMapper());
            TypeGenerators.Add(() => new SitecoreFieldTypeMapper());
            TypeGenerators.Add(() => new SitecoreIdMapper());
            TypeGenerators.Add(() => new SitecoreItemMapper());
            TypeGenerators.Add(() => new SitecoreInfoMapper());
            TypeGenerators.Add(() => new SitecoreNodeMapper());
            TypeGenerators.Add(() => new SitecoreLinkedMapper());
            TypeGenerators.Add(() => new SitecoreParentMapper());
            TypeGenerators.Add(() => new SitecoreDelegateMapper());
            TypeGenerators.Add(() => new SitecoreQueryMapper(queryParameterFactory.GetItems()));
        }
    }

    public abstract class AbstractConfigFactory<T> : IConfigFactory<T>
    {
        protected List<Func<T>> TypeGenerators { get; private set; }

        protected AbstractConfigFactory()
        {
            TypeGenerators = new List<Func<T>>();
        }

        protected abstract void AddTypes();

        public virtual IEnumerable<T> GetItems()
        {
            if (TypeGenerators.Count == 0)
            {
                AddTypes();
            }

            return TypeGenerators != null
                ? TypeGenerators.Select(f => f())
                : null;
        }
    }

    public interface IConfigFactory<out T>
    {
        IEnumerable<T> GetItems();
    }
}
