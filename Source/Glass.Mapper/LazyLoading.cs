namespace Glass.Mapper
{
    public enum LazyLoading
    {
        /// <summary>
        /// Lazy loading is enabled for the current model and all referenced Glass models.
        /// </summary>
        Enabled = 0,
        /// <summary>
        /// Lazy loading is disabled for the current model and all referenced Glass models.
        /// </summary>
        Disabled = 10,
        /// <summary>
        /// Lazy loading is disabled for the current model but all referenced model will be lazy loaded.
        /// </summary>
        OnlyReferenced = 20
    }
}
