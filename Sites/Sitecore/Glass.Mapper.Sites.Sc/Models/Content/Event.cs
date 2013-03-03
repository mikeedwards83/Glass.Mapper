using System;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.Globalization;

namespace Glass.Mapper.Sites.Sc.Models.Content
{
    [SitecoreType]
    public class Event
    {
        [SitecoreId]
        public virtual Guid Id { get; set; }

        [SitecoreInfo(SitecoreInfoType.Language)]
        public virtual Language Language { get; set; }

        [SitecoreField("Page Title")]
        public virtual string Title { get; set; }

        [SitecoreField]
        public virtual string MainBody { get; set; }


        [SitecoreField]
        public virtual string Abstract { get; set; }

        [SitecoreField]
        public virtual string Location { get; set; }

        [SitecoreField]
        public virtual DateTime Start { get; set; }

        [SitecoreField]
        public virtual DateTime End { get; set; }

        [SitecoreInfo(SitecoreInfoType.Url)]
        public virtual string Url { get; set; }
    }
}