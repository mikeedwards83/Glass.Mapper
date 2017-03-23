using System;
using System.Runtime.Caching;
using System.Threading;

namespace Glass.Mapper.Caching
{
    /// <summary>
    /// A cache manager implementation based on .NET memory cache.
    /// </summary>
    public class NetMemoryCacheManager : ICacheManager
    {
        private static MemoryCache _memoryCache = new MemoryCache(CacheName);

        private const string CacheName = "Glass.Mapper";

        // Absolute time in seconds
        public int AbsoluteExpiry { get; set; }
        
        /// <summary>
        /// Sliding time in seconds
        /// </summary>
        public int SlidingExpiry { get; set; }

        /// <summary>
        /// Net memory cache manage
        /// </summary>
        public NetMemoryCacheManager()
        {
            SlidingExpiry = 60 * 20;
        }

        public object this[string key]
        {
            get { return Get<object>(key); }
            set { AddOrUpdate(key, value); }
        }

        /// <summary>
        /// Destroys and recreates the cache.
        /// </summary>
        public void ClearCache()
        {
            // destroy and recreate the cache
            var cache = _memoryCache;
            var newMemoryCache = new MemoryCache(CacheName);
            Interlocked.Exchange(ref _memoryCache, newMemoryCache);
            cache.Dispose();
        }

        /// <summary>
        /// Adds or updates the cache.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        public void AddOrUpdate<T>(string key, T value)
        {
            if (_memoryCache.Contains(key))
            {
                _memoryCache.Remove(key);
            }

            if (value != null)
            {
                CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
                if (AbsoluteExpiry > 0)
                {
                    cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddSeconds(AbsoluteExpiry);
                    _memoryCache.Add(key, value, cacheItemPolicy);
                }
                else
                {
                    cacheItemPolicy.SlidingExpiration = new TimeSpan(0, 0, SlidingExpiry);
                    _memoryCache.Add(key, value, cacheItemPolicy);
                }
            }
        }

        public T Get<T>(string key) where T : class
        {
            return _memoryCache[key] as T;
        }

        public T GetValue<T>(string key) where T : struct
        {
            try
            {
                return (T)_memoryCache[key];
            }
            catch
            {
                return default(T);
            }
        }

        public bool Contains(string key)
        {
            return _memoryCache.Contains(key);
        }
    }
}
