using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Caching;
using Glass.Mapper.Pipelines.ObjectConstruction;

namespace Glass.Mapper.Umb.Caching
{
    public class CacheKeyGenerator : ICacheKeyGenerator
    {
        public string Generate(ObjectConstructionArgs args)
        {
            var context = args.AbstractTypeCreationContext as UmbracoTypeCreationContext;

            return string.Format("{0}{1}{2}{3}{4}{5}",
                args.Context.Name,
                context.UmbracoService.GlassContext.Name,
                context.Content.Id.ToString(),
                context.Content.Version,
                context.RequestedType.FullName,
                context.IsLazy
                );
        }
    }
}
