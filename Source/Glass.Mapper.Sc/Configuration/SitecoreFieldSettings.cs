using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Sc.Configuration
{
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
        InferType = 0x4
    }
}
