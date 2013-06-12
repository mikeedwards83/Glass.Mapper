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
using Glass.Mapper.Pipelines.DataMapperResolver;
using Glass.Mapper.Umb.Configuration;
using Umbraco.Core.Models;

namespace Glass.Mapper.Umb.DataMappers
{
    /// <summary>
    /// Class UmbracoPropertyNullableMapper
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TMapper">The type of the T mapper.</typeparam>
    public class UmbracoPropertyNullableMapper<T, TMapper> : AbstractUmbracoPropertyMapper where TMapper : AbstractUmbracoPropertyMapper, new() where T: struct 
    {
        private readonly AbstractUmbracoPropertyMapper _baseMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UmbracoPropertyNullableMapper{T, TMapper}" /> class.
        /// </summary>
        public UmbracoPropertyNullableMapper()
            : base(typeof(Nullable<T>))
        {
            _baseMapper = new TMapper();
        }

        /// <summary>
        /// Gets the Property.
        /// </summary>
        /// <param name="property">The Property.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// System.Object.
        /// </returns>
        public override object GetProperty(Property property, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context)
        {
            if (property.Value != null)
                return null;

            return _baseMapper.GetProperty(property, config, context);
        }

        /// <summary>
        /// Sets the Property.
        /// </summary>
        /// <param name="property">The Property.</param>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        public override void SetProperty(Property property, object value, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context)
        {
            if (value == null)
            {
                property.Value = null;
                return;
            }

            _baseMapper.SetProperty(property, (T)value, config, context);
        }


        /// <summary>
        /// Sets the Property value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override object SetPropertyValue(object value, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the Property value.
        /// </summary>
        /// <param name="propertyValue">The Property value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>
        /// System.Object.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override object GetPropertyValue(object propertyValue, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context)
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
    /// Class UmbracoPropertyNullableDateTimeMapper
    /// </summary>
    public class UmbracoPropertyNullableDateTimeMapper :
          UmbracoPropertyNullableMapper<DateTime, UmbracoPropertyDateTimeMapper> { }

    /// <summary>
    /// Class UmbracoPropertyNullableDecimalMapper
    /// </summary>
    public class UmbracoPropertyNullableDecimalMapper :
        UmbracoPropertyNullableMapper<Decimal, UmbracoPropertyDecimalMapper> { }

    /// <summary>
    /// Class UmbracoPropertyNullableDoubleMapper
    /// </summary>
    public class UmbracoPropertyNullableDoubleMapper :
        UmbracoPropertyNullableMapper<Double, UmbracoPropertyDoubleMapper> { }

    /// <summary>
    /// Class UmbracoPropertyNullableFloatMapper
    /// </summary>
    public class UmbracoPropertyNullableFloatMapper :
        UmbracoPropertyNullableMapper<float, UmbracoPropertyFloatMapper> { }

    /// <summary>
    /// Class UmbracoPropertyNullableGuidMapper
    /// </summary>
    public class UmbracoPropertyNullableGuidMapper :
        UmbracoPropertyNullableMapper<Guid, UmbracoPropertyGuidMapper> { }

    /// <summary>
    /// Class UmbracoPropertyNullableIntegerMapper
    /// </summary>
    public class UmbracoPropertyNullableIntegerMapper :
        UmbracoPropertyNullableMapper<int, UmbracoPropertyIntegerMapper> { }

    /// <summary>
    /// Class UmbracoPropertyNullableLongMapper
    /// </summary>
    public class UmbracoPropertyNullableLongMapper :
        UmbracoPropertyNullableMapper<int, UmbracoPropertyLongMapper> { }
}

