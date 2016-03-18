using Glass.Mapper.Caching;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CacheAdd;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CacheCheck
{
    public class CacheCheckTask : IObjectConstructionTask
    {
        private readonly ICacheManager _cacheManager;

        public CacheCheckTask(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public virtual void Execute(ObjectConstructionArgs args)
        {
            if (args.Result == null 
                && args.Configuration.Cachable 
                && DisableCache.Current == CacheSetting.Enabled
                && args.AbstractTypeCreationContext.CacheEnabled
                )
            {
                var key = GetKey(args);

                var cacheItem = _cacheManager.Get<object>(key);
                if (cacheItem != null)
                {
                    args.Result = cacheItem;
                    args.AbortPipeline();
                }
            }
        }

        public virtual string GetKey(ObjectConstructionArgs args)
        {
            return CacheAddTask.CreateCacheKey(args);
        }

    }
}
