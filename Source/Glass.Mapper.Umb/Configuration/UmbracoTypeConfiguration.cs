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
using Glass.Mapper.Configuration;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Glass.Mapper.Umb.Configuration
{
    public class UmbracoTypeConfiguration : AbstractTypeConfiguration
    {
        public string ContentTypeAlias { get; set; }

        public UmbracoIdConfiguration IdConfig { get; set; }
        public UmbracoInfoConfiguration VersionConfig { get; set; }

        /// <summary>
        /// Indicates that the class is used in a code first scenario.
        /// </summary>
        public bool CodeFirst { get; set; }

        /// <summary>
        /// Overrides the default content type name when using code first
        /// </summary>
        public string ContentTypeName { get; set; }

        public IContent ResolveItem(object target, IContentService contentService)
        {
            int id;
            Guid versionNumber = default(Guid);

            if (IdConfig == null)
                throw new NotSupportedException(
                    "You can not save a class that does not contain a property that represents the item ID. Ensure that at least one property has the UmbracoIdAttribute");

            if (IdConfig.PropertyInfo.PropertyType == typeof(int))
            {
                id = (int)IdConfig.PropertyInfo.GetValue(target, null);
            }
            else
            {
                throw new NotSupportedException("Can not get ID for item");
            }

            if (VersionConfig != null)
            {
                versionNumber = (Guid)VersionConfig.PropertyInfo.GetValue(target, null);
            }

            if (versionNumber != default(Guid))
            {
                return contentService.GetByVersion(versionNumber);
            }
            
            return contentService.GetById(id);
        }
    }
}



