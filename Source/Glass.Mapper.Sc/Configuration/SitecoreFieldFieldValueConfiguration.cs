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

using Sitecore.Data;

namespace Glass.Mapper.Sc.Configuration
{
    /// <summary>
    /// Used to populate default values of a field's field
    /// </summary>
    public class SitecoreFieldFieldValueConfiguration
    {
        /// <summary>
        /// The Id (Guid) of the field to load
        /// </summary>
        /// <value>The field id.</value>
        public ID FieldId { get; set; }

        /// <summary>
        /// The value for the field if using Code First
        /// </summary>
        /// <value>The field value.</value>
        public string FieldValue { get; set; }
    }
}

