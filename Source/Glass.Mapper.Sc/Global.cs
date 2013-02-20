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
using Sitecore.Data;

namespace Glass.Mapper.Sc
{
    public static class Global
    {
        public static string PipeEncoding
        {
            get { return "%pipe%"; }
        }

        public static class Fields
        {
            public const string DisplayName = "__Display name";
        }

        public static class IDs
        {
            public static class TemplateFieldIds 
            {
                #region Fields

                // /sitecore/templates/System/Templates/Template field/Validation Rules/Quick Action Bar
                public static readonly ID QuickActionBarFieldId = new ID("{337E20E1-999A-4EEA-85AD-B58A03AE75CC}");
                // /sitecore/templates/System/Templates/Template field/Validation Rules/Validate Button
                public static readonly ID ValidateButtonFieldId = new ID("{21828437-EA4B-40A1-8C61-4CE60EA41DB6}");
                // /sitecore/templates/System/Templates/Template field/Validation Rules/Validator Bar
                public static readonly ID ValidatorBarFieldId = new ID("{9C903E29-650D-4AF2-B9BD-526D5C14A1A5}");
                // /sitecore/templates/System/Templates/Template field/Validation Rules/Workflow
                public static readonly ID WorkflowFieldId = new ID("{53C432C4-7122-4E2D-8296-DB4184FD1735}");

                public static readonly string IsRequiredId = "{59D4EE10-627C-4FD3-A964-61A88B092CBC}";

                #endregion

            }
        }
    
}
}



