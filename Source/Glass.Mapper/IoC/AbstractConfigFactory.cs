using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Glass.Mapper.IoC
{
    public abstract class AbstractConfigFactory<T> : IConfigFactory<T>
    {
        protected List<Builder> TypeGenerators { get; private set; }


        private bool _finalised;

        protected AbstractConfigFactory()
        {
            TypeGenerators = new List<Builder>();
        }

        public void Insert<TK>(int index, Func<TK> add) where TK : T
        {
            if (_finalised)
            {
                throw new NotSupportedException("Configuration has been finalised and cannot be changed.");
            }

            if (add == null)
            {
                throw new NullReferenceException("Cannot insert with a null function");
            }

            TypeGenerators.Insert(index, new Builder { Type = typeof(TK), Func = () => add() });
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

            if (_finalised)
            {
                throw new NotSupportedException("Configuration has been finalised and cannot be changed.");
            }

            if (replace == null)
            {
                throw new NullReferenceException("Cannot replace with a null function");
            }


            TypeGenerators[index] = new Builder { Type = replace.Method.ReturnType, Func = () => replace() };
        }


        public virtual void Replace<TReplace, TK>(Func<TK> func) where TReplace : T where TK : T
        {
            var index = TypeGenerators.FindIndex(x => x.Type == typeof(TReplace));
            RemoveAt(index);
            Insert(index, func);
        }

        public virtual void InsertBefore<TBefore, TK>(Func<TK> func) where TBefore : T where TK : T
        {
            var index = TypeGenerators.FindIndex(x => x.Type == typeof(TBefore));
            Insert(index, func);
        }

        public virtual void InsertAfter<TAfter, TK>(Func<TK> func) where TAfter : T where TK : T
        {
            var index = TypeGenerators.FindIndex(x => x.Type == typeof(TAfter));
            Insert(index + 1, func);
        }


        public virtual void Remove<TRemove>() where TRemove : T
        {
            var index = TypeGenerators.FindIndex(x => x.Type == typeof(TRemove));
            RemoveAt(index);
        }

        public void Finalise()
        {
            _finalised = true;
        }

        /// <summary>
        /// Adds a function to the end of the current list
        /// </summary>
        public virtual void Add<TK>(Func<TK> add) where TK : T
        {
            if (_finalised)
            {
                throw new NotSupportedException("Configuration has been finalised and cannot be changed.");
            }

            if (add == null)
            {
                throw new NullReferenceException("Cannot add with a null function");
            }

            TypeGenerators.Add(
                new Builder { Type = typeof(TK), Func = () => add() }
                );
        }

        /// <summary>
        /// Removes a function at the given index
        /// </summary>
        public virtual void RemoveAt(int index)
        {
            if (_finalised)
            {
                throw new NotSupportedException("Configuration has been finalised and cannot be changed.");
            }

            TypeGenerators.RemoveAt(index);
        }

        /// <summary>
        /// Gets a list of the created objects 
        /// </summary>
        public virtual IEnumerable<T> GetItems()
        {
            if (!_finalised)
            {
                throw new NotSupportedException("Configuration has not been finalised and cannot be changed. Ensure that you call DependencyResolve.Finalise after creating and configuring the dependency resolver.");
            }

            IEnumerable<Func<T>> builders;
            if (TypeGenerators == null)
            {
                return null;
            }

            //generate the class outside of the lock encase there are any long running operations.
            //and then force them into an array to avoid any enumeration issues.
            return TypeGenerators.Select(x => x.Func()).ToArray();
        }

        protected struct Builder
        {
            public Type Type { get; set; }
            public Func<T> Func { get; set; }
        }
    }
}
