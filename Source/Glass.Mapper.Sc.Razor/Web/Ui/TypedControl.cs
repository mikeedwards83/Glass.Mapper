namespace Glass.Mapper.Sc.Razor.Web.Ui
{
    /// <summary>
    /// Class TypedControl
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public  class TypedControl<T> : AbstractRazorControl<T> where T:class
    {

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <returns>`0.</returns>
        public override T GetModel()
        {
            ISitecoreContext _context = new SitecoreContext(ContextName);
            return _context.CreateType<T>(GetDataSourceOrContextItem(), false, false);
        }
    }
}
