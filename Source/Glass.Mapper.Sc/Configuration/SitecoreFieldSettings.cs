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

namespace Glass.Mapper.Sc.Configuration
{
    /// <summary>
    /// Enum SitecoreFieldSettings
    /// </summary>
    [Flags]
    public enum SitecoreFieldSettings
    {
        /// <summary>
        /// The field carries out its default behaviour
        /// </summary>
        Default = 0x0,
        /// <summary>
        /// If used on a Rich Text field it stops the contents going through the render process
        /// and returns the raw HTML of the field
        /// </summary>
        RichTextRaw = 0x1,
        /// <summary>
        /// If the property type is another classes loaded by the  Mapper, indicates that the class should not be lazy loaded.
        /// </summary>
        DontLoadLazily = 0x2,
        /// <summary>
        /// Indicates the type should be inferred from the item template
        /// </summary>
        InferType = 0x4,
        /// <summary>
        /// The field will only be used to enable the page editor but values will not be read or written to it.
        /// </summary>
        PageEditorOnly = 0x8,
        /// <summary>
        /// Indicates that the RenderField pipeline should be called for Single Line Text Fields.
        /// </summary>
        ForceRenderField = 0x12,
    }
}




