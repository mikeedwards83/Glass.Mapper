using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Pipelines
{
    /// <summary>
    /// The abstract base class that represents a pipeline runner. The runner calls each individual task
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    public abstract class AbstractPipelineRunner<T, K> 
        where T:AbstractPipelineArgs, new () 
        where K:IPipelineTask<T>
    {
        IList<K> _tasks;

        public IEnumerable<K> Tasks { get { return _tasks; } }


        public AbstractPipelineRunner()
        {
            _tasks = new List<K>();
        }

        /// <summary>
        /// Adds a task to the list of tasks to be executed by the runner
        /// </summary>
        /// <param name="task"></param>
        public void Add(K task)
        {
            _tasks.Add(task);
        }

        /// <summary>
        /// Removes a task from the list of tasks to be executed by the runner
        /// </summary>
        /// <param name="task"></param>
        public void Remove(K task)
        {
            _tasks.Remove(task);
        }


        /// <summary>
        /// Runs a pipeline and returns the resultant arguments
        /// </summary>
        /// <returns></returns>
        public T Run()
        {
            var args = new T();

            foreach (var task in _tasks)
            {
                task.Execute(args);
            }

            return args;
        }





    }
}
