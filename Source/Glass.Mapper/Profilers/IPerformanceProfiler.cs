using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Profilers
{
    public interface IPerformanceProfiler
    {
        void Start(string key);
        void End(string key);
    }
}
