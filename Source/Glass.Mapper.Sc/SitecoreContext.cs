/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-

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

        public IEnumerable<T> QueryRelative<T>(string query, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = global::Sitecore.Context.Item;
            var results = item.Axes.SelectItems(query);
            return base.CreateTypes(isLazy, inferType, typeof(T), () => { return results; }) as IEnumerable<T>;

        }

        public T QuerySingleRelative<T>(string query, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = global::Sitecore.Context.Item;
            var result = item.Axes.SelectSingleItem(query);
            return base.CreateType<T>(result, isLazy, inferType);
        }


        public object GetCurrentItem(Type type, bool isLazy = false, bool inferType = false)
        {
            return base.CreateType(type, global::Sitecore.Context.Item, isLazy, inferType);
        }

        public T GetCurrentItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {
            return base.CreateType<T>(global::Sitecore.Context.Item, isLazy, inferType);
        }

        public T GetCurrentItem<T, K>(K param1, bool isLazy = false, bool inferType = false) where T : class
        {

            return base.CreateType<T, K>(global::Sitecore.Context.Item, param1, isLazy, inferType);

        }

        public T GetCurrentItem<T, K, L>(K param1, L param2, bool isLazy = false, bool inferType = false) where T : class
        {
            return base.CreateType<T, K, L>(global::Sitecore.Context.Item, param1, param2, isLazy, inferType);

        }

        public T GetCurrentItem<T, K, L, M>(K param1, L param2, M param3, bool isLazy = false, bool inferType = false) where T : class
        {
            return base.CreateType<T, K, L, M>(global::Sitecore.Context.Item, param1, param2, param3, isLazy, inferType);

        }

        public T GetCurrentItem<T, K, L, M, N>(K param1, L param2, M param3, N param4, bool isLazy = false, bool inferType = false) where T : class
        {
            return base.CreateType<T, K, L, M, N>(global::Sitecore.Context.Item, param1, param2, param3, param4, isLazy, inferType);

        }

        #endregion

    }
}



