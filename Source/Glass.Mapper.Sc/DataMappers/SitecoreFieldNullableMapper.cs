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
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Sc.Configuration;

namespace Glass.Mapper.Sc.DataMappers
{
    /// <summary>
    /// Class SitecoreFieldNullableMapper
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TMapper">The type of the T mapper.</typeparam>
    public class SitecoreFieldNullableMapper<T, TMapper> : AbstractSitecoreFieldMapper where TMapper : AbstractSitecoreFieldMapper, new() where T: struct 
    {
        private AbstractSitecoreFieldMapper _baseMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SitecoreFieldNullableMapper{T, TMapper}"/> class.
        /// </summary>
        public SitecoreFieldNullableMapper() : base(typeof(Nullable<T>))
        {
            _baseMapper = new TMapper();
        }

        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.Object.</returns>
        public override object GetField(Sitecore.Data.Fields.Field field, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            if (string.IsNullOrWhiteSpace(field.Value))
            {
                return null;
            }
            return _baseMapper.GetField(field, config, context);
        }

        /// <summary>
        /// Sets the field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        public override void SetField(Sitecore.Data.Fields.Field field, object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            if (value == null)
            {
                field.Value = null;
                return;
            }

            _baseMapper.SetField(field, (T)value, config, context);
        }


        /// <summary>
        /// Sets the field value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override string SetFieldValue(object value, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the field value.
        /// </summary>
        /// <param name="fieldValue">The field value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override object GetFieldValue(string fieldValue, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// Sets up the data mapper for a particular property
        /// </summary>
        /// <param name="args">The args.</param>
        public override void Setup(DataMapperResolverArgs args)
        {
            _baseMapper.Setup(args);
            base.Setup(args);
        }
    }

    /// <summary>
    /// Class SitecoreFieldNullableDateTimeMapper
    /// </summary>
    public class SitecoreFieldNullableDateTimeMapper :
          SitecoreFieldNullableMapper<DateTime, SitecoreFieldDateTimeMapper> { }

    /// <summary>
    /// Class SitecoreFieldNullableDecimalMapper
    /// </summary>
    public class SitecoreFieldNullableDecimalMapper :
        SitecoreFieldNullableMapper<Decimal, SitecoreFieldDecimalMapper> { }

    /// <summary>
    /// Class SitecoreFieldNullableDoubleMapper
    /// </summary>
    public class SitecoreFieldNullableDoubleMapper :
        SitecoreFieldNullableMapper<Double, SitecoreFieldDoubleMapper> { }

    /// <summary>
    /// Class SitecoreFieldNullableFloatMapper
    /// </summary>
    public class SitecoreFieldNullableFloatMapper :
        SitecoreFieldNullableMapper<float, SitecoreFieldFloatMapper> { }

    /// <summary>
    /// Class SitecoreFieldNullableGuidMapper
    /// </summary>
    public class SitecoreFieldNullableGuidMapper :
        SitecoreFieldNullableMapper<Guid, SitecoreFieldGuidMapper> { }

    /// <summary>
    /// Class SitecoreFieldNullableIntMapper
    /// </summary>
    public class SitecoreFieldNullableIntMapper :
        SitecoreFieldNullableMapper<int, SitecoreFieldIntegerMapper> { }
}




