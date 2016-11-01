using Glass.Mapper.Caching;

namespace Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CacheCheck
{
    public class CacheCheckTask : IObjectConstructionTask
    {
        protected ICacheManager CacheManager
        {
            get; private set;
        }

        private string _lazyLoadStackKey = "FEEED83A-6894-482A-AEFE-15F4EE79F0A4";

        protected ICacheKeyGenerator CacheKeyGenerator
        {
            get; private set;
        }
        public string Name { get { return "CacheCheckTask"; } }

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

                DisableLazyLoad.Push();
            }
        }
    }
}
