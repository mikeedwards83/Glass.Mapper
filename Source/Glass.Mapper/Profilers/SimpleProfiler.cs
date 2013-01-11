using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Profilers
{
    public class SimpleProfiler : IPerformanceProfiler
    {
        private Dictionary<string, Stopwatch> _watches = new Dictionary<string, Stopwatch>();

        public void Start(string key)
        {
            if (_watches.ContainsKey(key))
                throw new NotSupportedException("Watch with key {0} already started".Formatted(key));
            
            var watch = new Stopwatch();
            _watches.Add(key, watch);
            watch.Start();
        }

        public void End(string key)
        {
            if (!_watches.ContainsKey(key))
                throw new NotSupportedException("No Watch with key {0} found".Formatted(key));

            var watch = _watches[key];
            watch.Stop();
            _watches.Remove(key);
            Console.WriteLine("Watch {0}: Elapsed Ticks {1}".Formatted(key, watch.ElapsedTicks));
        }
    }
}
