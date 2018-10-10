namespace Glass.Mapper.Configuration
{
    /// <summary>
    /// Class ParentConfiguration
    /// </summary>
    public class ParentConfiguration : AbstractNodePropertyConfiguration
    {
       

        public ParentConfiguration() : base(false)
        {

        }

        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new ParentConfiguration();
        }

        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as ParentConfiguration;

       

            base.Copy(copy);
        }
    }
}




