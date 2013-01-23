/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-

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

    public class SitecoreFieldNullableDateTimeMapper :
          SitecoreFieldNullableMapper<DateTime, SitecoreFieldDateTimeMapper> { }

    public class SitecoreFieldNullableDecimalMapper :
        SitecoreFieldNullableMapper<Decimal, SitecoreFieldDecimalMapper> { }

    public class SitecoreFieldNullableDoubleMapper :
        SitecoreFieldNullableMapper<Double, SitecoreFieldDoubleMapper> { }

    public class SitecoreFieldNullableFloatMapper :
        SitecoreFieldNullableMapper<float, SitecoreFieldFloatMapper> { }

    public class SitecoreFieldNullableGuidMapper :
        SitecoreFieldNullableMapper<Guid, SitecoreFieldGuidMapper> { }

    public class SitecoreFieldNullableIntMapper :
        SitecoreFieldNullableMapper<int, SitecoreFieldIntegerMapper> { }
}



