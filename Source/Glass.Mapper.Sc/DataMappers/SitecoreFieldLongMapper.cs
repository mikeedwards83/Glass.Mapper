using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data.Fields;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreFieldLongMapper : AbstractSitecoreFieldMapper
    {
        public SitecoreFieldLongMapper()
            : base(typeof(long))
        {

        }

        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config,
                                             SitecoreDataMappingContext context)
        {
            if (fieldValue.IsNullOrEmpty()) return (long)0;
            long dValue = 0;
            if (long.TryParse(fieldValue, NumberStyles.Any, CultureInfo.InvariantCulture, out dValue)) return dValue;
            else throw new MapperException("Could not convert value to double");
        }

        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            if (value is long)
            {
                return value.ToString();
            }
            else
                throw new NotSupportedException("The value is not of type System.Double");
        }
    }
}
