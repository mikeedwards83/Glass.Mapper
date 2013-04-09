using System;
using Glass.Mapper.Umb.Configuration.Attributes;

namespace Glass.Mapper.Sites.Umb.Models.Content
{
    [UmbracoType(ContentTypeAlias = "Comment", AutoMap = true)]
    public class Comment
    {
        public virtual string Name { get; set; }

        public string FullName { get; set; }
        public virtual string Email { get; set; }
        
        [UmbracoProperty("Comment")]
        public virtual string Content { get; set; }
        
        [UmbracoProperty("__Created")]
        public virtual DateTime Date { get; set; }
    }
}