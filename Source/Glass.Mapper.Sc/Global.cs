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

namespace Glass.Mapper.Sc
{
    /// <summary>
    /// Class Global
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// Gets the pipe encoding.
        /// </summary>
        /// <value>The pipe encoding.</value>
        public static string PipeEncoding
        {
            get { return "%pipe%"; }
        }

        /// <summary>
        /// Class Fields
        /// </summary>
        public static class Fields
        {
            /// <summary>
            /// The display name
            /// </summary>
            public const string DisplayName = "__Display name";
        }

        /// <summary>
        /// Class IDs
        /// </summary>
        public static class IDs
        {
            /// <summary>
            /// Class TemplateFieldIds
            /// </summary>
            public static class TemplateFieldIds 
            {
                #region Fields

                // /sitecore/templates/System/Templates/Template field/Validation Rules/Quick Action Bar
                /// <summary>
                /// The quick action bar field id
                /// </summary>
                public static readonly ID QuickActionBarFieldId = new ID("{337E20E1-999A-4EEA-85AD-B58A03AE75CC}");
                // /sitecore/templates/System/Templates/Template field/Validation Rules/Validate Button
                /// <summary>
                /// The validate button field id
                /// </summary>
                public static readonly ID ValidateButtonFieldId = new ID("{21828437-EA4B-40A1-8C61-4CE60EA41DB6}");
                // /sitecore/templates/System/Templates/Template field/Validation Rules/Validator Bar
                /// <summary>
                /// The validator bar field id
                /// </summary>
                public static readonly ID ValidatorBarFieldId = new ID("{9C903E29-650D-4AF2-B9BD-526D5C14A1A5}");
                // /sitecore/templates/System/Templates/Template field/Validation Rules/Workflow
                /// <summary>
                /// The workflow field id
                /// </summary>
                public static readonly ID WorkflowFieldId = new ID("{53C432C4-7122-4E2D-8296-DB4184FD1735}");

                /// <summary>
                /// The is required id
                /// </summary>
                public static readonly string IsRequiredId = "{59D4EE10-627C-4FD3-A964-61A88B092CBC}";

                #endregion

            }
        }
    
}
}




