using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Web;
using Glass.Mapper.Sc.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Glass.Mapper.Sc
{ 
    public static class GlassServiceConfigurationExtensionMethods
    {
        public static IGlassServiceConfiguration AddMvcContext(
            this IGlassServiceConfiguration glassConfig)
        {
            glassConfig.ServiceCollection.AddTransient<IMvcContext, MvcContext>();

            return glassConfig;
        }
    }
}
