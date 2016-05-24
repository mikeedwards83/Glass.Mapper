using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Sc.Configuration
{
    public class SitecoreItemConfiguration : ItemConfiguration
    {
        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new SitecoreItemConfiguration();
        }

        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as SitecoreItemConfiguration;
         
            base.Copy(copy);
        }
    }
}
