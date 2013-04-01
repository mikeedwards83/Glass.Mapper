using System.Collections.Generic;
using Sitecore.Data;

namespace Glass.Mapper.Sites.Sc.Models.Content
{
    public class CommentPage
    {
        public virtual ID Id{get; set; }
        public virtual IEnumerable<Comment> Children { get; set; }
    }
}