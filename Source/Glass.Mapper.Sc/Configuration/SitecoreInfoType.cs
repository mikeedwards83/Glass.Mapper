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

namespace Glass.Mapper.Sc.Configuration
{
    public enum SitecoreInfoType
    {
        /// <summary>
        /// No value has been set
        /// </summary>
        NotSet,
        /// <summary>
        /// The item's content path. The property type must be System.String
        /// </summary>
        ContentPath,
        /// <summary>
        /// The item's display name. The property type must be System.String
        /// </summary>
        DisplayName,
        /// <summary>
        /// The item's full path. The property type must be System.String
        /// </summary>
        FullPath,
        /// <summary>
        /// The item's key . The property type must be System.String
        /// </summary>
        Key,
        /// <summary>
        /// The item's media URL. The property type must be System.String
        /// </summary>
        MediaUrl,
        /// <summary>
        /// The item's path. The property type must be System.String
        /// </summary>
        Path,
        /// <summary>
        /// The item's template Id. The property type must be System.Guid
        /// </summary>
        TemplateId,
        /// <summary>
        /// The item's template name. The property type must be System.String
        /// </summary>
        TemplateName,
        /// <summary>
        /// The item's URL. The property type must be System.String
        /// </summary>
        Url,
        /// <summary>
        /// The item's version. The property type must be System.Int32
        /// </summary>
        Version,
        /// <summary>
        /// The item's Name. The property type must be System.String
        /// </summary>
        Name,
        /// <summary>
        /// The items language. The property type must be Sitecore.Globalization.Language
        /// </summary>
        Language
    }
}



