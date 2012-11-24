using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration.Attributes;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    public class SitecoreAttributeConfigurationLoader : AttributeConfigurationLoader<SitecoreTypeConfiguration, SitecorePropertyConfiguration>
    {
        public SitecoreAttributeConfigurationLoader(params string[] assemblies): base(assemblies)
        {

        }
    }
}
