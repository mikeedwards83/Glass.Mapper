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
using Glass.Mapper.Sc.Configuration.Attributes;

namespace Glass.Mapper.Sites.Sc.Models.Content
{
    [SitecoreType(TemplateId = "{B6E1D6BA-EE54-4E71-8003-E6A1AF7119EB}", AutoMap = true)]
    public class Comment
    {
        public virtual string Name { get; set; }

        public string FullName { get; set; }
        public virtual string Email { get; set; }
        
        [SitecoreField("Comment")]
        public virtual string Content { get; set; }
        
        [SitecoreField("__Created")]
        public virtual DateTime Date { get; set; }
    }
}
