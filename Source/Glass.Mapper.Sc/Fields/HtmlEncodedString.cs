using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Glass.Mapper.Sc.Fields
{
    /// <summary>
    /// Represents a string that will be HTML encoded
    /// </summary>
    public class HtmlEncodedString
    {
        public string RawValue { get; set; }

        public HtmlEncodedString(string value)
        {
            RawValue = value;
        }

        public static implicit operator string(HtmlEncodedString encoded)
        {
            return encoded.ToString();
        }

        public static implicit operator HtmlEncodedString(string value)
        {
            return new HtmlEncodedString(value);
        }
        public override string ToString()
        {
            return HttpUtility.HtmlEncode(RawValue);
        }
    }
}
