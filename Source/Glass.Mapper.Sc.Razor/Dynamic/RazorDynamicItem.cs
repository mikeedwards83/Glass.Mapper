using Glass.Mapper.Sc.Dynamic;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.Razor.Dynamic
{
    public class RazorDynamicItem : DynamicItem
    {
        public RazorDynamicItem(Item item):base(item)
        {
        }
        protected override object GetField(string fieldName, global::Sitecore.Data.Items.Item item)
        {
            return new DynamicField(fieldName, item);
        }

        protected override DynamicItem CreateNew(Item item)
        {
            return new RazorDynamicItem(item);
        }
    }
}
