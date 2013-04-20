using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sites.Sc.Models.Misc;

namespace Glass.Mapper.Sites.Sc.Models.Content
{
    public class BlogPost : ContentBase
    {
        public virtual string MainBody { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string Author { get; set; }
    }
}