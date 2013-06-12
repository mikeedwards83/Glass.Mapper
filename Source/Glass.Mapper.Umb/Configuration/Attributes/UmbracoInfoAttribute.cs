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
using System.Reflection;
using System.Text;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Umb.Configuration.Attributes
{
    /// <summary>
    /// UmbracoInfoAttribute
    /// </summary>
    public class UmbracoInfoAttribute : InfoAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoInfoAttribute"/> class.
        /// </summary>
        public UmbracoInfoAttribute()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoInfoAttribute"/> class.
        /// </summary>
        /// <param name="infoType">Type of the info.</param>
        public UmbracoInfoAttribute(UmbracoInfoType infoType)
        {
            Type = infoType;
        }

        /// <summary>
        /// The type of information that should populate the property
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public UmbracoInfoType Type { get; set; }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <returns></returns>
        public override Mapper.Configuration.AbstractPropertyConfiguration Configure(System.Reflection.PropertyInfo propertyInfo)
        {
            var config = new UmbracoInfoConfiguration();
            Configure(propertyInfo, config);
            return config;
        }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="config">The config.</param>
        public void Configure(PropertyInfo propertyInfo, UmbracoInfoConfiguration config)
        {
            config.Type = this.Type;

            base.Configure(propertyInfo, config);
        }
    }
}

