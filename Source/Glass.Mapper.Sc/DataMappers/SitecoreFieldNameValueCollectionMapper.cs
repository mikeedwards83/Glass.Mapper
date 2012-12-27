using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreFieldNameValueCollectionMapper : AbstractSitecoreFieldMapper
    {
        public SitecoreFieldNameValueCollectionMapper() : base(typeof (NameValueCollection))
        {
        }


        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            NameValueCollection collection = value as NameValueCollection;

            if (collection != null)
            {
                return Utilities.ConstructQueryString(collection);
            }
            else return string.Empty;

        }

        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
                   return HttpUtility.ParseQueryString(fieldValue);

        }
    }
}
