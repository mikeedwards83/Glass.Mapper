using Glass.Mapper.Sc.Razor.Web.Ui;
using Glass.Mapper.Sites.Sc.Models.Misc;

namespace Glass.Mapper.Sites.Sc.Layouts.Razor
{
    public class B1 : AbstractRazorControl<ContentBase>
    {
        protected override void OnLoad(System.EventArgs e)
        {

            this.Placeholders = new[] {"PlaceTest"};
            base.OnLoad(e);
        }
        public override ContentBase GetModel()
        {
            var item = GetDataSourceOrContextItem();
            return SitecoreContext.CreateType<ContentBase>(item);
        }
    }
}
