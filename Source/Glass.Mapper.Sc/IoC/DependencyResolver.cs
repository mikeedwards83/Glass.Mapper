using Glass.Mapper.Caching;
using Glass.Mapper.Diagnostics;

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
            DataMapperResolverFactory = new DataMapperTaskConfigFactory();
            DataMapperFactory = new DataMapperConfigFactory(QueryParameterFactory);
            ConfigurationResolverFactory = new ConfigurationResolverConfigFactory();
            ObjectConstructionFactory = new ObjectConstructionTaskConfigFactory(this);
            ObjectSavingFactory = new ObjectSavingTaskConfigFactory();
            ConfigurationMapFactory = new ConfigurationMapConfigFactory();
            GlassHtmlFactory = new GlassHtmlFactory();
            ItemVersionHandler = new ItemVersionHandler(config);
        }

        public override Mapper.Config GetConfig()
        {
            return Config;
        }

        public override ILog GetLog()
        {
            return Log;
        }

        const string _modelCounterKey = "71E98635-D8C9-4A36-913C-7DCFD2B7BD49";
        public override ModelCounter GetModelCounter()
        {
            ModelCounter counter;
            if (!ThreadData.Contains(_modelCounterKey))
            {
               ThreadData.SetValue(_modelCounterKey, new ModelCounter());
            }
            counter = ThreadData.GetValue<ModelCounter>(_modelCounterKey);
            return counter;
        }

        public override ICacheManager GetCacheManager()
        {
            return CacheManager();
        }
    }
}
