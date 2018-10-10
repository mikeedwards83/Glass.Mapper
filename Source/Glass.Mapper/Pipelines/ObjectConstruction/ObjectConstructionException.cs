using System;

namespace Glass.Mapper.Pipelines.ObjectConstruction
{
    /// <summary>
    /// Class ObjectConstructionException
    /// </summary>
    public class ObjectConstructionException :ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.ApplicationException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public ObjectConstructionException(string message)
            : base(message)
        {
        }
    }
}




