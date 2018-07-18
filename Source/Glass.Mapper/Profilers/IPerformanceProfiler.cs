namespace Glass.Mapper.Profilers
{
    /// <summary>
    /// Interface IPerformanceProfiler
    /// </summary>
    public interface IPerformanceProfiler
    {
        /// <summary>
        /// Starts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        void Start(string key);
        /// <summary>
        /// Ends the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        void End(string key);

        void IndentIncrease();
        void IndentDecrease();
    }

    
}




