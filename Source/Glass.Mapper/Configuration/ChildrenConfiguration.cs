namespace Glass.Mapper.Configuration
{
    /// <summary>
    /// Class ChildrenConfiguration
    /// </summary>
    public class ChildrenConfiguration : AbstractNodePropertyConfiguration
    {
        public ChildrenConfiguration() : base(true)
        {
            
        }

        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new ChildrenConfiguration();
        }

        public virtual void GetPropertyOptions(GetOptions propertyOptions)
        {
            base.GetPropertyOptions(propertyOptions);
        }


    }
}




