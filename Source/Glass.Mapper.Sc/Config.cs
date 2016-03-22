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

using System.Collections.Generic;
using Glass.Mapper.Sc.RenderField;

namespace Glass.Mapper.Sc
{
    public class Config : Glass.Mapper.Config
    {
        public Config()
        {
            ForceItemInPageEditor = true;
            UseGlassHtmlLambdaCache = true;

            ImageAttributes = new HashSet<string>(new[]
           {
                ImageParameterKeys.BORDER,
                ImageParameterKeys.ALT,
                ImageParameterKeys.HSPACE,
                ImageParameterKeys.VSPACE,
                ImageParameterKeys.CLASS,
                ImageParameterKeys.WIDTHHTML,
                ImageParameterKeys.HEIGHTHTML
            });
            ImageQueryString = new HashSet<string>(new[]
            {
                ImageParameterKeys.OUTPUT_METHOD,
                ImageParameterKeys.ALLOW_STRETCH,
                ImageParameterKeys.IGNORE_ASPECT_RATIO,
                ImageParameterKeys.SCALE,
                ImageParameterKeys.MAX_WIDTH,
                ImageParameterKeys.MAX_HEIGHT,
                ImageParameterKeys.THUMBNAIL,
                ImageParameterKeys.BACKGROUND_COLOR,
                ImageParameterKeys.DATABASE,
                ImageParameterKeys.LANGUAGE,
                ImageParameterKeys.VERSION,
                ImageParameterKeys.DISABLE_MEDIA_CACHE,
                ImageParameterKeys.WIDTH,
                ImageParameterKeys.HEIGHT
            });
        }


        public HashSet<string> ImageAttributes { get; private set; }
        public HashSet<string> ImageQueryString { get; private set; }


        public bool ForceItemInPageEditor { get; set; }

        /// <summary>
        /// When using Lazy Enumerables this setting indicates that proxy objects
        /// should be used. Be default this is false.
        /// </summary>
        public bool UseProxiesForLazyEnumerables { get; set; }

        /// <summary>
        /// Indicates it the lambda expressions used by Editable expression in renderings are cached for performance.
        /// By default this is true, if you experience caching issue then set this to false.
        /// </summary>
        public bool UseGlassHtmlLambdaCache { get; set; }

        /// <summary>
        /// Completely disables the version count check mechinism across the entire solution
        /// </summary>
        public bool DisableVersionCount { get; set; }
    }
}
