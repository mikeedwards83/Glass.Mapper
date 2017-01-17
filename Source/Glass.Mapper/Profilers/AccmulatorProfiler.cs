using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.Profilers
{
    public class AccmulatorProfiler : SimpleProfiler
    {
        Dictionary<string, long> _total = new Dictionary<string, long>();

        public AccmulatorProfiler(TextWriter writer) : base(writer)
        {
        }

        protected override void End(string key, string indent, Stopwatch watch)
        {
            if (_total.ContainsKey(key))
            {
                _total[key] += watch.ElapsedTicks;
            }
            else
            {
                _total[key] = watch.ElapsedTicks;
            }
            var totalTicks = _total[key];

            Writer.WriteLine("{0} Timer for {1}: Elapsed Ticks {2} Elapsed Ms {3}. Total Ticks: {4}".Formatted(indent, key, watch.ElapsedTicks, watch.ElapsedMilliseconds, totalTicks));

            base.End(key, indent, watch);
        }

        
    }
}
