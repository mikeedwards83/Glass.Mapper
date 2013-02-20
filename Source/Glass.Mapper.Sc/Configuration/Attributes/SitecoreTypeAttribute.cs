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
using Glass.Mapper.Configuration.Attributes;
using Glass.Mapper.Configuration;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    public class SitecoreTypeAttribute : AbstractTypeAttribute
    {
        /// <summary>
        /// Indicates the template to use when trying to create an item
        /// </summary>
        public string TemplateId { get; set; }
        /// <summary>
        /// Indicates the branch to use when trying to create and item. If a template id is also specified the template Id will be use instead.
        /// </summary>
        public string BranchId { get; set; }

        /// <summary>
        /// Indicates that the class is used in a code first scenario.
        /// </summary>
        public bool CodeFirst { get; set; }

        /// <summary>
        /// Overrides the default template name when using code first
        /// </summary>
        public string TemplateName { get; set; }

        public override void Configure(Type type, Mapper.Configuration.AbstractTypeConfiguration config)
        {
            var scConfig = config as SitecoreTypeConfiguration;

            if (scConfig == null)
                throw new ConfigurationException(
                    "Type configuration is not of type {0}".Formatted(typeof (SitecoreTypeConfiguration).FullName));


            if (BranchId.IsNotNullOrEmpty())
                scConfig.BranchId = new ID(this.BranchId);
            else
                scConfig.BranchId = ID.Null;

            if (TemplateId.IsNotNullOrEmpty())
                scConfig.TemplateId = new ID(this.TemplateId);
            else
                scConfig.TemplateId = ID.Null;

            scConfig.CodeFirst = scConfig.CodeFirst;
            scConfig.TemplateName = scConfig.TemplateName;

            base.Configure(type, config);
        }
    }
}



