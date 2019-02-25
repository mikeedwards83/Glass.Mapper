namespace Glass.Mapper.Caching
{
    public interface ICacheManager
    {
        /// <summary>
        /// Retrieves an object from the default cache.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object this[string key] { get; set; }

        void ClearCache();

        void AddOrUpdate<T>(string key, T value);

        T Get<T>(string key) where T : class;

        T GetValue<T>(string key) where T : struct;

        bool Contains(string key);
    }
}
