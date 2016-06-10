using System;
using System.Collections.Generic;
using System.Web;

namespace Glass.Mapper.Sc.IoC
{
    public class SitecoreContextFactory : ISitecoreContextFactory
    {
        private const string CachedContextsKey = "CEC8A395-F2AE-48BD-A24F-4F40598094BD";

        public SitecoreContextFactory(IGlassContextProvider glassContextProvider)
        {
            GlassContextProvider = glassContextProvider;
        }

        public IGlassContextProvider GlassContextProvider { get; private set; }

        public ISitecoreContext GetSitecoreContext()
        {
            return GetSitecoreContext(GlassContextProvider.GetContext());
        }

        public ISitecoreContext GetSitecoreContext(string contextName)
        {
            Context context = String.IsNullOrEmpty(contextName) 
                ? GlassContextProvider.GetContext() 
                : GlassContextProvider.GetContext(contextName);

            return GetSitecoreContext(context);
        }

        public ISitecoreContext GetSitecoreContext(Context context)
        {
            if (context == null)
            {
                context = GlassContextProvider.GetContext();
            }

            Dictionary<string, SitecoreContext> cachedContexts = CachedContexts;
            if (cachedContexts == null)
            {
                return new SitecoreContext(context);
            }

            SitecoreContext sitecoreContext = null;

            string contextName = context.Name;
            if (cachedContexts.ContainsKey(contextName))
            {
                sitecoreContext = cachedContexts[contextName];
            }

            if (sitecoreContext != null)
            {
                return sitecoreContext;
            }

            sitecoreContext = new SitecoreContext(contextName);
            cachedContexts[contextName] = sitecoreContext;
            return sitecoreContext;
        }

        protected Dictionary<string, SitecoreContext> CachedContexts
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    throw new NotSupportedException("Cached Contexts are stored in the http context items collection, the http context is currently null");
                }

                if (Sitecore.Context.Items == null)
                {
                    return null;
                }

                var dictionary = HttpContext.Current.Items[CachedContextsKey] as Dictionary<string, SitecoreContext>;
                if (dictionary != null)
                {
                    return dictionary;
                }

                dictionary = new Dictionary<string, SitecoreContext>();
                HttpContext.Current.Items[CachedContextsKey] = dictionary;
                return dictionary;
            }
        }
    }
}
