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
        /// Initializes a new instance of the <see cref="SitecoreService"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="contextName">Name of the context.</param>
        public SitecoreService(Database database, string contextName = "Default")
            : base(database, contextName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreService" /> class.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="contextName">Name of the context.</param>
        public SitecoreService(string databaseName, string contextName = "Default")
            : base(databaseName, contextName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreService" /> class.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="context">The context.</param>
        public SitecoreService(string databaseName, Context context)
            : base(databaseName, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreService" /> class.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="context">The context.</param>
        public SitecoreService(Database database, Context context)
            : base(database, context)
        {
        }

        /// <summary>
        /// Creates a class from the specified item
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <param name="item">The item to load data from</param>
        /// <returns>
        /// The item as the specified type
        /// </returns>
        public T CreateClass<T>(bool isLazy, bool inferType, Item item) where T : class
        {
            return CreateType<T>(item, isLazy, inferType);
        }

        /// <summary>
        /// Creates a class from the specified item with a single constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <param name="item">The item to load data from</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <returns>
        /// The item as the specified type
        /// </returns>
        public T CreateClass<T, TK>(bool isLazy, bool inferType, Item item, TK param1)
        {
            return CreateType<T, TK>(item, param1, isLazy, inferType);
        }

        /// <summary>
        /// Creates a class from the specified item with a two constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <param name="item">The item to load data from</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <returns>
        /// The item as the specified type
        /// </returns>
        public T CreateClass<T, TK, TL>(bool isLazy, bool inferType, Item item, TK param1, TL param2)
        {
            return CreateType<T, TK, TL>(item, param1, param2, isLazy, inferType);
        }

        /// <summary>
        /// Creates a class from the specified item with a two constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM"></typeparam>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <param name="item">The item to load data from</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3"></param>
        /// <returns>
        /// The item as the specified type
        /// </returns>
        public T CreateClass<T, TK, TL, TM>(bool isLazy, bool inferType, Item item, TK param1, TL param2, TM param3)
        {
            return CreateType<T, TK, TL, TM>(item, param1, param2, param3, isLazy, inferType);
        }

        /// <summary>
        /// Creates a class from the specified item with a two constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM"></typeparam>
        /// <typeparam name="TN"></typeparam>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <param name="item">The item to load data from</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3"></param>
        /// <param name="param4"></param>
        /// <returns>
        /// The item as the specified type
        /// </returns>
        public T CreateClass<T, TK, TL, TM, TN>(bool isLazy, bool inferType, Item item, TK param1, TL param2, TM param3, TN param4)
        {
            return CreateType<T, TK, TL, TM, TN>(item, param1, param2, param3, param4, isLazy, inferType);
        }

        /// <summary>
        /// Creates a class from the specified item
        /// </summary>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <param name="type">The type to return</param>
        /// <param name="item">The item to load data from</param>
        /// <returns>
        /// The item as the specified type
        /// </returns>
        public object CreateClass(bool isLazy, bool inferType, Type type, Item item)
        {
            return CreateType(type, item, isLazy, inferType, null);
        }

        /// <summary>
        /// Create a collection of classes from the specified type
        /// </summary>
        /// <param name="isLazy">If true creates a proxy for each class</param>
        /// <param name="type">The type to return</param>
        /// <param name="getItems">A function that returns the list of items to load</param>
        /// <returns>
        /// An enumerable of the items as the specified type
        /// </returns>
        public System.Collections.IEnumerable CreateClasses(bool isLazy, Type type, Func<IEnumerable<Item>> getItems)
        {
            return CreateTypes(type, getItems, isLazy);
        }

        /// <summary>
        /// Create a collection of classes from the specified type
        /// </summary>
        /// <param name="isLazy">If true creates a proxy for each class</param>
        /// <param name="inferType">Infer the type to be loaded from the item template</param>
        /// <param name="type">The type to return</param>
        /// <param name="getItems">A function that returns the list of items to load</param>
        /// <returns>
        /// An enumerable of the items as the specified type
        /// </returns>
        public System.Collections.IEnumerable CreateClasses(bool isLazy, bool inferType, Type type, Func<IEnumerable<Item>> getItems)
        {
            return CreateTypes(type, getItems, isLazy, inferType);
        }
    }
}

