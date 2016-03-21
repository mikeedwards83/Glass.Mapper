using Glass.Mapper.Caching;

namespace Glass.Mapper.Sc.IoC
{
    public class DependencyResolver : AbstractDependencyResolver
    {
        public DependencyResolver(Config config)
        {
            Config = config;
            CacheManager = () => new NetMemoryCacheManager();
            QueryParameterFactory = new QueryParameterConfigFactory();
            DataMapperResolverFactory = new DataMapperTaskConfigFactory();
            DataMapperFactory = new DataMapperConfigFactory(QueryParameterFactory);
            ConfigurationResolverFactory = new ConfigurationResolverConfigFactory();
            ObjectConstructionFactory = new ObjectConstructionTaskConfigFactory(this);
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
