using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Profilers;

namespace Glass.Mapper.Sc.Profilers
{
    public class SitecoreProfiler : IPerformanceProfiler
    {
        public void Start(string key)
        {
            Sitecore.Diagnostics.Profiler.StartOperation(key);
        }

        public void End(string key)
        {
            Sitecore.Diagnostics.Profiler.EndOperation(key);
        }
    }
}
