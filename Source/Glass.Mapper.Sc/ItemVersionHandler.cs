using System;
using Glass.Mapper.Sc.IoC;
using Sitecore.Common;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    public class ItemVersionHandler : IItemVersionHandler
    {
        public virtual bool VersionCountEnabled(Config config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (config.DisableVersionCount)
            {
                return false;
            }

            if (config != null && config.ForceItemInPageEditor && GlassHtml.IsInEditingMode)
            {
                return false;
            }

            return Switcher<VersionCountState>.CurrentValue != VersionCountState.Disabled;
        }

        public virtual bool HasVersions(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return item.Versions.Count > 0;
        }

        public virtual bool VersionCountEnabledAndHasVersions(Item item, Config config)
        {
            return !VersionCountEnabled(config) || VersionCountEnabled(config) && HasVersions(item);
        }
    }
}
