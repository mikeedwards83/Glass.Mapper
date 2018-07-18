using Glass.Mapper.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using System;
using System.Linq;

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
        /// Gets or sets the item config
        /// </summary>
        public SitecoreItemConfiguration ItemConfig { get; set; }

        /// <summary>
        /// Gets or sets the ItemUri config
        /// </summary>
        public SitecoreInfoConfiguration ItemUriConfig { get; set; }
        /// <summary>
        /// Gets or sets the Name config
        /// </summary>
        public SitecoreInfoConfiguration NameConfig { get; set; }

        /// <summary>
        /// Gets or sets the Path config
        /// </summary>
        public SitecoreInfoConfiguration PathConfig { get; set; }


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
        /// Adds the property.
        /// </summary>
        /// <param name="property">The property.</param>
        public override void AddProperty(AbstractPropertyConfiguration property)
        {
            if (property is SitecoreIdConfiguration)
                IdConfig = property as SitecoreIdConfiguration;

            var infoProperty = property as SitecoreInfoConfiguration;

            if (infoProperty != null)
            {
                AssignInfoToKnownProperty(this, infoProperty);
            }

            if (property is SitecoreItemConfiguration)
                ItemConfig = property as SitecoreItemConfiguration;

            base.AddProperty(property);
        }

        public static void AssignInfoToKnownProperty(SitecoreTypeConfiguration typeConfig,
            SitecoreInfoConfiguration infoProperty)
        {
            switch (infoProperty.Type)
            {
                case SitecoreInfoType.Language:
                    typeConfig.LanguageConfig = infoProperty;
                    break;
                case SitecoreInfoType.Version:
                    typeConfig.VersionConfig = infoProperty;
                    break;
                case SitecoreInfoType.ItemUri:
                    typeConfig. ItemUriConfig = infoProperty;
                    break;
                case SitecoreInfoType.Name:
                    typeConfig.NameConfig = infoProperty;
                    break;
                case SitecoreInfoType.Path:
                    typeConfig.PathConfig = infoProperty;
                    break;
            }
        }



        public ID GetId(object target)
        {
            ID id = ID.Null;

            if (IdConfig != null)
            {
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
            }
            else if (ItemUriConfig != null)
            {
                var itemUri = (ItemUri)ItemUriConfig.PropertyInfo.GetValue(target, null);
                if (itemUri != null)
                {
                    id = itemUri.ItemID;
                }
            }
            
            return id;
        }

        /// <summary>
        /// Dumps information about the current type configuration to the Sitecore logs.
        /// </summary>
        public virtual void LogDumpConfiguration()
        {
            Sitecore.Diagnostics.Log.Debug("CD - Configuration Dump for {0}".Formatted(Type.FullName), this);

            var interaces = Type.GetInterfaces();
            if (interaces.Any())
            {
                Sitecore.Diagnostics.Log.Debug("CD - {0}".Formatted(interaces.Select(x => x.FullName).Aggregate((x, y) => "{0}, {1}".Formatted(x, y))), this);
            }
            if (Properties != null)
            {
                foreach (var property in Properties)
                {
                    Sitecore.Diagnostics.Log.Debug("CD - property: {0} - {1}".Formatted(property.PropertyInfo.Name, property.PropertyInfo.PropertyType.FullName), this);
                }
            }
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
            if (target == null)
                return null;

            ID id;
            Language language = null;
            int versionNumber = -1;

            if (ItemConfig != null)
            {
                var item = ItemConfig.PropertyGetter(target) as Item;
                if (item != null)
                    return item;
            }

            if (IdConfig == null && ItemUriConfig == null)
            {
                string message =
                    "Failed item resolve - You cannot save a class that does not contain a property that represents the item ID. Ensure that at least one property has been marked to contain the Sitecore ID. Type: {0}"
                        .Formatted(target.GetType().FullName);
                Sitecore.Diagnostics.Log.Error(message, this);
                
                LogDumpConfiguration();

                throw new NotSupportedException(message);
            }

            id = GetId(target);
            language = GetLanguage(target);
            versionNumber = GetVersion(target);

            if (language != null && versionNumber > 0)
            {
                return database.GetItem(id, language, global::Sitecore.Data.Version.Parse(versionNumber));
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


        public int GetVersion(object target)
        {
            int versionNumber = -1;
            
            if (VersionConfig != null)
            {
                var valueInt = VersionConfig.PropertyInfo.GetValue(target, null);
                if (valueInt is int)
                {
                    versionNumber = (int)valueInt;
                }
                else if (valueInt is string)
                {
                    int.TryParse(valueInt as string, out versionNumber);
                }
            }
            else if (ItemUriConfig != null)
            {
                var itemUri = (ItemUri)ItemUriConfig.PropertyInfo.GetValue(target, null);
                if (itemUri != null)
                {
                    versionNumber = itemUri.Version.Number;
                }
            }

            return versionNumber;
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
                object langValue = LanguageConfig.PropertyInfo.GetValue(target, null);
                
                if (langValue == null)
                {
                    language = Language.Current;
                }
                else if (langValue is Language)
                {
                    language = langValue as Language;
                }
                else if (langValue is string)
                {
                    language = LanguageManager.GetLanguage(langValue as string);
                }
            }
            else if (ItemUriConfig != null)
            {
                var itemUri = (ItemUri)ItemUriConfig.PropertyInfo.GetValue(target, null);
                if (itemUri != null)
                {
                    language = itemUri.Language;
                }
            }

            return language;
        }

        public string GetName(object target)
        {

            if (NameConfig == null)
                throw new MapperException("The type {0} does not have a property with attribute SitecoreInfo(SitecoreInfoType.Name)".Formatted(target.GetType().FullName));

            string name = string.Empty;

            try
            {
                name = NameConfig.PropertyInfo.GetValue(target, null).ToString();
                return name;
            }
            catch
            {
                throw new MapperException("Failed to get item name");
            }
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
                var idConfig = new SitecoreIdConfiguration();
                idConfig.PropertyInfo = property;
                return idConfig;
            }

            if (name.ToLowerInvariant() == "parent")
            {
                var parentConfig = new SitecoreParentConfiguration();
                parentConfig.PropertyInfo = property;
                return parentConfig;
            }
            if (name.ToLowerInvariant() == "children")
            {
                var childrenConfig = new SitecoreChildrenConfiguration();
                childrenConfig.PropertyInfo = property;
                return childrenConfig;
            }
            if (name.ToLowerInvariant() == "item" && property.PropertyType == typeof(Item))
            {
                var itemConfig = new SitecoreItemConfiguration();
                itemConfig.PropertyInfo = property;
                return itemConfig;
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

        public override void GetTypeOptions(GetOptions options)
        {

            base.GetTypeOptions(options);
            
            var scOptions = options as GetItemOptions;

            if (scOptions.EnforceTemplate == SitecoreEnforceTemplate.Default)
            {
                scOptions.EnforceTemplate = this.EnforceTemplate;
            }

            if (ID.IsNullOrEmpty(scOptions.TemplateId))
            {
                scOptions.TemplateId = this.TemplateId;
            }
        }
    }
}




