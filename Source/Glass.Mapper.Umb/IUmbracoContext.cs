namespace Glass.Mapper.Umb
{
    public interface IUmbracoContext
    {
        T GetCurrentPage<T>() where T:class;
    }
}