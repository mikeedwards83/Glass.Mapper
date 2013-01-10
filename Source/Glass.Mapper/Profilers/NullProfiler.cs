using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Profilers
{
    public class NullProfiler : IPerformanceProfiler
    {
        public void Start(string key)
        {
        }

        public void End(string key)
        {
        }
    }
}
