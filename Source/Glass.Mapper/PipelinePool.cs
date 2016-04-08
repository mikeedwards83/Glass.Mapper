using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Glass.Mapper
{
    public class PipelinePool<T> : IPipelinePool<T>
    {
        private readonly Queue<T> _queue;

        public PipelinePool(Context glassContext, int poolSize, Func<Context, T> creationFunc)
        {
            GlassContext = glassContext;
            CreationFunc = creationFunc;
            PoolSize = poolSize;
            _queue = new Queue<T>(poolSize);
            for (var i = 0; i < poolSize; i++)
            {
                AddObject();
            }
        }

        protected Context GlassContext { get; private set; }

        protected Func<Context, T> CreationFunc { get; private set; } 

        protected int PoolSize { get; private set; }

        public T GetFromPool()
        {
            // PoolSize of 1 is treated as a singleton
            if (PoolSize == 1)
            {
                return _queue.Peek();
            }

            AddObject();
            return _queue.Dequeue();
        }

        private void AddObject()
        {
            _queue.Enqueue(CreationFunc(GlassContext));
        }
    }
}
