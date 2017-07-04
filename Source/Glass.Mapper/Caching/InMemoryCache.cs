using System.Collections.Concurrent;

namespace Glass.Mapper.Caching
{
    public class InMemoryCache : AbstractCacheManager
    {
        private readonly ConcurrentDictionary<string, object> _innerCache = new ConcurrentDictionary<string, object>();

        protected ConcurrentDictionary<string, object> Cache { get { return _innerCache; } }


        public override void ClearCache()
        {
            Cache.Clear();
        }

        protected override void InternalAddOrUpdate<T>(string key, T value)
        {
            Cache.AddOrUpdate(key, value, (s, o) => value);
        }

        protected override object InternalGet(string key) 
        {
            object retVal;
            return Cache.TryGetValue(key, out retVal)
                ? retVal
                : null;
        }

        public override T GetValue<T>(string key)
        {
            object retVal;
            return Cache.TryGetValue(key, out retVal)
                ? (T)retVal
                : default(T);
        }

        public override bool Contains(string key)
        {
            return Cache.ContainsKey(key);
        }
    }
}
