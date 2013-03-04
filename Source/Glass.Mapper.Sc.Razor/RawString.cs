using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.Razor
{
    public class RawString : RazorEngine.Text.RawString
    {
        public RawString(string content)
            : base(content)
        {
        }

        public static implicit operator string (RawString raw){
            return raw.ToString();
        }
    }
}
