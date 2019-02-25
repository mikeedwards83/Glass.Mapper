using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.Caching
{
    public class CacheFactory
    {
        private ConcurrentDictionary<string, ICacheManager> _caches = new ConcurrentDictionary<string, ICacheManager>();

        public CacheFactory(Func<ICacheManager> createCacheManager)
        {
            CreateCacheManager = createCacheManager;
        }

        public Func<ICacheManager> CreateCacheManager { get; }

        public ICacheManager GetCache(string name)
        {
            var cache = _caches.GetOrAdd(name, CreateCacheManager());
            return cache;
        }

        public void Clear(string name)
        {
            ICacheManager cacheManager = null;
            if(_caches.TryRemove(name, out cacheManager))
            {
                cacheManager.ClearCache();
            }
        }
        public void ClearAll()
        {
            var keys = _caches.Keys;
            foreach (var key in keys)
            {
                Clear(key);
            }
        }
    }
}
