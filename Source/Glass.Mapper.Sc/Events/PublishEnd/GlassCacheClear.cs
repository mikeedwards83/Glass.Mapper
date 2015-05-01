using System;
using Glass.Mapper.Caching;

namespace Glass.Mapper.Sc.Events.PublishEnd
{
    public class GlassCacheClear 
    {
        public void ClearCache(object sender, EventArgs args)
        {
            Sitecore.Diagnostics.Log.Info("Started clearing all Glass.Mapper caches", this);

            foreach (var context in Glass.Mapper.Context.Contexts)
            {
                try
                {
                    var manager = context.Value.DependencyResolver.GetCacheManager();
                    if (manager != null)
                    {
                        Sitecore.Diagnostics.Log.Info("Started clearing Glass.Mapper cache for context {0}".Formatted(context.Key), this);
                        manager.ClearCache();
                        Sitecore.Diagnostics.Log.Info("Finished clearing Glass.Mapper cache for context {0}".Formatted(context.Key), this);
                    }

                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Error Clearing Glass.Mapper cache for context {0}".Formatted(context.Key), ex, this);

                }
            }

            Sitecore.Diagnostics.Log.Info("Finshed clearing all Glass.Mapper caches", this);


        }
    }
}
