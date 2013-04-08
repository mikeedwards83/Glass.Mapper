using System.Collections.Generic;
using Glass.Mapper.Sites.Umb.Models.Content;
using Glass.Mapper.Umb.Configuration.Attributes;

namespace Glass.Mapper.Sites.Umb.Models.Landing
{
    [UmbracoType(AutoMap =  true)]
    public class HomePage
    {
        public virtual string Title { get; set; }
        public virtual string MainBody { get; set; }
        public virtual IEnumerable<NewsArticle> News { get; set; }
    }
}