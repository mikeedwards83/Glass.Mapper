using System;
using Glass.Mapper.Sites.Umb.Models.Misc;
using Glass.Mapper.Umb.Configuration.Attributes;
using Glass.Mapper.Umb.PropertyTypes;

namespace Glass.Mapper.Sites.Umb.Models.Content
{
    [UmbracoType(AutoMap = true)]
    public class NewsArticle : ContentBase
    {
        public virtual int Id { get; set; }

        [UmbracoProperty]
        public virtual string Abstract { get; set; }

        [UmbracoProperty]
        public virtual string MainBody { get; set; }

        [UmbracoProperty]
        public virtual Image FeaturedImage { get; set; }

        [UmbracoProperty]
        public virtual DateTime Date { get; set; }
    }
}