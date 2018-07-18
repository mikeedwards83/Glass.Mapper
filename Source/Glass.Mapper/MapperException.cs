using System;

namespace Glass.Mapper
{
    /// <summary>
    /// Class MapperException
    /// </summary>
    public class MapperException : Exception 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MapperException(string message):base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="MapperException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        public MapperException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}




