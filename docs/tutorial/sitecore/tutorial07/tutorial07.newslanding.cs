using System.Collections.Generic;
using Glass.Mapper.Sites.Sc.Models.Content;
using Glass.Mapper.Sites.Sc.Models.Misc;

namespace Glass.Mapper.Sites.Sc.Models.Landing
{
    public class NewsLanding : ContentBase
    {
        public virtual IEnumerable<NewsArticle> Articles { get; set; }
    }
}