using System;

namespace Glass.Mapper.Sites.Sc.Models.Misc
{
    public class ContentBase
    {
        public virtual string Title { get; set; }

        public virtual Guid Id { get; set; }
      
        public virtual string Url { get; set; }
    }
}