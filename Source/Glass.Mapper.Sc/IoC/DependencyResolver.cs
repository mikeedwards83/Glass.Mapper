using System;
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
    public class DependencyResolver : AbstractDataResolver
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

        public override Mapper.Config GetConfig()
        {
            return Config;
        }

        public override ICacheManager GetCacheManager()
        {
            return CacheManager();
        }
    }
}
