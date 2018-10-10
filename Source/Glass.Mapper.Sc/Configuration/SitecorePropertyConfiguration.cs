using Glass.Mapper.Configuration;

namespace Glass.Mapper.Sc.Configuration
{
    //this might need to change to an interface
    /// <summary>
    /// Class SitecorePropertyConfiguration
    /// </summary>
	public class SitecorePropertyConfiguration : AbstractPropertyConfiguration
    {
        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new SitecorePropertyConfiguration();
        }

        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as SitecorePropertyConfiguration;

            base.Copy(copy);
        }
    }
}




