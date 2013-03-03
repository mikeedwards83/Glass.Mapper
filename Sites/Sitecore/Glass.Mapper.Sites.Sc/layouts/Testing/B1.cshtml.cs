using Glass.Mapper.Sc.Razor.Web.Ui;
using Glass.Mapper.Sites.Sc.Models.Misc;

namespace Glass.Mapper.Sites.Sc.Layouts.Razor
{
    public class B1 : AbstractRazorControl<ContentBase>
    {
        public override ContentBase GetModel()
        {
            var item = GetDataSourceOrContextItem();
            return SitecoreService.CreateType<ContentBase>(item);
        }
    }
}
