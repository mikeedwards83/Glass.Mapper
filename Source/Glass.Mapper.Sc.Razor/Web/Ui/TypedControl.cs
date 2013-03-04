namespace Glass.Mapper.Sc.Razor.Web.Ui
{
    public  class TypedControl<T> : AbstractRazorControl<T> where T:class
    {

        public override T GetModel()
        {
            ISitecoreContext _context = new SitecoreContext(ContextName);
            return _context.CreateType<T>(GetDataSourceOrContextItem(), false, false);
        }
    }
}
