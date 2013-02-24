using System;
using Glass.Mapper.Sc.Dynamic;

namespace Glass.Mapper.Sc.Web.Ui
{
    public class GlassDynamicUserControl : AbstractGlassUserControl
    {
        /// <summary>
        /// Model to render on the sublayout
        /// </summary>
        public dynamic Model { get; private set; }

        protected override void OnLoad(EventArgs e)
        {
            Model = new DynamicItem(LayoutItem);
            base.OnLoad(e);
        }
    }
}
