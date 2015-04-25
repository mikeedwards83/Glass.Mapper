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

        public void AddOrUpdate<T>(string key, T value) where T : class
        {
            cache.Add(key, value, policy);
        }

        public T Get<T>(string key) where T : class
        {
            if (!Contains(key))
            {
                return default(T);
            }

            return cache.Get(key) as T;
        }

        public bool Contains(string key)
        {
            return cache.Contains(key);
        }
    }
}
