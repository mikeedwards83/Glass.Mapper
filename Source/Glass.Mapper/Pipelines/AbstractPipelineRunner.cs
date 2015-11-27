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
        where K : IPipelineTask<T>
    {

        private readonly Func<T, T> _excuteTasks = args => { return args; };

        /// <summary>
        /// Gets the tasks.
        /// </summary>
        /// <value>The tasks.</value>
        public IEnumerable<K> Tasks { get; private set; }

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

            if (tasks != null)
            {
                Tasks = tasks;

                foreach (var task in Tasks.Reverse())
                {
                    _excuteTasks = CreateTaskExpression(task);
                }
            }

            Profiler = new NullProfiler();
        }

        protected virtual Func<T, T> CreateTaskExpression(K task)
        {
            var nextTask = _excuteTasks;

            return (args) =>
            {
                task.Execute(args);

                if (!args.IsAborted)
                    nextTask(args);

                return args;
            };
        }

        /// <summary>
        /// Runs a pipeline and returns the resultant arguments
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>`0.</returns>
        public virtual T Run(T args)
        {
            //            if (Tasks != null)
            //            {
            //                for (int i = Tasks.Length-1; i >= 0; i--)
            //                {
            //                    var task = Tasks[i];
            //#if DEBUG
            //                    Profiler.Start(task.GetType().FullName);
            //#endif
            //                    task.Execute(args);
            //#if DEBUG
            //                    Profiler.End(task.GetType().FullName);
            //#endif
            //                    if (args.IsAborted)
            //                        break;
            //                }
            //            }

            return _excuteTasks(args);
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




