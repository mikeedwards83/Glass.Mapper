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
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    /// <summary>
    /// Class SitecoreLinkedAttribute
    /// </summary>
    public class SitecoreLinkedAttribute : LinkedAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreLinkedAttribute"/> class.
        /// </summary>
        public SitecoreLinkedAttribute()
        {
            Option = SitecoreLinkedOptions.All;
        }


        /// <summary>
        /// Indicate weather All, References or Referred should be loaded
        /// </summary>
        /// <value>The option.</value>
        public SitecoreLinkedOptions Option { get; set; }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns>AbstractPropertyConfiguration.</returns>
        public override Mapper.Configuration.AbstractPropertyConfiguration Configure(System.Reflection.PropertyInfo propertyInfo)
        {
            var config = new SitecoreLinkedConfiguration();
            this.Configure(propertyInfo, config);
            return config;
        }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="config">The config.</param>
        public void Configure(PropertyInfo propertyInfo, SitecoreLinkedConfiguration config)
        {
            config.Option = this.Option;

            base.Configure(propertyInfo, config);
        }
    }
}




