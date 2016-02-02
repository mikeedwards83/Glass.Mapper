using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Web;
using System.Web.Caching;

namespace Glass.Mapper.Caching
{
    public class HttpCache : ICacheManager
    {
        private static ConcurrentBag<string> _keys = new ConcurrentBag<string>();

        //Absolute time in second
        public int AbsoluteExpiry { get; set; }
        /// <summary>
        /// Sliding time in seconds
        /// </summary>
        public int SlidingExpiry { get; set; }

        protected static ConcurrentBag<string> Keys { get { return _keys; } }

        public HttpCache()
        {

            //20 minute default
            SlidingExpiry = 60*20;
        }


        protected Cache Cache
        {
            get
            {
                return HttpContext.Current != null
                    ? HttpContext.Current.Cache
                    : HttpRuntime.Cache; 
            }
        }

        public object this[string key]
        {
            get { return Get<object>(key); }
            set { AddOrUpdate(key, value); }
        }

        public void ClearCache()
        {
            var cache = Cache;
            if (cache == null) return;

            var keys = Interlocked.Exchange(ref _keys, new ConcurrentBag<string>());

            foreach (var key in keys)
            {
                cache.Remove(key);
            }
        }

        public void AddOrUpdate<T>(string key, T value) where T : class
        {
            var cache = Cache;
            if (cache == null) return;

            if (AbsoluteExpiry > 0)
            {
                cache.Insert(key, value, null, DateTime.Now.AddSeconds(AbsoluteExpiry),
                    Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }
            else
            {
                cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, SlidingExpiry),
                    CacheItemPriority.Normal, null);
            }

            // duplicate keys here don't matter
            Keys.Add(key);
        }

        public T Get<T>(string key) where T : class
        {
            var cache = Cache;
            return cache != null
                ? cache[key] as T
                : null;
        }

        public bool Contains(string key)
        {
            return Get<object>(key) != null;
        }
    }
}
