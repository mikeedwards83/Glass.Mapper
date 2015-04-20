using System;
using System.Collections.Generic;
using Glass.Mapper.Caching;
using Glass.Mapper.IoC;
using Glass.Mapper.Maps;
using Glass.Mapper.Pipelines.ConfigurationResolver;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectSaving;
using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;

namespace Glass.Mapper.Sc.IoC
{
    public class DependencyResolver : IDependencyResolver
    {
        public DependencyResolver(Config config)
        {
            Config = config;
            CacheManager = () => new HttpCache();
            QueryParameterFactory = new QueryParameterConfigFactory();
            DataMapperResolverFactory = new DataMapperTaskConfigFactory();
            DataMapperFactory = new DataMapperConfigFactory(QueryParameterFactory);
            ConfigurationResolverFactory = new ConfigurationResolverConfigFactory();
            ObjectConstructionFactory = new ObjectConstructionTaskConfigFactory(CacheManager);
            ObjectSavingFactory = new ObjectSavingTaskConfigFactory();
            ConfigurationMapFactory = new ConfigurationMapConfigFactory();
        }

        public Func<ICacheManager> CacheManager { get; set; }
        public IConfigFactory<IDataMapperResolverTask> DataMapperResolverFactory { get;  set; }
        public IConfigFactory<AbstractDataMapper> DataMapperFactory { get; set; }
        public IConfigFactory<IConfigurationResolverTask> ConfigurationResolverFactory { get; set; }
        public IConfigFactory<IObjectConstructionTask> ObjectConstructionFactory { get; set; }
        public IConfigFactory<IObjectSavingTask> ObjectSavingFactory { get; set; }
        public Mapper.Config Config { get; set; }
        public IConfigFactory<ISitecoreQueryParameter> QueryParameterFactory { get; set; }
        public IConfigFactory<IGlassMap> ConfigurationMapFactory { get; set; } 

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

        public IEnumerable<IGlassMap> GetConfigurationMaps()
        {
            return ConfigurationMapFactory.GetItems();
        }
    }
}
