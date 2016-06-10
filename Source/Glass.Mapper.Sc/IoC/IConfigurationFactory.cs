namespace Glass.Mapper.Sc.IoC
{
    public interface IConfigurationFactory
    {
        ISitecoreContextFactory SitecoreContextFactory { get; set; }

        IGlassContextProvider GlassContextProvider { get; set; }

        IGlassHtmlFactory GlassHtmlFactory { get; set; }
    }
}
