using System;
using Glass.Mapper.Sc.Dynamic;

namespace Glass.Mapper.Sc.Web.Ui
{
    /// <summary>
    /// Class GlassDynamicUserControl
    /// </summary>
    public class GlassDynamicUserControl : AbstractGlassUserControl
    {
        /// <summary>
        /// Model to render on the sublayout
        /// </summary>
        /// <value>The model.</value>
        public dynamic Model { get; private set; }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            Model = new DynamicItem(LayoutItem);
            base.OnLoad(e);
        }
    }
}
