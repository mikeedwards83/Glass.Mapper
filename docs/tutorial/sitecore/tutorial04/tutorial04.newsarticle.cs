using System;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace Glass.Mapper.Sites.Sc.Models.Content
{
    [SitecoreType]
    public class NewsArticle 
    {
        [SitecoreField]
        public virtual string Title { get; set; }

        [SitecoreField]
        public virtual string Abstract { get; set; }

        [SitecoreField]
        public virtual string MainBody { get; set; }

        [SitecoreField]
        public virtual Image FeaturedImage { get; set; }

        [SitecoreField]
        public virtual DateTime Date { get; set; }

        [SitecoreInfo(SitecoreInfoType.Url)]
        public virtual string Url { get; set; }
    }
}