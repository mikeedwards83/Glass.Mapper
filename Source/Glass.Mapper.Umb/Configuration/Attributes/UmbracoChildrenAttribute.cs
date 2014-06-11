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


using System.Reflection;
using Glass.Mapper.Configuration;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Umb.Configuration.Attributes
{
    /// <summary>
    /// Indicates that the property should contain the children of the current item for the umbraco implementation
    /// </summary>
    public class UmbracoChildrenAttribute : ChildrenAttribute
    {
        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns></returns>
        public override AbstractPropertyConfiguration Configure(PropertyInfo propertyInfo)
        {
            var config = new UmbracoChildrenConfiguration {DocumentTypeAlias = DocumentTypeAlias};
            base.Configure(propertyInfo, config);
            return config;
        }

        /// <summary>
        /// Indicates which Document Type this property should map to
        /// </summary>
        public virtual string DocumentTypeAlias { get; set; }
    }
}




