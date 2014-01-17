using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Glass.Mapper.Caching
{
    public class InMemoryCache : ICacheManager
    {
        readonly ConcurrentDictionary<string, object> _innerCache = new ConcurrentDictionary<string, object>();

        public object this[string key]
        {
            get
            {
                if (_innerCache.ContainsKey(key))
                {
                    return _innerCache[key];
                }
                return null;
            }
            set
            {
                Add(key, value);
            }
        }

        public void ClearCache()
        {
            lock (_innerCache)
            {
                _innerCache.Clear();
            }
        }

        public void Add(string key, object value)
        {
            lock (_innerCache)
            {
                _innerCache.TryAdd(key, value);
            }
        }

        public bool Contains(string key)
        {
            return _innerCache.ContainsKey(key);
        }
    }
}
