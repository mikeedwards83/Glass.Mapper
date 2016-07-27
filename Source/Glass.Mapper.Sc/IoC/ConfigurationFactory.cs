using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glass.Mapper.Sc.IoC
{
    public class ConfigurationFactory : IConfigurationFactory
    {
        private static IConfigurationFactory _defaultConfigurationFactory = new ConfigurationFactory();

        public static IConfigurationFactory Default
        {
            get { return _defaultConfigurationFactory; }
            set { _defaultConfigurationFactory = value; }
        }

        public ConfigurationFactory()
        {
            GlassContextProvider = new GlassContextProvider();
            SitecoreContextFactory = new SitecoreContextFactory(GlassContextProvider);
           
        }

        public ISitecoreContextFactory SitecoreContextFactory { get; set; }

        public IGlassContextProvider GlassContextProvider { get; set; }

    }
}
