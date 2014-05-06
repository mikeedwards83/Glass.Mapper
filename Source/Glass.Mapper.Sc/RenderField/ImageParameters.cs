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
    public class ImageParameters : AbstractParameters
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
        /// <summary>
        /// The HEIGHT
        /// </summary>
        public const string HEIGHT = "h";
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

        /// <summary>
        /// Gets or sets the output method.
        /// </summary>
        /// <value>The output method.</value>
        public ImageOutputMethod OutputMethod
        {
            get { return (ImageOutputMethod)Enum.Parse(typeof(ImageOutputMethod), Parameters[OUTPUT_METHOD]); }
            set { Parameters[OUTPUT_METHOD] = Enum.GetName(typeof(ImageOutputMethod), value); }
        }

        /// <summary>
        /// Gets or sets the alt.
        /// </summary>
        /// <value>The alt.</value>
        public string Alt
        {
            get { return Parameters[ALT]; }
            set { Parameters[ALT] = value; }
        }

        /// <summary>
        /// Gets or sets the border.
        /// </summary>
        /// <value>The border.</value>
        public int Border
        {
            get { return Convert.ToInt32(Parameters[BORDER]); }
            set { Parameters[BORDER] = Convert.ToString(value); }
        }

        /// <summary>
        /// Gets or sets the H space.
        /// </summary>
        /// <value>The H space.</value>
        public int HSpace
        {
            get { return Convert.ToInt32(Parameters[HSPACE]); }
            set { Parameters[HSPACE] = Convert.ToString(value); }
        }

        /// <summary>
        /// Gets or sets the V space.
        /// </summary>
        /// <value>The V space.</value>
        public int VSpace
        {
            get { return Convert.ToInt32(Parameters[VSPACE]); }
            set { Parameters[VSPACE] = Convert.ToString(value); }
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
        /// Gets or sets a value indicating whether [allow stretch].
        /// </summary>
        /// <value><c>true</c> if [allow stretch]; otherwise, <c>false</c>.</value>
        public bool AllowStretch
        {
            get { return Convert.ToBoolean(Parameters[ALLOW_STRETCH]); }
            set { Parameters[ALLOW_STRETCH] = Convert.ToString(value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [ignore aspect ratio].
        /// </summary>
        /// <value><c>true</c> if [ignore aspect ratio]; otherwise, <c>false</c>.</value>
        public bool IgnoreAspectRatio
        {
            get { return Convert.ToBoolean(Parameters[IGNORE_ASPECT_RATIO]); }
            set { Parameters[IGNORE_ASPECT_RATIO] = Convert.ToString(value); }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public int Width
        {
            get { return Convert.ToInt32(Parameters[WIDTH]); }
            set { Parameters[WIDTH] = Convert.ToString(value); }
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        public int Height
        {
            get { return Convert.ToInt32(Parameters[HEIGHT]); }
            set { Parameters[HEIGHT] = Convert.ToString(value); }
        }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        /// <value>The scale.</value>
        public float Scale
        {
            get { return Convert.ToSingle(Parameters[SCALE]); }
            set { Parameters[SCALE] = Convert.ToString(value); }
        }

        /// <summary>
        /// Gets or sets the width of the max.
        /// </summary>
        /// <value>The width of the max.</value>
        public int MaxWidth
        {
            get { return Convert.ToInt32(Parameters[MAX_WIDTH]); }
            set { Parameters[MAX_WIDTH] = Convert.ToString(value); }
        }

        /// <summary>
        /// Gets or sets the height of the max.
        /// </summary>
        /// <value>The height of the max.</value>
        public int MaxHeight
        {
            get { return Convert.ToInt32(Parameters[MAX_HEIGHT]); }
            set { Parameters[MAX_HEIGHT] = Convert.ToString(value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ImageParameters"/> is thumbnail.
        /// </summary>
        /// <value><c>true</c> if thumbnail; otherwise, <c>false</c>.</value>
        public bool Thumbnail
        {
            get { return Convert.ToBoolean(Parameters[THUMBNAIL]); }
            set { Parameters[THUMBNAIL] = Convert.ToString(value); }
        }

        /// <summary>
        /// Gets or sets the color of the background.
        /// </summary>
        /// <value>The color of the background.</value>
        public string BackgroundColor
        {
            get { return Parameters[BACKGROUND_COLOR]; }
            set { Parameters[BACKGROUND_COLOR] = value; }
        }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>The database.</value>
        public string Database
        {
            get { return Parameters[DATABASE]; }
            set { Parameters[DATABASE] = value; }
        }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>The language.</value>
        public string Language
        {
            get { return Parameters[LANGUAGE]; }
            set { Parameters[LANGUAGE] = value; }
        }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version
        {
            get { return Parameters[VERSION]; }
            set { Parameters[VERSION] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [disable media cache].
        /// </summary>
        /// <value><c>true</c> if [disable media cache]; otherwise, <c>false</c>.</value>
        public bool DisableMediaCache
        {
            get { return Convert.ToBoolean(Parameters[DISABLE_MEDIA_CACHE]); }
            set { Parameters[DISABLE_MEDIA_CACHE] = Convert.ToString(value); }
        }
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




