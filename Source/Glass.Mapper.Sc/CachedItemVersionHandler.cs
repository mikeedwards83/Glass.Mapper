using System;
using Glass.Mapper.Caching;
using Glass.Mapper.Sc.IoC;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc
{
    public class CachedItemVersionHandler : ItemVersionHandler
    {
        private readonly ICacheManager _cacheManager;


        public CachedItemVersionHandler(ICacheManager cacheManager, Config config) : base(config)
        {
            _cacheManager = cacheManager;
        }

        protected virtual bool CanCache()
        {
            return Sitecore.Context.PageMode.IsNormal || Sitecore.Context.PageMode.IsDebugging;
        }


        public override bool HasVersions(Item item)
        {
            if (!CanCache())
            {
                return base.HasVersions(item);
            }

            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            string cacheKey = String.Format("GlassVersionCount_{0}", item.GetUniqueId());

            if (_cacheManager.Contains(cacheKey))
            {
                return _cacheManager.GetValue<bool>(cacheKey);
            }

            var result = base.HasVersions(item);
            _cacheManager.AddOrUpdate(cacheKey, result);
            return result;
        }
    }
}
