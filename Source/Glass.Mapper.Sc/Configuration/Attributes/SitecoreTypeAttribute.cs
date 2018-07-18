using System;
using Glass.Mapper.Configuration.Attributes;
using Glass.Mapper.Configuration;
using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration.Attributes
{
    /// <summary>
    /// Class SitecoreTypeAttribute
    /// </summary>
    public class SitecoreTypeAttribute : AbstractTypeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreTypeAttribute"/> class.
        /// </summary>
        public SitecoreTypeAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreTypeAttribute"/> class.
        /// </summary>
        /// <param name="templateId">The template id.</param>
        public SitecoreTypeAttribute(bool codeFirst, string templateId)
        {
            TemplateId = templateId;
        }

        /// <summary>
        /// Indicates the template to use when trying to create an item
        /// </summary>
        /// <value>The template id.</value>
        public string TemplateId { get; set; }
        /// <summary>
        /// Indicates the branch to use when trying to create and item. If a template id is also specified the template Id will be use instead.
        /// </summary>
        /// <value>The branch id.</value>
        public string BranchId { get; set; }

        /// <summary>
        /// Overrides the default template name when using code first
        /// </summary>
        /// <value>The name of the template.</value>
        public string TemplateName { get; set; }

        /// <summary>
        /// Forces Glass to do a template check and only returns an class if the item 
        /// matches the template ID or inherits a template with the templateId
        /// </summary>
        public SitecoreEnforceTemplate EnforceTemplate { get; set; }

        /// <summary>
        /// Indicates if Caching should be enabled or disabled for the type
        /// </summary>
        public Cache Cache { get; set; }

        /// <summary>
        /// Indicate if the type should be cached
        /// </summary>
        [Obsolete("Use Cache property")]
        public bool Cachable {
            get { return Cache.IsEnabled(); }
            set { Cache = value ? Cache.Enabled : Cache.Disabled; }
        }

        /// <summary>
        /// Configures the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="config">The config.</param>
        /// <exception cref="Glass.Mapper.Configuration.ConfigurationException">Type configuration is not of type {0}.Formatted(typeof (SitecoreTypeConfiguration).FullName)</exception>
        public override AbstractTypeConfiguration Configure(Type type)
        {
            var scConfig = new SitecoreTypeConfiguration();

            if (scConfig == null)
                throw new ConfigurationException(
                    "Type configuration is not of type {0}".Formatted(typeof(SitecoreTypeConfiguration).FullName));


            try
            {
                if (BranchId.HasValue())
                    scConfig.BranchId = new ID(this.BranchId);
                else
                    scConfig.BranchId = ID.Null;
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to convert BranchId for type {0}. Value was {1}".Formatted(type.FullName, this.TemplateId), ex);
            }

            try
            {
                if (TemplateId.HasValue())
                    scConfig.TemplateId = new ID(this.TemplateId);
                else
                    scConfig.TemplateId = ID.Null;
            }
            catch (Exception ex)
            {
                throw new MapperException("Failed to convert TemplateId for type {0}. Value was {1}".Formatted(type.FullName, this.TemplateId), ex);
            }

            if (TemplateId.IsNullOrEmpty() && this.EnforceTemplate.IsEnabled())
            {
                throw new ConfigurationException(
                    "The type {0} has EnforceTemplate set to true but no TemplateId".Formatted(type.FullName));
            }
            scConfig.EnforceTemplate = this.EnforceTemplate;

            scConfig.TemplateName = this.TemplateName;

            scConfig.Cache = this.Cache;

            base.Configure(type, scConfig);

            return scConfig;
        }
    }
}




