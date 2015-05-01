namespace Glass.Mapper.Caching
{
    public abstract class AbstractCacheManager : ICacheManager
    {
        public virtual object this[string key]
        {
            get { return Get(key); }
            set { AddOrUpdate(key, value); }
        }

        public abstract void ClearCache();

        public abstract void AddOrUpdate(string key, object value);

        public abstract object Get(string key);

        public virtual T Get<T>(string key) where T : class
        {
            return Get(key) as T;
        }

        public abstract bool Contains(string key);
    }
}
