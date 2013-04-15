using System;
using System.Collections.Generic;

namespace Glass.Mapper.Sites.Umb.Models.Content
{
    public class CommentPage
    {
        public virtual Guid Id {get; set; }
        public virtual IEnumerable<Comment> Children { get; set; }
    }
}