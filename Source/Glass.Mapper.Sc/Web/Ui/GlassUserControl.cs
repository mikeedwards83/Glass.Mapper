using System;
using System.Linq.Expressions;
using Glass.Mapper.Sc.RenderField;

namespace Glass.Mapper.Sc.Web.Ui
{
    public class GlassUserControl<T> : AbstractGlassUserControl where T : class
    {
        public GlassUserControl(ISitecoreContext context) :base(context){}
        public GlassUserControl() : base(){}
 
        protected override void OnLoad(EventArgs e)
        {
            Model = SitecoreContext.CreateType<T>(LayoutItem, false, false);
            base.OnLoad(e);
        }

        /// <summary>
        /// Model to render on the sublayout
        /// </summary>
        public T Model { get; set; }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model => model.Title where Title is field name.
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public string Editable(Expression<Func<T, object>> field)
        {
            return base.Editable(this.Model, field);
        }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model =&gt; model.Title where Title is field name.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public string Editable(Expression<Func<T, object>> field, string parameters)
        {
            return base.Editable(this.Model, field, parameters);
        }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model => model.Title where Title is field name.
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public string Editable(Expression<Func<T, object>> field, AbstractParameters parameters)
        {
            return base.Editable(this.Model, field, parameters);
        }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model => model.Title where Title is field name.
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public string Editable(Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput)
        {
            return   base.Editable(this.Model, field, standardOutput);
        }

        /// <summary>
        /// Makes a field editable via the Page Editor. Use the Model property as the target item, e.g. model => model.Title where Title is field name.
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public string Editable(Expression<Func<T, object>> field, Expression<Func<T, string>> standardOutput,
                               AbstractParameters parameters)
        {
            return base.Editable(this.Model, field, standardOutput, parameters);
        }



    }
}
