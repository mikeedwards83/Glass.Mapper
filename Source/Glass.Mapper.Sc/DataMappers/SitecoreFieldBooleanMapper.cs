using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using Sitecore.Data.Fields;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreFieldBooleanMapper : AbstractSitecoreFieldMapper
    {
        public SitecoreFieldBooleanMapper() : base(typeof (bool))
        {

        }

        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            return fieldValue == "1";
        }

        public override string SetFieldValue( object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            bool actual = (bool) value;
            return actual ? "1" : "0";
        }
    }
}
