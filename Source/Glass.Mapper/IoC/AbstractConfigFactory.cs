using System;
using System.Collections.Generic;
using System.Linq;

namespace Glass.Mapper.IoC
{
    public abstract class AbstractConfigFactory<T> : IConfigFactory<T>
    {
        private readonly object _lockObject = new object();

        protected List<Func<T>> TypeGenerators { get; private set; }

        protected AbstractConfigFactory()
        {
            lock (_lockObject)
            {
                TypeGenerators = new List<Func<T>>();
            }
        }

        public void Insert(int index, Func<T> add)
        {
            if (add == null)
            {
                throw new NullReferenceException("Cannot insert with a null function");
            }

            lock (_lockObject)
            {
                TypeGenerators.Insert(index, add);
            }
        }

        /// <summary>
        /// Inserts function as the first in the list. Same as Insert(0, ()=>new T())
        /// </summary>
        /// <param name="add"></param>
        public virtual void First(Func<T> add)
        {
           Insert(0, add);
        }

        /// <summary>
        /// Replaces the function at the given index
        /// </summary>
        public virtual void Replace(int index, Func<T> replace)
        {
            if (replace == null)
            {
                throw new NullReferenceException("Cannot replace with a null function");
            }

            lock (_lockObject)
            {
                TypeGenerators[index] = replace;
            }
        }

        /// <summary>
        /// Adds a function to the end of the current list
        /// </summary>
        public virtual void Add(Func<T> add)
        {
            if (add == null)
            {
                throw new NullReferenceException("Cannot add with a null function");
            }

            lock (_lockObject)
            {
                TypeGenerators.Add(add);
            }
        }

        /// <summary>
        /// Removes a function at the given index
        /// </summary>
        public virtual void RemoveAt(int index)
        {
            lock (_lockObject)
            {
                TypeGenerators.RemoveAt(index);
            }
        }

        /// <summary>
        /// Gets a list of the created objects 
        /// </summary>
        public virtual IEnumerable<T> GetItems()
        {
            lock (_lockObject)
            {
                if (TypeGenerators == null)
                {
                    return null;
                }

                return TypeGenerators.Select(f => f());
            }
        }
    }
}
