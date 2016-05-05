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

        public void Insert<TK>(int index, Func<TK> add) where TK : T
        {
            throw new NotSupportedException("Configuration cannot be added to container based config factories");
        }

        public void Add<TK>(Func<TK> add) where TK : T
        {
            throw new NotSupportedException("Configuration cannot be added to container based config factories");
        }

        public void First<TK>(Func<TK> add) where TK : T
        {
            throw new NotSupportedException("Configuration cannot be added to container based config factories");
        }

        public void Replace<TK>(int index, Func<TK> replace) where TK : T
        {
            throw new NotSupportedException("Configuration cannot be added to container based config factories");
        }

        public void InsertBefore<TBefore, TK>(Func<TK> func) where TBefore : T where TK : T
        {
            throw new NotSupportedException("Configuration cannot be added to container based config factories");
        }

        public void InsertAfter<TAfter, TK>(Func<TK> func) where TAfter : T where TK : T
        {
            throw new NotSupportedException("Configuration cannot be added to container based config factories");
        }

        public IEnumerable<T> GetItems()
        {
            return container.ResolveAll<T>();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException("Configuration cannot be added to container based config factories");

        }
    }
}
