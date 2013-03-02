            var newsLanding = loader.Add<NewsLanding>();
            newsLanding.Query(x=> x.Articles).Query("./*/*/*[@@templatename='NewsArticle']").IsRelative();
            newsLanding.Import(Misc.ContentBase);
            return loader;