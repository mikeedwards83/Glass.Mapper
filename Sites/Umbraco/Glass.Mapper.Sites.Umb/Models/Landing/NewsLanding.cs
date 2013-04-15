using System.Collections.Generic;
using Glass.Mapper.Sites.Umb.Models.Content;
using Glass.Mapper.Sites.Umb.Models.Misc;
using Glass.Mapper.Umb.Configuration.Attributes;

namespace Glass.Mapper.Sites.Umb.Models.Landing
{
    [UmbracoType(AutoMap = true)]
    public class NewsLanding : ContentBase
    {
        [UmbracoChildren]
        public virtual IEnumerable<NewsArticle> Articles { get; set; }
    }
}