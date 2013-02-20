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
    /// Used for specifying rendering parameters for an Image field when outputting through a field renderer.
    /// </summary>
    public class ImageParameters : AbstractParameters
    {
        public const string OUTPUT_METHOD = "outputMethod";
        public const string ALT = "alt";
        public const string BORDER = "border";
        public const string HSPACE = "hspace";
        public const string VSPACE = "vspace";
        public const string CLASS = "class";
        public const string ALLOW_STRETCH = "as";
        public const string IGNORE_ASPECT_RATIO = "iar";
        public const string WIDTH = "w";
        public const string HEIGHT = "h";
        public const string SCALE = "sc";
        public const string MAX_WIDTH = "mw";
        public const string MAX_HEIGHT = "mh";
        public const string THUMBNAIL = "thn";
        public const string BACKGROUND_COLOR = "bc";
        public const string DATABASE = "db";
        public const string LANGUAGE = "la";
        public const string VERSION = "vs";
        public const string DISABLE_MEDIA_CACHE = "disableMediaCache";

        public ImageOutputMethod OutputMethod
        {
            get { return (ImageOutputMethod)Enum.Parse(typeof(ImageOutputMethod), Parameters[OUTPUT_METHOD]); }
            set { Parameters[OUTPUT_METHOD] = Enum.GetName(typeof(ImageOutputMethod), value); }
        }

        public string Alt
        {
            get { return Parameters[ALT]; }
            set { Parameters[ALT] = value; }
        }

        public int Border
        {
            get { return Convert.ToInt32(Parameters[BORDER]); }
            set { Parameters[BORDER] = Convert.ToString(value); }
        }

        public int HSpace
        {
            get { return Convert.ToInt32(Parameters[HSPACE]); }
            set { Parameters[HSPACE] = Convert.ToString(value); }
        }

        public int VSpace
        {
            get { return Convert.ToInt32(Parameters[VSPACE]); }
            set { Parameters[VSPACE] = Convert.ToString(value); }
        }

        public string Class
        {
            get { return Parameters[CLASS]; }
            set { Parameters[CLASS] = value; }
        }

        public bool AllowStretch
        {
            get { return Convert.ToBoolean(Parameters[ALLOW_STRETCH]); }
            set { Parameters[ALLOW_STRETCH] = Convert.ToString(value); }
        }

        public bool IgnoreAspectRatio
        {
            get { return Convert.ToBoolean(Parameters[IGNORE_ASPECT_RATIO]); }
            set { Parameters[IGNORE_ASPECT_RATIO] = Convert.ToString(value); }
        }

        public int Width
        {
            get { return Convert.ToInt32(Parameters[WIDTH]); }
            set { Parameters[WIDTH] = Convert.ToString(value); }
        }

        public int Height
        {
            get { return Convert.ToInt32(Parameters[HEIGHT]); }
            set { Parameters[HEIGHT] = Convert.ToString(value); }
        }

        public float Scale
        {
            get { return Convert.ToSingle(Parameters[SCALE]); }
            set { Parameters[SCALE] = Convert.ToString(value); }
        }

        public int MaxWidth
        {
            get { return Convert.ToInt32(Parameters[MAX_WIDTH]); }
            set { Parameters[MAX_WIDTH] = Convert.ToString(value); }
        }

        public int MaxHeight
        {
            get { return Convert.ToInt32(Parameters[MAX_HEIGHT]); }
            set { Parameters[MAX_HEIGHT] = Convert.ToString(value); }
        }

        public bool Thumbnail
        {
            get { return Convert.ToBoolean(Parameters[THUMBNAIL]); }
            set { Parameters[THUMBNAIL] = Convert.ToString(value); }
        }

        public string BackgroundColor
        {
            get { return Parameters[BACKGROUND_COLOR]; }
            set { Parameters[BACKGROUND_COLOR] = value; }
        }

        public string Database
        {
            get { return Parameters[DATABASE]; }
            set { Parameters[DATABASE] = value; }
        }

        public string Language
        {
            get { return Parameters[LANGUAGE]; }
            set { Parameters[LANGUAGE] = value; }
        }

        public string Version
        {
            get { return Parameters[VERSION]; }
            set { Parameters[VERSION] = value; }
        }

        public bool DisableMediaCache
        {
            get { return Convert.ToBoolean(Parameters[DISABLE_MEDIA_CACHE]); }
            set { Parameters[DISABLE_MEDIA_CACHE] = Convert.ToString(value); }
        }
    }

    public enum ImageOutputMethod
    {
        Html,
        XHtml
    }
}



