using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sites.Sc.Models.Content;
using Glass.Mapper.Sites.Sc.Models.Misc;

namespace Glass.Mapper.Sites.Sc.Models.Landing
{
    public class BlogLanding : ContentBase
    {
        public virtual string MainBody { get; set; }
        public virtual IEnumerable<BlogPost> Posts { get; set; }
    }
}