using Glass.Mapper.Profilers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

namespace Glass.Mapper.Glimpse
{
    public class Profiler : IPerformanceProfiler, IDisposable
    {
        Stopwatch _watch;
        Stack<Timing> _timingsStack;
        List<Timing> _timings;

        public IEnumerable<Timing> Timings { get { return _timings; } }

        public Profiler()
        {

            _watch = new Stopwatch();
            _watch.Start();
            _timings = new List<Timing>();
            _timingsStack = new Stack<Timing>();
        }

        public void Start(string key)
        {
            _timingsStack.Push(
                    new Timing { Start = _watch.ElapsedTicks, Key = key }
                );
            
        }

        public void End(string key)
        {

            
            var last = _timingsStack.Pop();
            
            if (last == null)
                return;

            if(last.Key != key)
                throw new Exception("Messages in the wrong order");

            last.End = _watch.ElapsedTicks;
            _timings.Add(last);
        }

        public class Timing{
            public string Key { get; set; }
            public long Start{get;set;}
            public long End{get;set;}
            public long Duration { get { return End - Start; } }
        }


        public void Dispose()
        {
            //_timings = null;
            //_timingsStack = null;
            //_watch.Stop();
            //_watch = null;
        }
    }
}
