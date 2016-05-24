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


using Glass.Mapper.Configuration;

namespace Glass.Mapper.Sc.Configuration
{
    /// <summary>
    /// Class SitecoreInfoConfiguration
    /// </summary>
    public class SitecoreInfoConfiguration : InfoConfiguration
    {
        /// <summary>
        /// The type of information that should populate the property
        /// </summary>
        /// <value>The type.</value>
        public SitecoreInfoType Type { get; set; }

        /// <summary>
        /// UrlOptions, use in conjunction with SitecoreInfoType.Url
        /// </summary>
        /// <value>The URL options.</value>
        public SitecoreInfoUrlOptions UrlOptions { get; set; }

        /// <summary>
        /// MediaUrlOptions, use in conjunction with SitecoreInfoType.MediaUrl
        /// </summary>
        /// <value>The URL options.</value>
        public SitecoreInfoMediaUrlOptions MediaUrlOptions { get; set; }


        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new SitecoreInfoConfiguration();
        }

        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as SitecoreInfoConfiguration;
            config.Type = Type;
            config.UrlOptions = UrlOptions;
            config.MediaUrlOptions = MediaUrlOptions;
            base.Copy(copy);
        }
    }
}




