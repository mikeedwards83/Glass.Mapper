using System.Collections.Generic;
using Glass.Mapper.Sites.Sc.Models.Content;
using Glass.Mapper.Sites.Sc.Models.Misc;

namespace Glass.Mapper.Sites.Sc.Models.Landing
{
    
    public class HomePage: ContentBase
    {
        
        public virtual string MainBody { get; set; }

        public virtual IEnumerable<NewsArticle> News { get; set; }
    }
}