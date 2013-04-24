using Glass.Mapper.Sc.Configuration.Attributes;

namespace Glass.Mapper.Sc.Razor
{
    /// <summary>
    /// Class GlassRazorModuleLoader
    /// </summary>
    public class GlassRazorSettings
    {
        /// <summary>
        /// The context name
        /// </summary>
        public const string ContextName = "GlassRazor";


        ///// <summary>
        ///// Loads the specified resolver.
        ///// </summary>
        ///// <param name="resolver">The resolver.</param>
        ///// <returns>Context.</returns>
        //public static Context Load(IDependencyResolver resolver)
        //{
        //    var context = Context.Contexts.ContainsKey(ContextName) ? Context.Contexts[ContextName] : Context.Create(resolver, ContextName);

        //    var loader = new SitecoreAttributeConfigurationLoader("Glass.Mapper.Sc.Razor");
        //    context.Load(loader);

        //    return context;
        //}

    }
}
