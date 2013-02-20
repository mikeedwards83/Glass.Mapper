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

namespace Glass.Mapper.Configuration.Attributes
{
    public abstract class NodeAttribute : AbstractPropertyAttribute
    {
        public NodeAttribute()
        {
            IsLazy = true;
        }

        /// <summary>
        /// Indicates that the item should be loaded lazily. Default value is true. If false the item will be loaded when the containing object is created.
        /// </summary>
        public bool IsLazy
        {
            get;
            set;
        }

        /// <summary>
        /// The path to the item. If both a path and ID are specified the ID will be used.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The Id of the item. 
        /// </summary>
        public string Id { get; set; }

        public void Configure(PropertyInfo propertyInfo, NodeConfiguration config)
        {
            config.Id = this.Id;
            config.IsLazy = this.IsLazy;
            config.Path = this.Path;
            
            base.Configure(propertyInfo, config);
        }
    }
}



