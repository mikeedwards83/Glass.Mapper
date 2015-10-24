using Glass.Mapper.Caching;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CacheCheck
{
    public class CacheCheckTask : IObjectConstructionTask
    {
        private readonly ICacheManager _cacheManager;

        public CacheCheckTask(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public void Execute(ObjectConstructionArgs args)
        {
            if (args.Result == null 
                && args.Configuration.Cachable 
                && args.AbstractTypeCreationContext.CacheEnabled 
                && DisableCache.Current == CacheSetting.Enabled)
            {
                var key = args.Context.Name + args.AbstractTypeCreationContext.GetUniqueKey();

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
