using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.RenderField
{
    /// <summary>
    /// Used for specifying rendering parameters for a General Link field when outputting through a field renderer.
    /// </summary>
    public class LinkParameters : AbstractParameters
    {
        public const string TARGET = "target";
        public const string TITLE = "title";
        public const string CLASS = "class";
        public const string TEXT = "text";

        public string Target
        {
            get { return Parameters[TARGET]; }
            set { Parameters[TARGET] = value; }
        }

        public string Title
        {
            get { return Parameters[TITLE]; }
            set { Parameters[TITLE] = value; }
        }

        public string Class
        {
            get { return Parameters[CLASS]; }
            set { Parameters[CLASS] = value; }
        }

        public string Text
        {
            get { return Parameters[TEXT]; }
            set { Parameters[TEXT] = value; }
        }
    }
}
