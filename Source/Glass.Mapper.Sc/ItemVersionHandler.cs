using System;
using Glass.Mapper.Sc.IoC;
using Sitecore.Common;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    public class ItemVersionHandler : IItemVersionHandler
    {
        private readonly Config _config;

        private static int count = 0;
        public ItemVersionHandler(Config config)
        {
            _config = config;
        }

        public virtual bool VersionCountEnabled()
        {
           
            if (_config == null)
            {
                throw new ArgumentNullException("_config");
            }

            if (_config.DisableVersionCount)
            {
                return false;
            }

            if (count > 200)
            {
                
            }
            count++;
            if (_config != null && _config.ForceItemInPageEditor && GlassHtml.IsInEditingMode)
            {
                return false;
            }

            return Switcher<VersionCountState>.CurrentValue != VersionCountState.Disabled;
        }

        public virtual bool HasVersions(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            return item.Versions.Count > 0;
        }

        public virtual bool VersionCountEnabledAndHasVersions(Item item)
        {
            bool versionCountEnabled = VersionCountEnabled();
            return !versionCountEnabled || HasVersions(item);
        }
    }
}
