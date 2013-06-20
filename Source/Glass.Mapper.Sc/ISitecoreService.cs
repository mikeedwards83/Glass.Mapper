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
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// Interface ISitecoreService
    /// </summary>
    public interface ISitecoreService
    {
        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <value>The database.</value>
        Database Database { get; }
        /// <summary>
        /// Gets the glass context.
        /// </summary>
        /// <value>The glass context.</value>
        Context GlassContext { get; }

        #region  AddVersion

        /// <summary>
        /// Adds a version of the item
        /// </summary>
        /// <typeparam name="T">The type being added. The type must have a property with the SitecoreIdAttribute.</typeparam>
        /// <param name="target">The class to add a version to</param>
        /// <returns>``0.</returns>
        T AddVersion<T>(T target) where T : class;

        #endregion

        #region Create

        /// <summary>
        /// Creates a new Sitecore item.
        /// </summary>
        /// <typeparam name="T">The type of the new item to create. This type must have either a TemplateId or BranchId defined on the SitecoreClassAttribute or fluent equivalent</typeparam>
        /// <typeparam name="K">The type of the parent item</typeparam>
        /// <param name="parent">The parent of the new item to create. Must have the SitecoreIdAttribute or fluent equivalent</param>
        /// <param name="newItem">New item to create, must have the attribute SitecoreInfoAttribute of type SitecoreInfoType.Name or the fluent equivalent</param>
        /// <param name="updateStatistics">Indicates if the items stats should be updated when the item is saved</param>
        /// <param name="silent">If set to true, no events will be raised due to saving.</param>
        /// <returns>``0.</returns>
        T Create<T, K>(K parent, T newItem, bool updateStatistics = true, bool silent = false)
            where T : class
            where K : class;

        #endregion

        #region  CreateType

        /// <summary>
        /// Creates the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="item">The item.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <param name="constructorParameters">Parameters to pass to the constructor of the new class. Must be in the order specified on the consturctor.</param>
        /// <returns>System.Object.</returns>
        object CreateType(Type type, Item item, bool isLazy, bool inferType, params object[] constructorParameters);

        /// <summary>
        /// Creates a class from the specified item
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="item">The item to load data from</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        T CreateType<T>(Item item, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Creates a class from the specified item with a single constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <param name="item">The item to load data from</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        T CreateType<T, K>(Item item, K param1, bool isLazy = false, bool inferType = false);

        /// <summary>
        /// Creates a class from the specified item with a two constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <param name="item">The item to load data from</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        T CreateType<T, K, L>(Item item, K param1, L param2, bool isLazy = false, bool inferType = false);

        /// <summary>
        /// Creates a class from the specified item with a two constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <param name="item">The item to load data from</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
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
        /// <param name="item">The item to load data from</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        T CreateType<T, K, L, M, N>(Item item, K param1, L param2, M param3, N param4, bool isLazy = false, bool inferType = false);

        #endregion

        #region CreateTypes

        /// <summary>
        /// Create a collection of classes from the specified type
        /// </summary>
        /// <param name="isLazy">If true creates a proxy for each class</param>
        /// <param name="inferType">Infer the type to be loaded from the item template</param>
        /// <param name="type">The type to return</param>
        /// <param name="getItems">A function that returns the list of items to load</param>
        /// <returns>An enumerable of the items as the specified type</returns>
        IEnumerable CreateTypes(Type type, Func<IEnumerable<Item>> getItems, bool isLazy = false, bool inferType = false);

        #endregion

        #region Delete

        /// <summary>
        /// Delete an item from Sitecore
        /// </summary>
        /// <typeparam name="T">The type being deleted. The type must have a property with the SitecoreIdAttribute.</typeparam>
        /// <param name="item">The class to delete</param>
        void Delete<T>(T item) where T : class;

        #endregion

        #region Dynamics

        /// <summary>
        /// Returns a dynamic item that can be used with the dynamic keyword
        /// </summary>
        /// <param name="id">The ID of the item to return</param>
        /// <returns>dynamic.</returns>
        dynamic GetDynamicItem(Guid id);
        /// <summary>
        /// Returns a dynamic item that can be used with the dynamic keyword
        /// </summary>
        /// <param name="path">The path of the item to return</param>
        /// <returns>dynamic.</returns>
        dynamic GetDynamicItem(string path);
        /// <summary>
        /// Returns a dynamic item that can be used with the dynamic keyword
        /// </summary>
        /// <param name="item">The item to convert</param>
        /// <returns>dynamic.</returns>
        dynamic GetDynamicItem(Item item);

        #endregion

        #region GetItem - Path

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">The path.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// ``0.
        /// </returns>
        T GetItem<T>(string path, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="L"></typeparam>
        /// <param name="path">The path.</param>
        /// <param name="param1">The param1.</param>
        /// <param name="param2">The param2.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// ``0.
        /// </returns>
        T GetItem<T, K, L>(string path, K param1, L param2, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="L"></typeparam>
        /// <typeparam name="M"></typeparam>
        /// <param name="path">The path.</param>
        /// <param name="param1">The param1.</param>
        /// <param name="param2">The param2.</param>
        /// <param name="param3">The param3.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// ``0.
        /// </returns>
        T GetItem<T, K, L, M>(string path, K param1, L param2, M param3, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="L"></typeparam>
        /// <typeparam name="M"></typeparam>
        /// <typeparam name="N"></typeparam>
        /// <param name="path">The path.</param>
        /// <param name="param1">The param1.</param>
        /// <param name="param2">The param2.</param>
        /// <param name="param3">The param3.</param>
        /// <param name="param4">The param4.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// ``0.
        /// </returns>
        T GetItem<T, K, L, M, N>(string path, K param1, L param2, M param3, N param4, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="path">The path.</param>
        /// <param name="param1">The param1.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// ``0.
        /// </returns>
        T GetItem<T, K>(string path, K param1, bool isLazy = false, bool inferType = false) where T : class;

        #endregion

        #region GetItem - Path, Language

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        T GetItem<T>(string path, Language language, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        T GetItem<T, K>(string path, Language language, K param1, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        T GetItem<T, K, L>(string path, Language language, K param1, L param2, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        T GetItem<T, K, L, M>(string path, Language language, K param1, L param2, M param3, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <typeparam name="N">The type of the fourth constructor parameter</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        T GetItem<T, K, L, M, N>(string path, Language language, K param1, L param2, M param3, N param4, bool isLazy = false, bool inferType = false) where T : class;


        #endregion

        #region GetItem - Path, Language, Version


        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        T GetItem<T>(string path, Language language, global::Sitecore.Data.Version version, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        T GetItem<T, K>(string path, Language language, global::Sitecore.Data.Version version, K param1, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        T GetItem<T, K, L>(string path, Language language, global::Sitecore.Data.Version version, K param1, L param2, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
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
        /// <param name="path">The path.</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        T GetItem<T, K, L, M, N>(string path, Language language, global::Sitecore.Data.Version version, K param1, L param2, M param3, N param4, bool isLazy = false, bool inferType = false) where T : class;



        #endregion

        #region GetItem - Guid

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        T GetItem<T>(Guid id, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        T GetItem<T, K>(Guid id, K param1, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        T GetItem<T, K, L>(Guid id, K param1, L param2, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        T GetItem<T, K, L, M>(Guid id, K param1, L param2, M param3, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <typeparam name="N">The type of the fourth constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        T GetItem<T, K, L, M, N>(Guid id, K param1, L param2, M param3, N param4, bool isLazy = false, bool inferType = false) where T : class;


        #endregion

        #region GetItem - Guid, Language

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        T GetItem<T>(Guid id, Language language, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        T GetItem<T, K>(Guid id, Language language, K param1, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        T GetItem<T, K, L>(Guid id, Language language, K param1, L param2, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        T GetItem<T, K, L, M>(Guid id, Language language, K param1, L param2, M param3, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <typeparam name="N">The type of the fourth constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        T GetItem<T, K, L, M, N>(Guid id, Language language, K param1, L param2, M param3, N param4, bool isLazy = false, bool inferType = false) where T : class;





        #endregion

        #region GetItem - Guid, Language, Version

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        T GetItem<T>(Guid id, Language language, global::Sitecore.Data.Version version, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        T GetItem<T, K>(Guid id, Language language, global::Sitecore.Data.Version version, K param1, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        T GetItem<T, K, L>(Guid id, Language language, global::Sitecore.Data.Version version, K param1, L param2, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        T GetItem<T, K, L, M>(Guid id, Language language, global::Sitecore.Data.Version version, K param1, L param2, M param3, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <typeparam name="N">The type of the fourth constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        T GetItem<T, K, L, M, N>(Guid id, Language language, global::Sitecore.Data.Version version, K param1, L param2, M param3, N param4, bool isLazy = false, bool inferType = false) where T : class;


        #endregion

        #region Move

        /// <summary>
        /// Moves the specified item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="newParent">The new parent.</param>
        void Move<T, K>(T item, K newParent);

        #endregion
        
        #region Query

        /// <summary>
        /// Query Sitecore for a set of items. Proxy classes are created.
        /// </summary>
        /// <typeparam name="T">The type to return the items as</typeparam>
        /// <param name="query">The query to execute</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>Sitecore items as proxy classes of the specified type</returns>
        IEnumerable<T> Query<T>(string query, bool isLazy = false, bool inferType = false) where T : class;

        #endregion

        #region QuerySingle

        /// <summary>
        /// Query Sitecore for a single item.
        /// </summary>
        /// <typeparam name="T">The type to return the items as</typeparam>
        /// <param name="query">The query to execute</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>Sitecore item as the specified type</returns>
        T QuerySingle<T>(string query, bool isLazy = false, bool inferType = false) where T : class;


        #endregion

        #region Save

        /// <summary>
        /// Saves the specified target.
        /// </summary>
        /// <param name="updateStatistics">Indicates if the items stats should be updated when the item is saved</param>
        /// <param name="silent">If set to true, no events will be raised due to saving.</param>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target.</param>
        void Save<T>(T target, bool updateStatistics = true, bool silent = false);

        #endregion

        #region WriteToItem

        /// <summary>
        /// Writes to item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="updateStatistics">Indicates if the items stats should be updated when the item is saved</param>
        /// <param name="silent">If set to true, no events will be raised due to saving.</param>
        /// <param name="target">The object to read data from.</param>
        /// <param name="item">The item to write data to.</param>
        void WriteToItem<T>(T target, Item item, bool updateStatistics = true, bool silent = false);

        #endregion

       

       

   


        /// <summary>
        /// Creates the data mapping context.
        /// </summary>
        /// <param name="abstractTypeCreationContext">The abstract type creation context.</param>
        /// <param name="obj">The obj.</param>
        /// <returns>AbstractDataMappingContext.</returns>
        AbstractDataMappingContext CreateDataMappingContext(AbstractTypeCreationContext abstractTypeCreationContext, Object obj);
        /// <summary>
        /// Creates the data mapping context.
        /// </summary>
        /// <param name="creationContext">The creation context.</param>
        /// <returns>AbstractDataMappingContext.</returns>
        AbstractDataMappingContext CreateDataMappingContext(AbstractTypeSavingContext creationContext);

        /// <summary>
        /// Instantiates the object.
        /// </summary>
        /// <param name="abstractTypeCreationContext">The abstract type creation context.</param>
        /// <returns>System.Object.</returns>
        object InstantiateObject(AbstractTypeCreationContext abstractTypeCreationContext);
        /// <summary>
        /// Saves the object.
        /// </summary>
        /// <param name="abstractTypeSavingContext">The abstract type saving context.</param>
        void SaveObject(AbstractTypeSavingContext abstractTypeSavingContext);

        

    }
}




