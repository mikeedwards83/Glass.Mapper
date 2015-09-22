namespace Glass.Mapper.Sc.Web.Mvc
{
    public interface IRenderingContextWrapper
    {
        bool ContextActive { get; }

        bool HasDataSource { get; }

        string GetRenderingParameters();

        string GetDataSource();
    }
}
