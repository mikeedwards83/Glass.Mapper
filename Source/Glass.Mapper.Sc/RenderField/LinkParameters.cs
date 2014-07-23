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

namespace Glass.Mapper.Sc.RenderField
{
    /// <summary>
    /// Used for specifying rendering parameters for a General Link field when outputting through a field renderer.
    /// </summary>
    public class LinkParameters : AbstractParameters
    {
        /// <summary>
        /// The TARGET
        /// </summary>
        public const string TARGET = "target";
        /// <summary>
        /// The TITLE
        /// </summary>
        public const string TITLE = "title";
        /// <summary>
        /// The CLASS
        /// </summary>
        public const string CLASS = "class";
        /// <summary>
        /// The TEXT
        /// </summary>
        public const string TEXT = "text";

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        public string Target
        {
            get { return Parameters[TARGET]; }
            set { Parameters[TARGET] = value; }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return Parameters[TITLE]; }
            set { Parameters[TITLE] = value; }
        }

        /// <summary>
        /// Gets or sets the class.
        /// </summary>
        /// <value>The class.</value>
        public string Class
        {
            get { return Parameters[CLASS]; }
            set { Parameters[CLASS] = value; }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get { return Parameters[TEXT]; }
            set { Parameters[TEXT] = value; }
        }
    }
}




