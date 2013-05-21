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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Umb.Configuration;
using Umbraco.Core.Models;

namespace Glass.Mapper.Umb.DataMappers
{
    /// <summary>
    /// AbstractUmbracoPropertyMapper
    /// </summary>
    public abstract class AbstractUmbracoPropertyMapper : AbstractDataMapper
    {
        /// <summary>
        /// Gets the types handled.
        /// </summary>
        /// <value>
        /// The types handled.
        /// </value>
        public IEnumerable<Type> TypesHandled { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractUmbracoPropertyMapper"/> class.
        /// </summary>
        /// <param name="typesHandled">The types handled.</param>
        public AbstractUmbracoPropertyMapper(params Type[] typesHandled)
        {
            TypesHandled = typesHandled;
        }

        /// <summary>
        /// Maps data from the .Net property value to the CMS value
        /// </summary>
        /// <param name="mappingContext"></param>
        public override void MapToCms(AbstractDataMappingContext mappingContext)
        {
            var config = Configuration as UmbracoPropertyConfiguration;
            var context =  mappingContext  as UmbracoDataMappingContext;

            if (context.Content.Properties.Contains(config.PropertyAlias))
            {
                var property = context.Content.Properties[config.PropertyAlias];
                object value = Configuration.PropertyInfo.GetValue(mappingContext.Object, null);

                SetProperty(property, value, config, context);
            }
        }

        /// <summary>
        /// Maps data from the CMS value to the .Net property value
        /// </summary>
        /// <param name="mappingContext"></param>
        /// <returns></returns>
        public override object MapToProperty(AbstractDataMappingContext mappingContext)
        {
            var config = Configuration as UmbracoPropertyConfiguration;
            var context = mappingContext as UmbracoDataMappingContext;

            if (context.Content.Properties.Select(p => p.Alias.ToLowerInvariant()).Contains(config.PropertyAlias.ToLowerInvariant()))
            {
                var property = context.Content.Properties.FirstOrDefault(p => p.Alias.ToLowerInvariant() == config.PropertyAlias.ToLowerInvariant());
                return GetProperty(property, config, context);
            }

            return null;
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public virtual object GetProperty(Property property, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context)
        {
            var propertyValue = property.Value;

            return GetPropertyValue(propertyValue, config, context);
        }

        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        public virtual void SetProperty(Property property, object value, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context)
        {
            property.Value = SetPropertyValue(value, config, context);
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public abstract object SetPropertyValue(object value, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context);

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="propertyValue">The property value.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public abstract object GetPropertyValue(object propertyValue, UmbracoPropertyConfiguration config, UmbracoDataMappingContext context);

        /// <summary>
        /// Indicates that the data mapper will mapper to and from the property
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool CanHandle(Mapper.Configuration.AbstractPropertyConfiguration configuration, Context context)
        {
            return configuration is UmbracoPropertyConfiguration &&
                   TypesHandled.Any(x => x == configuration.PropertyInfo.PropertyType);
        }
    }

    /// <summary>
    /// LowercaseComparer
    /// </summary>
    public class LowercaseComparer : IEqualityComparer<string>
    {
        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="x">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <param name="y">The y.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Equals(object x, object y)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public int GetHashCode(object obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type string to compare.</param>
        /// <param name="y">The second object of type string to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Equals(string x, string y)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public int GetHashCode(string obj)
        {
            throw new NotImplementedException();
        }
    }
}

