using Glass.Mapper.Caching;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CacheCheck;

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
            if (args.Result != null && args.Configuration.Cachable)
            {
                var key = args.AbstractTypeCreationContext.GetUniqueKey();

                _cacheManager.Add(key, args.Result);
            }
        }
    }
}
