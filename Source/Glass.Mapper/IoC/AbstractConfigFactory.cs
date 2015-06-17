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
            lock (TypeGenerators)
            {
                TypeGenerators.Insert(index, add);
            }
        }

        public virtual void Add(Func<T> add)
        {
            lock (TypeGenerators)
            {
                TypeGenerators.Add(add);
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
        IEnumerable<T> GetItems();
    }
}
