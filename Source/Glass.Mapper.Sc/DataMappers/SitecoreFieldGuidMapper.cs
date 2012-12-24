using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data.Fields;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreFieldGuidMapper : AbstractSitecoreFieldMapper
    {
        public SitecoreFieldGuidMapper()
            : base(typeof(Guid))
        {

        }

        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config,
                                             SitecoreDataMappingContext context)
        {
            if (fieldValue.IsNullOrEmpty()) return Guid.Empty;


            return Guid.Parse(fieldValue);
        }

        public override void SetField(Field field, object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            if (value is Guid)
            {
                field.Value = ((Guid)value).ToString("B").ToUpper();
            }
            else throw new MapperException("The value is not of type System.Guid");
        }
    }
}
