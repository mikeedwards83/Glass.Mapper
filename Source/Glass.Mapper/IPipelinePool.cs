namespace Glass.Mapper
{
    public interface IPipelinePool<T>
    {
        T GetFromPool();
    }
}