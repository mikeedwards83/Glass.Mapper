using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.IoC
{
    public abstract class AbstractConfigFactory<T> : IConfigFactory<T>
    {
        private List<Func<T>> _typeGenerators = null;

        protected List<Func<T>> TypeGenerators
        {
            get
            {
                if (_typeGenerators == null)
                {
                    lock (this)
                    {
                        if (_typeGenerators == null)
                        {
                            _typeGenerators = new List<Func<T>>();
                            AddTypes();
                        }
                    }
                }
                return _typeGenerators;
            }
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
