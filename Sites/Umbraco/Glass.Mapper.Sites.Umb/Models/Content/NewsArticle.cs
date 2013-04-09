using System;
using System.Collections.Generic;
using Glass.Mapper.Sites.Umb.Models.Misc;
using Glass.Mapper.Umb.Configuration;
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

        [UmbracoProperty]
        public virtual File Document { get; set; }

        [UmbracoProperty("Tags", UmbracoPropertyType.Tags)]
        public virtual IEnumerable<string> Tags { get; set; }
    }
}