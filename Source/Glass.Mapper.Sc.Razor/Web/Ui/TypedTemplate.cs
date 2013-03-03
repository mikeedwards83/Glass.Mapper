namespace Glass.Mapper.Sc.Razor.Web.Ui
{
    public class TypedTemplate<T> : TemplateBase<T> where T:class
    {
        public TypedTemplate(ISitecoreService service) : base(service)
        {
        }

    }
}
