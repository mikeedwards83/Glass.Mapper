using System;
using System.Net;
using Glass.Mapper.Caching;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CacheAdd
{
    public class CacheAddTask : IObjectConstructionTask
    {
        private readonly ICacheManager _cacheManager;
        private readonly ICacheKeyGenerator _cacheKeyGenerator;

        public CacheAddTask(ICacheManager cacheManager, ICacheKeyGenerator cacheKeyGenerator)
        {
            _cacheManager = cacheManager;
            _cacheKeyGenerator = cacheKeyGenerator;
        }

        public virtual void Execute(ObjectConstructionArgs args)
        {
            if (args.Result != null 
                && args.Configuration.Cachable 
                && DisableCache.Current == CacheSetting.Enabled
                && args.AbstractTypeCreationContext.CacheEnabled
                )
            {
                var key = _cacheKeyGenerator.Generate(args);
                // This will also OVERRIDE any existing item that may already be cached (to be consistent across different cache impls)
                // Will allow for multiple threads to update the cached object on first load, when they are all racing to cache the item for the first time
                _cacheManager.AddOrUpdate(key, args.Result);
            }
        }

      
    }
}
