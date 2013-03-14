using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.Razor
{
    /// <summary>
    /// Class RawString
    /// </summary>
    public class RawString : RazorEngine.Text.RawString
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RawString"/> class.
        /// </summary>
        /// <param name="content">The content.</param>
        public RawString(string content)
            : base(content)
        {
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="RawString"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="raw">The raw.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string (RawString raw){
            return raw.ToString();
        }
    }
}
