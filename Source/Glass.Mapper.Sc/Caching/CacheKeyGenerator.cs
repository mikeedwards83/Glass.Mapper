using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Caching;
using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Sc.Caching
{
    public class CacheKeyGenerator : ICacheKeyGenerator
    {
        public string Generate(ObjectConstructionArgs args)
        {
            var context = args.AbstractTypeCreationContext as SitecoreTypeCreationContext;

            return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                context.SitecoreService.GlassContext.Name,
                Sitecore.Context.Site == null ? string.Empty : Sitecore.Context.Site.Name,
                context.Item.ID,
                context.Item["__Revision"],
                context.Item.Language.Name,
                context.Item.Database.Name,
                context.Options.Type.FullName,
                context.Options.Lazy
                );
        }
    }
}
