using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Glass.Mapper.IoC
{
    public abstract class AbstractConfigFactory<T> : IConfigFactory<T>
    {

        private ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();

        protected List<Builder> TypeGenerators { get; private set; }

        protected AbstractConfigFactory()
        {
            _cacheLock.EnterWriteLock();

            TypeGenerators = new List<Builder>();

            _cacheLock.ExitWriteLock();
        }

        public void Insert<TK>(int index, Func<TK> add) where TK : T
        {
            if (add == null)
            {
                throw new NullReferenceException("Cannot insert with a null function");
            }

            try
            {
                _cacheLock.EnterWriteLock();

                TypeGenerators.Insert(index, new Builder {Type = typeof(TK), Func = () => add()});
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Inserts function as the first in the list. Same as Insert(0, ()=>new T())
        /// </summary>
        /// <param name="add"></param>
        public virtual void First<TK>(Func<TK> add) where TK : T
        {
           Insert(0, add);
        }

        /// <summary>
        /// Replaces the function at the given index
        /// </summary>
        public virtual void Replace<K>(int index, Func<K> replace) where K : T
        {
            if (replace == null)
            {
                throw new NullReferenceException("Cannot replace with a null function");
            }


            try
            {
                _cacheLock.EnterWriteLock();

                TypeGenerators[index] = new Builder {Type = replace.Method.ReturnType, Func = () => replace()};
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
        }


        public virtual void Replace<TReplace, TK>(Func<TK> func) where TReplace : T where TK : T
        {
            var index = TypeGenerators.FindIndex(x => x.Type == typeof(TReplace));
            RemoveAt(index);
            Insert(index, func);
        }

        public virtual void InsertBefore<TBefore, TK>(Func<TK> func ) where TBefore : T where TK : T
        {
            var index = TypeGenerators.FindIndex(x => x.Type == typeof (TBefore));
            Insert(index, func);
        }

        public virtual void InsertAfter<TAfter, TK>(Func<TK> func) where TAfter : T where TK : T
        {
            var index = TypeGenerators.FindIndex(x => x.Type == typeof(TAfter));
            Insert(index+1, func);
        }


        public virtual void Remove<TRemove>() where TRemove : T 
        {
            var index = TypeGenerators.FindIndex(x => x.Type == typeof(TRemove));
            RemoveAt(index);
        }

        /// <summary>
        /// Adds a function to the end of the current list
        /// </summary>
        public virtual void Add<TK>(Func<TK> add) where TK : T
        {
            if (add == null)
            {
                throw new NullReferenceException("Cannot add with a null function");
            }

            try
            {
                _cacheLock.EnterWriteLock();
                TypeGenerators.Add(
                    new Builder {Type = typeof(TK), Func = () => add()}
                );
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes a function at the given index
        /// </summary>
        public virtual void RemoveAt(int index)
        {
            try
            {
                _cacheLock.EnterWriteLock();

                TypeGenerators.RemoveAt(index);

            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
            
        }

        /// <summary>
        /// Gets a list of the created objects 
        /// </summary>
        public virtual IEnumerable<T> GetItems()
        {
            IEnumerable<Func<T>> builders;

            try
            {
                _cacheLock.EnterReadLock();

                if (TypeGenerators == null)
                {
                    return null;
                }
                //we create a local copy of the generators to avoid any problems with the 
                // list being modified during enumeration and exit the lock ASAP
                builders = TypeGenerators.Select(x => x.Func).ToArray();
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }

            //generate the class outside of the lock encase there are any long running operations.
            //and then force them into an array to avoid any enumeration issues.
            return builders.Select(x => x()).ToArray();
        }

        protected struct Builder
        {
            public Type Type { get; set; }
            public Func<T> Func { get; set; } 
        }
    }
}
