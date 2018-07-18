using Glass.Mapper.Configuration;

namespace Glass.Mapper.Sc.Configuration
{
    /// <summary>
    /// SitecoreIgnoreConfiguration
    /// </summary>
    public class SitecoreIgnoreConfiguration : IgnoreConfiguration
    {
        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new SitecoreIgnoreConfiguration();
        }

        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as SitecoreIgnoreConfiguration;
            base.Copy(copy);
        }
    }
}

