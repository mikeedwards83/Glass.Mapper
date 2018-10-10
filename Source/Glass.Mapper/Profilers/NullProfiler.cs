
namespace Glass.Mapper.Profilers
{
    /// <summary>
    /// Class NullProfiler
    /// </summary>
    public class NullProfiler : IPerformanceProfiler
    {
        
        public static NullProfiler Instance { get; private set; }

        static NullProfiler()
        {
            Instance = new NullProfiler();
        }

        private NullProfiler() { }

        /// <summary>
        /// Starts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Start(string key)
        {
        }

        /// <summary>
        /// Ends the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void End(string key)
        {
        }

        public void IndentIncrease()
        {
        }

        public void IndentDecrease()
        {
        }
    }
}




