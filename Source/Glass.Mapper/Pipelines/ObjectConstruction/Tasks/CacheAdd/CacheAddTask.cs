using System;
using System.Net;
using Glass.Mapper.Caching;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CacheAdd
{
    public class CacheAddTask : IObjectConstructionTask
    {
        protected  ICacheManager CacheManager
        {
            get; private set;
        }

        protected  ICacheKeyGenerator CacheKeyGenerator
        {
            get; private set;
        }

        public string Name { get { return "CacheAddTask"; } }

        public CacheAddTask(ICacheManager cacheManager, ICacheKeyGenerator cacheKeyGenerator)
        {
            CacheManager = cacheManager;
            CacheKeyGenerator = cacheKeyGenerator;
        }

        public virtual void Execute(ObjectConstructionArgs args)
        {
            if (args.Result != null 
                && args.Configuration.Cachable 
                && DisableCache.Current == CacheSetting.Enabled
                && args.AbstractTypeCreationContext.CacheEnabled
                )
            {
                DisableLazyLoad.Pop(args.Parameters);

                var key = CacheKeyGenerator.Generate(args);
                // This will also OVERRIDE any existing item that may already be cached (to be consistent across different cache impls)
                // Will allow for multiple threads to update the cached object on first load, when they are all racing to cache the item for the first time
                CacheManager.AddOrUpdate(key, args.Result);
            }


        }


    }
}
