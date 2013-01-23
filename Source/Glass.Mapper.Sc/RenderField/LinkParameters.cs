/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-

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



