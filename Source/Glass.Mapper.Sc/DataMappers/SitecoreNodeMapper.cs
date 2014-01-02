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
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class SitecoreNodeMapper
    /// </summary>
    public class SitecoreNodeMapper : AbstractDataMapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreNodeMapper"/> class.
        /// </summary>
        public SitecoreNodeMapper()
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
            var scConfig = Configuration as SitecoreNodeConfiguration;
            var scContext = mappingContext as SitecoreDataMappingContext;
            var item = scContext.Item;

            Item targetItem = null;

            if (scConfig.Id.IsNotNullOrEmpty())
            {
                var guid = Guid.Empty;

                if (Guid.TryParse(scConfig.Id, out guid) && guid != Guid.Empty)
                {
                    targetItem = item.Database.GetItem(new ID(guid), item.Language);
                }
            }
            else if (!scConfig.Path.IsNullOrEmpty())
            {
                targetItem = item.Database.GetItem(scConfig.Path, item.Language);
            }

            if (targetItem == null || targetItem.Versions.Count == 0)
            {
                return null;
            }
            else
            {
                return scContext.Service.CreateType(scConfig.PropertyInfo.PropertyType, targetItem, scConfig.IsLazy,
                                                     scConfig.InferType, null);
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
            return configuration is SitecoreNodeConfiguration;// && context.TypeConfigurations.ContainsKey(configuration.PropertyInfo.PropertyType);
        }
    }
}




