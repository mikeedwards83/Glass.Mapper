using System;
using System.Runtime.Caching;

namespace Glass.Mapper.Caching
{
    public class NetMemoryCacheManager : ICacheManager
    {
        private static MemoryCache _memoryCache = new MemoryCache(CacheName);

        private const string CacheName = "Glass.Mapper";

        //Absolute time in second
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

            //20 minute default
            SlidingExpiry = 60*20;
        }


        public object this[string key]
        {
            get { return Get<object>(key); }
            set { AddOrUpdate(key, value); }
        }

        /// <summary>
        /// Destroys and recreates the cache
        /// </summary>
        public void ClearCache()
        {
            // destroy and recreate the cache
            _memoryCache = new MemoryCache(CacheName);
        }

        /// <summary>
        /// Adds or updates the cache.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        public void AddOrUpdate<T>(string key, T value) where T : class
        {
            if (_memoryCache.Contains(key))
            {
                _memoryCache.Remove(key);
            }

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

        public T Get<T>(string key) where T : class
        {
            return _memoryCache[key] as T;
        }

        public bool Contains(string key)
        {
            return _memoryCache.Contains(key);
        }
    }
}
