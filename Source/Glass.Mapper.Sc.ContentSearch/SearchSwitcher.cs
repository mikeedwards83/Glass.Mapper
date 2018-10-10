using System;

namespace Glass.Mapper.Sc.ContentSearch
{
    public class SearchSwitcher : IDisposable
    {
        private const string SwitcherKey = "AD66AE1A-8C25-443B-B1F4-C60587533A94";

        public static bool IsSearchContext
        {
            get
            {
                return Sitecore.Context.Items.Contains(SwitcherKey);
            }
        }

        public SearchSwitcher()
        {
            Sitecore.Context.Items[SwitcherKey] = "true";
        }

        public void Dispose()
        {
            Sitecore.Context.Items.Remove(SwitcherKey);
        }
    }
}
