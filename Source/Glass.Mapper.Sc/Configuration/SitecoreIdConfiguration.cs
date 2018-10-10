using Glass.Mapper.Configuration;

namespace Glass.Mapper.Sc.Configuration
{
    /// <summary>
    /// Class SitecoreIdConfiguration
    /// </summary>
    public class SitecoreIdConfiguration : IdConfiguration
    {
        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new SitecoreIdConfiguration();
        }

        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as SitecoreIdConfiguration;
            base.Copy(copy);
        }
    }
}




