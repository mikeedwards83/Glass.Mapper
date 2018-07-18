using Glass.Mapper.Profilers;

namespace Glass.Mapper.Sc.Profilers
{
    /// <summary>
    /// Class SitecoreProfiler
    /// </summary>
    public class SitecoreProfiler : IPerformanceProfiler
    {

        /// <summary>
        /// Starts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Start(string key)
        {
            Sitecore.Diagnostics.Profiler.StartOperation(key);
        }

        /// <summary>
        /// Ends the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void End(string key)
        {
            Sitecore.Diagnostics.Profiler.EndOperation(key);
        }


        public void IndentIncrease()
        {
        }

        public void IndentDecrease()
        {
        }
    }
}

