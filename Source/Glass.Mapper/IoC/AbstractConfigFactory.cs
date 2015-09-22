using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.IoC
{
    public abstract class AbstractConfigFactory<T> : IConfigFactory<T>
    {
        protected List<Func<T>> TypeGenerators { get; private set; }

        protected AbstractConfigFactory()
        {
            TypeGenerators = new List<Func<T>>();
        }

        public void Insert(int index,Func<T> add)
        {
            if (add == null)
            {
                throw new NullReferenceException("Cannot insert with a null function");
            }

            lock (TypeGenerators)
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
        /// <param name="add"></param>
        public virtual void Replace(int index, Func<T> replace)
        {
            if (replace == null)
            {
                throw new NullReferenceException("Cannot replace with a null function");
            }

            lock (TypeGenerators)
            {
                TypeGenerators[index] = replace;
            }
        }

        public virtual void Add(Func<T> add)
        {
            if (add == null)
            {
                throw new NullReferenceException("Cannot add with a null function");
            }

            lock (TypeGenerators)
            {
                TypeGenerators.Add(add);
            }
        }
        public virtual void RemoveAt(int index)
        {
            lock (TypeGenerators)
            {
                TypeGenerators.RemoveAt(index);
            }
        }

        public virtual IEnumerable<T> GetItems()
        {
            return TypeGenerators != null
                ? TypeGenerators.Select(f => f()).ToArray() //we want to force enumeration to avoid race conditions.s
                : null;
        }
    }

    public interface IConfigFactory<T>
    {
        void Insert(int index, Func<T> add);
        void Add(Func<T> add);
        void First(Func<T> add);
        void Replace(int index, Func<T> replace);
        IEnumerable<T> GetItems();
        void RemoveAt(int index);
    }
}
