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
using Glass.Mapper.Sites.Umb.Models.Content;
using Glass.Mapper.Umb.Configuration;
using Glass.Mapper.Umb.Configuration.Attributes;

namespace Glass.Mapper.Sites.Umb.Models.Landing
{
    [UmbracoType]
    public class EventsLanding
    {
        [UmbracoId]
        public virtual Guid Id { get; set; }

        [UmbracoInfo(UmbracoInfoType.ContentTypeName)]
        public virtual string ContentTypeName { get; set; }

        [UmbracoProperty("pageTitle")]
        public virtual string Title { get; set; }

        [UmbracoProperty]
        public virtual string MainBody { get; set; }

        [UmbracoChildren]
        public virtual IEnumerable<Event> Events { get; set; }
    }
}
