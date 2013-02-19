using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Fluent;
using Glass.Mapper.Sites.Sc.Models.Content;

namespace Glass.Mapper.Sites.Sc.Models.Config
{
    public class Content
    {
        public static SitecoreFluentConfigurationLoader Load()
        {
            var loader = new SitecoreFluentConfigurationLoader();

            var newsArticle = loader.Add<NewsArticle>();
            newsArticle.Field(x => x.Abstract);
            newsArticle.Field(x => x.Date);
            newsArticle.Field(x => x.FeaturedImage);
            newsArticle.Field(x => x.MainBody);
            newsArticle.Field(x => x.Title).FieldName("Page Title");
            newsArticle.Info(x => x.Url).InfoType(SitecoreInfoType.Url);

            return loader;
        }
    }
}