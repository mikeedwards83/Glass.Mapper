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
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace Glass.Mapper.Sc.Configuration
{
    /// <summary>
    /// Class SitecoreTypeConfiguration
    /// </summary>
    public class SitecoreTypeConfiguration : AbstractTypeConfiguration
    {
        /// <summary>
        /// Gets or sets the template id.
        /// </summary>
        /// <value>The template id.</value>
        public ID TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the branch id.
        /// </summary>
        /// <value>The branch id.</value>
        public ID BranchId { get; set; }

        /// <summary>
        /// Gets or sets the id config.
        /// </summary>
        /// <value>The id config.</value>
        public SitecoreIdConfiguration IdConfig { get; set; }
        /// <summary>
        /// Gets or sets the language config.
        /// </summary>
        /// <value>The language config.</value>
        public SitecoreInfoConfiguration LanguageConfig { get; set; }
        /// <summary>
        /// Gets or sets the version config.
        /// </summary>
        /// <value>The version config.</value>
        public SitecoreInfoConfiguration VersionConfig { get; set; }

        /// <summary>
        /// Indicates that the class is used in a code first scenario.
        /// </summary>
        /// <value><c>true</c> if [code first]; otherwise, <c>false</c>.</value>
        public bool CodeFirst { get; set; }

        /// <summary>
        /// Overrides the default template name when using code first
        /// </summary>
        /// <value>The name of the template.</value>
        public string TemplateName { get; set; }


        /// <summary>
        /// Adds the property.
        /// </summary>
        /// <param name="property">The property.</param>
        public override void AddProperty(AbstractPropertyConfiguration property)
        {
            if (property is SitecoreIdConfiguration)
                IdConfig = property as SitecoreIdConfiguration;

            var infoProperty = property as SitecoreInfoConfiguration;

            if (infoProperty != null && infoProperty.Type == SitecoreInfoType.Language)
                LanguageConfig = infoProperty;
            else if (infoProperty != null && infoProperty.Type == SitecoreInfoType.Version)
                VersionConfig = infoProperty;



            base.AddProperty(property);
        }

        /// <summary>
        /// Resolves the item.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="database">The database.</param>
        /// <returns>
        /// Item.
        /// </returns>
        /// <exception cref="System.NotSupportedException">You can not save a class that does not contain a property that represents the item ID. Ensure that at least one property has been marked to contain the Sitecore ID.
        /// or
        /// Cannot get ID for item</exception>
        public Item ResolveItem(object target, Database database)
        {
            ID id;
            Language language = null;
            int versionNumber = -1;

            if (IdConfig == null)
                throw new NotSupportedException(
                    "You can not save a class that does not contain a property that represents the item ID. Ensure that at least one property has been marked to contain the Sitecore ID.");

            if (IdConfig.PropertyInfo.PropertyType == typeof (Guid))
            {
                var guidId = (Guid) IdConfig.PropertyInfo.GetValue(target, null);
                id = new ID(guidId);
            }
            else if (IdConfig.PropertyInfo.PropertyType == typeof (ID))
            {
                id = IdConfig.PropertyInfo.GetValue(target, null) as ID;
            }
            else
            {
                throw new NotSupportedException("Cannot get ID for item");
            }

            language = GetLanguage(target);

            if (VersionConfig != null)
            {
                versionNumber = (int) VersionConfig.PropertyInfo.GetValue(target, null);
            }

            if (language != null && versionNumber > 0)
            {
                return database.GetItem(id, language, new global::Sitecore.Data.Version(versionNumber));
            }
            else if (language != null)
            {
                return database.GetItem(id, language);
            }
            else
            {
                return database.GetItem(id);
            }
        }

        /// <summary>
        /// Gets the language.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public Language GetLanguage(object target)
        {
            Language language = null;
            if (LanguageConfig != null)
            {
                language = LanguageConfig.PropertyInfo.GetValue(target, null) as Language;
                if (language == null)
                    language = Language.Current;
            }
            return language;
        }

        /// <summary>
        /// Called to map each property automatically
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected override AbstractPropertyConfiguration AutoMapProperty(System.Reflection.PropertyInfo property)
        {
            string name = property.Name;
            SitecoreInfoType infoType;

            if (name.ToLowerInvariant() == "id")
            {
                SitecoreIdConfiguration idConfig = new SitecoreIdConfiguration();
                idConfig.PropertyInfo = property;
                return idConfig;
            }

            if (name.ToLowerInvariant() == "parent")
            {
                SitecoreParentConfiguration parentConfig = new SitecoreParentConfiguration();
                parentConfig.PropertyInfo = property;
                return parentConfig;
            }
            if (name.ToLowerInvariant() == "children")
            {
                SitecoreChildrenConfiguration childrenConfig = new SitecoreChildrenConfiguration();
                childrenConfig.PropertyInfo = property;
                return childrenConfig;
            }

            if (Enum.TryParse(name, true, out infoType))
            {
                SitecoreInfoConfiguration infoConfig = new SitecoreInfoConfiguration();
                infoConfig.PropertyInfo = property;
                infoConfig.Type = infoType;
                return infoConfig;
            }

            SitecoreFieldConfiguration fieldConfig = new SitecoreFieldConfiguration();
            fieldConfig.FieldName = name;
            fieldConfig.PropertyInfo = property;
            return fieldConfig;
        }
    }
}




