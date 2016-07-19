using System;
using Glass.Mapper.Sc.IoC;
using Sitecore.Common;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    public class ItemVersionHandler : IItemVersionHandler
    {
        private readonly Config _config;

        public ItemVersionHandler(Config config)
        {
            _config = config;
        }

        public virtual bool VersionCountEnabled()
        {
           
            if (_config == null)
            {
                throw new ArgumentNullException(nameof(_config));
            }

            if (_config.DisableVersionCount)
            {
                return false;
            }

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
                throw new ArgumentNullException(nameof(item));
            }

            return item.Versions.Count > 0;
        }

        public virtual bool VersionCountEnabledAndHasVersions(Item item)
        {
            if (VersionCountEnabled() && HasVersions(item))
            {
                return true;
            }
            if (!VersionCountEnabled())
            {
                return true;
            }

            return false;
        }
    }
}
