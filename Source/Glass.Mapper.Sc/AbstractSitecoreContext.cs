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
using System.Web;
using Glass.Mapper.Sc.IoC;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// Class SitecoreContext
    /// </summary>
    public abstract class AbstractSitecoreContext : SitecoreService, ISitecoreContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreContext"/> class.
        /// </summary>
        protected AbstractSitecoreContext(Database database, Context context)
            : base(database, context)
        {
            
        }

        protected AbstractSitecoreContext(Database database, string contextName)
            : base(database, contextName)
        {

        }

        #region AbstractSitecoreContext Members






        /// <summary>
        /// Gets the home item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>``0.</returns>
        public T GetHomeItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {

            return GetItem<T>(Sitecore.Context.Site.StartPath, isLazy, inferType);
        }


        /// <summary>
        /// Gets the root item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>``0.</returns>
        public T GetRootItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {

            return GetItem<T>(Sitecore.Context.Site.RootPath, isLazy, inferType);
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
            Item item = Sitecore.Context.Item;
            var results = item.Axes.SelectItems(query);
            return CreateTypes(typeof(T), () => { return results; }, isLazy, inferType) as IEnumerable<T>;

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
            Item item = Sitecore.Context.Item;
            var result = item.Axes.SelectSingleItem(query);
            return CreateType<T>(result, isLazy, inferType);
        }


        /// <summary>
        /// Retrieves the current item as the specified type
        /// </summary>
        /// <returns>
        /// The current item as the specified type
        /// </returns>
        public dynamic GetCurrentDynamicItem()
        {
            return GetDynamicItem(Sitecore.Context.Item);
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
            return CreateType(type, Sitecore.Context.Item, isLazy, inferType, null);
        }

        private IGlassHtml _glassHtml = null;

        public IGlassHtml GlassHtml
        {
            get
            {
                if (_glassHtml == null)
                {
                    IDependencyResolver dependencyResolver = GlassContext.DependencyResolver as IDependencyResolver;
                    if (dependencyResolver != null)
                    {
                        _glassHtml = dependencyResolver.GlassHtmlFactory.GetGlassHtml(this);
                    }
                }
                return _glassHtml;
            } 
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
            return CreateType<T>(Sitecore.Context.Item, isLazy, inferType);
        }

        /// <summary>
        /// Retrieves the current item as the specified type
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetCurrentItem<T, TK>(TK param1, bool isLazy = false, bool inferType = false) where T : class
        {

            return CreateType<T, TK>(Sitecore.Context.Item, param1, isLazy, inferType);

        }

        /// <summary>
        /// Retrieves the current item as the specified type
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetCurrentItem<T, TK, TL>(TK param1, TL param2, bool isLazy = false, bool inferType = false) where T : class
        {
            return CreateType<T, TK, TL>(Sitecore.Context.Item, param1, param2, isLazy, inferType);

        }

        /// <summary>
        /// Retrieves the current item as the specified type
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetCurrentItem<T, TK, TL, TM>(TK param1, TL param2, TM param3, bool isLazy = false, bool inferType = false) where T : class
        {
            return CreateType<T, TK, TL, TM>(Sitecore.Context.Item, param1, param2, param3, isLazy, inferType);

        }

        /// <summary>
        /// Retrieves the current item as the specified type
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <typeparam name="TN">The type of the fourth constructor parameter</typeparam>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        public T GetCurrentItem<T, TK, TL, TM, TN>(TK param1, TL param2, TM param3, TN param4, bool isLazy = false, bool inferType = false) where T : class
        {
            return CreateType<T, TK, TL, TM, TN>(Sitecore.Context.Item, param1, param2, param3, param4, isLazy, inferType);

        }

        #endregion


        
    }
}




