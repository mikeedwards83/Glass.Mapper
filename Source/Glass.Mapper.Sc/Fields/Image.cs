using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.Fields
{
    public class Image
    {
        public string Alt { get; set; }
        public string Border { get; set; }
        public string Class { get; set; }
        public int Height { get; set; }
        public int HSpace { get; set; }
        public string Src { get; internal set; }
        public int VSpace { get; set; }
        public int Width { get; set; }
        public Guid MediaId { get; set; }

    }
}
