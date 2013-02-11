/*
   Copyright 2011 Michael Edwards
 
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data;

namespace Glass.Mapper.Sc.CodeFirst
{
    public class SectionInfo
    {
        public SectionInfo(string name, ID sectionId, ID templateId, int sectionSortOrder)
        {
            Name = name;
            SectionId = sectionId;
            TemplateId = templateId;
            SectionSortOrder = sectionSortOrder;
        }
        public string Name { get; set; }
        public ID SectionId { get; set; }
        public ID TemplateId { get; set; }
        public int SectionSortOrder { get; set; }
        public bool Existing { get; set; }
    }
}
