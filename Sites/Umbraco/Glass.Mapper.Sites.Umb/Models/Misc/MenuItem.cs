using System.Collections.Generic;
using Glass.Mapper.Umb.Configuration.Attributes;

namespace Glass.Mapper.Sites.Umb.Models.Misc
{
    [UmbracoType(AutoMap = true)]
    public class MenuItem
    {
        public virtual int Id { get; set; }

        public virtual string Title { get; set; }

        public virtual IEnumerable<MenuItem> Children { get; set; }
    }
}