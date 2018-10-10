using System;

namespace Glass.Mapper.Configuration
{
    /// <summary>
    /// Class IdConfiguration
    /// </summary>
    public class IdConfiguration : AbstractPropertyConfiguration
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public Type Type { get; set; }

        protected override AbstractPropertyConfiguration CreateCopy()
        {
            return new IdConfiguration();
        }

        protected override void Copy(AbstractPropertyConfiguration copy)
        {
            var config = copy as IdConfiguration;

            config.Type = Type;

            base.Copy(copy);
        }
    }
}




