using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    public class SitecoreContext : SitecoreService, ISitecoreContext
    {
        public SitecoreContext()
            : base(global::Sitecore.Context.Database)
        {

        }
        #region ISitecoreContext Members




        public T GetHomeItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {

            return base.GetItem<T>(global::Sitecore.Context.Site.StartPath, isLazy, inferType);
        }

        public IEnumerable<T> QueryRelative<T>(string query, bool isLazy, bool inferType) where T : class
        {
            Item item = global::Sitecore.Context.Item;
            var results = item.Axes.SelectItems(query);
            return base.CreateClasses(isLazy, inferType, typeof(T), () => { return results; }) as IEnumerable<T>;

        }

        public T QuerySingleRelative<T>(string query, bool isLazy, bool inferType) where T : class
        {
            Item item = global::Sitecore.Context.Item;
            var result = item.Axes.SelectSingleItem(query);
            return base.CreateClass<T>(result,isLazy, inferType);
        }


        public object GetCurrentItem(Type type, bool isLazy = false, bool inferType = false)
        {
            return base.CreateClass(type, global::Sitecore.Context.Item, isLazy, inferType);
        }

        public T GetCurrentItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {
            return base.CreateClass<T>(global::Sitecore.Context.Item,isLazy, inferType);
        }

        public T GetCurrentItem<T, K>(K param1, bool isLazy = false, bool inferType = false) where T : class
        {

            return base.CreateClass<T, K>(global::Sitecore.Context.Item, param1,isLazy, inferType);

        }

        public T GetCurrentItem<T, K, L>(K param1, L param2, bool isLazy = false, bool inferType = false) where T : class
        {
            return base.CreateClass<T, K, L>(global::Sitecore.Context.Item, param1, param2,isLazy, inferType);

        }

        public T GetCurrentItem<T, K, L, M>(K param1, L param2, M param3, bool isLazy = false, bool inferType = false) where T : class
        {
            return base.CreateClass<T, K, L, M>(global::Sitecore.Context.Item, param1, param2, param3, isLazy, inferType);

        }

        public T GetCurrentItem<T, K, L, M, N>(K param1, L param2, M param3, N param4, bool isLazy = false, bool inferType = false) where T : class
        {
            return base.CreateClass<T, K, L, M, N>(global::Sitecore.Context.Item, param1, param2, param3, param4, isLazy, inferType);

        }

        #endregion

    }
}
