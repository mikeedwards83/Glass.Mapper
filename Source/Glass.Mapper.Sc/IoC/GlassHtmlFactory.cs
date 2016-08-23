namespace Glass.Mapper.Sc.IoC
{
    public class GlassHtmlFactory : IGlassHtmlFactory
    {
        public IGlassHtml GetGlassHtml(ISitecoreContext sitecoreContext)
        {
            return new GlassHtml(sitecoreContext);
        }
    }
}
