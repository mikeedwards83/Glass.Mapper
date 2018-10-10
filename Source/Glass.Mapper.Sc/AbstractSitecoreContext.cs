using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc.IoC;
using Glass.Mapper.Sc.Web;
using Sitecore.Data;

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// Class SitecoreContext
    /// </summary>
    [Obsolete("This class is obsolete in V5")]
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

        [Obsolete("Use IRequestContext.GetContextItem ")]
        public T GetCurrentItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {
            return RequestContext.GetContextItem<T>(
                new GetKnownOptions()
                {
                    Lazy = GetLazyLoading(isLazy),
                    InferType = inferType
                });
        }

        [Obsolete("Use IRequestContext.GetContextItem ")]
        public T GetCurrentItem<T, TK>(TK param1, bool isLazy = false, bool inferType = false) where T : class
        {


            return RequestContext.GetContextItem<T>(
                new GetKnownOptions()
                {
                    Lazy = GetLazyLoading(isLazy),
                    InferType = inferType,
                    ConstructorParameters =  new object[] {param1}.Select(x=>new ConstructorParameter(param1.GetType(), param1)).ToArray()
                });
        }

        [Obsolete("Use IRequestContext.GetContextItem ")]
        public T GetCurrentItem<T, TK, TL>(TK param1, TL param2, bool isLazy = false, bool inferType = false) where T : class
        {

            return RequestContext.GetContextItem<T>(
                new GetKnownOptions()
                {
                    Lazy = GetLazyLoading(isLazy),
                    InferType = inferType,
                    ConstructorParameters = new object[] { param1, param2 }.Select(x => new ConstructorParameter(x.GetType(), x)).ToArray()
                });
        }

        [Obsolete("Use IRequestContext.GetContextItem ")]
        public T GetCurrentItem<T, TK, TL, TM>(TK param1, TL param2, TM param3, bool isLazy = false, bool inferType = false) where T : class
        {
            return RequestContext.GetContextItem<T>(
                new GetKnownOptions()
                {
                    Lazy = GetLazyLoading(isLazy),
                    InferType = inferType,
                    ConstructorParameters = new object[] { param1, param2, param3 }.Select(x => new ConstructorParameter(x.GetType(), x)).ToArray()
                });
        }

        [Obsolete("Use IRequestContext.GetContextItem ")]
        public T GetCurrentItem<T, TK, TL, TM, TN>(TK param1, TL param2, TM param3, TN param4, bool isLazy = false,
            bool inferType = false) where T : class
        {
            return RequestContext.GetContextItem<T>(
                new GetKnownOptions()
                {
                    Lazy = GetLazyLoading(isLazy),
                    InferType = inferType,
                    ConstructorParameters = new object[] { param1, param2, param3, param4 }.Select(x => new ConstructorParameter(x.GetType(), x)).ToArray()
                });

        }

        [Obsolete("Use IRequestContext.GetContextItem ")]
        public object GetCurrentItem(Type type, bool isLazy = false, bool inferType = false)
        {
            return RequestContext.GetContextItem(
                new GetKnownOptions()
                {
                    Type = type,
                    Lazy = GetLazyLoading(isLazy),
                    InferType = inferType,
                    ConstructorParameters = new object[] {  }.Select(x => new ConstructorParameter(x.GetType(), x)).ToArray()
                });
        }

        [Obsolete("Use ISitecoreService.GetItems")]
        public IEnumerable<T> QueryRelative<T>(string query, bool isLazy = false, bool inferType = false) where T : class
        {
            var result = RequestContext.SitecoreService.GetItems<T>(
                new GetItemsByQueryOptions(Sc.Query.New(query))
                {
                    Lazy = GetLazyLoading(isLazy),
                    InferType = inferType,
                    RelativeItem = RequestContext.ContextItem
                });

            return result;
        }

        [Obsolete("Use ISitecoreService.GetItem ")]
        public T QuerySingleRelative<T>(string query, bool isLazy = false, bool inferType = false) where T : class
        {

            return RequestContext.SitecoreService.GetItem<T>(
                new GetItemByQueryOptions()
                {
                    Query = Sc.Query.New(query),
                    Lazy = GetLazyLoading(isLazy),
                    InferType = inferType,
                    RelativeItem = RequestContext.ContextItem
                });
           
        }

        [Obsolete("Use IRequestContext.GetHomeItem ")]
        public T GetHomeItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {
            return RequestContext.GetHomeItem<T>(new GetKnownOptions()
            {
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType
            });
        }

        [Obsolete("Use IRequestContext.GetRootItem ")]
        public T GetRootItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {
            return RequestContext.GetRootItem<T>(new GetKnownOptions()
            {
                Lazy = GetLazyLoading(isLazy),
                InferType = inferType
            });
        }
    }
}




