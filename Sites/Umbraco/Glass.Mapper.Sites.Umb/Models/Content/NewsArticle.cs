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
using Glass.Mapper.Sites.Umb.Models.Misc;
using Glass.Mapper.Umb.Configuration;
using Glass.Mapper.Umb.Configuration.Attributes;
using Glass.Mapper.Umb.PropertyTypes;

namespace Glass.Mapper.Sites.Umb.Models.Content
{
    [UmbracoType(AutoMap = true)]
    public class NewsArticle : ContentBase
    {
        public virtual int Id { get; set; }

        [UmbracoProperty]
        public virtual string Abstract { get; set; }

        [UmbracoProperty]
        public virtual string MainBody { get; set; }

        [UmbracoInfo(UmbracoInfoType.Creator)]
        public virtual string Creator { get; set; }

        [UmbracoProperty]
        public virtual bool ShowImage { get; set; }

        [UmbracoProperty]
        public virtual Image FeaturedImage { get; set; }

        [UmbracoProperty]
        public virtual DateTime Date { get; set; }

        [UmbracoProperty]
        public virtual File Document { get; set; }

        [UmbracoProperty("Tags", UmbracoPropertyType.Tags)]
        public virtual IEnumerable<string> Tags { get; set; }
    }
}
