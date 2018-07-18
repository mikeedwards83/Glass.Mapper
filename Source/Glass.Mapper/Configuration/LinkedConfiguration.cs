
namespace Glass.Mapper.Configuration
{
    /// <summary>
    /// Class LinkedConfiguration
    /// </summary>
    public class LinkedConfiguration : AbstractNodePropertyConfiguration
    {
        public LinkedConfiguration() : base(true)
        {

        }

        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new LinkedConfiguration();
        }
    }
}




