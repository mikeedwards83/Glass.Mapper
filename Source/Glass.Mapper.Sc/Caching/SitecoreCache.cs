using System;
using Glass.Mapper.Caching;
using Sitecore.Caching;

namespace Glass.Mapper.Sc.Caching
{
    public class SitecoreCache : ICacheManager
    {

        private readonly Cache _objectCache;
        private readonly TimeSpan _cacheItemSlidingExpiration;

        public const string CacheName = "0E08C19C-39EF-4990-A8C9-BD167334BF84";

        public long CacheSize { get; private set; }

        public SitecoreCache()
            : this(100)
        {
            
           
        }
        public SitecoreCache(long cacheSize)
        {
            CacheSize = cacheSize;
            _objectCache = Cache.GetNamedInstance(CacheName, CacheSize);

            _cacheItemSlidingExpiration = System.Web.Caching.Cache.NoSlidingExpiration;
        }

        public  bool ContainsObject(ICacheKey cacheKey)
        {
           );
        }

        public  void AddObject(ICacheKey cacheKey, object objectForCaching)
        {
            _objectCache.Add(cacheKey.GetKey(), objectForCaching, 10, _cacheItemSlidingExpiration);
        }

        public void ClearCache()
        {
            _objectCache.Clear();
        }

        public  object GetObject(ICacheKey cacheKey)
        {
            return _objectCache.GetValue(cacheKey.GetKey());
        }

        public object this[string key]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void ClearCache()
        {
            Sitecore.Caching.CacheManager.ClearAllCaches();
        }

        public void Add(string key, object value)
        {
            throw new NotImplementedException();
        }

        public bool Contains(string key)
        {
            throw new NotImplementedException();
        }
    }
}
