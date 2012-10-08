using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper.Configuration
{
    public interface IConfigurationLoader
    {

        IEnumerable<AbstractTypeConfiguration> Load();
    }
}
