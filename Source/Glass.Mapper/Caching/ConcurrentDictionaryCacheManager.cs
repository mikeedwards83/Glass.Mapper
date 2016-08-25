using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.Caching
{
    public class ConcurrentDictionaryCacheManager : ICacheManager
    {
        static ConcurrentDictionary<string, object> _cache = new ConcurrentDictionary<string, object>();
        public object this[string key]
        {
            get { return _cache[key]; }
            set { _cache[key] = value; }
        }

        public void ClearCache()
        {
            _cache.Clear();
        }

        public void AddOrUpdate<T>(string key, T value)
        {
            _cache.AddOrUpdate(key,(k)=> value, (k,o)=> value);
        }

        public T Get<T>(string key) where T : class
        {
            return _cache[key] as T;
        }

        public T GetValue<T>(string key) where T : struct
        {
            return (T)_cache[key];
        }

        public bool Contains(string key)
        {
            return _cache.ContainsKey(key);
        }
    }
}
