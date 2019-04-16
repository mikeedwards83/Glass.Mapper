using Glass.Mapper.Caching;
using Glass.Mapper.Configuration;
using Glass.Mapper.Diagnostics;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CacheCheck
{
    public class CacheCheckTask : AbstractObjectConstructionTask
    {
        protected CacheFactory CacheFactory { get; private set; }

        protected ICacheKeyGenerator CacheKeyGenerator { get; private set; }

        public CacheCheckTask(CacheFactory cacheFactory, ICacheKeyGenerator cacheKeyGenerator)
        {
            CacheFactory = cacheFactory;
            CacheKeyGenerator = cacheKeyGenerator;
            Name = "CacheCheckTask";
        }

        public virtual string GetCacheName(ObjectConstructionArgs args)
        {
            return "Default";
        }

        public override void Execute(ObjectConstructionArgs args)
        {
            if (args.Result == null //if we already have a result don't do anything
                && args.Options.Cache.IsEnabled() //if the model has the cache explicitly  disabled
                && args.AbstractTypeCreationContext.CacheEnabled //
                && DisableCache.Current != Cache.Disabled //has the cache disabler been used? This is legacy and should go.
            )
            {
                var cachename = GetCacheName(args);
                var cacheManager = CacheFactory.GetCache(cachename);

                var key = CacheKeyGenerator.Generate(args);

                var cacheItem = cacheManager.Get<object>(key);
                if (cacheItem != null)
                {
                    args.Result = cacheItem;
                    ModelCounter.Instance.CachedModels++;
                }
                else
                {
                    base.Execute(args);
                }

                cacheManager.AddOrUpdate(key, args.Result);
            }
            else
            {
                base.Execute(args);
            }
        }
    }
}
