using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data.Fields;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreFieldDecimalMapper : AbstractSitecoreFieldMapper
    {
        public SitecoreFieldDecimalMapper() : base(typeof (decimal))
        {

        }

        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config,
                                             SitecoreDataMappingContext context)
        {
            if (fieldValue.IsNullOrEmpty()) return 0M;

            decimal dValue = 0;
            if (decimal.TryParse(fieldValue, NumberStyles.Any, CultureInfo.InvariantCulture, out dValue))
                return dValue;
            else throw new MapperException("Could not convert value to decimal");
        }

        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            if (value is decimal)
            {
                return value.ToString();
            }
            else
                throw new NotSupportedException("The value is not of type System.Decimal");
        }
    }
}
