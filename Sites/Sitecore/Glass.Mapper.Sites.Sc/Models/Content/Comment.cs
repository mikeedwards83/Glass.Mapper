using System;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace Glass.Mapper.Sites.Sc.Models.Content
{
    [SitecoreType(TemplateId = "{B6E1D6BA-EE54-4E71-8003-E6A1AF7119EB}", AutoMap = true)]
    public class Comment
    {
        public virtual string Name { get; set; }

        public string FullName { get; set; }
        public virtual string Email { get; set; }
        
        [SitecoreField("Comment")]
        public virtual string Content { get; set; }
        
        [SitecoreField("__Created")]
        public virtual DateTime Date { get; set; }
    }
}