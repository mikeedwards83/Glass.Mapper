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
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// Class SitecoreContext
    /// </summary>
    public class SitecoreContext : SitecoreService, ISitecoreContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreContext"/> class.
        /// </summary>
        public SitecoreContext()
            : base(global::Sitecore.Context.Database, GetContextFromSite())
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractService" /> class.
        /// </summary>
        /// <param name="contextName">Name of the context.</param>
        public SitecoreContext(string contextName)
            : base(global::Sitecore.Context.Database, contextName)
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreContext"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public SitecoreContext(Context context)
            : base(global::Sitecore.Context.Database, context)
        {

        }

        /// <summary>
        /// Used for unit tests only
        /// </summary>
        /// <param name="database"></param>
        internal SitecoreContext(Database database):
            base(database, GetContextFromSite())
        {
            
        }
        private static string GetContextFromSite()
        {
            if (Sitecore.Context.Site == null)
                return Context.DefaultContextName;

            return Sitecore.Context.Site.Properties["glassContext"] ?? Context.DefaultContextName;
        }

        #region ISitecoreContext Members






        /// <summary>
        /// Gets the home item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>``0.</returns>
        public T GetHomeItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {

            return base.GetItem<T>(global::Sitecore.Context.Site.StartPath, isLazy, inferType);
        }

        /// <summary>
        /// Performs a query relative to the current item
        /// </summary>
        /// <typeparam name="T">The type to cast classes to</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>IEnumerable{``0}.</returns>
        public IEnumerable<T> QueryRelative<T>(string query, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = global::Sitecore.Context.Item;
            var results = item.Axes.SelectItems(query);
            return base.CreateTypes(typeof(T), () => { return results; }, isLazy, inferType) as IEnumerable<T>;

        }

        /// <summary>
        /// Performs a query relative to the current item
        /// </summary>
        /// <typeparam name="T">The type to cast classes to</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>``0.</returns>
        public T QuerySingleRelative<T>(string query, bool isLazy = false, bool inferType = false) where T : class
        {
            Item item = global::Sitecore.Context.Item;
            var result = item.Axes.SelectSingleItem(query);
            return base.CreateType<T>(result, isLazy, inferType);
        }


        /// <summary>
        /// Retrieves the current item as the specified type
        /// </summary>
        /// <returns>
        /// The current item as the specified type
        /// </returns>
        public dynamic GetCurrentDynamicItem()
        {
            return base.GetDynamicItem(global::Sitecore.Context.Item);
        }

        /// <summary>
        /// Retrieves the current item as the specified type
        /// </summary>
        /// <param name="type">The type to return.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The current item as the specified type</returns>
        public object GetCurrentItem(Type type, bool isLazy = false, bool inferType = false)
        {
            return base.CreateType(type, global::Sitecore.Context.Item, isLazy, inferType);
        }

        /// <summary>
        /// Retrieves the current item as the specified type
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The current item as the specified type</returns>
        public T GetCurrentItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {
            return base.CreateType<T>(global::Sitecore.Context.Item, isLazy, inferType);
        }

        /// <summary>
        /// Retrieves the current item as the specified type
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetCurrentItem<T, K>(K param1, bool isLazy = false, bool inferType = false) where T : class
        {

            return base.CreateType<T, K>(global::Sitecore.Context.Item, param1, isLazy, inferType);

        }

        /// <summary>
        /// Retrieves the current item as the specified type
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetCurrentItem<T, K, L>(K param1, L param2, bool isLazy = false, bool inferType = false) where T : class
        {
            return base.CreateType<T, K, L>(global::Sitecore.Context.Item, param1, param2, isLazy, inferType);

        }

        /// <summary>
        /// Retrieves the current item as the specified type
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetCurrentItem<T, K, L, M>(K param1, L param2, M param3, bool isLazy = false, bool inferType = false) where T : class
        {
            return base.CreateType<T, K, L, M>(global::Sitecore.Context.Item, param1, param2, param3, isLazy, inferType);

        }

        /// <summary>
        /// Retrieves the current item as the specified type
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <typeparam name="N">The type of the fourth constructor parameter</typeparam>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetCurrentItem<T, K, L, M, N>(K param1, L param2, M param3, N param4, bool isLazy = false, bool inferType = false) where T : class
        {
            return base.CreateType<T, K, L, M, N>(global::Sitecore.Context.Item, param1, param2, param3, param4, isLazy, inferType);

        }

        #endregion

    }
}




