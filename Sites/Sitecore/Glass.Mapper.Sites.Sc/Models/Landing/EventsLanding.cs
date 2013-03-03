using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sites.Sc.Models.Content;
using Sitecore.Globalization;

namespace Glass.Mapper.Sites.Sc.Models.Landing
{
    [SitecoreType]
    public class EventsLanding
    {
        [SitecoreId]
        public virtual Guid Id { get; set; }

        [SitecoreInfo(SitecoreInfoType.Language)]
        public virtual Language Language { get; set; }

        [SitecoreField("Page Title")]
        public virtual string Title { get; set; }

        [SitecoreField]
        public virtual string MainBody { get; set; }

        [SitecoreQuery("./*/*/*[@@templatename='Event']", IsRelative = true)]
        public virtual IEnumerable<Event> Events { get; set; }
    }
}