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
            ProcessTypes();
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

        protected abstract void AddTypes();

        public virtual IEnumerable<T> GetItems()
        {
            return TypeGenerators != null
                ? TypeGenerators.Select(f => f())
                : null;
        }

        protected void ProcessTypes()
        {
            AddTypes();
        }
    }

    public interface IConfigFactory<T>
    {
        void Insert(int index, Func<T> add);
        void Add(Func<T> add);
        IEnumerable<T> GetItems();
    }
}
