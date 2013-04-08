using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Umb.Configuration;
using Umbraco.Core.Models;

namespace Glass.Mapper.Umb.DataMappers
{
    /// <summary>
    /// AbstractUmbracoPropertyMapper
    /// </summary>
    public abstract class AbstractUmbracoPropertyMapper : AbstractDataMapper
    {
        /// <summary>
        /// Gets the types handled.
        /// </summary>
        /// <value>
        /// The types handled.
        /// </value>
        public IEnumerable<Type> TypesHandled { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractUmbracoPropertyMapper"/> class.
        /// </summary>
        /// <param name="typesHandled">The types handled.</param>
        public AbstractUmbracoPropertyMapper(params Type[] typesHandled)
        {
            TypesHandled = typesHandled;
        }

        /// <summary>
        /// Maps data from the .Net property value to the CMS value
        /// </summary>
        /// <param name="mappingContext"></param>
        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            var config = Configuration as UmbracoPropertyConfiguration;
            var context =  mappingContext  as UmbracoDataMappingContext;

            if (context.Content.Properties.Contains(config.PropertyAlias))
            {
                var property = context.Content.Properties[config.PropertyAlias];
                object value = Configuration.PropertyInfo.GetValue(mappingContext.Object, null);

                SetProperty(property, value, config, context);
            }
        }

        /// <summary>
        /// Maps data from the CMS value to the .Net property value
        /// </summary>
        /// <param name="mappingContext"></param>
        /// <returns></returns>
        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var config = Configuration as UmbracoPropertyConfiguration;
            var context = mappingContext as UmbracoDataMappingContext;

            if (context.Content.Properties.Select(p => p.Alias.ToLowerInvariant()).Contains(config.PropertyAlias.ToLowerInvariant()))
            {
                var property = context.Content.Properties.FirstOrDefault(p => p.Alias.ToLowerInvariant() == config.PropertyAlias.ToLowerInvariant());
                return GetProperty(property, config, context);
            }

            return null;
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public virtual object GetProperty(Property property, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context)
        {
            var propertyValue = property.Value;

            return GetPropertyValue(propertyValue, config, context);
        }

        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        public virtual void SetProperty(Property property, object value, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context)
        {
            property.Value = SetPropertyValue(value, config, context);
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public abstract object SetPropertyValue(object value, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context);

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="propertyValue">The property value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public abstract object GetPropertyValue(object propertyValue, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context);

        /// <summary>
        /// Indicates that the data mapper will mapper to and from the property
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is UmbracoPropertyConfiguration &&
                   TypesHandled.Any(x => x == configuration.PropertyInfo.PropertyType);
        }
    }

    public class LowercaseComparer : IEqualityComparer<string>
    {
        public bool Equals(object x, object y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(object obj)
        {
            throw new NotImplementedException();
        }

        public bool Equals(string x, string y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(string obj)
        {
            throw new NotImplementedException();
        }
    }
}
