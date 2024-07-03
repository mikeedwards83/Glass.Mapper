using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if SC90 || SC91  || SC92  || SC93 || SC100 || SC101 || SC102 || SC103 || SC104
using Sitecore.Abstractions;
using Sitecore.DependencyInjection;
#endif


using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Resources.Media;

namespace Glass.Mapper.Sc
{
    public class SitecoreVersionAbstractions
    {
#if SC90 || SC91 || SC92 || SC93 || SC100 || SC101 || SC102 || SC103 || SC104

        internal static LazyResetable<BaseMediaManager> MediaManager = ServiceLocator.GetRequiredResetableService<BaseMediaManager>();
        internal static LazyResetable<BaseLinkManager> LinkManager = ServiceLocator.GetRequiredResetableService<BaseLinkManager>();

        internal static string renderingParamKey = "renderingParameters";
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
#if SC104
            //Added check for when rendering paramater request comes as Sitecore.Links.UrlBuilders.Helpers.ItemPathBuilder.GetRelativePath
            //implementation has been changed in Sitecore 10.4
            //below changes will return only url when requested for a item 
            if (item != null && item.Name != null && item.Name == renderingParamKey)
            {
                return "/renderingparameters";
            }
#endif
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
