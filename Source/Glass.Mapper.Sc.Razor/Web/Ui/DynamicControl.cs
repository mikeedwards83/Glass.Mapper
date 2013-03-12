using Glass.Mapper.Sc.Razor.Dynamic;

namespace Glass.Mapper.Sc.Razor.Web.Ui
{
    public class DynamicControl : AbstractRazorControl<dynamic>
    {
        public override dynamic GetModel()
        {


            return new RazorDynamicItem(GetDataSourceOrContextItem());
        }
    }
}
