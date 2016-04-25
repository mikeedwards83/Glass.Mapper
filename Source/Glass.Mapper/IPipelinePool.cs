namespace Glass.Mapper
{
    public interface IPipelinePool<T>
    {
        T GetFromPool();

        int Missed { get; }
    }
}