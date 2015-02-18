/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class SitecoreLinkedMapper
    /// </summary>
    public class SitecoreLinkedMapper : AbstractDataMapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreLinkedMapper"/> class.
        /// </summary>
        public SitecoreLinkedMapper()
        {
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
            var scConfig = Configuration as SitecoreLinkedConfiguration;
            var scContext = mappingContext as SitecoreDataMappingContext;

            Type genericType = Mapper.Utilities.GetGenericArgument(scConfig.PropertyInfo.PropertyType);

            var item = scContext.Item;

            //ME - i am not sure this is correct but there is an odd behaviour of references
                // languges come back as invariant, going with default language in this scenario
            var references = new Func<IEnumerable<Item>>(() =>{
                        var itemLinks = global::Sitecore.Configuration.Factory.GetLinkDatabase().GetReferences(item);
                        var items = itemLinks.Select(x => x.GetTargetItem());
                        return Utilities.GetLanguageItems(items, LanguageManager.DefaultLanguage, scContext.Service.Config);
                });

            var getItems = new Func<IEnumerable<Item>>(() =>
            {
                switch (scConfig.Option)
                {
                    case SitecoreLinkedOptions.All:
                        var itemLinks1 = references();
                        var itemLinks2 = global::Sitecore.Configuration.Factory.GetLinkDatabase().GetReferrers(item);
                        return itemLinks1.Union(itemLinks2.Select(x => x.GetSourceItem()));
                    case SitecoreLinkedOptions.References:
                        return references();
                    case SitecoreLinkedOptions.Referrers:
                        var itemLinks4 = global::Sitecore.Configuration.Factory.GetLinkDatabase().GetReferrers(item);
                        return itemLinks4.Select(x => x.GetSourceItem());
                    default:
                        return new List<Item>();
                }
            });

            return scContext.Service.CreateTypes(genericType, getItems, scConfig.IsLazy, scConfig.InferType);
        }

        /// <summary>
        /// Indicates that the data mapper will mapper to and from the property
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="context">The context.</param>
        /// <returns><c>true</c> if this instance can handle the specified configuration; otherwise, <c>false</c>.</returns>
        public override bool CanHandle(AbstractPropertyConfiguration configuration, Context context)
        {
            if (!(configuration is SitecoreLinkedConfiguration))
                return false;

            if (!configuration.PropertyInfo.PropertyType.IsGenericType) return false;

            Type outerType = Utilities.GetGenericOuter(configuration.PropertyInfo.PropertyType);
            Type innerType = Mapper.Utilities.GetGenericArgument(configuration.PropertyInfo.PropertyType);

            return typeof (IEnumerable<>) == outerType;// && context.TypeConfigurations.ContainsKey(innerType);
        }
    }
}




