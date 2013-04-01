using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;

//The namespace is kept the same as the original
namespace Glass.Sitecore.Mapper
{
    public class SitecoreContext : Glass.Mapper.Sc.SitecoreContext, ISitecoreContext
    {
        public T CreateClass<T>(bool isLazy, bool inferType, Item item) where T : class
        {
            return CreateType<T>(item, isLazy, inferType);
        }

        public T CreateClass<T, K>(bool isLazy, bool inferType, Item item, K param1)
        {
            return CreateType<T, K>(item, param1, isLazy, inferType);
        }

        public T CreateClass<T, K, L>(bool isLazy, bool inferType, Item item, K param1, L param2)
        {
            return CreateType<T, K, L>(item, param1, param2, isLazy, inferType);
        }

        public T CreateClass<T, K, L, M>(bool isLazy, bool inferType, Item item, K param1, L param2, M param3)
        {
            return CreateType<T, K, L, M>(item, param1, param2, param3, isLazy, inferType);
        }

        public T CreateClass<T, K, L, M, N>(bool isLazy, bool inferType, Item item, K param1, L param2, M param3, N param4)
        {
            return CreateType<T, K, L, M, N>(item, param1, param2, param3, param4, isLazy, inferType);
        }

        public object CreateClass(bool isLazy, bool inferType, Type type, Item item)
        {
            return CreateType(type, item, isLazy, inferType);
        }

        public System.Collections.IEnumerable CreateClasses(bool isLazy, Type type, Func<IEnumerable<Item>> getItems)
        {
            return CreateTypes(type, getItems, isLazy);
        }

        public System.Collections.IEnumerable CreateClasses(bool isLazy, bool inferType, Type type, Func<IEnumerable<Item>> getItems)
        {
            return CreateTypes(type, getItems, isLazy, inferType);
        }
    }
}
