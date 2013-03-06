using Glass.Mapper.Sc.Configuration.Attributes;

namespace Glass.Mapper.Sc.Razor
{
    public class GlassRazorModuleLoader
    {
        public const string ContextName = "GlassRazor";


        public static Context Load(IDependencyResolver resolver)
        {
            var context = Context.Contexts.ContainsKey(ContextName) ? Context.Contexts[ContextName] : Context.Create(resolver, ContextName);

            var loader = new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Razor");
            context.Load(loader);

            return context;
        }

    }
}
