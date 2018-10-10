using System;

namespace Glass.Mapper.Pipelines
{
    /// <summary>
    /// Interface IPipelineTask
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractPipelineTask<T>  where T: AbstractPipelineArgs
    {

        public string Name { get; protected set; }

        protected Action<T> NextAction { get; private set; }

        public void SetNext(Action<T> next)
        {
            NextAction = next;
        }

        /// <summary>
        /// Executes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        public virtual void Execute(T args)
        {
            Next(args);
        }
        public void Next(T args)
        {
            if (NextAction != null)
            {
                NextAction(args);
            }
        }
    }
}




