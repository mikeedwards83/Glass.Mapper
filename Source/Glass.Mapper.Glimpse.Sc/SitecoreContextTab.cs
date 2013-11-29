using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Configuration;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data;

namespace Glass.Mapper.Glimpse.Sc
{
    public class SitecoreContextTab : ContextTab
    {
        public override object MapTypeConfiguration(AbstractTypeConfiguration config)
        {
            var scConfig = config as SitecoreTypeConfiguration;

            return new 
            {
                Type = scConfig.Type.FullName,
                TemplateId =  ID.IsNullOrEmpty(scConfig.TemplateId) ? null : scConfig.TemplateId.ToString(),
                TemplateName = scConfig.TemplateName,
                BranchId = ID.IsNullOrEmpty(scConfig.BranchId) ? null : scConfig.BranchId.ToString(),
                AutoMap = scConfig.AutoMap,
                CodeFirst = scConfig.CodeFirst,
                Infos = scConfig.Properties.Where(x => x is SitecoreInfoConfiguration).Select(x => Map(x as SitecoreInfoConfiguration)),
                Fields = scConfig.Properties.Where(x => x is SitecoreFieldConfiguration).Select(x => Map(x as SitecoreFieldConfiguration)),
                Children = scConfig.Properties.Where(x => x is SitecoreChildrenConfiguration).Select(x => Map(x as SitecoreChildrenConfiguration)),
                Parent = scConfig.Properties.Where(x => x is SitecoreParentConfiguration).Select(x => Map(x as SitecoreParentConfiguration)),
                Query = scConfig.Properties.Where(x => x is SitecoreQueryConfiguration).Select(x => Map(x as SitecoreQueryConfiguration)),
                Node = scConfig.Properties.Where(x => x is SitecoreNodeConfiguration).Select(x => Map(x as SitecoreNodeConfiguration)),
                Linked = scConfig.Properties.Where(x => x is SitecoreLinkedConfiguration).Select(x => Map(x as SitecoreLinkedConfiguration)),
            };
        }

        protected object Map(SitecoreLinkedConfiguration config)
        {
            return new
            {
                Name = config.PropertyInfo.Name,
                ReturnType = CleanTypeName(config.PropertyInfo.PropertyType),
                config.Option,
                config.IsLazy,
                config.InferType
            };
        }
        protected object Map(SitecoreNodeConfiguration config)
        {
            return new
            {
                Name = config.PropertyInfo.Name,
                ReturnType = CleanTypeName(config.PropertyInfo.PropertyType),
                config.Id,
                config.Path,
                config.IsLazy,
                config.InferType
            };
        }
        protected object Map(SitecoreQueryConfiguration config)
        {
            return new
            {
                Name = config.PropertyInfo.Name,
                ReturnType = CleanTypeName(config.PropertyInfo.PropertyType),
                config.Query,
                config.IsRelative,
                config.IsLazy,
                config.InferType
            };
        }
        protected object Map(SitecoreParentConfiguration config)
        {
            return new
            {
                Name = config.PropertyInfo.Name,
                ReturnType = CleanTypeName(config.PropertyInfo.PropertyType),
                config.IsLazy,
                config.InferType
            };
        }

        protected object Map(SitecoreChildrenConfiguration config)
        {
            return new
            {
                Name = config.PropertyInfo.Name,
                ReturnType = CleanTypeName(config.PropertyInfo.PropertyType),
                config.IsLazy,
                config.InferType
            };
        }
        protected object Map(SitecoreInfoConfiguration config)
        {
            return new
            {
                Name = config.PropertyInfo.Name,
                ReturnType = CleanTypeName(config.PropertyInfo.PropertyType),
                Type = config.Type.ToString(),
            };
        }
        protected object Map(SitecoreFieldConfiguration config)
        {
            if (config.CodeFirst)
            {
                return new
                {
                    Name = config.PropertyInfo.Name,
                    ReturnType = CleanTypeName(config.PropertyInfo.PropertyType),
                    config.FieldName,
                    config.FieldId,
                    config.CodeFirst,
                    config.FieldSortOrder,
                    config.FieldSource,
                    config.FieldTitle,
                    config.FieldType,
                    config.IsRequired,
                    config.IsShared,
                    config.IsUnversioned,
                    config.SectionName,
                    config.SectionSortOrder,
                    config.ValidationErrorText,
                    config.ValidationRegularExpression,
                };
            }
            else
            {
                return new
                {
                    Name = config.PropertyInfo.Name,
                    ReturnType = CleanTypeName(config.PropertyInfo.PropertyType),
                    config.FieldName,
                    config.FieldId
                };
            }
        }
    }
}
