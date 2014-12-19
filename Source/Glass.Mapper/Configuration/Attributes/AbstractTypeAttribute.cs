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

namespace Glass.Mapper.Configuration.Attributes
{
    /// <summary>
    /// Class AbstractTypeAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public abstract class AbstractTypeAttribute : Attribute
    {

        public abstract AbstractTypeConfiguration Configure(Type type);
        /// <summary>
        /// Configures the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="config">The config.</param>
        protected virtual void Configure(Type type, AbstractTypeConfiguration config)
        {
            config.Type = type;
            config.ConstructorMethods = Utilities.CreateConstructorDelegates(type);
            config.AutoMap = AutoMap;
            config.Cachable = Cachable;
        }

        /// <summary>
        /// Indicates that the type is cachable by Glass.Mapper
        /// </summary>
        public bool Cachable { get; set; }

        /// <summary>
        /// Indicates that properties should be automapped rather than loaded explicitly. 
        /// </summary>
        public bool AutoMap { get; set; }

        /// <summary>
        /// Indicates that the class is used in a code first scenario.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [code first]; otherwise, <c>false</c>.
        /// </value>
        public bool CodeFirst { get; set; }
    }
}




