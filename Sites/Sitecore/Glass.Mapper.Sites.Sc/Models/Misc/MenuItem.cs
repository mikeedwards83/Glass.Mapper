using System.Collections.Generic;

namespace Glass.Mapper.Sites.Sc.Models.Misc
{
    public class MenuItem
    {
        public virtual string Title { get; set; }

        public virtual string Url { get; set; }

        public virtual IEnumerable<MenuItem> Children { get; set; }
    }
}