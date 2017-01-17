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
using System.IO;
using System.Text;

namespace Glass.Mapper.Profilers
{
    /// <summary>
    /// Class SimpleProfiler
    /// </summary>
    public class SimpleProfiler : BaseIndentingProfiler
    {
        protected TextWriter Writer { get; private set; }

      
        public SimpleProfiler(TextWriter writer)
        {
            Writer = writer;
        }

        private Stack<Tuple<string, Stopwatch>> _watches = new Stack<Tuple<string, Stopwatch>>();

        protected override void Start(string key, string indent)
        {
            var watch = new Stopwatch();
            _watches.Push(new Tuple<string, Stopwatch>(key, watch));
            watch.Start();
        }

        protected override void End(string key, string indent)
        {
            var tuple = _watches.Pop();
            if (tuple.Item1 != key)
            {
                throw new Exception("Failed to pop watch with key {0}. Have you ended all profiling in the correct order?".Formatted(key));
            }
            var watch = tuple.Item2;
            watch.Stop();
            End(key, indent, watch);
        }

        protected virtual void End(string key, string indent, Stopwatch watch)
        {
            Writer.WriteLine("{3} Timer for {0}: Elapsed Ticks {1} Elapsed Ms {2}".Formatted(key, watch.ElapsedTicks, watch.ElapsedMilliseconds, indent));
        }
    }
}




