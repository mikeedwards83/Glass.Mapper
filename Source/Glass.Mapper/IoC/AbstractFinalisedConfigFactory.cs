using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Glass.Mapper.IoC
{
    public abstract class AbstractConfigFactory<T> : IConfigFactory<T>
    {
        private List<Builder> _typeGenerators;

        protected IEnumerable<Builder> TypeGenerators
        {
            get { return _typeGenerators; }
        }

        protected AbstractConfigFactory()
        {
            _typeGenerators = new List<Builder>();
        }

        protected struct Builder
        {
            public Type Type { get; set; }
            public Func<T> Func { get; set; }
        }

        /// <summary>
        /// Inserts function as the first in the list. Same as Insert(0, ()=>new T())
        /// </summary>
        /// <param name="add"></param>
        public virtual void First<TK>(Func<TK> add) where TK : T
        {
            Insert(0, add);
        }
        

        public virtual void Replace<TReplace, TK>(Func<TK> func) where TReplace : T where TK : T
        {

            var index = _typeGenerators.FindIndex(x => x.Type == typeof(TReplace));
            RemoveAt(index);
            Insert(index, func);
        }

        public virtual void InsertBefore<TBefore, TK>(Func<TK> func) where TBefore : T where TK : T
        {
            var index = _typeGenerators.FindIndex(x => x.Type == typeof(TBefore));
            Insert(index, func);
        }

        public virtual void InsertAfter<TAfter, TK>(Func<TK> func) where TAfter : T where TK : T
        {
            var index = _typeGenerators.FindIndex(x => x.Type == typeof(TAfter));
            Insert(index + 1, func);
        }

        public virtual void Insert<TK>(int index, Func<TK> add) where TK : T
        {
            if (add == null)
            {
                throw new NullReferenceException("Cannot insert with a null function");
            }

            _typeGenerators.Insert(index, new Builder { Type = typeof(TK), Func = () => add() });
        }
        public virtual void Replace<K>(int index, Func<K> replace) where K : T
        {
            if (replace == null)
            {
                throw new NullReferenceException("Cannot replace with a null function");
            }

            _typeGenerators[index] = new Builder { Type = replace.Method.ReturnType, Func = () => replace() };
        }



        public virtual void Remove<TRemove>() where TRemove : T
        {
            var index = _typeGenerators.FindIndex(x => x.Type == typeof(TRemove));
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

            _typeGenerators.Add(
                new Builder { Type = typeof(TK), Func = () => add() }
            );
        }

        /// <summary>
        /// Removes a function at the given index
        /// </summary>
        public virtual void RemoveAt(int index)
        {
            _typeGenerators.RemoveAt(index);
        }

        /// <summary>
        /// Gets a list of the created objects 
        /// </summary>
        public virtual IEnumerable<T> GetItems()
        {
            if (_typeGenerators == null)
            {
                return null;
            }

            return _typeGenerators.Select(x => x.Func()).ToArray();

            throw new NotSupportedException(
                "Configuration has not been finalised. Ensure that you call DependencyResolve.Finalise after creating and configuring the dependency resolver.");
        }
    }


    public abstract class AbstractFinalisedConfigFactory<T> : AbstractConfigFactory<T>
    {

        private bool _finalised;

        public override void Insert<TK>(int index, Func<TK> add)
        {
            if (_finalised)
            {
                throw new NotSupportedException("Configuration has been finalised and cannot be changed.");
            }

            base.Insert(index, add);
        }

        

        /// <summary>
        /// Replaces the function at the given index
        /// </summary>
        public override void Replace<K>(int index, Func<K> replace)
        {
            if (_finalised)
            {
                throw new NotSupportedException("Configuration has been finalised and cannot be changed.");
            }

            base.Replace(index, replace);
        }

        public void Finalise()
        {
            _finalised = true;
        }

        /// <summary>
        /// Adds a function to the end of the current list
        /// </summary>
        public override void Add<TK>(Func<TK> add) 
        {
            if (_finalised)
            {
                throw new NotSupportedException("Configuration has been finalised and cannot be changed.");
            }

            base.Add(add);
        }

        /// <summary>
        /// Removes a function at the given index
        /// </summary>
        public override void RemoveAt(int index)
        {
            if (_finalised)
            {
                throw new NotSupportedException("Configuration has been finalised and cannot be changed.");
            }

            base.RemoveAt(index);
        }

        /// <summary>
        /// Gets a list of the created objects 
        /// </summary>
        public override IEnumerable<T> GetItems()
        {
            if (_finalised)
            {
                return base.GetItems();
            }

            throw new NotSupportedException(
                "Configuration has not been finalised. Ensure that you call DependencyResolve.Finalise after creating and configuring the dependency resolver.");
        }


    }
}
