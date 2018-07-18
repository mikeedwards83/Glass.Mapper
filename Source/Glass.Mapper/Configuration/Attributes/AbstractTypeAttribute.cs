using System;

namespace Glass.Mapper.Configuration.Attributes
{
    /// <summary>
    /// Class AbstractTypeAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public abstract class AbstractTypeAttribute : Attribute
    {

        public abstract AbstractTypeConfiguration Configure(Type type);

        /// <summary>
        /// Configures the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="config">The config.</param>
        protected virtual void Configure(Type type, AbstractTypeConfiguration config)
        {
            config.Type = type;
            config.ConstructorMethods = Utilities.CreateConstructorDelegates(type);
            config.AutoMap = AutoMap;
        }

        /// <summary>
        /// Indicates that properties should be automapped rather than loaded explicitly. 
        /// </summary>
        public bool AutoMap { get; set; }

    }
}




