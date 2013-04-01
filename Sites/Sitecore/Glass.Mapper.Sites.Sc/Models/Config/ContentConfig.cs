using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Fluent;
using Glass.Mapper.Sites.Sc.Models.Content;

namespace Glass.Mapper.Sites.Sc.Models.Config
{
    public class ContentConfig
    {
        public static SitecoreFluentConfigurationLoader Load()
        {
            var loader = new SitecoreFluentConfigurationLoader();

            var eventConfig = loader.Add<Event>().AutoMap();
            eventConfig.Field(x => x.Title);
            eventConfig.Id(x => x.Id);
            eventConfig.Info(x => x.Language).InfoType(SitecoreInfoType.Language);

            return loader;
        }
    }
}