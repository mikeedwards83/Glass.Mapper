namespace Glass.Mapper.Configuration
{
    /// <summary>
    /// Class FieldConfiguration
    /// </summary>
    public class FieldConfiguration : AbstractPropertyConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether [read only].
        /// </summary>
        /// <value><c>true</c> if [read only]; otherwise, <c>false</c>.</value>
        public bool ReadOnly { get; set; }

        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new FieldConfiguration();
        }

        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as FieldConfiguration;

            config.ReadOnly = ReadOnly;

            base.Copy(copy);
        }
    }
}




