


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Pipelines.DataMapperResolver;
using Sitecore.Data.Items;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Managers;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class SitecoreChildrenMapper
    /// </summary>
    public class SitecoreChildrenMapper : AbstractDataMapper
    {

        protected Type GenericType { get; set; }

        private ActivationManager.CompiledActivator<object> _activator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreChildrenMapper"/> class.
        /// </summary>
        public SitecoreChildrenMapper()
        {
            this.ReadOnly = true;
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
            var scContext = mappingContext as SitecoreDataMappingContext;
            var scConfig = Configuration as SitecoreChildrenConfiguration;

            Func<Database,IEnumerable<Item>> getItems = (database) =>
                ItemManager.GetChildren(scContext.Item, SecurityCheck.Enable, ChildListOptions.None);

            var options = new GetItemsByFuncOptions(getItems);
            options.Copy(mappingContext.Options);

            scConfig.GetPropertyOptions(options);
            return scContext.Service.GetItems(options);
        }

        public override void Setup(DataMapperResolverArgs args)
        {

            base.Setup(args);
            GenericType = Mapper.Utilities.GetGenericArgument(Configuration.PropertyInfo.PropertyType);

        }


        /// <summary>
        /// Indicates that the data mapper will mapper to and from the property
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if this instance can handle the specified configuration; otherwise, <c>false</c>.</returns>
        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration,  Context context)
        {
            return configuration is SitecoreChildrenConfiguration;
        }
    }
}




