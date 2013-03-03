using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Fluent;
using Glass.Mapper.Sites.Sc.Models.Misc;

namespace Glass.Mapper.Sites.Sc.Models.Config
{
    public class Misc
    {
        public static SitecoreType<ContentBase> ContentBase
        {
            get
            {
                var contentBase = new SitecoreType<ContentBase>();

                contentBase.Id(x => x.Id);
                contentBase.Field(x => x.Title).FieldName("Page Title");
                contentBase.Info(x => x.Url).InfoType(SitecoreInfoType.Url);

                return contentBase;
            }

        }
        
        public static SitecoreFluentConfigurationLoader Load()
        {
            var loader = new SitecoreFluentConfigurationLoader();
            
            var menuItem = loader.Add<MenuItem>();
            menuItem.Info(x => x.Url).InfoType(SitecoreInfoType.Url);
            menuItem.Children(x => x.Children);
            menuItem.Import(ContentBase);
            loader.Add(ContentBase);

            return loader;
        }

    }
}

