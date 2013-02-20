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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Dynamic;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace Glass.Mapper.Sc
{
    public interface ISitecoreService
    {
        Database Database { get; }
        Context GlassContext { get; }

        /// <summary>
        /// Adds a version of the item
        /// </summary>
        /// <typeparam name="T">The type being added. The type must have a property with the SitecoreIdAttribute.</typeparam>
        /// <param name="target">The class to add a version to</param>
        /// <returns></returns>
        T AddVersion<T>(T target) where T : class;

        T GetItem<T>(Guid id, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>        
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <returns>The Sitecore item as the specified type</returns>
        T GetItem<T>(Guid id, Language language, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>        
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <returns>The Sitecore item as the specified type</returns>
        T GetItem<T, K>(Guid id, Language language, K param1, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>        
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <returns>The Sitecore item as the specified type</returns>
        T GetItem<T, K, L>(Guid id, Language language, K param1, L param2, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>        
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <returns>The Sitecore item as the specified type</returns>
        T GetItem<T, K, L, M>(Guid id, Language language, K param1, L param2, M param3, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <typeparam name="N">The type of the fourth constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>        
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <returns>The Sitecore item as the specified type</returns>
        T GetItem<T, K, L, M, N>(Guid id, Language language, K param1, L param2, M param3, N param4, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>        
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <returns>The Sitecore item as the specified type</returns>
        T GetItem<T>(string path, Language language, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>        
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <returns>The Sitecore item as the specified type</returns>
        T GetItem<T, K>(string path, Language language, K param1, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>        
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <returns>The Sitecore item as the specified type</returns>
        T GetItem<T, K, L>(string path, Language language, K param1, L param2, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>        
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <returns>The Sitecore item as the specified type</returns>
        T GetItem<T, K, L, M>(string path, Language language, K param1, L param2, M param3, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <typeparam name="N">The type of the fourth constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>        
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <returns>The Sitecore item as the specified type</returns>
        T GetItem<T, K, L, M, N>(string path, Language language, K param1, L param2, M param3, N param4, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <returns>The Sitecore item as the specified type</returns>
        T GetItem<T>(string path, Language language, global::Sitecore.Data.Version version, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <returns>The Sitecore item as the specified type</returns>
        T GetItem<T, K>(string path, Language language, global::Sitecore.Data.Version version, K param1, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <returns>The Sitecore item as the specified type</returns>
        T GetItem<T, K, L>(string path, Language language, global::Sitecore.Data.Version version, K param1, L param2, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <returns>The Sitecore item as the specified type</returns>
        T GetItem<T, K, L, M>(string path, Language language, global::Sitecore.Data.Version version, K param1, L param2, M param3, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <typeparam name="N">The type of the fourth constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <returns>The Sitecore item as the specified type</returns>
        T GetItem<T, K, L, M, N>(string path, Language language, global::Sitecore.Data.Version version, K param1, L param2, M param3, N param4, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <returns>The Sitecore item as the specified type</returns>
        T GetItem<T>(Guid id, Language language, global::Sitecore.Data.Version version, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <returns>The Sitecore item as the specified type</returns>
        T GetItem<T, K>(Guid id, Language language, global::Sitecore.Data.Version version, K param1, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <returns>The Sitecore item as the specified type</returns>
        T GetItem<T, K, L>(Guid id, Language language, global::Sitecore.Data.Version version, K param1, L param2, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <returns>The Sitecore item as the specified type</returns>
        T GetItem<T, K, L, M>(Guid id, Language language, global::Sitecore.Data.Version version, K param1, L param2, M param3, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <typeparam name="N">The type of the fourth constructor parameter</typeparam>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <returns>The Sitecore item as the specified type</returns>
        T GetItem<T, K, L, M, N>(Guid id, Language language, global::Sitecore.Data.Version version, K param1, L param2, M param3, N param4, bool isLazy = false, bool inferType = false) where T : class;

        T GetItem<T>(string path, bool isLazy = false, bool inferType = false) where T : class;
        void Save<T>(T target);

        /// <summary>
        /// Query Sitecore for a set of items. Proxy classes are created.
        /// </summary>
        /// <typeparam name="T">The type to return the items as</typeparam>
        /// <param name="query">The query to execute</param>
        /// <returns>Sitecore items as proxy classes of the specified type</returns>
        IEnumerable<T> Query<T>(string query, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Query Sitecore for a single item. 
        /// </summary>
        /// <typeparam name="T">The type to return the items as</typeparam>
        /// <param name="query">The query to execute</param>
        /// <returns>Sitecore item as the specified type</returns>
        T QuerySingle<T>(string query, bool isLazy = false, bool inferType = false) where T : class;

        void WriteToItem<T>(T target, Item item, SitecoreTypeConfiguration config = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="item"></param>
        /// <param name="isLazy"></param>
        /// <param name="inferType"></param>
        /// <param name="constructorParameters">Parameters to pass to the constructor of the new class. Must be in the order specified on the consturctor.</param>
        /// <returns></returns>
        object CreateType(Type type, Item item, bool isLazy, bool inferType, params object[] constructorParameters);

        /// <summary>
        /// Create a collection of classes from the specified type
        /// </summary>
        /// <param name="isLazy">If true creates a proxy for each class</param>
        /// <param name="type">The type to return</param>
        /// <param name="getItems">A function that returns the list of items to load</param>
        /// <param name="inferType">Infer the type to be loaded from the item template</param>
        /// <returns>An enumerable of the items as the specified type</returns>
        IEnumerable CreateTypes(bool isLazy, bool inferType, Type type, Func<IEnumerable<Item>> getItems);

        AbstractDataMappingContext CreateDataMappingContext(AbstractTypeCreationContext abstractTypeCreationContext, Object obj);
        AbstractDataMappingContext CreateDataMappingContext(AbstractTypeSavingContext creationContext);

        /// <summary>
        /// Creates a class from the specified item
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="item">The item to load data from</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        T CreateType<T>(Item item, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Creates a class from the specified item with a single constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="item">The item to load data from</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <returns>The item as the specified type</returns>
        T CreateType<T, K>(Item item, K param1, bool isLazy = false, bool inferType = false);

        /// <summary>
        /// Creates a class from the specified item with a two constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="item">The item to load data from</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <returns>The item as the specified type</returns>
        T CreateType<T, K, L>(Item item, K param1, L param2, bool isLazy = false, bool inferType = false);

        /// <summary>
        /// Creates a class from the specified item with a two constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="item">The item to load data from</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <returns>The item as the specified type</returns>
        T CreateType<T, K, L, M>(Item item, K param1, L param2, M param3, bool isLazy = false, bool inferType = false);

        /// <summary>
        /// Creates a class from the specified item with a two constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <typeparam name="N">The type of the fourth constructor parameter</typeparam>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="item">The item to load data from</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <returns>The item as the specified type</returns>
        T CreateType<T, K, L, M, N>(Item item, K param1, L param2, M param3, N param4, bool isLazy = false, bool inferType = false);

        /// <summary>
        /// Creates a new Sitecore item. 
        /// </summary>
        /// <typeparam name="T">The type of the new item to create. This type must have either a TemplateId or BranchId defined on the SitecoreClassAttribute or fluent equivalent</typeparam>
        /// <typeparam name="K">The type of the parent item</typeparam>
        /// <param name="parent">The parent of the new item to create. Must have the SitecoreIdAttribute or fluent equivalent</param>
        /// <param name="newItem">New item to create, must have the attribute SitecoreInfoAttribute of type SitecoreInfoType.Name or the fluent equivalent</param>
        /// <returns></returns>
        T Create<T, K>(K parent, T newItem)
            where T : class
            where K : class;

        /// <summary>
        /// Delete an item from Sitecore
        /// </summary>
        /// <typeparam name="T">The type being deleted. The type must have a property with the SitecoreIdAttribute.</typeparam>
        /// <param name="item">The class to delete</param>
        void Delete<T>(T item) where T : class;

        void Move<T, K>(T item, K newParent);
        object InstantiateObject(AbstractTypeCreationContext abstractTypeCreationContext);
        void SaveObject(AbstractTypeSavingContext abstractTypeSavingContext);

        /// <summary>
        /// Returns a dynamic item that can be used with the dynamic keyword
        /// </summary>
        /// <param name="id">The ID of the item to return</param>
        /// <returns></returns>
        dynamic GetDynamicItem(Guid id);
        /// <summary>
        /// Returns a dynamic item that can be used with the dynamic keyword
        /// </summary>
        /// <param name="path">The path of the item to return</param>
        /// <returns></returns>
        dynamic GetDynamicItem(string path);
        /// <summary>
        /// Returns a dynamic item that can be used with the dynamic keyword
        /// </summary>
        /// <param name="item">The item to convert</param>
        /// <returns></returns>
        dynamic GetDynamicItem(Item item);

    }
}



