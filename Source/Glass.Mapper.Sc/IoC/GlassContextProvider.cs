using System;

namespace Glass.Mapper.Sc.IoC
{
    public class GlassContextProvider : IGlassContextProvider
    {
        public Context GetContext()
        {
            string contextName = GetContextFromSite();
            if (!String.IsNullOrEmpty(contextName))
            {
                return GetContext(contextName);
            }

            return Context.Default;
        }

        public Context GetContext(string contextName)
        {
            if (String.IsNullOrEmpty(contextName) || !Context.Contexts.ContainsKey(contextName))
            {
                return null;
            }

            return Context.Contexts[contextName];
        }

        protected virtual string GetContextFromSite()
        {
            if (Sitecore.Context.Site == null)
            {
                return null;
            }

            return Sitecore.Context.Site.Properties["glassContext"] ?? Context.DefaultContextName;
        }
    }
}
