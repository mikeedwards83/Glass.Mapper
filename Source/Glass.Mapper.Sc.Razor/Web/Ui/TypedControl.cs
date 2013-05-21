using System;

namespace Glass.Mapper.Sc.Razor.Web.Ui
{
    /// <summary>
    /// Class TypedControl
    /// </summary>
    public  class TypedControl : AbstractRazorControl<object> 
    {
        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <returns>`0.</returns>
        public override object GetModel()
        {
            return SitecoreContext.CreateType(View.Type, GetDataSourceOrContextItem(), false, false, null);
        }
    }
}
