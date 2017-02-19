using System;
using System.Runtime.Caching;
using System.Threading;

namespace Glass.Mapper.Caching
{
    public class NetMemoryCacheManager : AbstractCacheManager
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

        /// <summary>
        /// Destroys and recreates the cache
        /// </summary>
        public override void ClearCache()
        {
            // destroy and recreate the cache
            var newMemoryCache = new MemoryCache(CacheName);
            Interlocked.Exchange(ref _memoryCache, newMemoryCache);
        }

        /// <summary>
        /// Adds or updates the cache.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        protected override void InternalAddOrUpdate<T>(string key, T value)
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

        protected override object InternalGet(string key)
        {
            return _memoryCache[key];
        }

        public override T GetValue<T>(string key) 
        {
            try
            {
                return (T) _memoryCache[key];
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public override bool Contains(string key)
        {
            return _memoryCache.Contains(key);
        }
    }
}
