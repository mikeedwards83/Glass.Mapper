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
using Glass.Mapper.Sc.Configuration.Attributes;

namespace Glass.Mapper.Sc.Upgrade.Configuration.Attribute
{
    /// <summary>
    /// SitecoreClassAttribute
    /// </summary>
    public class SitecoreClassAttribute : SitecoreTypeAttribute
    {
        /// <summary>
        /// Indicates that the class can be used by Glass Sitecore Mapper
        /// </summary>
        public SitecoreClassAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreClassAttribute"/> class.
        /// </summary>
        /// <param name="codeFirst">if set to <c>true</c> [code first].</param>
        /// <param name="templateId">The template id.</param>
        public SitecoreClassAttribute(bool codeFirst, string templateId):base(codeFirst, templateId)
        {
        }

       

       
    }
}

