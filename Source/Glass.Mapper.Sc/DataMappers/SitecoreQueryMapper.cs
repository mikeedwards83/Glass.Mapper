
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.DataMappers.SitecoreQueryParameters;
using Sitecore.Data.Items;
using Sitecore.Data.Query;
using Sitecore.Globalization;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class SitecoreQueryMapper
    /// </summary>
    public class SitecoreQueryMapper : AbstractDataMapper
    {

        private readonly IEnumerable<ISitecoreQueryParameter> _parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreQueryMapper"/> class.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        public SitecoreQueryMapper(IEnumerable<ISitecoreQueryParameter> parameters)
        {
            _parameters = parameters;
            ReadOnly = true;
        }

        /// <summary>
        /// Maps data from the .Net property value to the CMS value
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns>The value to write</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Maps data from the CMS value to the .Net property value
        /// </summary>
        /// <param name="mappingContext">The mapping context.</param>
        /// <returns>System.Object.</returns>
        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var scConfig = Configuration as SitecoreQueryConfiguration;
            var scContext = mappingContext as SitecoreDataMappingContext;

            string query = ParseQuery(scConfig.Query, scContext.Item);

           

            if (scConfig.PropertyInfo.PropertyType.IsGenericType)
            {
                var options = new GetItemsByQueryOptions();

                options.Copy(mappingContext.Options);

                options.Query = Sc.Query.New(query);

                scConfig.GetPropertyOptions(options);

                if (scConfig.IsRelative)
                {
                    options.RelativeItem =scContext.Item;
                }

                var result = scContext.Service.GetItems(options);
                return result;
            }
            else
            {
                var options = new GetItemByQueryOptions();
                options.Copy(mappingContext.Options);

                options.Query = Sc.Query.New(query);
                scConfig.GetPropertyOptions(options);

                if (scConfig.IsRelative)
                {
                    options.RelativeItem = scContext.Item;
                }

                return scContext.Service.GetItem(options);
            }
        }

        /// <summary>
        /// Indicates that the data mapper will mapper to and from the property
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if this instance can handle the specified configuration; otherwise, <c>false</c>.</returns>
        public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
        {
            if (!(configuration is SitecoreQueryConfiguration)) return false;

            if (configuration.PropertyInfo.PropertyType.IsGenericType)
            {

                Type outerType = Utilities.GetGenericOuter(configuration.PropertyInfo.PropertyType);
                Type innerType = Mapper.Utilities.GetGenericArgument(configuration.PropertyInfo.PropertyType);

                return typeof (IEnumerable<>) == outerType;// && context.TypeConfigurations.ContainsKey(innerType);
            }
            else
            {
                //We are now assuming auto-mapping works
                return true;
                //return context.TypeConfigurations.ContainsKey(configuration.PropertyInfo.PropertyType);
            }
        }

        /// <summary>
        /// Parses the query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="item">The item.</param>
        /// <returns>System.String.</returns>
        public string ParseQuery(string query, Item item)
        {
            StringBuilder sb = new StringBuilder(query);
            if (_parameters != null)
            {
                foreach (var param in _parameters)
                {
                    sb.Replace("{" + param.Name + "}", param.GetValue(item));
                }
            }
            return sb.ToString();
        }

        
    }
}




