using System;
using System.Collections.Generic;
using Glass.Mapper.Sc.IoC;
using Glass.Mapper.Sc.Web;
using Sitecore.Data;

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// Class SitecoreContext
    /// </summary>
    public abstract class AbstractSitecoreContext : SitecoreService, ISitecoreContext
    {
        protected IRequestContext RequestContext { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreContext"/> class.
        /// </summary>
        protected AbstractSitecoreContext(Database database, Context context)
            : base(database, context)
        {
            RequestContext = new RequestContext(this);
        }

        protected AbstractSitecoreContext(Database database, string contextName)
            : base(database, contextName)
        {
            RequestContext = new RequestContext(this);

        }

        public T GetCurrentItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {
            return RequestContext.GetContextItem<T>(b=>b.Lazy(ISitecoreServiceLegacyExtensions.GetLazyLoading(isLazy)).InferType(inferType));
        }

        public T GetCurrentItem<T, TK>(TK param1, bool isLazy = false, bool inferType = false) where T : class
        {


            return RequestContext.GetContextItem<T>(
                b => b.Lazy(ISitecoreServiceLegacyExtensions.GetLazyLoading(isLazy))
                    .InferType(inferType)
                    .AddParam(param1)
            );
        }

        public T GetCurrentItem<T, TK, TL>(TK param1, TL param2, bool isLazy = false, bool inferType = false) where T : class
        {
            return RequestContext.GetContextItem<T>(
                b => b.Lazy(ISitecoreServiceLegacyExtensions.GetLazyLoading(isLazy))
                    .InferType(inferType)
                    .AddParam(param1)
                    .AddParam(param2)
            );
        }

        public T GetCurrentItem<T, TK, TL, TM>(TK param1, TL param2, TM param3, bool isLazy = false, bool inferType = false) where T : class
        {
            return RequestContext.GetContextItem<T>(
                b => b.Lazy(ISitecoreServiceLegacyExtensions.GetLazyLoading(isLazy))
                    .InferType(inferType)
                    .AddParam(param1)
                    .AddParam(param2)
                    .AddParam(param3)
                    );
        }

        public T GetCurrentItem<T, TK, TL, TM, TN>(TK param1, TL param2, TM param3, TN param4, bool isLazy = false,
            bool inferType = false) where T : class
        {
            return RequestContext.GetContextItem<T>(
                b => b.Lazy(ISitecoreServiceLegacyExtensions.GetLazyLoading(isLazy))
                    .InferType(inferType)
                    .AddParam(param1)
                    .AddParam(param2)
                    .AddParam(param3)
                    .AddParam(param4));

        }

        public object GetCurrentItem(Type type, bool isLazy = false, bool inferType = false)
        {
            return RequestContext.GetContextItem(
                b => b.Type(type)
                    .Lazy(ISitecoreServiceLegacyExtensions.GetLazyLoading(isLazy))
                    .InferType(inferType));
        }

        public IEnumerable<T> QueryRelative<T>(string query, bool isLazy = false, bool inferType = false) where T : class
        {
            return RequestContext.SitecoreService.GetItemsByQuery<T>(query, 
                b => b.RelativeTo(RequestContext.ContextItem)
                    .Lazy(ISitecoreServiceLegacyExtensions.GetLazyLoading(isLazy))
                    .InferType(inferType));
        }

        public T QuerySingleRelative<T>(string query, bool isLazy = false, bool inferType = false) where T : class
        {
            return RequestContext.SitecoreService.GetItemByQuery<T>(query,
                b => b.RelativeTo(RequestContext.ContextItem)
                    .Lazy(ISitecoreServiceLegacyExtensions.GetLazyLoading(isLazy))
                    .InferType(inferType));
        }

        public T GetHomeItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {
            return RequestContext.GetHomeItem<T>(b =>
                b.Lazy(ISitecoreServiceLegacyExtensions.GetLazyLoading(isLazy)).InferType(inferType));

        }

        public T GetRootItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {
            return RequestContext.GetRootItem<T>(b =>
                b.Lazy(ISitecoreServiceLegacyExtensions.GetLazyLoading(isLazy)).InferType(inferType));
        }
    }
}




