using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Fields;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreFieldHtmlEncodingMapper : Glass.Mapper.Sc.DataMappers.AbstractSitecoreFieldMapper
    {
        public SitecoreFieldHtmlEncodingMapper()
            : base(new[] { typeof(HtmlEncodedString) })
        {
        }
        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            HtmlEncodedString encoded = value as HtmlEncodedString;
            return value == null ? null : encoded.RawValue;
        }

        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            return new HtmlEncodedString(fieldValue);
        }
    }
}
