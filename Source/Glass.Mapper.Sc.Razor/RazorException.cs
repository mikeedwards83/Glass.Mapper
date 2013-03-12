using System;

namespace Glass.Mapper.Sc.Razor
{
    public class RazorException: ApplicationException
    {
        public RazorException(string message)
            : base(message)
        {
        }
        public RazorException(string message, Exception innerException): base(message, innerException)
        {
        }
    }
}
