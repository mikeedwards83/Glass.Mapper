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
using System.Text;

namespace Glass.Mapper.Sc.Fields
{
    public class Link
    {
        public string Anchor { get; set; }
        public string Class { get; set; }
        public string Text { get; set; }
        public string Query { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Target { get; set; }
        public Guid TargetId { get; set; }
        public LinkType Type { get; set; }
    }
}



