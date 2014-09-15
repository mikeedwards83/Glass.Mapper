using System;
using System.Linq;
using System.Reflection;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.LuceneProvider;
using Sitecore.ContentSearch.Utilities;

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

            //try to find IndexFieldAttributes first
            if (member.DeclaringType != null)
            {
                var interfaces = member.DeclaringType.GetInterfaces();
                var interfaceFromProperty = interfaces.FirstOrDefault(@interface => @interface.GetProperties().Any(p => p.Name == member.Name));

                if (interfaceFromProperty != null)
                {
                    var indexConfig = interfaceFromProperty.GetMember(member.Name).First().GetCustomAttribute<IndexFieldAttribute>(true);
                    if (indexConfig != null) return base.GetIndexFieldName(indexConfig.IndexFieldName);
                }
            }

            //try to translate FieldName next
            var fieldConfig = member.GetCustomAttribute<SitecoreFieldAttribute>(true);
            if (fieldConfig == null && member.DeclaringType != null)
            {
                var interfaces = member.DeclaringType.GetInterfaces();
                foreach (var @interface in interfaces.Where(@interface => @interface.GetProperties().Any(p => p.Name == member.Name)))
                {
                    fieldConfig = @interface.GetMember(member.Name).First().GetCustomAttribute<SitecoreFieldAttribute>(true);
                    if (fieldConfig != null) break;
                }
            }

            if (fieldConfig != null && !string.IsNullOrEmpty(fieldConfig.FieldName))
                return base.GetIndexFieldName(fieldConfig.FieldName);

            if (fieldConfig == null)
            {
                var infoConfig = member.GetCustomAttribute<SitecoreInfoAttribute>(true);

                if (infoConfig == null && member.DeclaringType != null)
                {
                    var interfaces = member.DeclaringType.GetInterfaces();
                    foreach (var @interface in interfaces.Where(@interface => @interface.GetProperties().Any(p => p.Name == member.Name)))
                    {
                        infoConfig = @interface.GetMember(member.Name).First().GetCustomAttribute<SitecoreInfoAttribute>(true);
                        if (infoConfig != null) break;
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

            var idConfig = member.GetCustomAttribute<SitecoreIdAttribute>(true);

            if (idConfig == null && member.DeclaringType != null)
            {
                var interfaces = member.DeclaringType.GetInterfaces();
                foreach (var @interface in interfaces.Where(@interface => @interface.GetProperties().Any(p => p.Name == member.Name)))
                {
                    idConfig = @interface.GetMember(member.Name).First().GetCustomAttribute<SitecoreIdAttribute>(true);
                    if (idConfig != null) break;
                }
            }

            if (idConfig != null)
            {
                return base.GetIndexFieldName(BuiltinFields.Group);
            }

            return base.GetIndexFieldName(member);
        }
    }
}
