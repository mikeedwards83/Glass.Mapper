using System;
using Glass.Mapper.Umb.Configuration;
using Umbraco.Core.Models;

namespace Glass.Mapper.Umb
{
    public interface IUmbracoService: IAbstractService
    {
        object CreateType(Type type, IContent content, bool isLazy, bool inferType,
                          params object[] constructorParameters);

        T GetItem<T>(int id, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Creates a class from the specified content
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="content">The content to load data from</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <returns>The item as the specified type</returns>
        T CreateType<T>(IContent content, bool isLazy = false, bool inferType = false) where T : class;

        /// <summary>
        /// Creates a class from the specified content with a single constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="content">The content to load data from</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <returns>The item as the specified type</returns>
        T CreateType<T, K>(IContent content, K param1, bool isLazy = false, bool inferType = false);

        /// <summary>
        /// Creates a class from the specified content with a two constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="content">The content to load data from</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <returns>The item as the specified type</returns>
        T CreateType<T, K, L>(IContent content, K param1, L param2, bool isLazy = false, bool inferType = false);

        /// <summary>
        /// Creates a class from the specified content with a two constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="content">The content to load data from</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <returns>The item as the specified type</returns>
        T CreateType<T, K, L, M>(IContent content, K param1, L param2, M param3, bool isLazy = false, bool inferType = false);

        /// <summary>
        /// Creates a class from the specified content with a two constructor parameter
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <typeparam name="K">The type of the first constructor parameter</typeparam>
        /// <typeparam name="L">The type of the second constructor parameter</typeparam>
        /// <typeparam name="M">The type of the third constructor parameter</typeparam>
        /// <typeparam name="N">The type of the fourth constructor parameter</typeparam>
        /// <param name="isLazy">If true creates a proxy for the class</param>
        /// <param name="content">The content to load data from</param>
        /// <param name="inferType">Infer the type to be loaded from the template</param>
        /// <param name="param1">The value of the first parameter of the constructor</param>       
        /// <param name="param2">The value of the second parameter of the constructor</param>
        /// <param name="param3">The value of the third parameter of the constructor</param>
        /// <param name="param4">The value of the fourth parameter of the constructor</param>
        /// <returns>The item as the specified type</returns>
        T CreateType<T, K, L, M, N>(IContent content, K param1, L param2, M param3, N param4, bool isLazy = false, bool inferType = false);

        /// <summary>
        /// Creates a new umbraco class. 
        /// </summary>
        /// <typeparam name="T">The type of the new item to create. This type must have either a TemplateId or BranchId defined on the UmbracoClassAttribute or fluent equivalent</typeparam>
        /// <param name="parent">The parent of the new item to create. Must have the UmbracoIdAttribute or fluent equivalent</param>
        /// <param name="newItem">New item to create, must have the attribute UmbracoInfoAttribute of type UmbracoInfoType.Name or the fluent equivalent</param>
        /// <returns></returns>
        T Create<T>(int parent, T newItem)
            where T : class;

        void WriteToItem<T>(T target, IContent content, UmbracoTypeConfiguration config = null);
    }
}
