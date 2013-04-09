using System;
using Glass.Mapper.Umb.Configuration.Attributes;

namespace Glass.Mapper.Sites.Umb.Models.Misc
{
    [UmbracoType(AutoMap = true)]
    public class ContentBase
    {
        public virtual string Title { get; set; }

        [UmbracoId]
        public virtual Guid Key { get; set; }
    }
}