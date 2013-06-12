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
    /// <summary>
    /// Class SectionInfo
    /// </summary>
    public class SectionInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SectionInfo" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="sectionId">The section id.</param>
        /// <param name="templateId">The template id.</param>
        /// <param name="sectionSortOrder">The section sort order.</param>
        public SectionInfo(string name, ID sectionId, ID templateId, int sectionSortOrder)
        {
            Name = name;
            SectionId = sectionId;
            TemplateId = templateId;
            SectionSortOrder = sectionSortOrder;
        }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the section id.
        /// </summary>
        /// <value>The section id.</value>
        public ID SectionId { get; set; }
        /// <summary>
        /// Gets or sets the template id.
        /// </summary>
        /// <value>The template id.</value>
        public ID TemplateId { get; set; }
        /// <summary>
        /// Gets or sets the section sort order.
        /// </summary>
        /// <value>The section sort order.</value>
        public int SectionSortOrder { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SectionInfo" /> is existing.
        /// </summary>
        /// <value><c>true</c> if existing; otherwise, <c>false</c>.</value>
        public bool Existing { get; set; }
    }
}

