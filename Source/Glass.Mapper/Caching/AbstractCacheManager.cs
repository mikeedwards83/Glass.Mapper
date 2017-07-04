using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.Caching
{
    public abstract class AbstractCacheManager : ICacheManager
    {
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

        public void AddOrUpdate<T>(string key, T value)
        {
            if (value == null)
            {
                InternalAddOrUpdate(key, new NullValue());
            }
            else
            {
                InternalAddOrUpdate(key, value);
            }
        }

        public T Get<T>(string key) where T : class
        {
            var result = InternalGet(key);
            return result is NullValue ? null : result as T;
        }

        public abstract bool Contains(string key);
        public abstract void ClearCache();
        public abstract T GetValue<T>(string key) where T : struct;

        protected abstract void InternalAddOrUpdate<T>(string key, T value);
        protected abstract object InternalGet(string key);
        

        protected class NullValue
        {
        }
    }
}
