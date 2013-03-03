using Glass.Mapper.Sc;
using Glass.Mapper.Sites.Sc.Models.Content;
using Sitecore.Data.Comparers;

namespace Glass.Mapper.Sites.Sc.Client.Item
{
    public class EventSortOrder : DefaultComparer
    {
        protected override int DoCompare(Sitecore.Data.Items.Item item1, Sitecore.Data.Items.Item item2)
        {
            if (item1.TemplateName == "Event")
            {
                var evnt1 = item1.GlassCast<Event>();
                var evnt2 = item2.GlassCast<Event>();

                return evnt1.Start.CompareTo(evnt2.Start);
            }
               
            return base.DoCompare(item1, item2);
        }
    }
}