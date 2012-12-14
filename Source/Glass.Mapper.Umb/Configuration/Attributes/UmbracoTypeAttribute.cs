using System;
using Glass.Mapper.Configuration.Attributes;
using Glass.Mapper.Configuration;

namespace Glass.Mapper.Umb.Configuration.Attributes
{
    public class UmbracoTypeAttribute : AbstractTypeAttribute
    {
        /// <summary>
        /// Indicates the document type to use when trying to create an item
        /// </summary>
        public int DocumentTypeId { get; set; }

        public override void Configure(Type type, AbstractTypeConfiguration config)
        {
            var umbConfig = config as UmbracoTypeConfiguration;

            if (umbConfig == null)
                throw new ConfigurationException(
                    "Type configuration is not of type {0}".Formatted(typeof(UmbracoTypeConfiguration).FullName));

            umbConfig.DocumentTypeId = this.DocumentTypeId;

            base.Configure(type, config);
        }
    }
}
