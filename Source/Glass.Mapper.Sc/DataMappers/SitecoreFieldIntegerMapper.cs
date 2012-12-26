using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data.Fields;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreFieldIntegerMapper : AbstractSitecoreFieldMapper
    {
        public SitecoreFieldIntegerMapper()
            : base(typeof(int))
        {

        }

        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config,
                                             SitecoreDataMappingContext context)
        {
            if (fieldValue.IsNullOrEmpty()) return 0;
            int dValue = 0;
            if (int.TryParse(fieldValue, NumberStyles.Any, CultureInfo.InvariantCulture, out dValue)) return dValue;
            else throw new MapperException("Could not convert value to double");
        }

        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            if (value is int)
            {
                return value.ToString();
            }
            else
                throw new NotSupportedException("The value is not of type System.Double");
        }
    }
}
