using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Abstractions;
using Sitecore.ContentSearch.SolrProvider;
using Sitecore.ContentSearch.Utilities;

namespace Glass.Mapper.Sc.ContentSearch.SolrProvider
{
    public class GlassSolrFieldNameTranslator : SolrFieldNameTranslator
    {
        private readonly SolrFieldMap fieldMap;
        private readonly ISettings settings;
        private readonly SolrIndexSchema schema;

        public GlassSolrFieldNameTranslator(SolrSearchIndex index) : base(index)
        {
            this.fieldMap = index.Configuration.FieldMap as SolrFieldMap;
            this.schema = index.Schema as SolrIndexSchema;
            this.settings = index.Locator.GetInstance<ISettings>();
        }

        public override string GetIndexFieldName(MemberInfo member)
        {
            var memberAttribute = GetIndexFieldNameFormatterAttribute(member);
            if (memberAttribute != null) return base.GetIndexFieldName(member);

            //try to get fieldname from glass
            if (member.DeclaringType != null)
            {
                var typeConfig = Context.Contexts.Any() ?
                    Context.Contexts.Select(c => c.Value)
                        .Select(c => c.GetTypeConfigurationFromType<SitecoreTypeConfiguration>(member.DeclaringType))
                        .FirstOrDefault(c => c != null)
                    : null;
                var fieldConfig = typeConfig == null ? null : typeConfig.Properties.FirstOrDefault(p => p.PropertyInfo == member);
                if (fieldConfig != null)
                {
                    Type returnType = fieldConfig.PropertyInfo.PropertyType;
                    if (fieldConfig is SitecoreInfoConfiguration && !string.IsNullOrEmpty(((SitecoreInfoConfiguration)fieldConfig).BuiltInFieldName))
                    {
                        return base.GetIndexFieldName(((SitecoreInfoConfiguration)fieldConfig).BuiltInFieldName, returnType);
                    }
                    else if (fieldConfig is SitecoreFieldConfiguration)
                    {
                        //we need to check if the field is mapped on typename, but can't call base processfieldname, because it's private
                        var fieldName = ((SitecoreFieldConfiguration)fieldConfig).FieldName;
                        var lowerInvariant = StripKnownExtensions(fieldName).Replace(" ", "_").ToLowerInvariant();
                        var searchFieldConfig = fieldMap.GetFieldConfiguration(lowerInvariant) as SolrSearchFieldConfiguration;
                        if (searchFieldConfig != null) //found as mapping, proceed as normal
                            return base.GetIndexFieldName(fieldName, returnType);
                        if (schema.AllFieldNames.Contains(lowerInvariant)) //field explicitly configured, proceed
                            return lowerInvariant;

                        var fieldType = Utilities.GetFieldType(((SitecoreFieldConfiguration)fieldConfig).FieldType, ((SitecoreFieldConfiguration)fieldConfig).CustomFieldType);
                        searchFieldConfig = this.fieldMap.GetFieldConfigurationByFieldTypeName(fieldType) as SolrSearchFieldConfiguration;
                        if (searchFieldConfig != null) //ok, we found our fieldtype mapping, add it as FieldByFieldName like base.processfieldname does, and proceed as normal
                        {
                            this.fieldMap.AddFieldByFieldName(lowerInvariant, searchFieldConfig);
                            return base.GetIndexFieldName(fieldName, returnType);
                        }

                        searchFieldConfig = fieldMap.GetFieldConfiguration(returnType) as SolrSearchFieldConfiguration;
                        if (searchFieldConfig != null) //found as type mapping, proceed as normal
                            return base.GetIndexFieldName(fieldName, returnType);
                    }
                }
            }
            return base.GetIndexFieldName(member);
        }
    }
}
