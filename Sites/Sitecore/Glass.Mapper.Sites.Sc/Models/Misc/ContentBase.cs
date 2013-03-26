using System;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace Glass.Mapper.Sites.Sc.Models.Misc
{
    [SitecoreType(AutoMap = true)]
    public class ContentBase
    {
        public virtual string Title { get; set; }

        public virtual Guid Id { get; set; }
      
        public virtual string Url { get; set; }
    }
}