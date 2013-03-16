using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper;
using Sitecore.Data;
using Sitecore.Data.Items;

//The namespace is kept the same as the original
namespace Glass.Sitecore.Mapper
{
    /// <summary>
    /// This is class is the upgrade class for Glass.Sitecore.Mapper.SitecoreService
    /// </summary>
    public class SitecoreService : Glass.Mapper.Sc.SitecoreService, ISitecoreService
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Sc.SitecoreService"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="contextName">Name of the context.</param>
        public SitecoreService(Database database, string contextName = "Default")
            : base(database, contextName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sc.SitecoreService"/> class.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="contextName">Name of the context.</param>
        public SitecoreService(string databaseName, string contextName = "Default")
            : base(databaseName, contextName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sc.SitecoreService"/> class.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="context">The context.</param>
        public SitecoreService(string databaseName, Context context)
            : base(databaseName, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sc.SitecoreService"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="context">The context.</param>
        public SitecoreService(Database database, Context context)
            : base(database, context)
        {
        }

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
