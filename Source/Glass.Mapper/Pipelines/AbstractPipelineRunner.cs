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
using System.Linq;
using Glass.Mapper.Profilers;

namespace Glass.Mapper.Pipelines
{
    /// <summary>
    /// The abstract base class that represents a pipeline runner. The runner calls each individual task
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    public abstract class AbstractPipelineRunner<T, K> : IDisposable
        where T : AbstractPipelineArgs
        where K : AbstractPipelineTask<T>
    {

        private readonly Action<T> _excuteTasks = args => { };

        /// <summary>
        /// Gets the tasks.
        /// </summary>
        /// <value>The tasks.</value>
        public IEnumerable<K> Tasks { get;  set; }

        /// <summary>
        /// Gets or sets the profiler.
        /// </summary>
        /// <value>The profiler.</value>
        public IPerformanceProfiler Profiler { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractPipelineRunner{T, K}"/> class.
        /// </summary>
        /// <param name="tasks">The tasks.</param>
        protected AbstractPipelineRunner(IEnumerable<K> tasks)
        {
            //Tasks = tasks.Reverse().ToArray();
            Tasks = tasks;

            K previous = null;
            K first = null;
            foreach(var current in tasks)
            {
                if (first == null)
                {
                    first = current;
                }

                if (previous == null)
                {
                    previous = current;
                }
                else
                {
                    previous.SetNext(args=> current.Execute(args));
                    previous = current;
                }
                    
            }

            Profiler = NullProfiler.Instance;

            if (first != null)
            {
                _excuteTasks = (args) => first.Execute(args);
            }
        }
       

        /// <summary>
        /// Runs a pipeline and returns the resultant arguments
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>`0.</returns>
        public virtual T Run(T args)
        {
            _excuteTasks(args);
            return args;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (Tasks != null)
            {
                foreach (var task in Tasks)
                {
                    var disposableTask = task as IDisposable;
                    if (disposableTask != null)
                    {
                        disposableTask.Dispose();
                    }
                }
            }

            Tasks = null;
        }
    }
}




