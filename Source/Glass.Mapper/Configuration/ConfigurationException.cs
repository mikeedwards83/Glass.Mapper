using System;

namespace Glass.Mapper.Configuration
{
    /// <summary>
    /// Class ConfigurationException
    /// </summary>
    public class ConfigurationException:ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.ApplicationException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public ConfigurationException(string message):base(message)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        public ConfigurationException(string message, Exception ex)
            : base(message, ex)
        {

        }
    }
}




