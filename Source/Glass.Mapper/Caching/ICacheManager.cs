namespace Glass.Mapper.Caching
{
    public interface ICacheManager
    {
        object this[string key] { get; set; }
        void ClearCache();
        void AddOrUpdate<T>(string key, T value) where T : class;
        T Get<T>(string key) where T : class;
        bool Contains(string key);
    }
}
