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

using System.Collections.Generic;
using Glass.Mapper.Profilers;

namespace Glass.Mapper.Pipelines
{
    /// <summary>
    /// The abstract base class that represents a pipeline runner. The runner calls each individual task
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    public abstract class AbstractPipelineRunner<T, K> 
        where T:AbstractPipelineArgs
        where K:IPipelineTask<T>
    {
        IEnumerable<K> _tasks;

        /// <summary>
        /// Gets the tasks.
        /// </summary>
        /// <value>The tasks.</value>
        public IEnumerable<K> Tasks { get { return _tasks; } }

        /// <summary>
        /// Gets or sets the profiler.
        /// </summary>
        /// <value>The profiler.</value>
        public IPerformanceProfiler Profiler { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractPipelineRunner{T, K}"/> class.
        /// </summary>
        /// <param name="tasks">The tasks.</param>
        public AbstractPipelineRunner(IEnumerable<K> tasks)
        {
            _tasks = tasks;
            Profiler = new NullProfiler();
        }

        /// <summary>
        /// Runs a pipeline and returns the resultant arguments
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>`0.</returns>
        public virtual T Run(T args)
        {
            if (_tasks != null)
            {
                foreach (var task in _tasks)
                {
#if DEBUG
                    Profiler.Start(task.GetType().FullName);
#endif
                    task.Execute(args);
#if DEBUG
                    Profiler.End(task.GetType().FullName);
#endif 
                    if (args.IsAborted)
                        break;
                }
            }
            
            return args;
        }





    }
}




