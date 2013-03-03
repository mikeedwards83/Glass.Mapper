namespace Glass.Mapper.Sc.Razor
{
    public interface IRazorControl
    {
        string View
        {
            get;
            set;
        }
        string ContextName { get; set; }

    }
}
