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
using Glass.Mapper.Configuration.Attributes;
using Glass.Mapper.Configuration;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    /// <summary>
    /// Class SitecoreTypeAttribute
    /// </summary>
    public class SitecoreTypeAttribute : AbstractTypeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreTypeAttribute"/> class.
        /// </summary>
        public SitecoreTypeAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreTypeAttribute"/> class.
        /// </summary>
        /// <param name="codeFirst">if set to <c>true</c> [code first].</param>
        /// <param name="templateId">The template id.</param>
        public SitecoreTypeAttribute(bool codeFirst, string templateId)
        {
            TemplateId = templateId;
            CodeFirst = codeFirst;
        }

        /// <summary>
        /// Indicates the template to use when trying to create an item
        /// </summary>
        /// <value>The template id.</value>
        public string TemplateId { get; set; }
        /// <summary>
        /// Indicates the branch to use when trying to create and item. If a template id is also specified the template Id will be use instead.
        /// </summary>
        /// <value>The branch id.</value>
        public string BranchId { get; set; }

        /// <summary>
        /// Overrides the default template name when using code first
        /// </summary>
        /// <value>The name of the template.</value>
        public string TemplateName { get; set; }

        /// <summary>
        /// Forces Glass to do a template check and only returns an class if the item 
        /// matches the template ID or inherits a template with the templateId
        /// </summary>
        public SitecoreEnforceTemplate EnforceTemplate { get; set; }

        /// <summary>
        /// Configures the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="config">The config.</param>
        /// <exception cref="Glass.Mapper.Configuration.ConfigurationException">Type configuration is not of type {0}.Formatted(typeof (SitecoreTypeConfiguration).FullName)</exception>
        public override AbstractTypeConfiguration Configure(Type type)
        {
            var scConfig = new SitecoreTypeConfiguration();

            if (scConfig == null)
                throw new ConfigurationException(
                    "Type configuration is not of type {0}".Formatted(typeof(SitecoreTypeConfiguration).FullName));


            try
            {
                if (BranchId.IsNotNullOrEmpty())
                    scConfig.BranchId = new ID(this.BranchId);
                else
                    scConfig.BranchId = ID.Null;
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to convert BranchId for type {0}. Value was {1}".Formatted(type.FullName, this.TemplateId), ex);
            }

            try
            {
                if (TemplateId.IsNotNullOrEmpty())
                    scConfig.TemplateId = new ID(this.TemplateId);
                else
                    scConfig.TemplateId = ID.Null;
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to convert TemplateId for type {0}. Value was {1}".Formatted(type.FullName, this.TemplateId), ex);
            }




            if (TemplateId.IsNullOrEmpty() && this.EnforceTemplate != SitecoreEnforceTemplate.No)
            {
                throw new ConfigurationException(
                    "The type {0} has EnforceTemplate set to true but no TemplateId".Formatted(type.FullName));
            }
            scConfig.EnforceTemplate = this.EnforceTemplate;




            scConfig.CodeFirst = this.CodeFirst;
            scConfig.TemplateName = this.TemplateName;

            base.Configure(type, scConfig);

            return scConfig;
        }
    }
}




