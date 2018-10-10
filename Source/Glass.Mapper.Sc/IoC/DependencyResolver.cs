using Glass.Mapper.Caching;
using Glass.Mapper.Diagnostics;

namespace Glass.Mapper.Sc.IoC
{
    public class DependencyResolver : AbstractDependencyResolver
    {
        public DependencyResolver(Config config)
        {
            Config = config;
            LazyLoadingHelper = new LazyLoadingHelper();
            Log = new Log();
            CacheManager = () => new NetMemoryCacheManager();
            QueryParameterFactory = new QueryParameterConfigFactory(this);
            DataMapperResolverFactory = new DataMapperTaskConfigFactory(this);
            DataMapperFactory = new DataMapperConfigFactory(this);
            ConfigurationResolverFactory = new ConfigurationResolverConfigFactory(this);
            ObjectConstructionFactory = new ObjectConstructionTaskConfigFactory(this);
            ObjectSavingFactory = new ObjectSavingTaskConfigFactory(this);
            ConfigurationMapFactory = new ConfigurationMapConfigFactory(this);
            GlassHtmlFactory = new GlassHtmlFactory(this);
            LazyLoadingHelper = new LazyLoadingHelper();
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
