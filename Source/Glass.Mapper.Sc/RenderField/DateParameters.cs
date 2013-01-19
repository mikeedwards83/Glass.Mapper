using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.RenderField
{
    /// <summary>
    /// Used for specifying rendering parameters for a Date or Datetime field when outputting through a field renderer.
    /// </summary>
    public class DateParameters : AbstractParameters
    {
        public const string FORMAT = "format";

        public string Format
        {
            get { return Parameters[FORMAT]; }
            set { Parameters[FORMAT] = value; }
        }
    }
}
