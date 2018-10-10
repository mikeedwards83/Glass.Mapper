using Glass.Mapper.Caching;
using Glass.Mapper.Configuration;
using Glass.Mapper.Diagnostics;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CacheCheck
{
    public class CacheCheckTask : AbstractObjectConstructionTask
    {
        protected ICacheManager CacheManager { get; private set; }

        protected ICacheKeyGenerator CacheKeyGenerator { get; private set; }

        public CacheCheckTask(ICacheManager cacheManager, ICacheKeyGenerator cacheKeyGenerator)
        {
            CacheManager = cacheManager;
            CacheKeyGenerator = cacheKeyGenerator;
            Name = "CacheCheckTask";
        }

        public override void Execute(ObjectConstructionArgs args)
        {
            if (args.Result == null
                && args.AbstractTypeCreationContext.Options.Cache.IsEnabled()
                && DisableCache.Current != Cache.Disabled
                && args.AbstractTypeCreationContext.CacheEnabled
            )
            {
                var key = CacheKeyGenerator.Generate(args);

                var cacheItem = CacheManager.Get<object>(key);
                if (cacheItem != null)
                {
                    args.Result = cacheItem;
                    ModelCounter.Instance.CachedModels++;
                }
                else
                {
                    base.Execute(args);
                }

                CacheManager.AddOrUpdate(key, args.Result);
            }
            else
            {
                base.Execute(args);
            }
        }
    }
}
