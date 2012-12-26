using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.Fields
{
    public class Link
    {
        public string Anchor { get; set; }
        public string Class { get; set; }
        public string Text { get; set; }
        public string Query { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Target { get; set; }
        public Guid TargetId { get; set; }
        public LinkType Type { get; set; }
    }
}
