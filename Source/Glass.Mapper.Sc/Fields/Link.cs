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
using System.Collections.Specialized;
using System.Linq;
using Sitecore.Collections;

namespace Glass.Mapper.Sc.Fields
{
    /// <summary>
    /// Class Link
    /// </summary>
    [Serializable]
    public class Link
    {
        /// <summary>
        /// Gets or sets the anchor.
        /// </summary>
        /// <value>The anchor.</value>
        public string Anchor { get; set; }
        /// <summary>
        /// Gets or sets the class.
        /// </summary>
        /// <value>The class.</value>
        public string Class { get; set; }
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }
        /// <summary>
        /// Gets or sets the query.
        /// </summary>
        /// <value>The query.</value>
        public string Query { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }
        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        public string Target { get; set; }
        /// <summary>
        /// Gets or sets the target id.
        /// </summary>
        /// <value>The target id.</value>
        public Guid TargetId { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public LinkType Type { get; set; }


        public const string UrlFormat = "{0}{1}";



        [Obsolete("User the SafeDictionary alternative")]
        public string BuildUrl(NameValueCollection attributes = null)
        {
            return BuildUrl(attributes.ToSafeDictionary());
        }

        public string BuildUrl(SafeDictionary<string> attributes = null)
        {
            if (attributes == null)
            {
                attributes = new SafeDictionary<string>();
            }

            Func<string, Func<string>, string> getValue = (key, func) =>
            {
                var value = attributes[key] ?? func();
                attributes.Remove(key);
                return value;
            };

            var url = Url;

            if (Type == LinkType.Anchor)
            {
                url = string.Empty;
            }

            UrlBuilder builder = new UrlBuilder(url);

            var query = getValue("query", () => Query);
            var anchor = getValue("anchor", () => Anchor);

            if (query.IsNotNullOrEmpty())
                builder.AddToQueryString(query);

            return UrlFormat.Formatted(builder.ToString(), anchor.IsNullOrEmpty() ? "" : "#" + anchor);
        }
    }
}




