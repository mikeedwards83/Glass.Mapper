using System;
using Glass.Mapper.Caching;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CacheAdd
{
    public class CacheAddTask : IObjectConstructionTask
    {
        private readonly ICacheManager _cacheManager;

        public CacheAddTask(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public void Execute(ObjectConstructionArgs args)
        {
            if (args.Result != null 
                && args.Configuration.Cachable 
                && args.AbstractTypeCreationContext.CacheEnabled 
                && DisableCache.Current == CacheSetting.Enabled)
            {
                var key = args.Context.Name + args.AbstractTypeCreationContext.GetUniqueKey();

                // This will also OVERRIDE any existing item that may already be cached (to be consistent across different cache impls)
                // Will allow for multiple threads to update the cached object on first load, when they are all racing to cache the item for the first time
                _cacheManager.AddOrUpdate(key, args.Result);
            }
        }
    }
}
