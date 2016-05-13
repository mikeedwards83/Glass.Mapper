using Glass.Mapper.Caching;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Pipelines.DataMapperResolver.Tasks;

namespace Glass.Mapper.Sc.IoC
{
    public class DependencyResolver : AbstractDependencyResolver
    {
        public DependencyResolver(Config config)
        {
            Config = config;
            Log = new Log();
            CacheManager = () => new NetMemoryCacheManager();
            QueryParameterFactory = new QueryParameterConfigFactory();
            DataMapperResolver = new DataMapperResolver();
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

        public override ILog GetLog()
        {
            return Log;
        }

        public override ICacheManager GetCacheManager()
        {
            return CacheManager();
        }
    }
}
