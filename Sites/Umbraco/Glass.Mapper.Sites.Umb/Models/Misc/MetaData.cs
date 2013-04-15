using System;
using System.Collections.Generic;
using Glass.Mapper.Umb.Configuration.Attributes;

namespace Glass.Mapper.Sites.Umb.Models.Misc
{
    [UmbracoType(AutoMap = true)]
    public class MetaData
    {
        public virtual int Id { get; set; }

        public virtual string Title { get; set; }

        public virtual DateTime UpdateDate { get; set; }

        public virtual MetaData Parent { get; set; }

        public virtual IEnumerable<MetaData> Children { get; set; }
    }
}