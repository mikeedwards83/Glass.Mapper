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
    [Obsolete("All methods in this interface are no obsolete in V5")]
    public interface ISitecoreServiceLegacy : IAbstractService
    {
     
        #region Create

        /// <summary>
        /// Creates a new Sitecore item.
        /// </summary>
        /// <typeparam name="T">The type of the new item to create. This type must have either a TemplateId or BranchId defined on the SitecoreClassAttribute or fluent equivalent</typeparam>
        /// <typeparam name="TK">The type of the parent item</typeparam>
        /// <param name="parent">The parent of the new item to create. Must have the SitecoreIdAttribute or fluent equivalent</param>
        /// <param name="newItem">New item to create, must have the attribute SitecoreInfoAttribute of type SitecoreInfoType.Name or the fluent equivalent</param>
        /// <param name="updateStatistics">Indicates if the items stats should be updated when the item is saved</param>
        /// <param name="silent">If set to true, no events will be raised due to saving.</param>
        /// <returns>``0.</returns>
        [Obsolete("User ISitecoreService.CreateItem")]
        T Create<T, TK>(TK parent, T newItem, bool updateStatistics = true, bool silent = false)
            where T : class
            where TK : class;

        /// <summary>
        /// Creates a new Sitecore item.
        /// </summary>
        /// <typeparam name="T">The type of the new item to create. This type must have either a TemplateId or BranchId defined on the SitecoreClassAttribute or fluent equivalent</typeparam>
        /// <typeparam name="TK">The type of the parent item</typeparam>
        /// <param name="parent">The parent of the new item to create. Must have the SitecoreIdAttribute or fluent equivalent</param>
        /// <param name="newName">The name of the new item</param>
        /// <param name="updateStatistics">Indicates if the items stats should be updated when the item is saved</param>
        /// <param name="silent">If set to true, no events will be raised due to saving.</param>
        /// <returns>``0.</returns>
        [Obsolete("User ISitecoreService.CreateItem")]
        T Create<T, TK>(TK parent, string newName, Language language = null, bool updateStatistics = true, bool silent = false)
            where T : class
            where TK : class;

        #endregion

        #region  CreateType

          /// <summary>
        /// Creates a class from the specified item
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="item">The item to load data from</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        [Obsolete("Use Cast<T> instead")]
        T CreateType<T>(Item item, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Casts a class from the specified item
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="item">The item to load data from</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        [Obsolete("User ISitecoreService.GetItem")]
        T Cast<T>(Item item, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Creates a class from the specified item with a single constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <param name="item">The item to load data from</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        [Obsolete("User ISitecoreService.GetItem")]
        T CreateType<T, TK>(Item item, TK param1, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Creates a class from the specified item with a two constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <param name="item">The item to load data from</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        [Obsolete("User ISitecoreService.GetItem")]
        T CreateType<T, TK, TL>(Item item, TK param1, TL param2, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Creates a class from the specified item with a two constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <param name="item">The item to load data from</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        [Obsolete("User ISitecoreService.GetItem")]
        T CreateType<T, TK, TL, TM>(Item item, TK param1, TL param2, TM param3, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Creates a class from the specified item with four constructor parameters
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <typeparam name="TN">The type of the fourth constructor parameter</typeparam>
        /// <param name="item">The item to load data from</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        [Obsolete("User ISitecoreService.GetItem")]
        T CreateType<T, TK, TL, TM, TN>(Item item, TK param1, TL param2, TM param3, TN param4, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Creates a class from the specified item with five constructor parameters
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <typeparam name="TN">The type of the fourth constructor parameter</typeparam>
        /// <typeparam name="TO">The type of the fifth constructor parameter</typeparam>
        /// <param name="item">The item to load data from</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <param name="param5">The value of the fifth parameter of the constructor</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        [Obsolete("User ISitecoreService.GetItem")]
        T CreateType<T, TK, TL, TM, TN, TO>(Item item, TK param1, TL param2, TM param3, TN param4, TO param5, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Creates a class from the specified item with constructor parameters
        /// </summary>
        /// <param name="item">The item to load data from</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="constructorParameters">The constructor parameters - maximum 10</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        [Obsolete("User ISitecoreService.GetItem")]
        T CreateType<T>(Item item, bool isLazy = false, bool inferType = false, params object[] constructorParameters) where T : class;

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
        [Obsolete("User ISitecoreService.GetItems")]
        IEnumerable CreateTypes(Type type, Func<IEnumerable<Item>> getItems, bool isLazy = false, bool inferType = false);

        #endregion

        #region Delete

        /// <summary>
        /// Delete an item from Sitecore
        /// </summary>
        /// <typeparam name="T">The type being deleted. The type must have a property with the SitecoreIdAttribute.</typeparam>
        /// <param name="item">The class to delete</param>
        [Obsolete("User ISitecoreService.DeleteItem")]
        void Delete<T>(T item) where T : class;

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
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T>(string path, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TL"></typeparam>
        /// <param name="path">The path.</param>
        /// <param name="param1">The param1.</param>
        /// <param name="param2">The param2.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// ``0.
        /// </returns>
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T, TK, TL>(string path, TK param1, TL param2, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TL"></typeparam>
        /// <typeparam name="TM"></typeparam>
        /// <param name="path">The path.</param>
        /// <param name="param1">The param1.</param>
        /// <param name="param2">The param2.</param>
        /// <param name="param3">The param3.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// ``0.
        /// </returns>
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T, TK, TL, TM>(string path, TK param1, TL param2, TM param3, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TL"></typeparam>
        /// <typeparam name="TM"></typeparam>
        /// <typeparam name="TN"></typeparam>
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
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T, TK, TL, TM, TN>(string path, TK param1, TL param2, TM param3, TN param4, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TK"></typeparam>
        /// <param name="path">The path.</param>
        /// <param name="param1">The param1.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// ``0.
        /// </returns>
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T, TK>(string path, TK param1, bool isLazy = false, bool inferType = false) where T : class;

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
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T>(string path, Language language, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T, TK>(string path, Language language, TK param1, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T, TK, TL>(string path, Language language, TK param1, TL param2, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
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
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T, TK, TL, TM>(string path, Language language, TK param1, TL param2, TM param3, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <typeparam name="TN">The type of the fourth constructor parameter</typeparam>
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
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T, TK, TL, TM, TN>(string path, Language language, TK param1, TL param2, TM param3, TN param4, bool isLazy = false, bool inferType = false) where T : class;


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
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T>(string path, Language language, Sitecore.Data.Version version, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T, TK>(string path, Language language, Sitecore.Data.Version version, TK param1, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
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
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T, TK, TL>(string path, Language language, Sitecore.Data.Version version, TK param1, TL param2, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <param name="path">The path.</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>The Sitecore item as the specified type</returns>
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T, TK, TL, TM>(string path, Language language, Sitecore.Data.Version version, TK param1, TL param2, TM param3, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <typeparam name="TN">The type of the fourth constructor parameter</typeparam>
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
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T, TK, TL, TM, TN>(string path, Language language, Sitecore.Data.Version version, TK param1, TL param2, TM param3, TN param4, bool isLazy = false, bool inferType = false) where T : class;



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
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T>(Guid id, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T, TK>(Guid id, TK param1, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T, TK, TL>(Guid id, TK param1, TL param2, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T, TK, TL, TM>(Guid id, TK param1, TL param2, TM param3, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <typeparam name="TN">The type of the fourth constructor parameter</typeparam>
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
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T, TK, TL, TM, TN>(Guid id, TK param1, TL param2, TM param3, TN param4, bool isLazy = false, bool inferType = false) where T : class;


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
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T>(Guid id, Language language, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T, TK>(Guid id, Language language, TK param1, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T, TK, TL>(Guid id, Language language, TK param1, TL param2, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
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
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T, TK, TL, TM>(Guid id, Language language, TK param1, TL param2, TM param3, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <typeparam name="TN">The type of the fourth constructor parameter</typeparam>
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
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T, TK, TL, TM, TN>(Guid id, Language language, TK param1, TL param2, TM param3, TN param4, bool isLazy = false, bool inferType = false) where T : class;





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
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T>(Guid id, Language language, Sitecore.Data.Version version, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <param name="id">The ID of the Sitecore item</param>
        /// <param name="language">The language of the item to return</param>
        /// <param name="version">The version of the item to return</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>
        /// The Sitecore item as the specified type
        /// </returns>
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T, TK>(Guid id, Language language, Sitecore.Data.Version version, TK param1, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
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
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T, TK, TL>(Guid id, Language language, Sitecore.Data.Version version, TK param1, TL param2, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
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
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T, TK, TL, TM>(Guid id, Language language, Sitecore.Data.Version version, TK param1, TL param2, TM param3, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Retrieve a Sitecore item as the specified type.
        /// </summary>
        /// <typeparam name="T">The type to return the Sitecore item as</typeparam>
        /// <typeparam name="TK">The type of the first constructor parameter</typeparam>
        /// <typeparam name="TL">The type of the second constructor parameter</typeparam>
        /// <typeparam name="TM">The type of the third constructor parameter</typeparam>
        /// <typeparam name="TN">The type of the fourth constructor parameter</typeparam>
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
        [Obsolete("User ISitecoreService.GetItem")]
        T GetItem<T, TK, TL, TM, TN>(Guid id, Language language, Sitecore.Data.Version version, TK param1, TL param2, TM param3, TN param4, bool isLazy = false, bool inferType = false) where T : class;


        #endregion

        #region Move

        /// <summary>
        /// Moves the specified item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TK"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="newParent">The new parent.</param>
        [Obsolete("Use ISitecoreService.MoveItem")]
        void Move<T, TK>(T item, TK newParent);

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
        [Obsolete("Use ISitecoreService.GetItems with builder")]
        IEnumerable<T> Query<T>(string query, bool isLazy = false, bool inferType = false) where T : class;


        /// <summary>
        /// Query Sitecore for a set of items. Proxy classes are created.
        /// </summary>
        /// <typeparam name="T">The type to return the items as</typeparam>
        /// <param name="query">The query to execute</param>
        /// <param name="language">The language of the items to return</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns>Sitecore items as proxy classes of the specified type</returns>
        [Obsolete("Use ISitecoreService.GetItems with builder")]
        IEnumerable<T> Query<T>(string query, Language language, bool isLazy = false, bool inferType = false)
            where T : class;

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
        [Obsolete("Use ISitecoreService.GetItem with builder")]
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
        [Obsolete("Use ISitecoreService.SaveItem with builder")]
        void Save<T>(T target, bool updateStatistics = true, bool silent = false) where T : class;

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
        [Obsolete("Use ISitecoreService.WriteToItem with builder")]
        void WriteToItem<T>(T target, Item item, bool updateStatistics = true, bool silent = false) where T : class;

        #endregion


        /// <summary>
        /// Map data from Sitecore to an existing Sitecore item. The configuration for the item must already be loaded.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        [Obsolete("Use ISitecorService.Populate")]
        void Map<T>(T target) where T : class;
    }
}



