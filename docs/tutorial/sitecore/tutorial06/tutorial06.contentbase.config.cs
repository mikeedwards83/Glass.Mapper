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
        
    }
}

