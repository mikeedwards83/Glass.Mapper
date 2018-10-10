using System;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class SitecoreFieldTypeMapper
    /// </summary>
    public class SitecoreFieldTypeMapper : AbstractSitecoreFieldMapper
    {

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.Object.</returns>
        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {

            var item = context.Item;

            if (fieldValue.IsNullOrEmpty()) return null;

            Guid id = Guid.Empty;
            Item target;

            if (Guid.TryParse(fieldValue, out id)) {

                target = item.Database.GetItem(new ID(id), item.Language);
            }
            else
            {
                target = item.Database.GetItem(fieldValue, item.Language);
            }

            if (target == null) return null;

            var getOptions = new GetItemByItemOptions();
            getOptions.Copy(context.Options);
            getOptions.Item = target;

            config.GetPropertyOptions(getOptions);
            return context.Service.GetItem(getOptions);
        }

        /// <summary>
        /// Sets the field value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NullReferenceException">Could not find item to save value {0}.Formatted(Configuration)</exception>
        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context", "The context was incorrectly set");
            
            if(context.Service == null)
                throw new NullReferenceException("The context's service property was null");

            if (context.Service.GlassContext == null)
                throw new NullReferenceException("The service glass context is null");
            
            if (context.Service.Database == null)
                throw new NullReferenceException("The database is not set for the service");

            if (value == null)
                return string.Empty;

            var type = value.GetType();

            var typeConfig = context.Service.GlassContext.GetTypeConfiguration<SitecoreTypeConfiguration>(value);

            if(typeConfig == null)
                throw new NullReferenceException("The type {0} has not been loaded into context {1}".Formatted(type.FullName, context.Service.GlassContext.Name));

            var item = typeConfig.ResolveItem(value, context.Service.Database);
            if(item == null)
                throw new NullReferenceException("Could not find item to save value {0}".Formatted(Configuration));

            return item.ID.ToString();
        }

        /// <summary>
        /// Determines whether this instance can handle the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if this instance can handle the specified configuration; otherwise, <c>false</c>.</returns>
        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration,  Context context)
        {
            return configuration is SitecoreFieldConfiguration;// context[configuration.PropertyInfo.PropertyType] != null &&
                   
        }

     

       
    }
}




