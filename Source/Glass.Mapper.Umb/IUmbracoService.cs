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
using System.Web.Mvc;
using Glass.Mapper.Umb.Configuration;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Glass.Mapper.Umb
{
    /// <summary>
    /// IUmbracoService
    /// </summary>
    public interface IUmbracoService: IAbstractService
    {
        /// <summary>
        /// Gets or sets the content service.
        /// </summary>
        /// <value>
        /// The content service.
        /// </value>
        IContentService ContentService { get; }

        /// <summary>
        /// Gets the home item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns></returns>
        T GetHomeItem<T>(bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The id.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns></returns>
        T GetItem<T>(int? id, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The id.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <returns></returns>
        T GetItem<T>(Guid id, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Saves the specified target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target.</param>
        void Save<T>(T target);
        
        /// <summary>
        /// Creates the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="content">The content.</param>
        /// <param name="isLazy">if set to <c>true</c> [is lazy].</param>
        /// <param name="inferType">if set to <c>true</c> [infer type].</param>
        /// <param name="constructorParameters">The constructor parameters.</param>
        /// <returns></returns>
        object CreateType(Type type, IContent content, bool isLazy, bool inferType,
                          params object[] constructorParameters);
        
        /// <summary>
        /// Creates a class from the specified content
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="content">The content to load data from</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>
        /// The item as the specified type
        /// </returns>
        T CreateType<T>(IContent content, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Creates a class from the specified content with a single constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <param name="content">The content to load data from</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>
        /// The item as the specified type
        /// </returns>
        T CreateType<T, K>(IContent content, K param1, bool isLazy = false, bool inferType = false);

        /// <summary>
        /// Creates a class from the specified content with a two constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <param name="content">The content to load data from</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>
        /// The item as the specified type
        /// </returns>
        T CreateType<T, K, L>(IContent content, K param1, L param2, bool isLazy = false, bool inferType = false);

        /// <summary>
        /// Creates a class from the specified content with a two constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <param name="content">The content to load data from</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>
        /// The item as the specified type
        /// </returns>
        T CreateType<T, K, L, M>(IContent content, K param1, L param2, M param3, bool isLazy = false, bool inferType = false);

        /// <summary>
        /// Creates a class from the specified content with a two constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <typeparam name="N">The type of the fourth constructor parameter</typeparam>
        /// <param name="content">The content to load data from</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>
        /// The item as the specified type
        /// </returns>
        T CreateType<T, K, L, M, N>(IContent content, K param1, L param2, M param3, N param4, bool isLazy = false, bool inferType = false);

        /// <summary>
        /// Creates a new umbraco class.
        /// </summary>
        /// <typeparam name="T">The type of the new item to create. This type must have either a TemplateId or BranchId defined on the UmbracoClassAttribute or fluent equivalent</typeparam>
        /// <typeparam name="TParent"></typeparam>
        /// <param name="parent">The parent of the new item to create. Must have the UmbracoIdAttribute or fluent equivalent</param>
        /// <param name="newItem">New item to create, must have the attribute UmbracoInfoAttribute of type UmbracoInfoType.Name or the fluent equivalent</param>
        /// <returns></returns>
        T Create<T, TParent>(TParent parent, T newItem)
            where T : class
            where TParent : class;

        /// <summary>
        /// Writes to item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The target.</param>
        /// <param name="content">The content.</param>
        /// <param name="config">The config.</param>
        void WriteToItem<T>(T target, IContent content, UmbracoTypeConfiguration config = null);

        /// <summary>
        /// Deletes an item
        /// </summary>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="MapperException"></exception>
        void Delete<T>(T item) where T : class;
    }
}

