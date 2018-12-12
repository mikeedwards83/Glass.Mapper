using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Abstractions;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Links;
using Sitecore.Resources.Media;

namespace Glass.Mapper.Sc
{
    public class SitecoreVersionAbstractions
    {
#if SC90 || SC91

        internal static LazyResetable<BaseMediaManager> MediaManager = ServiceLocator.GetRequiredResetableService<BaseMediaManager>();
        internal static LazyResetable<BaseLinkManager> LinkManager = ServiceLocator.GetRequiredResetableService<BaseLinkManager>();


        public static string GetMediaUrl(MediaItem media, MediaUrlOptions mediaUrlOptions)
        {
            return MediaManager.Value.GetMediaUrl(media, mediaUrlOptions);
        }
        public static string GetMediaUrl(MediaItem media)
        {
            return MediaManager.Value.GetMediaUrl(media);
        }

        public static string GetItemUrl(Item item, UrlOptions urlOptions)
        {
            return LinkManager.Value.GetItemUrl(item, urlOptions);
        }

        public static string GetItemUrl(Item item)
        {
            return LinkManager.Value.GetItemUrl(item);
        }

#else

        public static string GetMediaUrl(MediaItem media, MediaUrlOptions mediaUrlOptions)
        {
            return MediaManager.GetMediaUrl(media, mediaUrlOptions);
        }

        public static string GetMediaUrl(MediaItem media)
        {
            return MediaManager.GetMediaUrl(media);
        }

        public static string GetItemUrl(Item item, UrlOptions urlOptions)
        {
            return LinkManager.GetItemUrl(item, urlOptions);
        }

        public static string GetItemUrl(Item item)
        {
            return LinkManager.GetItemUrl(item);
        }

#endif
    }
}
