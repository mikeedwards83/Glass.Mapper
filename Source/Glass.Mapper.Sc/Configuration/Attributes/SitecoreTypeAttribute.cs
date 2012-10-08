using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration.Attributes;

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
    }
}
