using System;
using System.Collections.Generic;

namespace Glass.Mapper.IoC
{
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
