using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Glass.Mapper.IoC;

namespace Glass.Mapper
{
    public class PipelinePool<T> : IPipelinePool<T>
    {
        private readonly Queue<T> _queue;
        private Task _task;

        public PipelinePool(IDependencyResolver dependencyResolver, int poolSize, Func<IDependencyResolver, T> creationFunc)
        {
            DependencyResolver = dependencyResolver;
            CreationFunc = creationFunc;
            PoolSize = poolSize;
            Threshold = PoolSize/2;
            _queue = new Queue<T>();
            _task = new Task(AddObject);
            _task.RunSynchronously();
        }

        protected IDependencyResolver DependencyResolver { get; private set; }

        protected Func<IDependencyResolver, T> CreationFunc { get; private set; } 

        protected int PoolSize { get; private set; }

        protected int Threshold { get; private set; }

        public int Missed { get; private set; }

        public T GetFromPool()
        {
            T result;
            // PoolSize of 1 is treated as a singleton
            if (PoolSize == 1)
            {   
                return _queue.Peek();
            }

            RefreshQueue();

            if (_queue.Count > 0)
            {
                return _queue.Dequeue();
            }

            Missed++;
            return CreationFunc(DependencyResolver);
        }

        private void RefreshQueue()
        {
            if (_queue.Count >= Threshold || _task.Status == TaskStatus.Running)
            {
                return;
            }

            _task = new Task(AddObject);
            _task.Start();
        }

        private void AddObject()
        {
            for (var i = 0; i < PoolSize; i++)
            {
                if (_queue.Count == PoolSize)
                {
                    return;
                }

                _queue.Enqueue(CreationFunc(DependencyResolver));
            }
        }
    }
}
