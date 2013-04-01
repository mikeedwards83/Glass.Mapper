using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Xml;

namespace Glass.Mapper.Sites.Sc.Services.BbcNews
{
    public class RssService : IRssService
    {
        public IEnumerable<SyndicationItem> GetArticles(string url)
        {
            var xml = XmlReader.Create(url);
            var feed = SyndicationFeed.Load(xml);
            return feed.Items;
        }
    }
}