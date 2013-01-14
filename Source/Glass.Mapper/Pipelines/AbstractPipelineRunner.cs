using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public IEnumerable<K> Tasks { get { return _tasks; } }

        public IPerformanceProfiler Profiler { get; set; }

        public AbstractPipelineRunner(IEnumerable<K> tasks)
        {
            _tasks = tasks;
            Profiler = new NullProfiler();
        }

        /// <summary>
        /// Runs a pipeline and returns the resultant arguments
        /// </summary>
        /// <returns></returns>
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
