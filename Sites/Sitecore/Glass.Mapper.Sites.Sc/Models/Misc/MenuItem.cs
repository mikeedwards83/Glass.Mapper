using System.Collections.Generic;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace Glass.Mapper.Sites.Sc.Models.Misc
{
    [SitecoreType(AutoMap = true)]
    public class MenuItem 
    {
        public virtual string Title { get; set; }

        public virtual string Url { get; set; }

        public virtual IEnumerable<MenuItem> Children { get; set; }
    }
}