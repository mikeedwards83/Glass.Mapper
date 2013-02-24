using System;
using Glass.Mapper.Sc.Fields;

namespace Glass.Mapper.Sites.Sc.Models.Content
{
    public class NewsArticle
    {
        public virtual Guid Id { get; set; }

        public virtual string Title { get; set; }

        public virtual string Abstract { get; set; }

        public virtual string MainBody { get; set; }

        public virtual Image FeaturedImage { get; set; }

        public virtual DateTime Date { get; set; }

        public virtual string Url { get; set; }
    }
}