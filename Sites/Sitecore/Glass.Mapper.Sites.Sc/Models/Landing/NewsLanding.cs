using System.Collections.Generic;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sites.Sc.Models.Content;
using Glass.Mapper.Sites.Sc.Models.Misc;

namespace Glass.Mapper.Sites.Sc.Models.Landing
{
    [SitecoreType(AutoMap = true)]
    public class NewsLanding : ContentBase
    {
        [SitecoreQuery("./*/*/*[@@templatename='NewsArticle']", IsRelative = true)]
        public virtual IEnumerable<NewsArticle> Articles { get; set; }
    }
}