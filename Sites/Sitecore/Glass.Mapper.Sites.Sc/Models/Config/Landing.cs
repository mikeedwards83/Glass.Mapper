using Glass.Mapper.Sc.Configuration.Fluent;
using Glass.Mapper.Sites.Sc.Models.Landing;

namespace Glass.Mapper.Sites.Sc.Models.Config
{
    public class Landing
    {
        public static SitecoreFluentConfigurationLoader Load()
        {
            var loader = new SitecoreFluentConfigurationLoader();

            var homePage = loader.Add<HomePage>();
            homePage.Field(x => x.MainBody);
            homePage.Field(x => x.News);


            var newsLanding = loader.Add<NewsLanding>();
            newsLanding.Query(x=> x.Articles).Query("./*/*/*[@@templatename='NewsArticle']").IsRelative();
            newsLanding.Import(Misc.ContentBase);
            return loader;
        }
    }
}