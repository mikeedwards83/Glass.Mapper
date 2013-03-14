using Glass.Mapper.Sc.Razor.Dynamic;

namespace Glass.Mapper.Sc.Razor.Web.Ui
{
    /// <summary>
    /// Class DynamicControl
    /// </summary>
    public class DynamicControl : AbstractRazorControl<dynamic>
    {
        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <returns>dynamic.</returns>
        public override dynamic GetModel()
        {


            return new RazorDynamicItem(GetDataSourceOrContextItem());
        }
    }
}
