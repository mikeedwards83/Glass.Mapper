namespace Glass.Mapper.Configuration
{
    /// <summary>
    /// Ignore Configuration
    /// </summary>
    public class IgnoreConfiguration : AbstractPropertyConfiguration
    {
        public IgnoreConfiguration()
        {
            this.Ignore = true;
        }
        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new InfoConfiguration();
        }
    }
}

