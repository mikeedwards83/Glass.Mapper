using System;
using Glass.Mapper.Sc.Builders;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Glass.Mapper.Sc.Web
{
    public class RequestContext : IRequestContext
    {
        public ISitecoreService SitecoreService { get; private set; }

        public RequestContext(ISitecoreService sitecoreService)
        {
            SitecoreService = sitecoreService;
        }

        /// <summary>
        /// Maps the Sitecore.Context.Item to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        public T GetContextItem<T>()
            where T : class
        {
            return GetContextItem<T>(new GetKnownOptions());
        }

        /// <summary>
        /// Maps the Sitecore.Context.Item to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="options">Options for how the model will be retrieved</param>
        /// <returns></returns>
        public virtual object GetContextItem(GetKnownOptions options) 
        {
            Assert.IsNotNull(options, "options must no be  null");

            options.Item = ContextItem;

            return SitecoreService.GetItem(options);
        }

        /// <summary>
        /// Maps the Sitecore.Context.Item to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="options">Options for how the model will be retrieved</param>
        /// <returns></returns>
        public virtual T GetContextItem<T>(GetKnownOptions options) where T : class
        {
            Assert.IsNotNull(options, "options must no be  null");

            options.Item = ContextItem;

            return SitecoreService.GetItem<T>(options);
        }


        /// <summary>
        /// Maps the Sitecore.Context.Site.StartPath to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        public virtual T GetHomeItem<T>()
            where T : class
        {
            return GetHomeItem<T>(new GetKnownOptions());
        }

        /// <summary>
        /// Maps the Sitecore.Context.Site.StartPath to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="options">Options for how the model will be retrieved</param>
        /// <returns></returns>
        public virtual T GetHomeItem<T>(GetKnownOptions options) where T : class
        {
            Assert.IsNotNull(options, "options must no be  null");

            var item = SitecoreService.Database.GetItem(Sitecore.Context.Site.StartPath);
            options.Item = item;
            return SitecoreService.GetItem<T>(options);

        }


       

        /// <summary>
        /// Maps the Sitecore.Context.Site.RootPath to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        public virtual T GetRootItem<T>()
            where T : class
        {
            return GetRootItem<T>( new GetKnownOptions());
        }

        /// <summary>
        /// Maps the Sitecore.Context.Site.RootPath to a model
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="options">Options for how the model will be retrieved</param>
        /// <returns></returns>
        public virtual T GetRootItem<T>(GetKnownOptions options) where T : class
        {
            Assert.IsNotNull(options, "options must no be  null");

            var item = SitecoreService.Database.GetItem(Sitecore.Context.Site.RootPath);
            options.Item = item;
            return SitecoreService.GetItem<T>(options);
        }



        public Item ContextItem
        {
            get { return Sitecore.Context.Item; }
        }
    }
}
