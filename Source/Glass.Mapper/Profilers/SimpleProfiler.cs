/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-

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



