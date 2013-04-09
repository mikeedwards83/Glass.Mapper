using System.Collections.Generic;
using Glass.Mapper.Sites.Umb.Models.Content;
using Glass.Mapper.Umb.Configuration;
using Glass.Mapper.Umb.Configuration.Attributes;

namespace Glass.Mapper.Sites.Umb.Models.Landing
{
    [UmbracoType(AutoMap =  true)]
    public class HomePage
    {
        public virtual string Title { get; set; }
        public virtual string MainBody { get; set; }
        public virtual int NewsArticlesToShow { get; set; }
        public virtual string HeroColour { get; set; }
        [UmbracoProperty("NewsLanding", UmbracoPropertyType.ContentPicker)]
        public virtual NewsLanding NewsLanding { get; set; }
    }
}