using System;
using System.Collections.Generic;

namespace Glass.Mapper.Sc.IoC
{
    public class SitecoreContextFactory : ISitecoreContextFactory
    {
        private const string CachedContextsKey = "CEC8A395-F2AE-48BD-A24F-4F40598094BD";

        private static ISitecoreContextFactory _defaultSitecoreContextFactory = new SitecoreContextFactory();

        public static ISitecoreContextFactory Default
        {
            get { return _defaultSitecoreContextFactory; }
            set { _defaultSitecoreContextFactory = value; }
        }

        public SitecoreContextFactory() : this(IoC.GlassContextProvider.Default)
        {
        }

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
                Context providerContext = GlassContextProvider.GetContext();
                if (providerContext == null)
                {
                    throw new NotSupportedException("Sitecore Context Requires a Glass Context");
                }

                context = providerContext;
            }

            string cacheKey = String.Format("GlassContext_{0}_{1}", context.Name, CachedContextsKey);
            
            SitecoreContext sitecoreContext = Sitecore.Context.Items[cacheKey] as SitecoreContext;

            if (sitecoreContext == null)
            {
                string contextName = context.Name;
                sitecoreContext = new SitecoreContext(contextName);
                Sitecore.Context.Items[cacheKey] = sitecoreContext;
            }

            return sitecoreContext;
        }
    }
}
