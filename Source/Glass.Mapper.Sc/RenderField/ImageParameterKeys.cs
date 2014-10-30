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

namespace Glass.Mapper.Sc.RenderField
{
    /// <summary>
    /// Used for specifying rendering parameters for an Image field when outputting through a field renderer.
    /// </summary>
    public class ImageParameterKeys
    {
        /// <summary>
        /// The OUTPU t_ METHOD
        /// </summary>
        public const string OUTPUT_METHOD = "outputMethod";

        /// <summary>
        /// The ALT
        /// </summary>
        public const string ALT = "alt";

        /// <summary>
        /// The BORDER
        /// </summary>
        public const string BORDER = "border";

        /// <summary>
        /// The HSPACE
        /// </summary>
        public const string HSPACE = "hspace";

        /// <summary>
        /// The VSPACE
        /// </summary>
        public const string VSPACE = "vspace";

        /// <summary>
        /// The CLASS
        /// </summary>
        public const string CLASS = "class";

        /// <summary>
        /// The ALLO w_ STRETCH
        /// </summary>
        public const string ALLOW_STRETCH = "as";

        /// <summary>
        /// The IGNOR e_ ASPEC t_ RATIO
        /// </summary>
        public const string IGNORE_ASPECT_RATIO = "iar";

        /// <summary>
        /// The WIDTH
        /// </summary>
        public const string WIDTH = "w";
        public const string WIDTHHTML = "width";

        /// <summary>
        /// The HEIGHT
        /// </summary>
        public const string HEIGHT = "h";
        public const string HEIGHTHTML = "height";

        /// <summary>
        /// The SCALE
        /// </summary>
        public const string SCALE = "sc";

        /// <summary>
        /// The MA x_ WIDTH
        /// </summary>
        public const string MAX_WIDTH = "mw";

        /// <summary>
        /// The MA x_ HEIGHT
        /// </summary>
        public const string MAX_HEIGHT = "mh";

        /// <summary>
        /// The THUMBNAIL
        /// </summary>
        public const string THUMBNAIL = "thn";

        /// <summary>
        /// The BACKGROUN d_ COLOR
        /// </summary>
        public const string BACKGROUND_COLOR = "bc";

        /// <summary>
        /// The DATABASE
        /// </summary>
        public const string DATABASE = "db";

        /// <summary>
        /// The LANGUAGE
        /// </summary>
        public const string LANGUAGE = "la";

        /// <summary>
        /// The VERSION
        /// </summary>
        public const string VERSION = "vs";

        /// <summary>
        /// The DISABL e_ MEDI a_ CACHE
        /// </summary>
        public const string DISABLE_MEDIA_CACHE = "disableMediaCache";
    }

    /// <summary>
    /// Enum ImageOutputMethod
    /// </summary>
    public enum ImageOutputMethod
    {
        /// <summary>
        /// The HTML
        /// </summary>
        Html,
        /// <summary>
        /// The X HTML
        /// </summary>
        XHtml
    }
}




