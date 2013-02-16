using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Fluent;
using Glass.Mapper.Sites.Sc.Tutorial1.Model;

namespace Glass.Mapper.Sites.Sc.Tutorial1.Config
{
    public class FluentConfig
    {
        public void Config()
        {
            var loader = new SitecoreFluentConfigurationLoader();
            
            var demoClass = loader.Add<DemoClass>();

            demoClass.Id(x => x.Id);
            demoClass.Field(x => x.Title);
            demoClass.Info(x => x.Url).InfoType(SitecoreInfoType.Url);
        }
    }
}
