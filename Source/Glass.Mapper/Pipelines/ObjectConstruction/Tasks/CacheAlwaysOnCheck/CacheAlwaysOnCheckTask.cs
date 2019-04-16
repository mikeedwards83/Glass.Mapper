using Glass.Mapper.Caching;
using Glass.Mapper.Configuration;
using Glass.Mapper.Diagnostics;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CacheCheck
{
    public class CacheAlwaysOnCheckTask : AbstractObjectConstructionTask
    {
        protected CacheFactory CacheFactory { get; private set; }

        protected ICacheKeyGenerator CacheKeyGenerator { get; private set; }

        public CacheAlwaysOnCheckTask(CacheFactory cacheFactory, ICacheKeyGenerator cacheKeyGenerator)
        {
                     CacheFactory  = cacheFactory;
            CacheKeyGenerator = cacheKeyGenerator;
            Name = "CacheAlwaysOnCheckTask";
        }

        public virtual string GetCacheName(ObjectConstructionArgs args)
        {
            return "Default";
        }
        public override void Execute(ObjectConstructionArgs args)
        {
            if (args.Result == null 
                && args.Options.Cache !=  Cache.Disabled)
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
