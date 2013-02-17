using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Fluent;
using Glass.Mapper.Sites.Sc.Tutorial2.Model;
using Sitecore.Data;

namespace Glass.Mapper.Sites.Sc.Tutorial2.Config
{
    public class Tutorial2Config
    {
        public static SitecoreFluentConfigurationLoader Config()
        {
            var loader = new SitecoreFluentConfigurationLoader();

            var demo = loader.Add<DemoClass>();

            // This field is mapped by using the name of the property
            demo.Field(x => x.Image);
            // This field is mapped by using the ID of the field we want to load
            demo.Field(x => x.Title).FieldId(new ID("{BD43F50B-2FBB-4E04-9F79-1B666DF4D6BD}"));
            // This field is mapped using the Field name passed. 
            demo.Field(x => x.MainContent).FieldName("Main Content");
            demo.Field(x => x.Created).FieldName("__Created");

            demo.Info(x => x.Name).InfoType(SitecoreInfoType.Name);
            demo.Info(x => x.TemplateName).InfoType(SitecoreInfoType.TemplateName);
            demo.Info(x => x.Path).InfoType(SitecoreInfoType.FullPath);

            return loader;
        }
    }
}