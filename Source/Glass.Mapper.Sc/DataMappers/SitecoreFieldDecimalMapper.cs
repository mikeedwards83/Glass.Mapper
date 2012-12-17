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

        public override object GetFieldValue(Field field, SitecoreFieldConfiguration config,
                                             SitecoreDataMappingContext context)
        {
            if (field.Value.IsNullOrEmpty()) return 0M;

            decimal dValue = 0;
            if (decimal.TryParse(field.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out dValue))
                return dValue;
            else throw new MapperException("Could not convert value to decimal");
        }

        public override void SetFieldValue(Field field, object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            if (value is decimal)
            {
                field.Value = value.ToString();
            }
            else
                throw new NotSupportedException("The value is not of type System.Decimal");
        }
    }
}
