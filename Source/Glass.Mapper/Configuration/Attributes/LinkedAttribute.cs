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

namespace Glass.Mapper.Configuration.Attributes
{
    /// <summary>
    /// Class LinkedAttribute
    /// </summary>
    public abstract class LinkedAttribute : AbstractPropertyAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkedAttribute"/> class.
        /// </summary>
        public LinkedAttribute()
        {
            IsLazy = true;
        }
        /// <summary>
        /// Indicates if linked items should be loaded lazily. Default value is true. If false linked items will be loaded when the contain object is created.
        /// </summary>
        /// <value><c>true</c> if this instance is lazy; otherwise, <c>false</c>.</value>
        public bool IsLazy
        {
            get;
            set;
        }
        /// <summary>
        /// Indicates the type should be inferred from the item template
        /// </summary>
        /// <value><c>true</c> if [infer type]; otherwise, <c>false</c>.</value>
        public bool InferType { get; set; }

        /// <summary>
        /// Configures the specified info.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="config">The config.</param>
        public void Configure(PropertyInfo info, LinkedConfiguration config)
        {
            config.IsLazy = IsLazy;
            config.InferType = InferType;

            base.Configure(info, config);
        }
    }
}




