using System;
using Glass.Mapper.Sc.Fields;
using Glass.Mapper.Sites.Sc.Models.Misc;

namespace Glass.Mapper.Sites.Sc.Models.Content
{
    public class NewsArticle : ContentBase
    {
        public virtual string Abstract { get; set; }

        public virtual string MainBody { get; set; }

        public virtual Image FeaturedImage { get; set; }

        public virtual DateTime Date { get; set; }
    }
}