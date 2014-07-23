using System;

namespace Glass.Mapper.Sc.ContentSearch
{
    public class SearchSwitcher : IDisposable
    {
        private const string SwitcherKey = "AD66AE1A-8C25-443B-B1F4-C60587533A94";
        private static object switchlock = new object();
        private bool isOuter = false;
        
        public static bool IsSearchContext
        {
            get
            {
                lock (switchlock)
                {
                    return Sitecore.Context.Items.Contains(SwitcherKey);
                }
            }
        }

        public SearchSwitcher()
        {
            if (!IsSearchContext)
            {
                lock (switchlock)
                {
                    if (!Sitecore.Context.Items.Contains(SwitcherKey))
                    {
                        isOuter = true;
                        Sitecore.Context.Items[SwitcherKey] = "true";
                    }
                }
            }
        }

        public void Dispose()
        {
            if (isOuter)
            {
                lock (switchlock)
                {
                    Sitecore.Context.Items.Remove(SwitcherKey);
                }
            }
        }
    }
}
