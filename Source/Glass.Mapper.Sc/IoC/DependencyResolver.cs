using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Caching;
using Glass.Mapper.IoC;
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

namespace Glass.Mapper.Sc.IoC
{
    public class DependencyResolver : IDependencyResolver
    {

        public DependencyResolver(Config config)
        {
            Config = config;
            CacheManager = ()=>new HttpCache();
            QueryParameterFactory = new QueryParameterConfigFactory();
            DataMapperResolverFactory = new DataMapperTaskConfigFactory();
            DataMapperFactory = new DataMapperConfigFactory(QueryParameterFactory);
            ConfigurationResolverFactory = new ConfigurationResolverConfigFactory();
            ObjectConstructionFactory = new ObjectConstructionTaskConfigFactory(CacheManager);
            ObjectSavingFactory = new ObjectSavingTaskConfigFactory();
        }

        public Func<ICacheManager> CacheManager { get; set; }
        public AbstractConfigFactory<IDataMapperResolverTask> DataMapperResolverFactory { get;  set; }
        public AbstractConfigFactory<AbstractDataMapper> DataMapperFactory { get;  set; }
        public AbstractConfigFactory<IConfigurationResolverTask> ConfigurationResolverFactory { get;  set; }
        public AbstractConfigFactory<IObjectConstructionTask> ObjectConstructionFactory { get;  set; }
        public AbstractConfigFactory<IObjectSavingTask> ObjectSavingFactory { get;  set; }
        public Glass.Mapper.Config Config { get; set; }
        public AbstractConfigFactory<ISitecoreQueryParameter> QueryParameterFactory { get; set; }


        public Mapper.Config GetConfig()
        {
            return Config;
        }

        public ICacheManager GetCacheManager()
        {
            return CacheManager();
        }

        public IEnumerable<IDataMapperResolverTask> GetDataMapperResolverTasks()
        {
            return DataMapperResolverFactory.GetItems();
        }

        public IEnumerable<AbstractDataMapper> GetDataMappers()
        {
            return DataMapperFactory.GetItems();
        }

        public IEnumerable<IConfigurationResolverTask> GetConfigurationResolverTasks()
        {
            return ConfigurationResolverFactory.GetItems();
        }

        public IEnumerable<IObjectConstructionTask> GetObjectConstructionTasks()
        {
            return ObjectConstructionFactory.GetItems();
        }

        public IEnumerable<IObjectSavingTask> GetObjectSavingTasks()
        {
            return ObjectSavingFactory.GetItems();
        }

        IEnumerable<ISitecoreQueryParameter> IDependencyResolver.QueryParameterFactory()
        {
            return QueryParameterFactory.GetItems();
        }
    }

    public class DataMapperTaskConfigFactory : AbstractConfigFactory<IDataMapperResolverTask>
    {
        protected override void AddTypes()
        {
            Add(() => new DataMapperStandardResolverTask());
        }
    }

    public class ObjectSavingTaskConfigFactory : AbstractConfigFactory<IObjectSavingTask>
    {
        protected override void AddTypes()
        {
            Add(() => new StandardSavingTask());
        }
    }

    public class ObjectConstructionTaskConfigFactory : AbstractConfigFactory<IObjectConstructionTask>
    {
        private readonly Func<ICacheManager> cacheManager;

        public ObjectConstructionTaskConfigFactory(Func<ICacheManager> cacheManager)
        {
            this.cacheManager = cacheManager;
        }

        protected override void AddTypes()
        {
            Add(() => new CreateDynamicTask());
            Add(() => new SitecoreItemTask());
            Add(() => new CacheCheckTask(cacheManager()));
            Add(() => new EnforcedTemplateCheck());
            Add(() => new CreateMultiInferaceTask());
            Add(() => new CreateConcreteTask());
            Add(() => new CreateInterfaceTask());
            Add(() => new CacheAddTask(cacheManager()));
        }
    }

    public class QueryParameterConfigFactory : AbstractConfigFactory<ISitecoreQueryParameter>
    {
        protected override void AddTypes()
        {
            Add(() => new ItemDateNowParameter());
            Add(() => new ItemEscapedPathParameter());
            Add(() => new ItemIdNoBracketsParameter());
            Add(() => new ItemIdParameter());
            Add(() => new ItemPathParameter());
        }
    }

    public class ConfigurationResolverConfigFactory : AbstractConfigFactory<IConfigurationResolverTask>
    {
        protected override void AddTypes()
        {
            Add(() => new SitecoreItemResolverTask());
            Add(() => new MultiInterfaceResolverTask());
            Add(() => new ConfigurationStandardResolverTask());
            Add(() => new ConfigurationOnDemandResolverTask<SitecoreTypeConfiguration>());
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
            Add(() => new SitecoreIgnoreMapper());
            Add(() => new SitecoreChildrenCastMapper());
            Add(() => new SitecoreChildrenMapper());
            Add(() => new SitecoreFieldBooleanMapper());
            Add(() => new SitecoreFieldDateTimeMapper());
            Add(() => new SitecoreFieldDecimalMapper());
            Add(() => new SitecoreFieldDoubleMapper());
            Add(() => new SitecoreFieldEnumMapper());
            Add(() => new SitecoreFieldFileMapper());
            Add(() => new SitecoreFieldFloatMapper());
            Add(() => new SitecoreFieldGuidMapper());
            Add(() => new SitecoreFieldHtmlEncodingMapper());
            Add(() => new SitecoreFieldIEnumerableMapper());
            Add(() => new SitecoreFieldImageMapper());
            Add(() => new SitecoreFieldIntegerMapper());
            Add(() => new SitecoreFieldLinkMapper());
            Add(() => new SitecoreFieldLongMapper());
            Add(() => new SitecoreFieldNameValueCollectionMapper());
            Add(() => new SitecoreFieldDictionaryMapper());
            Add(() => new SitecoreFieldNullableDateTimeMapper());
            Add(() => new SitecoreFieldNullableDoubleMapper());
            Add(() => new SitecoreFieldNullableDecimalMapper());
            Add(() => new SitecoreFieldNullableFloatMapper());
            Add(() => new SitecoreFieldNullableGuidMapper());
            Add(() => new SitecoreFieldNullableIntMapper());
            Add(() => new SitecoreFieldRulesMapper());
            Add(() => new SitecoreFieldStreamMapper());
            Add(() => new SitecoreFieldStringMapper());
            Add(() => new SitecoreFieldTypeMapper());
            Add(() => new SitecoreIdMapper());
            Add(() => new SitecoreItemMapper());
            Add(() => new SitecoreInfoMapper());
            Add(() => new SitecoreNodeMapper());
            Add(() => new SitecoreLinkedMapper());
            Add(() => new SitecoreParentMapper());
            Add(() => new SitecoreDelegateMapper());
            Add(() => new SitecoreQueryMapper(queryParameterFactory.GetItems()));
        }
    }

}
