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

using System.Collections.Specialized;
using Sitecore.Collections;

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// Class ExtensionMethods
    /// </summary>
    public static class ExtensionMethods
    {
       #region NameValueCollection

        public static SafeDictionary<string> ToSafeDictionary(this NameValueCollection collection)
        {
            var safeDictionary = new SafeDictionary<string>();
            
            if (collection != null)
            {
                foreach (var key in collection.AllKeys)
                {
                    safeDictionary.Add(key, collection[key]);
                }
            }

            return safeDictionary;
        }

        #endregion

        public static bool HasValue(this Glass.Mapper.Sc.Fields.Image image)
        {
            return image != null && image.Src.HasValue();
        }
        public static bool HasValue(this Glass.Mapper.Sc.Fields.Link link)
        {
            return link != null && link.Url.HasValue();
        }
    }
}




