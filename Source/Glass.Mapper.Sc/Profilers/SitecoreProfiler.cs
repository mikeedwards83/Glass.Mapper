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

using Glass.Mapper.Profilers;

namespace Glass.Mapper.Sc.Profilers
{
    /// <summary>
    /// Class SitecoreProfiler
    /// </summary>
    public class SitecoreProfiler : IPerformanceProfiler
    {
        /// <summary>
        /// Starts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Start(string key)
        {
            Sitecore.Diagnostics.Profiler.StartOperation(key);
        }

        /// <summary>
        /// Ends the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void End(string key)
        {
            Sitecore.Diagnostics.Profiler.EndOperation(key);
        }
    }
}

