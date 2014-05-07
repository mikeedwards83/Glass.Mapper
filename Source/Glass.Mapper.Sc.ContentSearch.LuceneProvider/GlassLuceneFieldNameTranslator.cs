using System.Linq;
using System.Reflection;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.LuceneProvider;

namespace Glass.Mapper.Sc.ContentSearch.LuceneProvider
{
    public class GlassLuceneFieldNameTranslator : LuceneFieldNameTranslator
    {
        public GlassLuceneFieldNameTranslator(ILuceneProviderIndex index) : base(index)
        {
        }

        public override string GetIndexFieldName(MemberInfo member)
        {
            var memberAttribute = GetIndexFieldNameFormatterAttribute(member);
            if (memberAttribute != null) return base.GetIndexFieldName(member);

            var fieldConfig = member.GetCustomAttribute<SitecoreFieldAttribute>(true);
            if (fieldConfig == null && member.DeclaringType != null)
            {
                var interfaceFromProperty = member.DeclaringType.GetInterfaces().FirstOrDefault(inter => inter.GetProperty(member.Name) != null);
                if (interfaceFromProperty != null)
                {
                    fieldConfig = interfaceFromProperty.GetMember(member.Name).First().GetCustomAttribute<SitecoreFieldAttribute>(true);
                }
            }

            if (fieldConfig != null && !string.IsNullOrEmpty(fieldConfig.FieldName))
                return base.GetIndexFieldName(fieldConfig.FieldName);

            if (fieldConfig == null)
            {
                var infoConfig = member.GetCustomAttribute<SitecoreInfoAttribute>(true);

                if (infoConfig == null && member.DeclaringType != null)
                {
                    var interfaceFromProperty = member.DeclaringType.GetInterfaces().FirstOrDefault(inter => inter.GetProperty(member.Name) != null);
                    if (interfaceFromProperty != null)
                    {
                        infoConfig = interfaceFromProperty.GetMember(member.Name).First().GetCustomAttribute<SitecoreInfoAttribute>(true);
                    }
                }

                if (infoConfig != null)
                {
                    switch (infoConfig.Type)
                    {
                        case SitecoreInfoType.DisplayName:
                            return base.GetIndexFieldName(BuiltinFields.DisplayName);
                        case SitecoreInfoType.FullPath:
                            return base.GetIndexFieldName(BuiltinFields.FullPath);
                        case SitecoreInfoType.TemplateId:
                            return base.GetIndexFieldName(BuiltinFields.Template);
                        case SitecoreInfoType.TemplateName:
                            return base.GetIndexFieldName(BuiltinFields.TemplateName);
                        case SitecoreInfoType.Url:
                            return base.GetIndexFieldName(BuiltinFields.Url);
                        case SitecoreInfoType.Version:
                            return base.GetIndexFieldName(BuiltinFields.Version);
                        case SitecoreInfoType.Name:
                            return base.GetIndexFieldName(BuiltinFields.Name);
                        case SitecoreInfoType.Language:
                            return base.GetIndexFieldName(BuiltinFields.Language);
                    }
                }
            }

            return base.GetIndexFieldName(member);
        }
    }
}
