using Glass.Mapper.Profilers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Profiling;
namespace Glass.Mapper.MiniProfiler
{
    public class Profiler : IPerformanceProfiler , IDisposable
    {
        Dictionary<string, IDisposable> _steps = new Dictionary<string, IDisposable>();

        public void Start(string key)
        {
            var profiler = StackExchange.Profiling.MiniProfiler.Current;
            var step = profiler.Step(key, ProfileLevel.Info);
            _steps.Add(key, step);
        }

        public void End(string key)
        {
            if (_steps.ContainsKey(key))
            {
                var step =_steps[key];
                if(step != null)
                    step.Dispose();
                _steps.Remove(key);

            }
        }

        public void Dispose()
        {
            _steps.ForEach(x => x.Value.Dispose());
        }

    }
}
