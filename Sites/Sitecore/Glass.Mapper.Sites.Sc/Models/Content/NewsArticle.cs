using System;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Glass.Mapper.Sites.Sc.Models.Misc;

namespace Glass.Mapper.Sites.Sc.Models.Content
{
    [SitecoreType(AutoMap = true)]
    public class NewsArticle : ContentBase
    {
        [SitecoreField]
        public virtual string Abstract { get; set; }

        [SitecoreField]
        public virtual string MainBody { get; set; }

        [SitecoreField]
        public virtual Image FeaturedImage { get; set; }

        [SitecoreField]
        public virtual DateTime Date { get; set; }
    }
}