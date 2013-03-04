namespace Glass.Mapper.Sc.Razor.Web.Ui
{
    public class TemplateModel<T>
    {
        public AbstractRazorControl<T> Control
        {
            get;
            set;
        }
        public T Model { get; set; }
    }
}