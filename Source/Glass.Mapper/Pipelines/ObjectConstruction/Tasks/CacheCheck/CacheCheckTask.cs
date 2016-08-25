using Glass.Mapper.Caching;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CacheCheck
{
    public class CacheCheckTask : IObjectConstructionTask
    {
        protected ICacheManager CacheManager
        {
            get; private set;
        }

        protected ICacheKeyGenerator CacheKeyGenerator
        {
            get; private set;
        }

        public CacheCheckTask(ICacheManager cacheManager, ICacheKeyGenerator cacheKeyGenerator)
        {
            CacheManager = cacheManager;
            CacheKeyGenerator = cacheKeyGenerator;
        }

        public virtual void Execute(ObjectConstructionArgs args)
        {
            if (args.Result == null 
                && args.Configuration.Cachable 
                && DisableCache.Current == CacheSetting.Enabled
                && args.AbstractTypeCreationContext.CacheEnabled
                )
            {
                var key = CacheKeyGenerator.Generate(args);

                var cacheItem = CacheManager.Get<object>(key);
                if (cacheItem != null)
                {
                    args.Result = cacheItem;
                    args.AbortPipeline();
                }
            }
        }
    }
}
