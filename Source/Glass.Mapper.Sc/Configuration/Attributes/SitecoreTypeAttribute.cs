using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration.Attributes;
using Glass.Mapper.Configuration;

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


        public override void Configure(Type type, Mapper.Configuration.AbstractTypeConfiguration config)
        {
            var scConfig = config as SitecoreTypeConfiguration;

            if (scConfig == null)
                throw new ConfigurationException(
                    "Type configuration is not of type {0}".Formatted(typeof (SitecoreTypeConfiguration).FullName));


            scConfig.BranchId =  new Guid(this.BranchId);
            scConfig.TemplateId = new Guid(this.TemplateId);


            base.Configure(type, config);
        }
    }
}
