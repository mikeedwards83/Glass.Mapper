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
using System.Reflection;

namespace Glass.Mapper.Configuration
{
    /// <summary>
    /// Represents the configuration for a property on a .Net type
    /// </summary>
    public abstract class AbstractPropertyConfiguration
    {
		private PropertyInfo _propertyInfo;
		
		/// <summary>
		/// Gets or sets the property info.
		/// </summary>
		/// <value>The property info.</value>
		public PropertyInfo PropertyInfo
		{
			get { return _propertyInfo; }
			set
			{
				_propertyInfo = value;

				PropertyGetter = Utilities.GetPropertyFunc(value);
				PropertySetter = Utilities.SetPropertyAction(value);
			}
		}

		/// <summary>
		/// Function to get the underlying property value
		/// </summary>
		public Func<object, object> PropertyGetter { get; private set; }
		
		/// <summary>
		/// Action to set the underyling property value
		/// </summary>
		public Action<object, object> PropertySetter { get; private set; }

        /// <summary>
        /// Gets the mapper.
        /// </summary>
        /// <value>The mapper.</value>
        public AbstractDataMapper Mapper  { get; internal set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            if (PropertyInfo == null)
                return "AbstractPropertyConfiguration: Property: Null";

            return "AbstractPropertyConfiguration Property: {0} Type: {1} Assembly: {2}".Formatted(PropertyInfo.Name,
                                                                     PropertyInfo.ReflectedType.FullName,
                                                                     PropertyInfo.ReflectedType.Assembly.FullName);
        }

        protected  abstract AbstractPropertyConfiguration CreateCopy();

        protected virtual void Copy(AbstractPropertyConfiguration copy)
        {
            copy.PropertyInfo = PropertyInfo;
        }
        public  AbstractPropertyConfiguration Copy()
        {
            var configCopy = CreateCopy();
            Copy(configCopy);
            return configCopy;
        }
    }
}




