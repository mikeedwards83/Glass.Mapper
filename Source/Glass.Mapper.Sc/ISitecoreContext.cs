using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc
{
   public interface ISitecoreContext : ISitecoreService
    {
        /// <summary>
        /// Retrieves the current item as the specified type
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <returns>The current item as the specified type</returns>
        T GetCurrentItem<T>(bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieves the current item as the specified type
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <returns>The Sitecore item as the specified type</returns>
        T GetCurrentItem<T, K>(K param1, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieves the current item as the specified type
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <returns>The Sitecore item as the specified type</returns>
        T GetCurrentItem<T, K, L>(K param1, L param2, bool isLazy = false, bool inferType = false) where T : class;

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
        /// <returns>The Sitecore item as the specified type</returns>
        T GetCurrentItem<T, K, L, M>(K param1, L param2, M param3, bool isLazy = false, bool inferType = false) where T : class;

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
        /// <returns>The Sitecore item as the specified type</returns>
        T GetCurrentItem<T, K, L, M, N>(K param1, L param2, M param3, N param4, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieves the current item as the specified type
        /// </summary>
        /// <param name="type">The type to return.</param>
        /// <returns>The current item as the specified type</returns>
        object GetCurrentItem(Type type, bool isLazy = false, bool inferType = false);

        /// <summary>
        /// Performs a query relative to the current item
        /// </summary>
        /// <typeparam name="T">The type to cast classes to</typeparam>
        /// <returns></returns>
        IEnumerable<T> QueryRelative<T>(string query, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Performs a query relative to the current item
        /// </summary>
        /// <typeparam name="T">The type to cast classes to</typeparam>
        /// <returns></returns>
        T QuerySingleRelative<T>(string query, bool isLazy = false, bool inferType = false) where T : class;

        T GetHomeItem<T>(bool isLazy = false, bool inferType = false) where T : class;
    }
}

