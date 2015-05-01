using System.Collections.Concurrent;

namespace Glass.Mapper.Caching
{
    public class InMemoryCache : ICacheManager
    {
        private readonly ConcurrentDictionary<string, object> _innerCache = new ConcurrentDictionary<string, object>();

        protected ConcurrentDictionary<string, object> Cache { get { return _innerCache; } }

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
            Cache.Clear();
        }

        public void AddOrUpdate<T>(string key, T value) where T : class
        {
            Cache.AddOrUpdate(key, value, (s, o) => value);
        }

        public T Get<T>(string key) where T : class
        {
            object retVal;
            return Cache.TryGetValue(key, out retVal)
                ? retVal as T
                : null;
        }

        public bool Contains(string key)
        {
            return Cache.ContainsKey(key);
        }
    }
}
