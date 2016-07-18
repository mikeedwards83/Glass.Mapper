using System;
using Glass.Mapper.Sc.IoC;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    public class CachedItemVersionHandler : ItemVersionHandler
    {
        public override bool HasVersions(Item item)
        {
            if (!Sitecore.Context.PageMode.IsNormal && !Sitecore.Context.PageMode.IsDebugging)
            {
                return base.HasVersions(item);
            }

            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            string cacheKey = String.Format("GlassVersionCount_{0}", item.ID.Guid);

            if (ConfigurationFactory.Default.CacheManager.Contains(cacheKey))
            {
                return ConfigurationFactory.Default.CacheManager.GetValue<bool>(cacheKey);
            }

            var result = base.HasVersions(item);
            ConfigurationFactory.Default.CacheManager.AddOrUpdate(cacheKey, result);
            return result;
        }
    }
}
