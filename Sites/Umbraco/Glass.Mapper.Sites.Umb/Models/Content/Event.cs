using System;

namespace Glass.Mapper.Sites.Umb.Models.Content
{
    public class Event 
    {
        public virtual int Id { get; set; }

        public virtual string ContentTypeName { get; set; }

        public virtual string Title { get; set; }

        public virtual string MainBody { get; set; }

        public virtual string Abstract { get; set; }

        public virtual string Location { get; set; }

        public virtual DateTime Start { get; set; }

        public virtual DateTime End { get; set; }

        public virtual string Url { get; set; }
    }
}