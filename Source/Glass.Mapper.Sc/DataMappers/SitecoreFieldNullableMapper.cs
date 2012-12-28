using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.DataMappers
{
    public class SitecoreFieldNullableMapper<T, TMapper> : AbstractSitecoreFieldMapper where TMapper : AbstractSitecoreFieldMapper, new() where T: struct 
    {
        private AbstractSitecoreFieldMapper _baseMapper;

        public SitecoreFieldNullableMapper() : base(typeof(Nullable<T>))
        {
            _baseMapper = new TMapper();
        }

        public override object GetField(Sitecore.Data.Fields.Field field, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            if (string.IsNullOrWhiteSpace(field.Value))
            {
                return null;
            }
            return _baseMapper.GetField(field, config, context);
        }

        public override void SetField(Sitecore.Data.Fields.Field field, object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            if (value == null)
            {
                field.Value = string.Empty;
                return;
            }

            _baseMapper.SetField(field, (T)value, config, context);
        }


        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            throw new NotImplementedException();
        }

        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            throw new NotImplementedException();
        }

        

        public override void Setup(Pipelines.DataMapperResolver.DataMapperResolverArgs args)
        {
            _baseMapper.Setup(args);
            base.Setup(args);
        }
    }
}
