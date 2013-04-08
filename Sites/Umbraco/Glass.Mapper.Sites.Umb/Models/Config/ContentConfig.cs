using Glass.Mapper.Sites.Umb.Models.Content;
using Glass.Mapper.Umb.Configuration;
using Glass.Mapper.Umb.Configuration.Fluent;

namespace Glass.Mapper.Sites.Umb.Models.Config
{
    public class ContentConfig
    {
        public static UmbracoFluentConfigurationLoader Load()
        {
            var loader = new UmbracoFluentConfigurationLoader();

            var eventConfig = loader.Add<Event>().AutoMap();
            eventConfig.Property(x => x.Title);
            eventConfig.Id(x => x.Id);
            eventConfig.Info(x => x.ContentTypeName).InfoType(UmbracoInfoType.ContentTypeName);

            return loader;
        }
    }
}