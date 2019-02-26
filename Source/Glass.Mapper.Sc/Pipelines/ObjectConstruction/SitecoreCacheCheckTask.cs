using Glass.Mapper.Caching;
using Glass.Mapper.Pipelines.ObjectConstruction;
using Glass.Mapper.Pipelines.ObjectConstruction.Tasks.CacheCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.Sc.Pipelines.ObjectConstruction
{
    public class SitecoreCacheCheckTask : CacheCheckTask
    {
        public SitecoreCacheCheckTask(CacheFactory cacheFactory, ICacheKeyGenerator cacheKeyGenerator)
            : base(cacheFactory, cacheKeyGenerator)
        {
        }
        public override string GetCacheName(ObjectConstructionArgs args)
        {
            var options = args.Options as GetOptionsSc;

            if (options != null && options.Site != null)
            {
                return options.Site.Name;
            }

            return base.GetCacheName(args);
        }
    }
}
