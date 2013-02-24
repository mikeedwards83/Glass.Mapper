using System.Collections.Generic;

namespace Glass.Mapper.Sites.Sc.Models.Misc
{
    public class MenuItem : ContentBase
    {
        public virtual IEnumerable<MenuItem> Children { get; set; }
    }
}