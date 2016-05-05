using System;
using System.Collections.Generic;

namespace Glass.Mapper.IoC
{
    public interface IConfigFactory<T>
    {
        void Insert<TK>(int index, Func<TK> add) where TK : T;

        void Add<TK>(Func<TK> add) where TK : T;

        void First<TK>(Func<TK> add) where TK : T;

        void Replace<TK>(int index, Func<TK> replace) where TK : T;

        void InsertBefore<TBefore, TK>(Func<TK> func) where TBefore : T where TK : T;

        void InsertAfter<TAfter, TK>(Func<TK> func) where TAfter : T where TK : T;

        IEnumerable<T> GetItems();

        void RemoveAt(int index);
    }
}
