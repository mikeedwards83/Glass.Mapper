namespace Glass.Mapper.Sc.IoC
{
    public interface ISitecoreContextFactory
    {
        ISitecoreContext GetSitecoreContext();

        ISitecoreContext GetSitecoreContext(string contextName);

        ISitecoreContext GetSitecoreContext(Context context);
    }
}