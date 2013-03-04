using System.Collections.Generic;
namespace Glass.Mapper.Profilers
{
    public class ChainedProfiler : IPerformanceProfiler
    {
        public IEnumerable<IPerformanceProfiler> Profilers { get; set; }

        public ChainedProfiler(IEnumerable<IPerformanceProfiler> profilers)
        {
            Profilers = profilers;
        }

        public ChainedProfiler()
        {
            Profilers = new IPerformanceProfiler[]{};
        }

        public void Start(string key)
        {
            if (Profilers != null)
                Profilers.ForEach(x => x.Start(key));
        }

        public void End(string key)
        {
            if (Profilers != null)
                Profilers.ForEach(x => x.End(key));
        }
    }
}
