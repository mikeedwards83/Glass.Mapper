using System;
using System.Collections.Generic;
using Glass.Mapper.Sites.Umb.Models.Content;
using Glass.Mapper.Umb.Configuration;
using Glass.Mapper.Umb.Configuration.Attributes;

namespace Glass.Mapper.Sites.Umb.Models.Landing
{
    [UmbracoType]
    public class EventsLanding
    {
        [UmbracoId]
        public virtual Guid Id { get; set; }

        [UmbracoInfo(UmbracoInfoType.ContentTypeName)]
        public virtual string ContentTypeName { get; set; }

        [UmbracoProperty("pageTitle")]
        public virtual string Title { get; set; }

        [UmbracoProperty]
        public virtual string MainBody { get; set; }

        [UmbracoChildren]
        public virtual IEnumerable<Event> Events { get; set; }
    }
}