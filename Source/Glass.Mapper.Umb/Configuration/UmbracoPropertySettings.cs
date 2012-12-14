using System;

namespace Glass.Mapper.Umb.Configuration
{
    [Flags]
    public enum UmbracoPropertySettings
    {
        /// <summary>
        /// The property carries out its default behaviour
        /// </summary>
        Default = 0x0,
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
