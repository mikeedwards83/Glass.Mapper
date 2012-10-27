using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Sc.Configuration
{
    public class SitecoreTypeConfiguration : AbstractTypeConfiguration
    {
        public Guid TemplateId { get; set; }

        public Guid BranchId { get; set; }


    }
}
