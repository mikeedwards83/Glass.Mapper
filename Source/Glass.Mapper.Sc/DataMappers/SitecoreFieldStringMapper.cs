using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Web.UI.WebControls;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreFieldStringMapper : AbstractSitecoreFieldMapper
    {
        public SitecoreFieldStringMapper() : base(typeof (string))
        {
        }

        public override object GetField(Sitecore.Data.Fields.Field field, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            if (field == null)
                return string.Empty;

            if (field.Type.StartsWith("Rich Text") && config.Setting != SitecoreFieldSettings.RichTextRaw)
            {
                FieldRenderer renderer = new FieldRenderer();
                renderer.Item = field.Item;
                renderer.FieldName = field.Name;
                renderer.Parameters = string.Empty;
                return renderer.Render();
            }
            else return field.Value;
        }


        public override void SetField(Sitecore.Data.Fields.Field field, object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            if (field == null)
            {
                return;
            }
            else if (field.Type.StartsWith("Rich Text") && config.Setting != SitecoreFieldSettings.RichTextRaw)
            {
                throw new NotSupportedException("It is not possible to save data from a rich text field when the data isn't raw."
                    + "Set the SitecoreFieldAttribute setting property to SitecoreFieldSettings.RichTextRaw for property {0} on type {1}".Formatted(config.PropertyInfo.Name, config.PropertyInfo.ReflectedType.FullName));
            }
            else
            {
                string fieldValue = (value ?? "").ToString();
                field.Value = fieldValue;
            }
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
}
