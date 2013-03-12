using System;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace Glass.Mapper.Sc.Razor.Model
{
    [SitecoreType]
    public class GlassRazorFolder
    {
        [SitecoreId]
        public virtual Guid Id { get; set; }
    }
}
