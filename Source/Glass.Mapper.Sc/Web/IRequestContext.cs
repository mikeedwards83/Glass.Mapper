using System;
using Glass.Mapper.Sc.Builders;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.Web
{
    public interface IRequestContext
    {
        ISitecoreService SitecoreService { get; }

        /// <summary>
        /// Maps the Sitecore.Context.Item to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        T GetContextItem<T>()
            where T : class;
        
        /// <summary>
        /// Maps the Sitecore.Context.Item to a model
        /// </summary>
        /// <param name="options">Options for how the model will be retrieved</param>
        /// <returns></returns>
        object GetContextItem(GetKnownOptions options) ;

        /// <summary>
        /// Maps the Sitecore.Context.Item to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="options">Options for how the model will be retrieved</param>
        /// <returns></returns>
        T GetContextItem<T>(GetKnownOptions options) where T : class;

        /// <summary>
        /// Maps the Sitecore.Context.Site.StartPath to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        T GetHomeItem<T>()
            where T : class;

        /// <summary>
        /// Maps the Sitecore.Context.Site.StartPath to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="options">Options for how the model will be retrieved</param>
        /// <returns></returns>
        T GetHomeItem<T>(GetKnownOptions options) where T : class;

        /// <summary>
        /// Maps the Sitecore.Context.Site.RootPath to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        T GetRootItem<T>()
            where T : class;

        /// <summary>
        /// Maps the Sitecore.Context.Site.RootPath to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="options">Options for how the model will be retrieved</param>
        /// <returns></returns>
        T GetRootItem<T>(GetKnownOptions options) where T : class;

        Item ContextItem { get; }
       
    }
}
