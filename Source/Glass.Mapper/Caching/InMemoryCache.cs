using System.Collections.Concurrent;
using System.Runtime.Caching;

namespace Glass.Mapper.Caching
{
    public class InMemoryCache : ICacheManager
    {
        private const string GlassCacheName = "Glass";
        private static MemoryCache cache = new MemoryCache(GlassCacheName);
        private readonly CacheItemPolicy policy;
        private readonly ConcurrentDictionary<string, object> _innerCache = new ConcurrentDictionary<string, object>();

        public InMemoryCache(CacheItemPolicy policy)
        {
            this.policy = policy;
        }

        public object this[string key]
        {
            get
            {
                return Get<object>(key);
            }
            set
            {
                AddOrUpdate(key, value);
            }
        }

        public void ClearCache()
        {
            cache.Dispose();
            cache = new MemoryCache(GlassCacheName);
        }

        public void AddOrUpdate(string key, object value)
        {
            cache.Add(key, value, policy);
        }

        public object Get(string key)
        {
            if (!Contains(key))
            {
                return null;
            }

            return cache.Get(key);
        }

        public T Get<T>(string key) where T : class
        {
            return Get(key) as T;
        }

        public bool Contains(string key)
        {
            return cache.Contains(key);
        }
    }
}
