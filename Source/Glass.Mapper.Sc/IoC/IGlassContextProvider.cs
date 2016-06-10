namespace Glass.Mapper.Sc.IoC
{
    public interface IGlassContextProvider
    {
        Context GetContext();

        Context GetContext(string contextName);
    }
}
