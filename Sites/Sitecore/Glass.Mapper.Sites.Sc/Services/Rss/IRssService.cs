using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace Glass.Mapper.Sites.Sc.Services.BbcNews
{
    public interface IRssService
    {
        IEnumerable<SyndicationItem> GetArticles(string url);
    }
}