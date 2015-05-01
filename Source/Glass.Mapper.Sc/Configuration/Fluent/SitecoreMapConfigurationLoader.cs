using Glass.Mapper.Maps;

namespace Glass.Mapper.Sc.Configuration.Fluent
{
    public class SitecoreMapConfigurationLoader : SitecoreFluentConfigurationLoader, IMapProvider
    {
        public IGlassMap[] Maps { get; private set; }

        public SitecoreMapConfigurationLoader(IGlassMap[] maps)
        {
            Maps = maps;
        }
    }
}
