namespace Glass.Mapper.Sc.IoC
{
    public class GlassHtmlFactory : IGlassHtmlFactory
    {
        protected IDependencyResolver DependencyResolver { get; }

        public GlassHtmlFactory(IDependencyResolver dependencyResolver)
        {
            DependencyResolver = dependencyResolver;
        }


        public IGlassHtml GetGlassHtml(ISitecoreService sitecoreService)
        {
            return new GlassHtml(sitecoreService);
        }
    }
}
