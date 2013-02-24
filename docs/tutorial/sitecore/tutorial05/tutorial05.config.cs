            var newsArticle = loader.Add<NewsArticle>();
            newsArticle.Id(x => x.Id);
            newsArticle.Field(x => x.Abstract);
            newsArticle.Field(x => x.Date);
            newsArticle.Field(x => x.FeaturedImage);
            newsArticle.Field(x => x.MainBody);
            newsArticle.Field(x => x.Title).FieldName("Page Title");
            newsArticle.Info(x => x.Url).InfoType(SitecoreInfoType.Url);