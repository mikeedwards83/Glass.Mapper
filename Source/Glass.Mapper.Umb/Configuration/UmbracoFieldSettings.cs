using System;

namespace Glass.Mapper.Umb.Configuration
{
    [Flags]
    public enum UmbracoFieldSettings
    {
        /// <summary>
        /// The field carries out its default behaviour
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
