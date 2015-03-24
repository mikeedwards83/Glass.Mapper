using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data.Fields;

namespace Glass.Mapper.Sc.DataMappers
{
    public abstract class SitecoreFieldScFieldBaseMapper<T> : AbstractSitecoreFieldMapper where T : CustomField
    {

        public SitecoreFieldScFieldBaseMapper()
            : base(typeof(T))
        {
            this.ReadOnly = true;
        }

        protected abstract T Create(Field field);

        public override object GetField(Sitecore.Data.Fields.Field field, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            return Create(field);
        }

        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            throw new NotImplementedException();
        }

        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class SitecoreFieldScCheckboxMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.CheckboxField>
    {
        protected override CheckboxField Create(Field field)
        {
            return new CheckboxField(field);
        }
    }

    public class SitecoreFieldScCustomCustomFieldMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.CustomCustomField>
    {
        protected override CustomCustomField Create(Field field)
        {
            return new CustomCustomField(field);
        }
    }

    public class SitecoreFieldScDatasourceFieldMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.DatasourceField>
    {
        protected override DatasourceField Create(Field field)
        {
            return new DatasourceField(field);
        }
    }

    public class SitecoreFieldScDateFieldMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.DateField>
    {
        protected override DateField Create(Field field)
        {
            return new DateField(field);
        }
    }


    public class SitecoreFieldScFileDropAreaFieldMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.FileDropAreaField>
    {
        protected override FileDropAreaField Create(Field field)
        {
            return new FileDropAreaField(field);
        }
    }
    public class SitecoreFieldScFileFieldMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.FileField>
    {
        protected override FileField Create(Field field)
        {
            return new FileField(field);
        }
    }
    public class SitecoreFieldScGroupedDroplinkFieldMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.GroupedDroplinkField>
    {
        protected override GroupedDroplinkField Create(Field field)
        {
            return new GroupedDroplinkField(field);
        }
    }
    public class SitecoreFieldScGroupedDroplistFieldMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.GroupedDroplistField>
    {
        protected override GroupedDroplistField Create(Field field)
        {
            return new GroupedDroplistField(field);
        }
    }
    public class SitecoreFieldScHtmlFieldMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.HtmlField>
    {
        protected override HtmlField Create(Field field)
        {
            return new HtmlField(field);
        }
    }
    public class SitecoreFieldScImageFieldMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.ImageField>
    {
        protected override ImageField Create(Field field)
        {
            return new ImageField(field);
        }
    }
    public class SitecoreFieldScInternalLinkFieldMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.InternalLinkField>
    {
        protected override InternalLinkField Create(Field field)
        {
            return new InternalLinkField(field);
        }
    }
    public class SitecoreFieldScLayoutFieldMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.LayoutField>
    {
        protected override LayoutField Create(Field field)
        {
            return new LayoutField(field);
        }
    }
    public class SitecoreFieldScLinkFieldMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.LinkField>
    {
        protected override LinkField Create(Field field)
        {
            return new LinkField(field);
        }
    }
    public class SitecoreFieldScLockFieldMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.LockField>
    {
        protected override LockField Create(Field field)
        {
            return new LockField(field);
        }
    }
    public class SitecoreFieldScLookupFieldMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.LookupField>
    {
        protected override LookupField Create(Field field)
        {
            return new LookupField(field);
        }
    }
  
    public class SitecoreFieldScMultilistFieldMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.MultilistField>
    {
        protected override MultilistField Create(Field field)
        {
            return new MultilistField(field);
        }
    }
    public class SitecoreFieldScNameValueListFieldMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.NameValueListField>
    {
        protected override NameValueListField Create(Field field)
        {
            return new NameValueListField(field);
        }
    }
    public class SitecoreFieldPagePreviewFieldScMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.PagePreviewField>
    {
        protected override PagePreviewField Create(Field field)
        {
            return new PagePreviewField(field);
        }
    }

    public class SitecoreFieldScPropertyFieldMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.PropertyField>
    {
        protected override PropertyField Create(Field field)
        {
            return new PropertyField(field);
        }
    }
    public class SitecoreFieldScReferenceFieldMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.ReferenceField>
    {
        protected override ReferenceField Create(Field field)
        {
            return new ReferenceField(field);
        }
    }
    public class SitecoreFieldScRendererFieldMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.RendererField>
    {
        protected override RendererField Create(Field field)
        {
            return new RendererField(field);
        }
    }
    public class SitecoreFieldScTemplateFieldSourceFieldMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.TemplateFieldSourceField>
    {
        protected override TemplateFieldSourceField Create(Field field)
        {
            return new TemplateFieldSourceField(field);
        }
    }
    public class SitecoreFieldScTextFieldMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.TextField>
    {
        protected override TextField Create(Field field)
        {
            return new TextField(field);
        }
    }
    public class SitecoreFieldScThumbnailFieldMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.ThumbnailField>
    {
        protected override ThumbnailField Create(Field field)
        {
            return new ThumbnailField(field);
        }
    }
    public class SitecoreFieldScValueLookupFieldMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.ValueLookupField>
    {
        protected override ValueLookupField Create(Field field)
        {
            return new ValueLookupField(field);
        }
    }
    public class SitecoreFieldScVersionLinkFieldMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.VersionLinkField>
    {
        protected override VersionLinkField Create(Field field)
        {
            return new VersionLinkField(field);
        }
    }
    public class SitecoreFieldScWordDocumentFieldMapper : SitecoreFieldScFieldBaseMapper<Sitecore.Data.Fields.WordDocumentField>
    {
        protected override WordDocumentField Create(Field field)
        {
            return new WordDocumentField(field);
        }
    }


}
