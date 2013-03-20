using System;
using Sitecore.Globalization;

namespace Glass.Mapper.Sites.Sc.Models.Content
{
    public class Event
    {
        public virtual Guid Id { get; set; }

        public virtual Language Language { get; set; }

        public virtual string Title { get; set; }

        public virtual string MainBody { get; set; }

        public virtual string Abstract { get; set; }

        public virtual string Location { get; set; }

        public virtual DateTime Start { get; set; }

        public virtual DateTime End { get; set; }

        public virtual string Url { get; set; }
    }
}