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
            GlassHtmlFactory = new GlassHtmlFactory();
            ItemVersionHandler = new ItemVersionHandler();
        }

        public ISitecoreContextFactory SitecoreContextFactory { get; set; }

        public IGlassContextProvider GlassContextProvider { get; set; }

        public IGlassHtmlFactory GlassHtmlFactory { get; set; }

        public IItemVersionHandler ItemVersionHandler { get; set; }
    }
}
