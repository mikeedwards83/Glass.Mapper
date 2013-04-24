namespace Glass.Mapper.Sc.Razor.Web.Ui
{
    /// <summary>
    /// Class TemplateModel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TemplateModel<T>
    {
        /// <summary>
        /// Gets or sets the control.
        /// </summary>
        /// <value>The control.</value>
        public AbstractRazorControl<T> Control
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
        public T Model { get; set; }
    }
}