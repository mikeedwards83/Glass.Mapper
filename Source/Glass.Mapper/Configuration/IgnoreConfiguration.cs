namespace Glass.Mapper.Configuration
{
    /// <summary>
    /// Ignore Configuration
    /// </summary>
    public class IgnoreConfiguration : AbstractPropertyConfiguration
    {
        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new InfoConfiguration();
        }
    }
}

