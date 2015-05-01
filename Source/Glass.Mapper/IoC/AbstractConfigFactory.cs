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
            TypeGenerators.Insert(index, add);
        }

        public virtual void Add(Func<T> add)
        {
                TypeGenerators.Add(add);
        }

        protected abstract void AddTypes();

        public virtual IEnumerable<T> GetItems()
        {
            if (TypeGenerators.Count == 0)
            {
                AddTypes();
            }

            return TypeGenerators != null
                ? TypeGenerators.Select(f => f())
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
