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




