using System;

namespace Glass.Mapper.Configuration.Attributes
{
    /// <summary>
    /// Specifies the type of data mapper to use when mapping this property.
    /// The type must inherit from <see cref="AbstractDataMapper"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DataMapperAttribute : Attribute
    {
        public Type DataMapperType { get; set; }

        public DataMapperAttribute(Type dataMapperType)
        {
            DataMapperType = dataMapperType;
        }
    }
}
