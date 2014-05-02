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
using Umbraco.Core.Models;

namespace Glass.Mapper.Umb
{
    /// <summary>
    /// UmbracoDataMappingContext
    /// </summary>
    public class UmbracoDataMappingContext : AbstractDataMappingContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoDataMappingContext"/> class.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="content">The content.</param>
        /// <param name="service">The service.</param>
        public UmbracoDataMappingContext(object obj, IContent content, IUmbracoService service, bool publishedOnly)
            : base(obj)
        {
            Content = content;
            Service = service;
            PublishedOnly = publishedOnly;
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public IContent Content { get; set; }

        /// <summary>
        /// Gets or sets the service.
        /// </summary>
        /// <value>
        /// The service.
        /// </value>
        public IUmbracoService Service { get; set; }


        public bool PublishedOnly { get; set; }
    }
}

