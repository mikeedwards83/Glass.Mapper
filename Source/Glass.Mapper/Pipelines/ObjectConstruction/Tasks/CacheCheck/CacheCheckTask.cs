using Glass.Mapper.Caching;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CacheCheck
{
    public class CacheCheckTask : IObjectConstructionTask
    {
        private readonly ICacheManager _cacheManager;
        private readonly ICacheKeyGenerator _cacheKeyGenerator;

        public CacheCheckTask(ICacheManager cacheManager, ICacheKeyGenerator cacheKeyGenerator)
        {
            _cacheManager = cacheManager;
            _cacheKeyGenerator = cacheKeyGenerator;
        }

        public virtual void Execute(ObjectConstructionArgs args)
        {
            if (args.Result == null 
                && args.Configuration.Cachable 
                && DisableCache.Current == CacheSetting.Enabled
                && args.AbstractTypeCreationContext.CacheEnabled
                )
            {
                var key = _cacheKeyGenerator.Generate(args);

                var cacheItem = _cacheManager.Get<object>(key);
                if (cacheItem != null)
                {
                    args.Result = cacheItem;
                    args.AbortPipeline();
                }
            }
        }
    }
}
