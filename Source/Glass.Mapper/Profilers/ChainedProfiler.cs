using System.Collections.Generic;
namespace Glass.Mapper.Profilers
{
    /// <summary>
    /// Class ChainedProfiler
    /// </summary>
    public class ChainedProfiler : IPerformanceProfiler
    {
        /// <summary>
        /// Gets or sets the profilers.
        /// </summary>
        /// <value>The profilers.</value>
        public IEnumerable<IPerformanceProfiler> Profilers { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChainedProfiler"/> class.
        /// </summary>
        /// <param name="profilers">The profilers.</param>
        public ChainedProfiler(IEnumerable<IPerformanceProfiler> profilers)
        {
            Profilers = profilers;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChainedProfiler"/> class.
        /// </summary>
        public ChainedProfiler()
        {
            Profilers = new IPerformanceProfiler[]{};
        }

        /// <summary>
        /// Starts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Start(string key)
        {
            if (Profilers != null)
                Profilers.ForEach(x => x.Start(key));
        }

        /// <summary>
        /// Ends the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void End(string key)
        {
            if (Profilers != null)
                Profilers.ForEach(x => x.End(key));
        }
    }
}
