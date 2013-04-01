namespace Glass.Mapper.Sc.Razor
{
    /// <summary>
    /// Interface IRazorControl
    /// </summary>
    public interface IRazorControl
    {
        /// <summary>
        /// Gets or sets the view.
        /// </summary>
        /// <value>The view.</value>
        string View
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the name of the context.
        /// </summary>
        /// <value>The name of the context.</value>
        string ContextName { get; set; }

    }
}
