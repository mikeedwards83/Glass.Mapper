using Glass.Mapper.Sc.Configuration.Attributes;

namespace Glass.Mapper.Sc.Razor
{
    public class GlassRazorModuleLoader
    {
        public const string ContextName = "GlassRazor";


        public static Context Load(IGlassConfiguration config = null)
        {
            if (config == null)
                config = new GlassConfig();
            
            var context = Context.Contexts.ContainsKey(ContextName) ? Context.Contexts[ContextName] : Context.Create(config, ContextName);

            var loader = new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Razor");
            context.Load(loader);

            return context;
        }

    }
}
