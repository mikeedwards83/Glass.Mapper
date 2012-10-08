using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Configuration
{
    public class ConfigurationException:ApplicationException
    {
        public ConfigurationException(string message):base(message)
        {

        }
        public ConfigurationException(string message, Exception ex)
            : base(message, ex)
        {

        }
    }
}
