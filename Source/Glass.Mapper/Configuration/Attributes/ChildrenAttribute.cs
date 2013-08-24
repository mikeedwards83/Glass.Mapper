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

namespace Glass.Mapper.Configuration.Attributes
{
    /// <summary>
    /// Indicates that the property should contain the children of the current item
    /// </summary>
    public abstract class ChildrenAttribute : AbstractPropertyAttribute
    {
        //ME - does this need to be an abstract class?

        /// <summary>
        /// Used to retrieve the children of an item as a specific type.
        /// </summary>
        public ChildrenAttribute()
        {
            IsLazy = true;
        }

        /// <summary>
        /// Indicates if children should be loaded lazily. Default value is true. If false all children will be loaded when the containing object is created.
        /// </summary>
        /// <value><c>true</c> if this instance is lazy; otherwise, <c>false</c>.</value>
        public virtual bool IsLazy
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates the type should be inferred from the item template
        /// </summary>
        /// <value><c>true</c> if [infer type]; otherwise, <c>false</c>.</value>
        public virtual bool InferType 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Configures the specified property info.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="config">The config.</param>
        public void Configure(System.Reflection.PropertyInfo propertyInfo, ChildrenConfiguration config)
        {
            config.InferType = InferType;
            config.IsLazy = IsLazy;

            base.Configure(propertyInfo, config);
        }
    }
}




