using System;
using Glass.Mapper.Configuration;
using umbraco.cms.businesslogic.web;

namespace Glass.Mapper.Umb.Configuration
{
    public class UmbracoTypeConfiguration : AbstractTypeConfiguration
    {
        public int DocumentTypeId { get; set; }

        public UmbracoIdConfiguration IdConfig { get; set; }
      //  public UmbracoInfoConfiguration LanguageConfig { get; set; }
      //  public UmbracoInfoConfiguration VersionConfig { get; set; }

        public Document ResolveItem(object target)//, Database database)
        {
            return null;
            //ID id;
            //Language language = null;
            //int versionNumber = -1;

            //if (IdConfig == null)
            //    throw new NotSupportedException(
            //        "You can not save a class that does not contain a property that represents the item ID. Ensure that at least one property has the UmbracoIdAttribute");

            //if (IdConfig.PropertyInfo.PropertyType == typeof (Guid))
            //{
            //    var guidId = (Guid) IdConfig.PropertyInfo.GetValue(target, null);
            //    id = new ID(guidId);
            //}
            //else if (IdConfig.PropertyInfo.PropertyType == typeof (ID))
            //{
            //    id = IdConfig.PropertyInfo.GetValue(target, null) as ID;
            //}
            //else
            //{
            //    throw new NotSupportedException("Can not get ID for item");
            //}

            //if (LanguageConfig != null)
            //{
            //    language = LanguageConfig.PropertyInfo.GetValue(target, null) as Language;
            //    if (language == null)
            //        language = Language.Current;
            //}

            //if (VersionConfig != null)
            //{
            //    versionNumber = (int) VersionConfig.PropertyInfo.GetValue(target, null);
            //}

            //if (language != null && versionNumber > 0)
            //{
            //    return database.GetItem(id, language, new global::Umbraco.Data.Version(versionNumber));
            //}
            //else if (language != null)
            //{
            //    return database.GetItem(id, language);
            //}
            //else
            //{
            //    return database.GetItem(id);
            //}
        }
    }
}
