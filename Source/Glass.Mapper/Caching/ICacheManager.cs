namespace Glass.Mapper.Caching
{
    public interface ICacheManager
    {
        object this[string key] { get; set; }
        void ClearCache();
        void AddOrUpdate(string key, object value);
        object Get(string key);
        T Get<T>(string key) where T : class;
        bool Contains(string key);
    }
}
