namespace Glass.Mapper.Configuration
{
    public class SelfConfiguration : AbstractNodePropertyConfiguration
    {

        public SelfConfiguration() : base(false)
        {

        }

        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new SelfConfiguration();
        }
   
      
    }
}