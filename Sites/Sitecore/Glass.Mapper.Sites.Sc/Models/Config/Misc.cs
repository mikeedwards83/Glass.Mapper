using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Fluent;
using Glass.Mapper.Sites.Sc.Models.Misc;

namespace Glass.Mapper.Sites.Sc.Models.Config
{
    public class Misc
    {
        public static SitecoreFluentConfigurationLoader Load()
        {
            var loader = new SitecoreFluentConfigurationLoader();
            
            var menuItem = loader.Add<MenuItem>();
            menuItem.Field(x => x.Title).FieldName("Page Title");
            menuItem.Info(x => x.Url).InfoType(SitecoreInfoType.Url);
            menuItem.Children(x => x.Children);

            return loader;
        }
    }
}

