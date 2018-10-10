using System.Collections.Generic;

namespace Glass.Mapper.Configuration
{
    /// <summary>
    /// Interface IConfigurationLoader
    /// </summary>
    public interface IConfigurationLoader
    {

        /// <summary>
        /// Loads this instance.
        /// </summary>
        /// <returns>IEnumerable{AbstractTypeConfiguration}.</returns>
        IEnumerable<AbstractTypeConfiguration> Load();
    }
}




