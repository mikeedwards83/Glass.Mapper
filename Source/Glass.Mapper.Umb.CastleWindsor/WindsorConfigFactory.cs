using System;
using System.Collections.Generic;
using Castle.Windsor;
using Glass.Mapper.IoC;

namespace Glass.Mapper.Umb.CastleWindsor
{
    public class WindsorConfigFactory<T> : IConfigFactory<T>
    {
        private IWindsorContainer container;

        public WindsorConfigFactory(IWindsorContainer container)
        {
            this.container = container;
        }

        public void Insert(int index, Func<T> add)
        {
            throw new NotSupportedException("Configuration cannot be added to container based config factories");
        }

        public void Add(Func<T> add)
        {
            throw new NotSupportedException("Configuration cannot be added to container based config factories");
        }

        public void First(Func<T> add)
        {
            throw new NotSupportedException("Configuration cannot be added to container based config factories");

        }

        public void Replace(int index, Func<T> replace)
        {
            throw new NotSupportedException("Configuration cannot be added to container based config factories");

        }

        public T[] GetItems()
        {
            return container.ResolveAll<T>();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException("Configuration cannot be added to container based config factories");

        }
    }
}
