using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreFieldDateTimeMapper : AbstractSitecoreFieldMapper
    {
        public SitecoreFieldDateTimeMapper():
            base(typeof(DateTime))
        {

        }

        public override object GetFieldValue(Sitecore.Data.Fields.Field field, Configuration.SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            return global::Sitecore.DateUtil.IsoDateToDateTime(field.Value);
        }

        public override void SetFieldValue(Sitecore.Data.Fields.Field field, object value, Configuration.SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            if (value is DateTime)
            {
                DateTime date = (DateTime)value;
                field.Value = global::Sitecore.DateUtil.ToIsoDate(date);
            }
            else
                throw new NotSupportedException("The value is not of type System.DateTime");
        }
    }
}
